using App.Application;
using App.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanApp.API.Filters
{
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        { //action metotlar çalışmadan önce burası çalışacak

            var idValue = context.ActionArguments.Values.FirstOrDefault();

            var idKey = context.ActionArguments.Keys.FirstOrDefault();

            if (idValue == null && idKey != "id")
            {
                await next();
                return;
            }
            if (idValue is not TId id)
            {
                await next();
                return;
            }

            if (await genericRepository.AnyAsync(id))
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;

            // action method name
            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ServiceResult.Fail($"Data bulunamamıştır. ({entityName}) ({actionName})");

            context.Result = new NotFoundObjectResult(result);
        }
    }
}
