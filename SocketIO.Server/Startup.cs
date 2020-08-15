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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            webSocketOptions.AllowedOrigins.Add("http://localhost");
            webSocketOptions.AllowedOrigins.Add("https://localhost");
            webSocketOptions.AllowedOrigins.Add("http://localhost:5000");
            webSocketOptions.AllowedOrigins.Add("https://localhost:5001");
            webSocketOptions.AllowedOrigins.Add("http://localhost:4200");

            app.UseWebSockets(webSocketOptions);
            app.MapSockets("/ws", provider.GetService<WebSocketMessageHandler>());
            app.UseStaticFiles();
        }
    }
}
