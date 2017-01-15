using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Web.Filters
{
    public class ValidateGuestbookExistsAttribute : TypeFilterAttribute
    {
        public ValidateGuestbookExistsAttribute() : base(typeof(ValidateGuestbookExistsFilterImpl))
        {
        }

        private class ValidateGuestbookExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IGuestbookRepository _guestbookRepository;

            public ValidateGuestbookExistsFilterImpl(IGuestbookRepository guestbookRepository)
            {
                _guestbookRepository = guestbookRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, 
                ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if (_guestbookRepository.GetById(id.Value) == null)
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
