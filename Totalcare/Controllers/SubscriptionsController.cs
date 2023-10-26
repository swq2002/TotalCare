using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Totalcare.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Totalcare.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ModelContext _context;

        public SubscriptionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Subscriptions.Include(s => s.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Subscriptionid == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }
        [HttpGet]
        // GET: Subscriptions/Create
        public IActionResult Create(int id)
        {

            var subInfo = _context.Subscriptiontypes.Where(x=>x.Id==id).FirstOrDefault();
            return View(subInfo);


        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(decimal price, decimal typeid)
        {
           
            if (ModelState.IsValid)
            {
                

                var cardNumber = Convert.ToInt64(HttpContext.Request.Form["Cardnum"]);
                var expiryDate = HttpContext.Request.Form["Expirydate"];
                var cvv = Convert.ToInt64(HttpContext.Request.Form["Cvv"]);

                var bank = new Bank
                {
                    Cardnum = cardNumber,
                   Expirydate = expiryDate,
                    Cvv = cvv
                };
                var payment = _context.Banks
                    .Where(x => x.Cardnum == bank.Cardnum && x.Expirydate == bank.Expirydate && x.Cvv == bank.Cvv && x.Balance >= price)
                    .FirstOrDefault();

                if (payment != null)
                {
                    payment.Balance -= price;
                    var checkSubscription= (Subscription)_context.Subscriptions.Where(x => x.Userid == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
                    if(checkSubscription != null)
                    {
                        checkSubscription.Typeid = 3;


                        _context.Update(payment);
                        _context.Update(checkSubscription);

                    }
                    else
                    {
                        var subscription = new Subscription
                        {
                            Userid = HttpContext.Session.GetInt32("UserId"),
                            Subscriptiondate = DateTime.Now,
                            Paymentstatus = "Approved",
                            Typeid = typeid
                        };

                        _context.Update(payment);
                        _context.Add(subscription);



                    }
                  

                    _context.SaveChanges();

                    return RedirectToAction("GenerateInvoicePdf", "Users");
                }
                else
                {
                    TempData["Error"] = "Your card has been declined. Please check your card details.";

                }



            }

            return RedirectToAction(nameof(Create));
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", subscription.Userid);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Subscriptionid,Userid,Subscriptiondate,Paymentstatus")] Subscription subscription)
        {
            if (id != subscription.Subscriptionid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.Subscriptionid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", subscription.Userid);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Subscriptionid == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'ModelContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


      

        private bool SubscriptionExists(decimal id)
        {
          return (_context.Subscriptions?.Any(e => e.Subscriptionid == id)).GetValueOrDefault();
        }
    }
}
