using System;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}