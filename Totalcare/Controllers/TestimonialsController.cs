using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");


            var user = await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(m => m.Userid == id);

            var checkTestimonials = (Testimonial)_context.Testimonials.Where(x => x.Userid == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (checkTestimonials != null)
            {
                ViewBag.View = "Show";


              





            }
            else
            {
                ViewBag.View = "Create";


            }
            var viewModel = new UserViewModel
            {
                User = user,
                Testimonial = checkTestimonials


            };
            return View(viewModel);

        }
        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialtext,Testimonialsubject")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                testimonial.Userid =  HttpContext.Session.GetInt32("UserId");
                testimonial.Testimonialdate= DateTime.Now;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit()

        {
            var id = HttpContext.Session.GetInt32("UserId");


            var user = await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(m => m.Userid == id);

            var checkTestimonials = (Testimonial)_context.Testimonials.Where(x => x.Userid == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();

            var viewModel = new UserViewModel
            {
                User = user,
                Testimonial = checkTestimonials


            };
            return View(viewModel);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Testimonialid,Userid,Testimonialtext,Testimonialsubject")] Testimonial testimonial)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Attach(testimonial);
                    testimonial.Testimonialdate = DateTime.Now;
                    _context.Entry(testimonial).Property(u => u.Testimonialsubject).IsModified = true;
                    _context.Entry(testimonial).Property(u => u.Testimonialtext).IsModified = true;
                   

                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Show()
        {
            return View();
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }
    }
}
