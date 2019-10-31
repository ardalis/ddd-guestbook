using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    public class GuestbookController : BaseApiController
    {
        private readonly IRepository _repository;

        public GuestbookController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Guestbook/1
        [HttpGet("{id:int}")]
        [VerifyGuestbookExists]
        public IActionResult GetById(int id)
        {
            var guestbook = _repository.GetById<Guestbook>(id, "Entries");
            return Ok(guestbook);
        }

        [HttpPost("{id:int}/NewEntry")]
        [VerifyGuestbookExists]
        public IActionResult NewEntry(int id, [FromBody] GuestbookEntry entry)
        {
            var guestbook = _repository.GetById<Guestbook>(id, "Entries");

            guestbook.AddEntry(entry);
            _repository.Update(guestbook);

            return Ok(guestbook);
        }
    }
}