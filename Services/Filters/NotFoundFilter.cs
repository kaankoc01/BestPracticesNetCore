using App.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Filters
{
    public class NotFoundFilter<T,TId>(IGenericRepository<T,TId> genericRepository) :Attribute, IAsyncActionFilter where T : class where TId :struct
    {
     
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        { //action metotlar çalışmadan önce burası çalışacak

            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next();
                return;
            }

            

            var hasEntity = await genericRepository.AnyAsync((TId)id);




            await next();
            // action metotlar çalıştıktan sonra burası çalışacak



        }
    }
}
