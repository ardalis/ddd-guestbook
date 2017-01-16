using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Web.ApiModels
{
    public class GuestbookDTO
    {
        public int Id { get; set; }
        public List<GuestbookEntryDTO> Entries { get; set; }


        public static GuestbookDTO FromGuestbook(Guestbook guestbook)
        {
            return new GuestbookDTO()
            {
                Id = guestbook.Id,
                Entries = guestbook.Entries.Select(e => GuestbookEntryDTO.FromEntry(e)).ToList()
            };
        }
    }
}