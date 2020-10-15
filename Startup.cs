using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints => {

                endpoints.MapGet("/cookie", async context => {
                    int counter1 =
                        int.Parse(context.Request.Cookies["counter1"] ?? "0") + 1;
                    context.Response.Cookies.Append("counter1", counter1.ToString(),
                        new CookieOptions
                        {
                            MaxAge = TimeSpan.FromMinutes(30)
                        });
                    int counter2 =
                        int.Parse(context.Request.Cookies["counter2"] ?? "0") + 1;
                    context.Response.Cookies.Append("counter2", counter1.ToString(),
                        new CookieOptions
                        {
                            MaxAge = TimeSpan.FromMinutes(30)
                        });
                    await context.Response
                        .WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");
                });

                endpoints.MapGet("clear", context => {
                    context.Response.Cookies.Delete("counter1");
                    context.Response.Cookies.Delete("counter2");
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                });

                endpoints.MapFallback(async context =>
                    await context.Response.WriteAsync("Hello World!"));
            });
        }
    }
}
