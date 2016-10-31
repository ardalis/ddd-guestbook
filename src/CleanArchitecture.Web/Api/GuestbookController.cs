using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web.ApiModels;
using CleanArchitecture.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    [ValidateGuestbookExists]
    public class GuestbookController : Controller
    {
        private readonly IRepository<Guestbook> _guestbookRepository;

        public GuestbookController(IRepository<Guestbook> guestbookRepository)
        {
            _guestbookRepository = guestbookRepository;
        }

        // GET: api/Guestbook/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var guestbook = _guestbookRepository.GetById(id);
            return Ok(guestbook);
        }

    // POST: api/Guestbook/NewEntry
    [HttpPost("{id:int}/NewEntry")]
    public async Task<IActionResult> NewEntry(int id, [FromBody] GuestbookEntry entry)
    {
        var guestbook = _guestbookRepository.GetById(id);
        guestbook.AddEntry(entry);
        _guestbookRepository.Update(guestbook);

        return Ok(guestbook);
    }
    }
}
