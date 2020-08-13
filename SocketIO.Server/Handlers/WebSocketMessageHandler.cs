using SocketIO.Server.SocketManager;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketIO.Server.Handlers
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connection) : base(connection) { }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Connections.GetId(socket);
            await SendMessageToAll($"{socketId} connected");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = Connections.GetId(socket);
            await SendMessageToAll($"{socketId} disconnected");
            await base.OnDisconnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = Connections.GetId(socket);
            await SendMessageToAll($"{socketId}: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
        }
    }
}
