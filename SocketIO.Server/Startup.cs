using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocketIO.Server.Handlers;
using SocketIO.Server.SocketManager;

namespace SocketIO.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            app.UseWebSockets();
            app.MapSockets("/ws", provider.GetService<WebSocketMessageHandler>());
            app.UseStaticFiles();
        }
    }
}
