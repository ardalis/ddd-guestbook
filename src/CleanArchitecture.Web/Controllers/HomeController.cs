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
        private readonly IGuestbookRepository _guestbookRepository;
        private readonly IMessageSender _messageSender;
        private readonly IGuestbookService _guestbookService;

        public HomeController(IGuestbookRepository guestbookRepository,
            IMessageSender messageSender,
            IGuestbookService guestbookService)
        {
            _guestbookRepository = guestbookRepository;
            _messageSender = messageSender;
            _guestbookService = guestbookService;
        }

        public IActionResult Index()
        {
            InitializeData();

            var guestbook = _guestbookRepository.GetById(1);
            var viewModel = new HomePageViewModel();
            viewModel.GuestbookName = guestbook.Name;
            viewModel.PreviousEntries.AddRange(guestbook.Entries);

            return View(viewModel);
        }

        private void InitializeData()
        {
            if (!_guestbookRepository.List().Any())
            {
                var newGuestbook = new Guestbook() { Name = "My Guestbook" };
                newGuestbook.Entries.Add(new GuestbookEntry()
                {
                    EmailAddress = "steve@deviq.com",
                    Message = "Hi!"
                });
                _guestbookRepository.Add(newGuestbook);
            }
        }

        [HttpPost]
        public IActionResult Index(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guestbook = _guestbookRepository.GetById(1);
                guestbook.AddEntry(model.NewEntry);
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
