using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using data.collection.util.Extentions;
using ZR.DataHunter.Api;

namespace data.collection.api.Middlewares
{
    public class RequestBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            string body = await context.Request.Body?.ReadToStringAsync(Encoding.UTF8);
            context.RequestServices.GetService<RequestBody>().Body = body;

            await _next(context);
        }
    }
}
