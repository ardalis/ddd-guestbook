using System;

namespace CleanArchitecture.Web.ApiModels
{
    public class GuestbookEntryDTO
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTimeOffset DateTimeCreated { get; set; }
    }
}