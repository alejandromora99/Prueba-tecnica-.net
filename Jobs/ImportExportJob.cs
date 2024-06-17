using Quartz;
using System.Threading.Tasks;

namespace CronProject.Jobs
{
    public class ImportExportJob : IJob
    {
        private readonly CsvImportService _csvImportService;

        public ImportExportJob(CsvImportService csvImportService)
        {
            _csvImportService = csvImportService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var inputFilePath = "./input.csv"; // Ruta del archivo de entrada
            var outputFilePath = "./output.csv"; // Ruta del archivo de salida

            await _csvImportService.ImportAndExportCsvAsync(inputFilePath, outputFilePath);
        }
    }
}
