﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SocketIO.Server.SocketManager
{
    public static class SocketExtension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                    services.AddSingleton(type);
            }

            return services;
        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler handler)
        {
            return app.Map(path, a => a.UseMiddleware<SocketMiddleware>(handler));
        }
    }
}
