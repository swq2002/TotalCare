using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class SharedController : Controller
    {
        private readonly ModelContext _context;

        public SharedController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var contactInfo = _context.Contactus.FirstOrDefault();
            return View(contactInfo);
        }
    }
}


