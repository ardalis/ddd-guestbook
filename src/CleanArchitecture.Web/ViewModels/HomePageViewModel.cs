using CleanArchitecture.Web.ApiModels;
using System.Collections.Generic;

namespace CleanArchitecture.Web.ViewModels
{
    public class HomePageViewModel
    {
        public string GuestbookName { get; set; }
        public List<GuestbookEntryDTO> PreviousEntries { get; } = new List<GuestbookEntryDTO>();
        public GuestbookEntryDTO NewEntry { get; set; }
    }
}
