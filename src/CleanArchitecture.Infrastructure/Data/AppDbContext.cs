using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CleanArchitecture.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Guestbook> Guestbooks { get; set; }
        public DbSet<GuestbookEntry> GuestbookEntries { get; set; }

        public override int SaveChanges()
        {
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();
            var result = base.SaveChanges();
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var navigation = modelBuilder.Entity<Guestbook>()
                .Metadata.FindNavigation(nameof(Guestbook.Entries));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}