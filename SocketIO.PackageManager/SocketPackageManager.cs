using System.Text;

namespace SocketIO.PackageManager
{
    public static class SocketPackageManager
    {
        public static byte[] SerializePackage(SocketPackage package)
        {
            var packageSerialized = new StringBuilder();
            packageSerialized.Append($"origin{package.SocketOriginID}");
            packageSerialized.Append($"@target{package.SocketTargetID}");
            packageSerialized.Append($"@message{package.Message}");

            return Encoding.UTF8.GetBytes(packageSerialized.ToString());
        }

        public static SocketPackage DeserializePackage(byte[] package, int size)
        {
            string packageString = Encoding.UTF8.GetString(package, 0, size);
            return DeserializePackage(packageString);
        }

        public static SocketPackage DeserializePackage(string package)
        {
            string[] arrayPack = package.Split('@');

            if (arrayPack.Length != 3)
                arrayPack = new string[3] { "?", "?", "?" };

            return new SocketPackage()
            {
                SocketOriginID = arrayPack[0].Replace("origin", ""),
                SocketTargetID = arrayPack[1].Replace("target", ""),
                Message = arrayPack[2].Replace("message", "")
            };
        }
    }
}
