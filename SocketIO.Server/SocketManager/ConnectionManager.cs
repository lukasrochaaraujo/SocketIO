using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketIO.Server.SocketManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _Connections = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _Connections.FirstOrDefault(c => c.Key.Equals(id)).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnctions()
        {
            return _Connections;
        }

        public string GetId(WebSocket socket)
        {
            return _Connections.FirstOrDefault(c => c.Value.Equals(socket)).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _Connections.TryRemove(id, out var socket);
            await socket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }

        public string AddSocket(WebSocket socket)
        {
            string id = GetConnectionId();
            _Connections.TryAdd(id, socket);
            return id;
        }

        public string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
