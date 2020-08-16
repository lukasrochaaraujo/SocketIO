using SocketIO.Server.SocketManager;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocketIO.Test
{
    public class WebSocketGhost : WebSocket
    {
        public override WebSocketCloseStatus? CloseStatus => throw new NotImplementedException();

        public override string CloseStatusDescription => throw new NotImplementedException();

        public override WebSocketState State => throw new NotImplementedException();

        public override string SubProtocol => throw new NotImplementedException();

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class ServerTest
    {
        private ConnectionManager ConnectionManager;
        private string SocketID;

        public ServerTest()
        {
            ConnectionManager = new ConnectionManager();
        }

        [Fact]
        public void AddSocket()
        {
            var socket = new WebSocketGhost();
            SocketID = ConnectionManager.AddSocket(socket);
            var socketInserted = ConnectionManager.GetAllConnctions().First();
            Assert.True(ConnectionManager.GetAllConnctions().Count == 1);
            Assert.True(socketInserted.Key.Equals(SocketID));
        }

        [Fact]
        public void RemoveSocket()
        {
            ConnectionManager.RemoveSocketAsync(SocketID).GetAwaiter();
            Assert.True(ConnectionManager.GetAllConnctions().Count == 0);
        }
    }
}
