    using GymManagement.Areas.Identity.Data;
using GymManagement.Models;
using GymManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GymManagement.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MemberController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Index - List All Members
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Members
                .Include(m => m.Package)
                .Include(m => m.Trainer)    
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m =>
                    m.FullName.Contains(searchString) ||
                    m.Contact.Contains(searchString) ||
                    m.MemberStatus.Contains(searchString));
            }

            var members = await query.ToListAsync();
            return View(members);
        }
        #endregion


        #region Create Member
        public IActionResult Create()
        {
            ViewBag.Packages = _context.Packages.ToList();
            ViewBag.Trainers = _context.Trainers.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;

                    if (model.photo != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.photo.CopyToAsync(fileStream);
                        }
                    }

                    var member = new Member
                    {
                        FullName = model.FullName,
                        Gender = model.Gender,
                        Contact = model.Contact,
                        MemberStatus = model.MemberStatus,
                        DayTiming = model.DayTiming,
                        AccountOpenDate = DateTime.Now,
                        PackageId = model.PackageId,
                        TrainerId = model.TrainerId,
                        AdmissionFee = model.AdmissionFee,
                        MonthlyFee = model.MonthlyFee,
                        Discount = model.Discount,
                        TotalAmount = model.TotalAmount,
                        Account = model.Account,
                        MemberImage = uniqueFileName
                    };

                    _context.Members.Add(member);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Member Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            ViewBag.Packages = _context.Packages.ToList();
            ViewBag.Trainers = _context.Trainers.ToList();
            return View(model);
        }
        #endregion

        #region Edit Member
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            var viewModel = new MemberViewModel
            {
                Id = member.Id,
                FullName = member.FullName,
                Gender = member.Gender,
                Contact = member.Contact,
                MemberStatus = member.MemberStatus,
                DayTiming = member.DayTiming,
                PackageId = member.PackageId,
                PackageExpiryDate = member.PackageExpiryDate,
                TrainerId = member.TrainerId ?? 0, // Default to 0 if TrainerId is null
                AdmissionFee = member.AdmissionFee,
                MonthlyFee = member.MonthlyFee,
                Discount = member.Discount,
                TotalAmount = member.TotalAmount,
                Account = member.Account,
            };

            ViewBag.Packages = _context.Packages.ToList();
            ViewBag.Trainers = _context.Trainers.ToList();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var member = await _context.Members.FindAsync(id);
                    if (member == null)
                    {
                        return NotFound();
                    }

                    if (model.photo != null)
                    {
                        // Delete old image if exists
                        if (member.MemberImage != null)
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", member.MemberImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.photo.CopyToAsync(fileStream);
                        }

                        member.MemberImage = uniqueFileName;
                    }

                    member.FullName = model.FullName;
                    member.Gender = model.Gender;
                    member.Contact = model.Contact;
                    member.MemberStatus = model.MemberStatus;
                    member.DayTiming = model.DayTiming;
                    member.PackageId = model.PackageId;
                    member.PackageExpiryDate = model.PackageExpiryDate;
                    member.TrainerId = model.TrainerId;
                    member.AdmissionFee = model.AdmissionFee;
                    member.MonthlyFee = model.MonthlyFee;
                    member.Discount = model.Discount;
                    member.TotalAmount = model.TotalAmount;
                    member.Account = model.Account;

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Member Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            ViewBag.Packages = _context.Packages.ToList();
            ViewBag.Trainers = _context.Trainers.ToList();
            return View(model);
        }
        #endregion

        #region Delete Member

        // GET Action - Display the delete confirmation view
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST Action - Handle the deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                if (member.MemberImage != null)
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", member.MemberImage);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Members with Trainers
        public async Task<IActionResult> MembersWithTrainers(string searchString)
        {
            var membersWithTrainers = _context.Members
                .Include(m => m.Trainer) // Include the Trainer navigation property
                .AsQueryable(); // Make it queryable for filtering

            // Apply search filter if searchString is not null or empty
            if (!string.IsNullOrEmpty(searchString))
            {
                membersWithTrainers = membersWithTrainers.Where(m =>
                    m.FullName.Contains(searchString) ||
                    m.Contact.Contains(searchString) ||
                    m.Trainer.FullName.Contains(searchString)); // Searching in Trainer's name
            }

            return View(await membersWithTrainers.ToListAsync());
        }
        #endregion

        #region Members with Packages and Search Logic
        public async Task<IActionResult> MembersWithPackages(string searchString)
        {
            // Fetch members with packages and include related navigation properties
            var query = _context.Members
                .Include(m => m.Package) // Include the Package navigation property
                .AsQueryable();

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); // Normalize the search string to lowercase
                query = query.Where(m =>
                    m.FullName.ToLower().Contains(searchString) ||
                    m.Contact.ToLower().Contains(searchString));
            }

            // Execute query and fetch data asynchronously
            var membersWithPackages = await query.ToListAsync();

            // Return the filtered list to the view
            return View(membersWithPackages);
        }
        #endregion

        public async Task<IActionResult> GetPackagesByGender(string gender)
        {
            if (string.IsNullOrEmpty(gender))
            {
                return Json(new List<Package>());
            }

            var packages = gender == "Male"
                ? await _context.Packages.Where(p => p.Gender == "Male").ToListAsync()
                : await _context.Packages.Where(p => p.Gender == "Female").ToListAsync();

            return Json(packages);
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(m => m.Id == id);
        }
    }
}
