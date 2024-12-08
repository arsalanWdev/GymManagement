using GymManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace GymManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DownloadReport()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReport(DateTime selectedDate)
        {
            var walkIns = _context.WalkIns
                .Where(w => w.VisitDate.Date == selectedDate.Date)
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("WalkInReport");

                // Add column headers
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Contact";
                worksheet.Cells[1, 3].Value = "Purpose";
                worksheet.Cells[1, 4].Value = "Visit Date";

                // Map data into the worksheet rows
                int row = 2;
                foreach (var walkIn in walkIns)
                {
                    worksheet.Cells[row, 1].Value = walkIn.Name ?? string.Empty;
                    worksheet.Cells[row, 2].Value = walkIn.Contact ?? string.Empty;
                    worksheet.Cells[row, 3].Value = walkIn.Purpose ?? string.Empty;
                    worksheet.Cells[row, 4].Value = walkIn.VisitDate.ToString("yyyy-MM-dd");
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"WalkInReport_{selectedDate:yyyyMMdd}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
