using CleanArchitecture.Core.Entities;
using System.Collections.Generic;

namespace CleanArchitecture.Core.Interfaces
{

    public interface IGuestbookRepository : IRepository<Guestbook>
    {
        List<GuestbookEntry> ListEntries(ISpecification<GuestbookEntry> spec);
    }
}