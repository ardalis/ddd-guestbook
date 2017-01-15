using System;
using System.Linq.Expressions;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Core.Specifications
{
    public class GuestbookWithEntriesSpec : ISpecification<Guestbook>
    {
        public Expression<Func<Guestbook, bool>> Criteria
        {
            get { return e => true; }
        }

        public Expression<Func<Guestbook, object>> Include
        {
            get { return e => e.Entries; }
        }
    }
}