using CronProject.Data;
using CronProject.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

public class CsvImportService
{
    private readonly ApplicationDbContext _context;

    public CsvImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ImportCsvAsync(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null, // Ignorar la validación del encabezado
            MissingFieldFound = null,// No lanzar excepción si falta un campo (Por el campo ID)
            PrepareHeaderForMatch = args => args.Header.ToLower(),// Asegurarse de que los encabezados coincidan con los nombres de las propiedades
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" }; // Configuro CsvHelper con el formato de fecha que tendra el csv (por defecto usa DateTime)

        var records = csv.GetRecords<DataRecordWithoutId>();

        foreach (var record in records)
        {
            var dataRecord = new DataRecord
            {
                RutClienteEnvia = record.RutClienteEnvia,
                NombreClienteEnvia = record.NombreClienteEnvia,
                IdTransaccion = record.IdTransaccion,
                RutClienteRecibe = record.RutClienteRecibe,
                NombreClienteRecibe = record.NombreClienteRecibe,
                CodigoBancoReceptor = record.CodigoBancoReceptor,
                NombreBancoReceptor = record.NombreBancoReceptor,
                MontoTransferencia = record.MontoTransferencia,
                MonedaTransferencia = record.MonedaTransferencia,
                FechaTransferencia = record.FechaTransferencia
            };
            _context.DataRecords.Add(dataRecord);
        }

        await _context.SaveChangesAsync();
    }
}

public class DataRecordWithoutId
{
    public required string RutClienteEnvia { get; set; }
    public required string NombreClienteEnvia { get; set; }
    public required string IdTransaccion { get; set; }
    public required string RutClienteRecibe { get; set; }
    public required string NombreClienteRecibe { get; set; }
    public required string CodigoBancoReceptor { get; set; }
    public required string NombreBancoReceptor { get; set; }
    public decimal MontoTransferencia { get; set; }
    public required string MonedaTransferencia { get; set; }
    public DateTime FechaTransferencia { get; set; }
}

