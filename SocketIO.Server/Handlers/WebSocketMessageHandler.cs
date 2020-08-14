using SocketIO.PackageManager;
using SocketIO.Server.SocketManager;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SocketIO.Server.Handlers
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connection) : base(connection) { }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var package = new SocketPackage()
            {
                SocketOriginID = Connections.GetId(socket),
                Message = "connected"
            };
            await SendMessageToAll(package.ToString());
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var package = new SocketPackage()
            {
                SocketOriginID = Connections.GetId(socket),
                Message = "disconnected"
            };
            await SendMessageToAll(package.ToString());
            await base.OnDisconnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var package = SocketPackageManager.DeserializePackage(buffer, result.Count);
            package.SocketOriginID = Connections.GetId(socket);
            
            if (string.IsNullOrWhiteSpace(package.SocketTargetID))
                await SendMessageToAll(package.ToString());
            else
                await SendMessage(package.SocketTargetID, package.ToString());
        }
    }
}
