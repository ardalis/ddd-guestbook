using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Data
{
    public class GuestbookRepository : EfRepository<Guestbook>
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
    }
}