using System;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Specifications;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Data
{
    public class GuestbookRepositoryListWithEntriesSpecShould : EfRepositoryTestBase
    {
        // [Fact] -- Ignoring for now; need to test against real db
        public void NotIncludeRelatedEntriesWithoutSpec()
        {
            // add data
            var repository = GetRepository<Guestbook>();
            string name = Guid.NewGuid().ToString();
            var guestbook = new Guestbook() { Name = name };
            repository.Add(guestbook);
            guestbook.AddEntry(new GuestbookEntry() { EmailAddress = "test@test.com", Message = "test 1" });
            repository.Update(guestbook);

            var result = repository.List().FirstOrDefault(g => g.Name == name);

            Assert.NotNull(result);
            Assert.Equal(0, result.Entries.Count());
        }

        [Fact]
        public void IncludeRelatedEntries()
        {
            // add data
            var repository = GetRepository<Guestbook>();
            string name = Guid.NewGuid().ToString();
            var guestbook = new Guestbook() { Name=name};
            repository.Add(guestbook);
            guestbook.AddEntry(new GuestbookEntry() {EmailAddress="test@test.com", Message="test 1"});
            repository.Update(guestbook);
                        
            var spec = new GuestbookWithEntriesSpec();

            var result = repository.List(spec).FirstOrDefault(g => g.Name == name);

            Assert.NotNull(result);
            Assert.Equal(1, result.Entries.Count());
        }
    }
}