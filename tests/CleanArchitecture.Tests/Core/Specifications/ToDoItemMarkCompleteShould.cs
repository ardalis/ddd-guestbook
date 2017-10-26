using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CleanArchitecture.Tests.Core.Entities
{
    public class GuestbookNotificationPolicyCriteriaShould
    {
        private List<GuestbookEntry> TestEntries()
        {
            var entries = new List<GuestbookEntry>();
            entries.Add(new GuestbookEntry { Id = 1, DateTimeCreated = DateTime.UtcNow, EmailAddress = "test1@test.com", Message = "1" });
            entries.Add(new GuestbookEntry { Id = 2, DateTimeCreated = DateTime.UtcNow.AddHours(-2), EmailAddress = "test2@test.com", Message = "2" });
            entries.Add(new GuestbookEntry { Id = 3, DateTimeCreated = DateTime.UtcNow.AddHours(-20), EmailAddress = "test3@test.com", Message = "3" });
            entries.Add(new GuestbookEntry { Id = 4, DateTimeCreated = DateTime.UtcNow.AddDays(-1).AddSeconds(-1), EmailAddress = "test4@test.com", Message = "4" });
            return entries;
        }
        [Fact]
        public void NotIncludeEntryTriggeringNotification()
        {
            var entries = TestEntries();
            var spec = new GuestbookNotificationPolicy(1);

            var entriesToNotify = entries.Where(spec.Criteria.Compile());

            Assert.DoesNotContain(entriesToNotify, e => e.Id == 1);
        }

        [Fact]
        public void NotIncludeEntriesOver1DayOld()
        {
            var entries = TestEntries();
            var spec = new GuestbookNotificationPolicy(1);

            var entriesToNotify = entries.Where(spec.Criteria.Compile());

            Assert.DoesNotContain(entriesToNotify, e => e.Id == 4);
        }

        [Fact]
        public void IncludeEntriesFromLast24Hours()
        {
            var entries = TestEntries();
            var spec = new GuestbookNotificationPolicy(1);

            var entriesToNotify = entries.Where(spec.Criteria.Compile());

            Assert.NotNull(entriesToNotify.SingleOrDefault(e => e.Id == 2));
            Assert.NotNull(entriesToNotify.SingleOrDefault(e => e.Id == 3));
        }
    }
}