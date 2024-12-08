using GymManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace GymManagement.Controllers
{
    public class MemberReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberReportController(ApplicationDbContext context)
        {
            this._context = context;
        }
        // Index - Filter Members
        public async Task<IActionResult> Index(string name, string contact, string gender, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Members.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(name))
                query = query.Where(m => m.FullName.Contains(name));
            if (!string.IsNullOrEmpty(contact))
                query = query.Where(m => m.Contact.Contains(contact));
            if (!string.IsNullOrEmpty(gender))
                query = query.Where(m => m.Gender == gender);
            if (startDate.HasValue)
                query = query.Where(m => m.AccountOpenDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.AccountOpenDate <= endDate.Value);

            var members = await query.Include(m => m.Package)
                                      .Include(m => m.Trainer)
                                      .ToListAsync();

            return View(members);
        }

        // Download Excel Report
        public async Task<IActionResult> DownloadReport(string name, string contact, string gender, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Members.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(name))
                query = query.Where(m => m.FullName.Contains(name));
            if (!string.IsNullOrEmpty(contact))
                query = query.Where(m => m.Contact.Contains(contact));
            if (!string.IsNullOrEmpty(gender))
                query = query.Where(m => m.Gender == gender);
            if (startDate.HasValue)
                query = query.Where(m => m.AccountOpenDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.AccountOpenDate <= endDate.Value);

            var members = await query.Include(m => m.Package)
                                      .Include(m => m.Trainer)
                                      .ToListAsync();

            // Generate Excel Report
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Members Report");

                // Header row
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Full Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Contact";
                worksheet.Cells[1, 5].Value = "Status";
                worksheet.Cells[1, 6].Value = "Day Timing";
                worksheet.Cells[1, 7].Value = "Trainer Name";
                worksheet.Cells[1, 8].Value = "Package Name";
                worksheet.Cells[1, 9].Value = "Admission Fee";
                worksheet.Cells[1, 10].Value = "Monthly Fee";
                worksheet.Cells[1, 11].Value = "Discount";
                worksheet.Cells[1, 12].Value = "Total Amount";
                worksheet.Cells[1, 13].Value = "Account";
                worksheet.Cells[1, 14].Value = "Account Open Date";

                // Data rows
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];
                    worksheet.Cells[i + 2, 1].Value = member.Id;
                    worksheet.Cells[i + 2, 2].Value = member.FullName;
                    worksheet.Cells[i + 2, 3].Value = member.Gender;
                    worksheet.Cells[i + 2, 4].Value = member.Contact;
                    worksheet.Cells[i + 2, 5].Value = member.MemberStatus;
                    worksheet.Cells[i + 2, 6].Value = member.DayTiming;
                    worksheet.Cells[i + 2, 7].Value = member.Trainer?.FullName ?? "No Trainer";
                    worksheet.Cells[i + 2, 8].Value = member.Package?.PackageName ?? "No Package";
                    worksheet.Cells[i + 2, 9].Value = member.AdmissionFee;
                    worksheet.Cells[i + 2, 10].Value = member.MonthlyFee;
                    worksheet.Cells[i + 2, 11].Value = member.Discount;
                    worksheet.Cells[i + 2, 12].Value = member.TotalAmount;
                    worksheet.Cells[i + 2, 13].Value = member.Account;
                    worksheet.Cells[i + 2, 14].Value = member.AccountOpenDate.ToShortDateString();
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MembersReport.xlsx");
            }
        }
    }
}
