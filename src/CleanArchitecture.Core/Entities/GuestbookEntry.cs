using CleanArchitecture.Core.SharedKernel;
using System;

namespace CleanArchitecture.Core.Entities
{

    public class GuestbookEntry : BaseEntity
    {
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTimeOffset DateTimeCreated { get; set; } = DateTime.UtcNow;
    }

}