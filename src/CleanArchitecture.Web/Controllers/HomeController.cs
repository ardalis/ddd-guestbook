using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mail;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Guestbook> _guestbookRepository;

        public HomeController(IRepository<Guestbook> guestbookRepository)
        {
            _guestbookRepository = guestbookRepository;
        }

        public IActionResult Index()
        {
            if (!_guestbookRepository.List().Any())
            {
                var newGuestbook = new Guestbook() { Name = "My Guestbook" };
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "steve@deviq.com", Message = "Hi!", DateTimeCreated = DateTime.UtcNow.AddHours(-2) });
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "mark@deviq.com", Message = "Hi again!", DateTimeCreated = DateTime.UtcNow.AddHours(-1) });
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "michelle@deviq.com", Message = "Hello!" });
                _guestbookRepository.Add(newGuestbook);
            }

            var guestbook = _guestbookRepository.GetById(1); // hardcoded for this sample; could support multi-tenancy in real app
            var viewModel = new HomePageViewModel();
            viewModel.GuestbookName = guestbook.Name;
            viewModel.PreviousEntries.AddRange(guestbook.Entries);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guestbook = _guestbookRepository.GetById(1);

                // notify all previous entries
                foreach (var entry in guestbook.Entries)
                {
                    var message = new MailMessage();
                    message.To.Add(new MailAddress("guestbook@whatever.com"));
                    message.From = new MailAddress(entry.EmailAddress);
                    message.Subject = "New guestbook entry added";
                    message.Body =  model.NewEntry.Message ;
                    using (var client = new SmtpClient("localhost",25))
                    {
                        client.Send(message);
                    }
                }

                guestbook.Entries.Add(model.NewEntry);
                _guestbookRepository.Update(guestbook);

                model.PreviousEntries.Clear();
                model.PreviousEntries.AddRange(guestbook.Entries);
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
