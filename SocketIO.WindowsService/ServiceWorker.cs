using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SocketIO.PackageManager;

namespace SocketIO.WindowsService
{
    public class ServiceWorker : BackgroundService
    {
        private static ClientWebSocket Socket;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await StartWebsockets();
            }
        }

        public async Task StartWebsockets()
        {
            Socket = new ClientWebSocket();
            await Socket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            var package = new SocketPackage()
            {
                Message = DeviceInfo.CollectData()
            };
            await Socket.SendAsync(new ArraySegment<byte>(package.ToBytes()), WebSocketMessageType.Text, true, CancellationToken.None);
            var receive = ReceiveAsync(Socket);
            await Task.WhenAll(receive);
        }

        public async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var package = SocketPackageManager.DeserializePackage(buffer, result.Count);

                if (CommandService.IsACommand(package.Message))
                {
                    package.ReverseOrigins();
                    package.Message = CommandService.ExecuteCommand(package.Message);
                    await Socket.SendAsync(new ArraySegment<byte>(package.ToBytes()), WebSocketMessageType.Text, true, CancellationToken.None);
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }
    }
}
