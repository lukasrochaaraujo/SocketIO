using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketIO.Server.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager Connections { get; set; }

        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => Connections.AddSocket(socket));
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Connections.RemoveSocketAsync(Connections.GetId(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessage(string id, string message)
        {
            await SendMessage(Connections.GetSocketById(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var conn in Connections.GetAllConnctions())
                await SendMessage(conn.Value, message);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
