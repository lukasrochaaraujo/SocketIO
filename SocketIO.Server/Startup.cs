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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
            services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyOrigin()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            app.UseWebSockets().UseCors();
            app.MapSockets("/ws", provider.GetService<WebSocketMessageHandler>());
            app.UseStaticFiles();
        }
    }
}
