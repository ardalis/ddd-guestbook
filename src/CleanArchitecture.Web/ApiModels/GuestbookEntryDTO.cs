using System;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Web.ApiModels
{
    public class GuestbookEntryDTO
    {
        public int Id { get; set; }
        public int GuestbookId { get; set; }
        public string Message { get; set; }
        public string EmailAddress { get; set; }
        public DateTimeOffset DateTimeCreated { get; set; } = DateTime.UtcNow;

        public GuestbookEntry ToEntry()
        {
            return new GuestbookEntry()
            {
                GuestbookId = this.GuestbookId,
                EmailAddress = this.EmailAddress,
                Message = this.Message
            };
        }

        public static GuestbookEntryDTO FromEntry(GuestbookEntry entry)
        {
            return new GuestbookEntryDTO()
            {
                DateTimeCreated = entry.DateTimeCreated,
                EmailAddress = entry.EmailAddress,
                GuestbookId = entry.GuestbookId,
                Id = entry.Id,
                Message = entry.Message
            };
        }

    }
}