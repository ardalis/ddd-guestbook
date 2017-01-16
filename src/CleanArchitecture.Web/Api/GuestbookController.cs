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
        private readonly IGuestbookRepository _guestbookRepository;

        public GuestbookController(IGuestbookRepository guestbookRepository)
        {
            _guestbookRepository = guestbookRepository;
        }

        // GET: api/Guestbook/1
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var guestbook = _guestbookRepository.GetById(id);
            var dto = GuestbookDTO.FromGuestbook(guestbook);
            return Ok(dto);
        }

    // POST: api/Guestbook/NewEntry
    [HttpPost("{id:int}/NewEntry")]
    public async Task<IActionResult> NewEntry(int id, [FromBody] GuestbookEntryDTO entry)
    {
        var guestbook = _guestbookRepository.GetById(id);

        var newEntry = entry.ToEntry();
        guestbook.AddEntry(newEntry);
        _guestbookRepository.Update(guestbook);

        var dto = GuestbookDTO.FromGuestbook(guestbook); 
        return Ok(dto);
    }
    }
}
