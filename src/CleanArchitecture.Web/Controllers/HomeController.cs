using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IMessageSender _messageSender;

        public HomeController(IRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        public IActionResult Index()
        {
            if (!_repository.List<Guestbook>().Any())
            {
                var newGuestbook = new Guestbook { Name = "My Guestbook" };
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "steve@deviq.com", Message = "Hi!", DateTimeCreated = DateTime.UtcNow.AddHours(-2) });
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "mark@deviq.com", Message = "Hi again!", DateTimeCreated = DateTime.UtcNow.AddHours(-1) });
                newGuestbook.Entries.Add(new GuestbookEntry { EmailAddress = "michelle@deviq.com", Message = "Hello!" });
                _repository.Add(newGuestbook);
            }

            var guestbook = _repository.GetById<Guestbook>(1); // hardcoded for this sample; could support multi-tenancy in real app
            List<GuestbookEntry> guestbookEntries = _repository.List<GuestbookEntry>();
            guestbook.Entries.Clear(); // remove in-memory Guestbook Entries
            guestbook.Entries.AddRange(guestbookEntries); // Fetch Entries from database.
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
                var guestbook = _repository.GetById<Guestbook>(1);
                List<GuestbookEntry> guestbookEntries = _repository.List<GuestbookEntry>();
                guestbook.Entries.Clear();
                guestbook.Entries.AddRange(guestbookEntries); // maintain existing Guestbook Entries

                // notify all previous entries
                foreach (var entry in guestbook.Entries)
                {
                    _messageSender.SendGuestbookNotificationEmail(entry.EmailAddress, model.NewEntry.Message);
                }

                guestbook.Entries.Add(model.NewEntry);
                _repository.Update(guestbook);

                model.PreviousEntries.Clear();
                model.PreviousEntries.AddRange(guestbook.Entries);
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
