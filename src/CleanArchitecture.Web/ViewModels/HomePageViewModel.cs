using System.Collections.Generic;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Web.ViewModels
{
    public class HomePageViewModel
    {
        public string GuestbookName { get; set; }
        public List<GuestbookEntry> PreviousEntries { get; } = new List<GuestbookEntry>();

        public GuestbookEntry NewEntry { get; set; }
    }
}