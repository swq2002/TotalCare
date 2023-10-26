using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class HomeController : Controller
    {

        private readonly ModelContext _context;

        public HomeController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            ViewData["contactInfo"] = _context.Aboutus.FirstOrDefault() ;

            return View();
        }
    }
}
