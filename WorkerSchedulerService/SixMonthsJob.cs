using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Quartz;

public class SixMonthsJob : IJob
{
    private readonly ILogger<SixMonthsJob> _logger;

    public SixMonthsJob(ILogger<SixMonthsJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Six months job executed at: {time}", DateTimeOffset.Now);

        // Generate Excel file for six months
        CreateExcelFile();

        return Task.CompletedTask;
    }

    private void CreateExcelFile()
    {
        string filePath = $"SixMonthsReport_{DateTime.Now:yyyyMMdd}.xlsx";

        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Six Months Report");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Date";

            worksheet.Cells[2, 1].Value = 1;
            worksheet.Cells[2, 2].Value = "Six Month Data";
            worksheet.Cells[2, 3].Value = DateTime.Now;

            var file = new FileInfo(filePath);
            package.SaveAs(file);
        }

        _logger.LogInformation("Six months Excel file created: {filePath}", filePath);
    }
}
