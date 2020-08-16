using SocketIO.PackageManager;
using SocketIO.Service.DeviceInfo;
using SocketIO.Service.Logger;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketIO.Service
{
    public class WorkerService
    {
        private static ClientWebSocket Socket;

        public async Task StartAsync()
        {
            await StartWebsocket();
        }

        public async Task StopAsync()
        {
            await Socket?.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
        }

        private async Task StartWebsocket()
        {
            Socket = new ClientWebSocket();
            var package = CollectDeviceData();

            TRY_RECONNECT:
            try
            {
                await Socket.ConnectAsync(new Uri("ws://localhost:5000/ws"), CancellationToken.None);
            }
            catch
            {
                Socket = new ClientWebSocket();
                await Task.Delay(5000);
                goto TRY_RECONNECT;
            }

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
                    var log = new LogCommand();
                    log.Command = package.Message;

                    package.ReverseOrigins();
                    package.Message = CommandService.ExecuteCommand(package.Message);

                    log.Output = !CommandService.IsALogReaderCommand(log.Command) ? package.Message : "log recover";
                    LoggerService.Log(log);
                }
                else if (package.Message.Contains("connected"))
                {
                    package = CollectDeviceData();
                }
                else
                {
                    package = null;
                }

                if (package != null)
                    await Socket.SendAsync(new ArraySegment<byte>(package.ToBytes()), WebSocketMessageType.Text, true, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
                }
            }
        }

        public SocketPackage CollectDeviceData()
        {
            return new SocketPackage() { Message = DeviceCollector.CollectData().Serialize() };
        }
    }
}
