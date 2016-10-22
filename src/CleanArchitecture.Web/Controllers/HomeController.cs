using System;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using MimeKit;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Guestbook> _guestbookRepository;
        private readonly IMessageSender _messageSender;

        public HomeController(IRepository<Guestbook> guestbookRepository,
            IMessageSender messageSender)
        {
            _guestbookRepository = guestbookRepository;
            _messageSender = messageSender;
        }

        public IActionResult Index()
        {
            if (!_guestbookRepository.List().Any())
            {
                var newGuestbook = new Guestbook() {Name = "My Guestbook"};
                newGuestbook.Entries.Add(new GuestbookEntry()
                {
                    EmailAddress = "steve@deviq.com",
                    Message = "Hi!" });
                _guestbookRepository.Add(newGuestbook);
            }

            //var guestbook = _guestbookRepository.List().FirstOrDefault();
            var guestbook = _guestbookRepository.GetById(1);
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
                guestbook.Entries.Add(model.NewEntry);
                _guestbookRepository.Update(guestbook);

                // send updates to previous entries made within last day
                var emailsToNotify = guestbook.Entries
                    .Where(e => e.DateTimeCreated > DateTimeOffset.UtcNow.AddDays(-1))
                    .Select(e => e.EmailAddress);
                foreach (var emailAddress in emailsToNotify)
                {
                    string messageBody = "{model.NewEntry.EmailAddress} left new message {model.NewEntry.Message}";
                    _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
                }

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
