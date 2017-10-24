using CleanArchitecture.Core.Entities;
using CleanArchitecture.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CleanArchitecture.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var guestbook = new Guestbook() { Name = "My Guestbook" };
            guestbook.Entries.Add(new GuestbookEntry { EmailAddress = "steve@deviq.com", Message = "Hi!", DateTimeCreated = DateTime.UtcNow.AddHours(-2) });
            guestbook.Entries.Add(new GuestbookEntry { EmailAddress = "mark@deviq.com", Message = "Hi again!", DateTimeCreated = DateTime.UtcNow.AddHours(-1) });
            guestbook.Entries.Add(new GuestbookEntry { EmailAddress = "michelle@deviq.com", Message = "Hello!" });

            var viewModel = new HomePageViewModel();
            viewModel.GuestbookName = guestbook.Name;
            viewModel.PreviousEntries.AddRange(guestbook.Entries);
            return View(viewModel);
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
