# Lab 5 Specification

## Goal
Extract query logic used when sending notifications into a *Specification*.

## Topics Used
Specification

## Requirements

The Guestbook currently sends an email to the person who last left a message, in addition to others. This is a bug.

Pull the filtering logic into its own type where it can be reused and tested.

## Detailed Steps

- Extract the querying/filtering logic from *GuestbookNotificationHandler*
    - Put it into a new *GuestbookNotificationPolicy* class
- Create `ISpecification<T>` in Core/Interfaces
    - One method: `Expression<Func<T, bool>> Criteria { get; }`
- Have GuestbookNotificationPolicy implement `ISpecification<GuestbookEntry>` and implement the Criteria property
- Add a `List(Specification<T> spec)` method to `IRepository<T>`
- Implement the new List method in Infrastructure/EfRepository
    - **Note:** The `.Include()` method doesn't support filtering. The simplest approach is to make two trips to the database. Add a `DbSet<GuestbookEntry>` to AppDbContext if necessary.
- Write some unit tests for your new `GuestbookNotificationPolicy` to confirm it works as you expect

