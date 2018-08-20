using System.Collections.Generic;

namespace CleanArchitecture.Web.ApiModels
{
    public class GuestbookDTO
    {
        public int Id { get; set; }
        public List<GuestbookEntryDTO> Entries { get; set; } = new List<GuestbookEntryDTO>();
    }
}
