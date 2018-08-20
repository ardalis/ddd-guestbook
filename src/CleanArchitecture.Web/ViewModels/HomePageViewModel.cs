using CleanArchitecture.Web.ApiModels;
using System.Collections.Generic;

namespace CleanArchitecture.Web.ViewModels
{
    public class HomePageViewModel
    {
        public string GuestbookName { get; set; }
        public List<GuestbookEntryDTO> PreviousEntries { get; set; } = new List<GuestbookEntryDTO>();
        public GuestbookEntryDTO NewEntry { get; set; }
    }
}
