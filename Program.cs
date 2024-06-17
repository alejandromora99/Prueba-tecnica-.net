using CronProject.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using CronProject.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Configuro Kestrel para que escuche los puertos especificos que aparecen en launchSettings (no podia conectarme por https)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5271); // HTTP port
    options.ListenAnyIP(7169, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS port
    });
});

// añado servicios
builder.Services.AddControllers();

// Definir el tamaño del lote
int batchSize = 100; 


//le entrego nueva instancia con tamaño del lote definido
builder.Services.AddScoped<CsvImportService>(provider => 
    new CsvImportService(provider.GetRequiredService<IServiceScopeFactory>(), 
                         provider.GetRequiredService<CsvExportService>(), 
                         batchSize));
builder.Services.AddScoped<CsvExportService>();

// Configuro entidades del framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Quartz.NET
builder.Services.AddQuartz(q =>
{
    // q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("ImportExportJob");

    q.AddJob<ImportExportJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ImportExportJob-trigger")
        .WithCronSchedule("0 0 23 * * ?")); // Programar a las 23:00 cada dia
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
