using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Data
{
    public class GuestbookRepository : EfRepository<Guestbook>, IGuestbookRepository
    {
        public GuestbookRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override Guestbook GetById(int id)
        {
            return _dbContext.Guestbooks
                .Include(g => g.Entries)
                .FirstOrDefault(g => g.Id == id);
        }

        public List<GuestbookEntry> ListEntries(ISpecification<GuestbookEntry> spec)
        {
            return _dbContext.GuestbookEntries
                .Where(spec.Criteria)
                .ToList();
        }
    }
}