using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthenticationController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;   
        }
        public IActionResult Register()
        {
            ViewData["contactInfo"] = _context.Aboutus.FirstOrDefault();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Fullname,Email,Phonenumber,Password,Profilepicture,ImageFile")] User user)
        {
            ViewData["contactInfo"] = _context.Aboutus.FirstOrDefault();

            if (ModelState.IsValid)
            {

                if (user.ImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);

                    }
                    user.Profilepicture = fileName;
                }
                var userr = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();
                if (userr == null)
                {
                    user.Roleid = 2;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                    ViewBag.Error = "Email is already used please try another email";
                }
            }
            return View();
        }
        public IActionResult Login()
        {
            ViewData["contactInfo"] = _context.Aboutus.FirstOrDefault();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Userid,Email,Password")] User user)
        {
            ViewData["contactInfo"] = _context.Aboutus.FirstOrDefault();

            var auth = _context.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (auth != null)
            {


                switch (auth.Roleid)
                {

                    case 1:

                        HttpContext.Session.SetInt32("UserId", (int)auth.Userid);


                        return RedirectToAction("Index", "Admin");

                    case 2:

                        
                        HttpContext.Session.SetInt32("UserId", (int)auth.Userid);


                        return RedirectToAction("Index", "Users");

                }


            }
            else
            {
                ViewBag.Error = "Wrong password or email";
            }






            return View();

        }


    }
}
