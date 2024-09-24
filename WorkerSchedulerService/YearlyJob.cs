using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Quartz;

public class YearlyJob : IJob
{
    private readonly ILogger<YearlyJob> _logger;

    public YearlyJob(ILogger<YearlyJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Yearly job executed at: {time}", DateTimeOffset.Now);

        // Generate Excel file for the yearly report
        CreateExcelFile();

        return Task.CompletedTask;
    }

    private void CreateExcelFile()
    {
        string filePath = $"YearlyReport_{DateTime.Now:yyyyMMdd}.xlsx";

        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Yearly Report");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Date";

            worksheet.Cells[2, 1].Value = 1;
            worksheet.Cells[2, 2].Value = "Yearly Data";
            worksheet.Cells[2, 3].Value = DateTime.Now;

            var file = new FileInfo(filePath);
            package.SaveAs(file);
        }

        _logger.LogInformation("Yearly Excel file created: {filePath}", filePath);
    }
}
