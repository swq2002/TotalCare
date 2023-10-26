using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Totalcare.Models;

namespace Totalcare.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConverter _pdfConverter;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public UsersController(ModelContext context, IWebHostEnvironment webHostEnvironment,
            IConverter _pdfConverter, IRazorViewEngine _razorViewEngine, ITempDataProvider tempDataProvider)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            this._pdfConverter = _pdfConverter;
            this._razorViewEngine = _razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        // GET: Users
        public async Task<IActionResult> Index(int flag = 0)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {

                return RedirectToAction("Login", "Authentication");

            }
            else
            {
                TempData["Layout"] = "_UserLayout";
                var id = HttpContext.Session.GetInt32("UserId");



                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(m => m.Userid == id);

                var subscriptionTypes = _context.Subscriptiontypes.ToList();

                

              var checkSub = _context.Subscriptions.Where(x => x.Userid == user.Userid).FirstOrDefault();

                if (checkSub != null)
                {
                    var beneficiaries = _context.Beneficiaries.Where(b => b.Userid == checkSub.Subscriptionid).ToList();
                    var viewModel = new UserViewModel
                    {
                        User = user,
                        SubscriptionTypes = subscriptionTypes,
                        Subscription = checkSub,
                        Beneficiaries = beneficiaries
                    };
                    if (flag == 0)
                    {
                        ViewBag.View = "Subscribed";


                    }
                    else
                    {
                        ViewBag.View = "Upgrade";

                    }
                    return View(viewModel);

                }
                else
                {
                    ViewBag.View = "Pricing";
                    var viewModel = new UserViewModel
                    {
                        User = user,
                        SubscriptionTypes = subscriptionTypes,
                    };
                    return View(viewModel);

                }



            }


        }
        public IActionResult Pricing()
        {


            return View();



        }




        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
            var id = HttpContext.Session.GetInt32("UserId");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(Convert.ToDecimal(id));
            if (user == null)
            {
                return NotFound();
            }
            return View(new UserViewModel { User = user });
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Userid,Password,Fullname,Email,Phonenumber,Profilepicture")] User user)
        {



            if (ModelState.IsValid)
            {

                try
                {
                    _context.Attach(user);

                    _context.Entry(user).Property(u => u.Fullname).IsModified = true;
                    _context.Entry(user).Property(u => u.Email).IsModified = true;
                    _context.Entry(user).Property(u => u.Phonenumber).IsModified = true;
                    _context.Entry(user).Property(u => u.Password).IsModified = true;

                    await _context.SaveChangesAsync();
                    ViewBag.Update = "Your profile information has been updated";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["UpdateMessage"] = "Your profile information has been updated";
                return RedirectToAction(nameof(Edit));
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(decimal id, [Bind("Userid,Profilepicture,ImageFile")] User user)
        {



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
                try
                {
                    _context.Attach(user);

                    // Specify which properties should be marked as modified
                    _context.Entry(user).Property(u => u.Profilepicture).IsModified = true;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["UploadMessage"] = "Your profile picture has been updated";

                return RedirectToAction(nameof(Edit));
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }
        public IActionResult ThankYou()
        {


            return View();
        }
        public IActionResult Subscribed()
        {
            return View();
        }
        public IActionResult Upgrade()
        {
            var subscriptionTypes = _context.Subscriptiontypes.ToList();


            var viewModel = new UserViewModel
            {
                SubscriptionTypes = subscriptionTypes,

            };

            return View(viewModel);
        }
        public async Task<IActionResult> Invoice()
        {
            var id = HttpContext.Session.GetInt32("UserId");



            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
            var aboutUs = _context.Aboutus.FirstOrDefault();
            var sub = _context.Subscriptions.Include(t => t.Type)
                .Where(x => x.Userid == user.Userid).FirstOrDefault();

            var invoiceViewModel = new InvoiceViewModel
            {

                User = user,
                Aboutu = aboutUs,
                Subscription = sub
            };

            return View(invoiceViewModel);
        }

        public async Task SendEmailWithInvoiceAsync(string recipientEmail, byte[] invoiceBytes)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Totalcare Insurance", "saif2002.selawi@gmail.com"));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Your Totalcare Insurance Invoice";

            var body = new TextPart("html")
            {
                Text = @"<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            background-color: #ffffff;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #333;
        }
        p {
            color: #555;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>Dear Totalcare Insurance Subscriber,</h1>
        <p>Thank you for your subscription. We appreciate your business!</p>
        <p>Find your invoice attached to this email.</p>
        <p>Best regards,</p>
        <p>Totalcare Insurance Team</p>
    </div>
</body>
</html>"
            };

            // Create an attachment
            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(new MemoryStream(invoiceBytes), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "invoice.pdf"
            };

            // Create a multipart message with the body and attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            // Set the message body
            message.Body = multipart;

            // Configure the SMTP client (use your Gmail credentials)
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, useSsl: false);
                await client.AuthenticateAsync("saif2002.selawi@gmail.com", "pwdizkvfjrhamtoz");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        public async Task<string> RenderRazorViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var viewEngineResult = _razorViewEngine.FindView(ControllerContext, "Invoice", false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Unable to find the view: {viewName}");
            }

            var view = viewEngineResult.View;
            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model // Set the model for the view
            };

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    ControllerContext,
                    view,
                    viewData,
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return sw.ToString();
            }
        }

        public async Task<IActionResult> GenerateInvoicePdf()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            var aboutUs = _context.Aboutus.FirstOrDefault();
            var sub = _context.Subscriptions.Include(t => t.Type)
                .Where(x => x.Userid == user.Userid).FirstOrDefault();

            var invoiceViewModel = new InvoiceViewModel
            {
                User = user,
                Aboutu = aboutUs,
                Subscription = sub
            };

            // Render the Razor view to a string
            var htmlContent = await RenderRazorViewToStringAsync("Invoice", invoiceViewModel);

            // Convert HTML content to a PDF
            var pdf = _pdfConverter.Convert(new HtmlToPdfDocument
            {
                Objects = {
            new ObjectSettings
            {
                HtmlContent = htmlContent, // Corrected the await here
                WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = { FontSize = 12, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
            FooterSettings = { FontSize = 12, Right = "Generated [date]", Line = true, Spacing = 2.812 },
            }
        },
            });

            // Save the PDF to a byte array
            byte[] pdfBytes = pdf;

            // Attach the PDF to the email and send it
            await SendEmailWithInvoiceAsync(user.Email, pdfBytes);

            return View("ThankYou");
        }

        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
    }
}
