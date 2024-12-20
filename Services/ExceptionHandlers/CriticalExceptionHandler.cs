using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Services.ExceptionHandlers
{
    public class CriticalExceptionHandler() : IExceptionHandler
    {
        // amacı : geriye bir dto dönmek değil , böyle bir hata geldiğinde araya ek iş katmanı eklicez 
        // mesela mail atma , loglama , sms atma gibi işlemler yapılabilir
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //business logic yapacağız.
            if(exception is CriticalException)
            {
                //loglama
                //mail atma
                //sms atma
                //notification atma
                Console.WriteLine("Hata ile Sms Gönderildi");
                 // işlemi burada bitiriyoruz. return true
            }


            return ValueTask.FromResult(false); // bir sonraki exception handler'a geç diyoruz. false ise.
        }
    }
}
