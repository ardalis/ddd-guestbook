using System.Collections.Generic;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IGuestbookRepository : IRepository<Guestbook>
    {
        List<GuestbookEntry> ListEntries(ISpecification<GuestbookEntry> spec);
    }
}