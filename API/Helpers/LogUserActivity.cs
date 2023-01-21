using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Esperar hasta que se ejecute la action...
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var uof = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>(); //Obtiene el uofsitorio de user...
            var user = await uof.UserRepository.GetUserById(userId);
            user.LastActive = DateTime.Now;
            await uof.Complete();
        }
    }
}