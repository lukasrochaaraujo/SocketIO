using SocketIO.PackageManager;
using Xunit;

namespace SocketIO.Test
{
    public class UnitTest1
    {
        [Fact]
        public void DeserializePackageTest()
        {
            string packageString = "originDI8CDIC8DCI8@targetIK324IK3RK45IK@messageHI";
            var package = SocketPackageManager.DeserializePackage(packageString);
            Assert.Equal("DI8CDIC8DCI8", package.SocketOriginID);
            Assert.Equal("IK324IK3RK45IK", package.SocketTargetID);
            Assert.Equal("HI", package.Message);
        }
    }
}
