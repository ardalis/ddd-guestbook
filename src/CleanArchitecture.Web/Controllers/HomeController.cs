using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            if (!_repository.List<Guestbook>().Any())
            {
                var newGuestbook = new Guestbook { Name = "My Guestbook" };
                newGuestbook.AddEntry(new GuestbookEntry { EmailAddress = "steve@deviq.com", Message = "Hi!", DateTimeCreated = DateTime.UtcNow.AddHours(-2) });
                newGuestbook.AddEntry(new GuestbookEntry { EmailAddress = "mark@deviq.com", Message = "Hi again!", DateTimeCreated = DateTime.UtcNow.AddHours(-1) });
                newGuestbook.AddEntry(new GuestbookEntry { EmailAddress = "michelle@deviq.com", Message = "Hello!" });
                newGuestbook.Events.Clear();
                _repository.Add(newGuestbook);
            }

            var guestbook = _repository.GetById<Guestbook>(1); // hardcoded for this sample; could support multi-tenancy in real app
            var viewModel = new HomePageViewModel();
            viewModel.GuestbookName = guestbook.Name;
            viewModel.PreviousEntries.AddRange(guestbook.Entries
                .Select(e => new GuestbookEntryDTO
                {
                    DateTimeCreated = e.DateTimeCreated,
                    EmailAddress = e.EmailAddress,
                    Id = e.Id,
                    Message = e.Message
                }));
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guestbook = _repository.GetById<Guestbook>(1);

                guestbook.AddEntry(new GuestbookEntry
                {
                    DateTimeCreated = model.NewEntry.DateTimeCreated,
                    EmailAddress = model.NewEntry.EmailAddress,
                    Id = model.NewEntry.Id,
                    Message = model.NewEntry.Message
                });
                _repository.Update(guestbook);

                model.PreviousEntries.Clear();
                model.PreviousEntries.AddRange(guestbook.Entries
                    .Select(e => new GuestbookEntryDTO
                    {
                        DateTimeCreated = e.DateTimeCreated,
                        EmailAddress = e.EmailAddress,
                        Id = e.Id,
                        Message = e.Message
                    }));
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
