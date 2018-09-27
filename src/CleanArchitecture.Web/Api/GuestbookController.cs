using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    [VerifyGuestbookExists]
    public class GuestbookController : Controller
    {
        private readonly IRepository _repository;

        public GuestbookController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var guestbook = _repository.GetById<Guestbook>(id);
            var entries = _repository.List<GuestbookEntry>();
            guestbook.Entries.Clear();
            guestbook.Entries.AddRange(entries);
            return Ok(guestbook);
        }

        [HttpPost("{id:int}/NewEntry")]
        public IActionResult NewEntry(int id, [FromBody] GuestbookEntry entry)
        {
            var guestbook = _repository.GetById<Guestbook>(id);
            var entries = _repository.List<GuestbookEntry>();
            guestbook.Entries.Clear();
            guestbook.Entries.AddRange(entries);
            guestbook.AddEntry(entry);
            _repository.Update(guestbook);

            return Ok(guestbook);
        }
    }
}
