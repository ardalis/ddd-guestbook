using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Filters
{
    public class VerifyGuestbookExistsAttribute : TypeFilterAttribute
    {
        public VerifyGuestbookExistsAttribute() : base(typeof(VerifyGuestbookExistsFilter))
        {
        }

        private class VerifyGuestbookExistsFilter : IAsyncActionFilter
        {
            private readonly IRepository<Guestbook> _guestbookRepository;

            public VerifyGuestbookExistsFilter(IRepository<Guestbook> guestbookRepository)
            {
                _guestbookRepository = guestbookRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if ((_guestbookRepository.GetById(id.Value)) == null)
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
