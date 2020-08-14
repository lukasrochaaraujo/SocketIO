using System.Text;

namespace SocketIO.PackageManager
{
    public class SocketPackage
    {
        public string SocketOriginID { get; set; }

        public string SocketTargetID { get; set; }

        public string Message { get; set; }

        public void ReverseOrigins()
        {
            string targetIdTemp = SocketTargetID;
            SocketTargetID = SocketOriginID;
            SocketOriginID = targetIdTemp;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }

        public override string ToString()
        {
            return $"origin{SocketOriginID}@target{SocketTargetID}@message{Message}";
        }
    }
}
