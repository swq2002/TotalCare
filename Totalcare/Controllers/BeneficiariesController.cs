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
    public class BeneficiariesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BeneficiariesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Beneficiaries
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Beneficiaries.Include(b => b.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Beneficiaries/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Beneficiaryid == id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return View(beneficiary);
        }

        // GET: Beneficiaries/Create
        public async Task<IActionResult> Create()
        {

            var checkType = (Subscription)_context.Subscriptions.Include(typ => typ.Type).Where(x => x.Userid == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            if (checkType != null)
            {
                if (checkType.Type.Name.Contains("Individual Plan"))
                {
                    return RedirectToAction("Index", "Users", new { flag = 1 });


                }
                else
                {
                    var id = HttpContext.Session.GetInt32("UserId");


                    var user = await _context.Users
                                        .Include(u => u.Role)
                                        .FirstOrDefaultAsync(m => m.Userid == id); ;
                    var viewModel = new UserViewModel
                    {
                        User = user,
                        Beneficiary = new Beneficiary()


                    };
                    return View(viewModel);


                }
            }
            else
            {
                return RedirectToAction("Login", "Authentication");


            }
        }

        // POST: Beneficiaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Relationship,Proofdocument,ImageFile")] Beneficiary beneficiary)
        {
            if (ModelState.IsValid)
            {
                if (beneficiary.ImageFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + beneficiary.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await beneficiary.ImageFile.CopyToAsync(fileStream);

                    }
                    beneficiary.Proofdocument = fileName;
                }
                beneficiary.Userid = _context.Subscriptions.Where(x => x.Userid == HttpContext.Session.GetInt32("UserId")).Select(s=>s.Subscriptionid).FirstOrDefault();
                beneficiary.Requeststatus = "Waiting";
                _context.Add(beneficiary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beneficiary);
        }

        // GET: Beneficiaries/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid", beneficiary.Userid);
            return View(beneficiary);
        }

        // POST: Beneficiaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Beneficiaryid,Userid,Relationship,Proofdocument,Requeststatus")] Beneficiary beneficiary)
        {
            if (id != beneficiary.Beneficiaryid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beneficiary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeneficiaryExists(beneficiary.Beneficiaryid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid", beneficiary.Userid);
            return View(beneficiary);
        }

        // GET: Beneficiaries/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Beneficiaryid == id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return View(beneficiary);
        }

        // POST: Beneficiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Beneficiaries == null)
            {
                return Problem("Entity set 'ModelContext.Beneficiaries'  is null.");
            }
            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary != null)
            {
                _context.Beneficiaries.Remove(beneficiary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeneficiaryExists(decimal id)
        {
            return (_context.Beneficiaries?.Any(e => e.Beneficiaryid == id)).GetValueOrDefault();
        }
    }
}
