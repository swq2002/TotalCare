using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.IO.Filters;
using System;
using System.Data;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        public AdminController(ModelContext context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            TempData["Layout"] = "_AdminLayout";

            var users = _context.Users.Where(u => u.Roleid == 2).ToList();
            var subs = _context.Subscriptions.Include(s => s.Type).ToList();
            var testimonials = _context.Testimonials.ToList();
            ViewBag.Profit1 = Convert.ToDouble(Convert.ToInt32(subs.Where(s => s.Typeid == 1).ToList().Count) * subs.Where(s => s.Typeid == 1).FirstOrDefault().Type.Price);
            ViewBag.Profit2 = Convert.ToDouble(Convert.ToInt32(subs.Where(s => s.Typeid == 2).ToList().Count) * subs.Where(s => s.Typeid == 2).FirstOrDefault().Type.Price);
            // ViewBag.Profit3 = Convert.ToDouble(Convert.ToInt32(subs.Where(s => s.Typeid == 3).ToList().Count) * subs.Where(s => s.Typeid == 3).FirstOrDefault().Type.Price);

            var adminViewModel = new AdminViewModel
            {
                Users = users,
                Subscriptions = subs,
                Testimonials = testimonials

            };

            return View(adminViewModel);
        }
        public IActionResult ManageSubs()
        {
            var subs = _context.Subscriptions.Include(s => s.Type).Include(u => u.User).ToList();
            var adminViewModel = new AdminViewModel
            {
                Subscriptions = subs,

            };
            return View(adminViewModel);
        }
        [HttpPost]
        public IActionResult ManageSubs(DateTime? startDate, DateTime? endDate)
        {
            var subs = _context.Subscriptions.Include(s => s.Type).Include(u => u.User).ToList();
            var adminViewModel = new AdminViewModel
            {
                Subscriptions = subs,

            };
            if (startDate == null && endDate == null)
            {

                return View(adminViewModel);


            }
            else if (startDate != null && endDate == null)
            {
                var result = adminViewModel.Subscriptions.Where(x => x.Subscriptiondate.Value.Date >= startDate).ToList();
                adminViewModel.Subscriptions = result;
                return View(adminViewModel);

            }
            else if (startDate == null && endDate != null)
            {
                var result = adminViewModel.Subscriptions.Where(x => x.Subscriptiondate.Value.Date >= endDate).ToList();
                adminViewModel.Subscriptions = result;
                return View(adminViewModel);

            }
            else
            {
                var result = adminViewModel.Subscriptions.Where(x => x.Subscriptiondate.Value.Date >= startDate && x.Subscriptiondate.Value.Date <= endDate).ToList();
                adminViewModel.Subscriptions = result;
                return View(adminViewModel);

            }


        }
        [HttpGet]
        public IActionResult ManageBeneficiaries(string searchStringW, string searchStringA, string searchStringR)
        {
            var adminViewModel = new AdminViewModel();

            var subsWaiting = _context.Beneficiaries
                .Include(u => u.User)
                .Include(u => u.User.User)
                .Where(st => st.Requeststatus == "Waiting")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringW))
            {
                subsWaiting = subsWaiting.Where(ben => ben.User.User.Fullname.Contains(searchStringW));
            }

            adminViewModel.BeneficiariesWaiting = subsWaiting.ToList();

            var subsRejected = _context.Beneficiaries
                .Include(u => u.User)
                .Include(u => u.User.User)
                .Where(st => st.Requeststatus == "Rejected")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringR))
            {
                subsRejected = subsRejected.Where(ben => ben.User.User.Fullname.Contains(searchStringR));
            }

            adminViewModel.BeneficiariesRejected = subsRejected.ToList();

            // Get the data for BeneficiariesAccepted
            var subsAccepted = _context.Beneficiaries
                .Include(u => u.User)
                .Include(u => u.User.User)
                .Where(st => st.Requeststatus == "Accepted")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringA))
            {
                subsAccepted = subsAccepted.Where(ben => ben.User.User.Fullname.Contains(searchStringA));
            }

            adminViewModel.BeneficiariesAccepted = subsAccepted.ToList();
            return View(adminViewModel);
        }






        public IActionResult BenRequest(string status, decimal id)
        {
            var ben = _context.Beneficiaries.Where(benId => benId.Beneficiaryid == id).FirstOrDefault();
            ben.Requeststatus = status;
            _context.SaveChanges();
            return RedirectToAction("ManageBeneficiaries");
        }
        public IActionResult TesRequest(string status, decimal id)
        {
            var tes = _context.Testimonials.Where(benId => benId.Testimonialid == id).FirstOrDefault();
            tes.Requeststatus = status;
            _context.SaveChanges();
            return RedirectToAction("ManageTestimonials");
        }

        public IActionResult ManageTestimonials(string searchStringW, string searchStringA, string searchStringR)
        {
            var adminViewModel = new AdminViewModel();

            var tesWaiting = _context.Testimonials
                .Include(u => u.User)
                .Where(st => st.Requeststatus == "Waiting")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringW))
            {
                tesWaiting = tesWaiting.Where(u => u.User.Fullname.Contains(searchStringW));
            }

            adminViewModel.TestimonialsWaiting = tesWaiting.ToList();

            var tesRejected = _context.Testimonials
                .Include(u => u.User)
                .Where(st => st.Requeststatus == "Rejected")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringR))
            {
                tesRejected = tesRejected.Where(u => u.User.Fullname.Contains(searchStringR));
            }

            adminViewModel.TestimonialsRejected = tesRejected.ToList();

            var tesAccepted = _context.Testimonials
                .Include(u => u.User)
                .Where(st => st.Requeststatus == "Accepted")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringA))
            {
                tesAccepted = tesAccepted.Where(ben => ben.User.Fullname.Contains(searchStringA));
            }

            adminViewModel.TestimonialsAccepted = tesAccepted.ToList();
            return View(adminViewModel);
        }

        public IActionResult DownloadFile(string filePath)
        {
            string filePathWithDirectory = Path.Combine("Images", filePath);
            string filePhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePathWithDirectory);

            if (System.IO.File.Exists(filePhysicalPath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePhysicalPath);

                string fileExtension = Path.GetExtension(filePhysicalPath);
                string contentType = GetContentType(fileExtension);

                return File(fileBytes, contentType, Path.GetFileName(filePhysicalPath));
            }
            else
            {
                return NotFound(); 
            }
        }

        private string GetContentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream"; 
            }
        }

    }
}
