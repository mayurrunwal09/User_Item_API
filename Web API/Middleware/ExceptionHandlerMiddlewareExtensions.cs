using System.Runtime.CompilerServices;

namespace Web_API.Middleware
{

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

    /* public static class ExceptionHandlerMiddlewareExtensions
     {
         // Extension method used to add the middleware to the HTTP request pipeline.
         public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
         {
             app.UseMiddleware<ExceptionHandlerMiddleware>();
         }
     }*/
}
