using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class GuestbookController : Controller
    {
        private readonly IRepository<Guestbook> _guestbookRepository;

        public GuestbookController(IRepository<Guestbook> guestbookRepository)
        {
            _guestbookRepository = guestbookRepository;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var guestbook = _guestbookRepository.GetById(id);
            if (guestbook == null)
            {
                return NotFound(id);
            }
            return Ok(guestbook);
        }
    }
}
