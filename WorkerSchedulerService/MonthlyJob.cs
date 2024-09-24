using System;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Quartz;

public class MonthlyJob : IJob
{
    private readonly ILogger<MonthlyJob> _logger;

    public MonthlyJob(ILogger<MonthlyJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Monthly job executed at: {time}", DateTimeOffset.Now);

        // Generate the Excel file
        CreateExcelFile();

        // Generate PDF file
        CreatePdfFile();

        return Task.CompletedTask;
    }

    private void CreateExcelFile()
    {
        string filePath = $"MonthlyReport_{DateTime.Now:yyyyMMdd}.xlsx";

        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            // Create a new worksheet
            var worksheet = package.Workbook.Worksheets.Add("Report");

            // Add some data
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Date";

            worksheet.Cells[2, 1].Value = 1;
            worksheet.Cells[2, 2].Value = "Sample Data";
            worksheet.Cells[2, 3].Value = DateTime.Now;

            // Save the Excel file
            var file = new FileInfo(filePath);
            package.SaveAs(file);
        }

        _logger.LogInformation("Excel file created: {filePath}", filePath);
    }

    private void CreatePdfFile()
    {
        string pdfFilePath = $"MonthlyReport_{DateTime.Now:yyyyMMdd}.pdf";

        // Create PDF document
        using (var writer = new PdfWriter(pdfFilePath))
        {
            using (var pdf = new PdfDocument(writer))
            {
                var document = new Document(pdf);

                // Add content to PDF
                document.Add(new Paragraph("Monthly Report"));
                document.Add(new Paragraph($"Generated on: {DateTime.Now}"));

                // Add table to PDF
                var table = new Table(3); // 3 columns
                table.AddCell("ID");
                table.AddCell("Name");
                table.AddCell("Date");

                table.AddCell("1");
                table.AddCell("Sample Data");
                table.AddCell(DateTime.Now.ToString());

                document.Add(table);
                document.Close();
            }
        }

        _logger.LogInformation("PDF file created: {pdfFilePath}", pdfFilePath);
    }
}
