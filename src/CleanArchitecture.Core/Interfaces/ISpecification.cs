using CleanArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}