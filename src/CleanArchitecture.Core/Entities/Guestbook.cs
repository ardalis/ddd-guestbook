using System.Collections.Generic;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class Guestbook : BaseEntity
    {
        public string Name { get; set; }
        public List<GuestbookEntry> Entries { get; } = new List<GuestbookEntry>(); 
    }
}