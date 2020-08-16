using SocketIO.Service.DeviceInfo;
using SocketIO.Service.Logger;
using System;
using System.IO;
using Xunit;

namespace SocketIO.Test
{
    public class ServiceTest
    {
        [Fact]
        public void CollectDataTest()
        {
            var deviceInfo = DeviceCollector.CollectData();
            Assert.Equal(deviceInfo.HostName, Environment.MachineName);
            Assert.Equal(deviceInfo.WindowsVersion, Environment.OSVersion.VersionString);
            Assert.Equal(deviceInfo.DotNetVersion, Environment.Version.ToString());
        }

        [Fact]
        public void LoggerTest()
        {
            File.Delete(LoggerService.LogPath);
            var firstLog = new LogCommand()
            {
                Command = "firstLog",
                Output = "firstLog"
            };
            var secondLog = new LogCommand()
            {
                Command = "secondLog",
                Output = "secondLog"
            };
            var thirdLog = new LogCommand()
            {
                Command = "thirdLog",
                Output = "thirdLog"
            };
            LoggerService.Log(firstLog);
            LoggerService.Log(secondLog);
            LoggerService.Log(thirdLog);
            Assert.True(File.Exists(LoggerService.LogPath));
            var firstLogString = LoggerService.ReadFirst();
            var thirdLogString = LoggerService.ReadLast();
            Assert.Equal(firstLogString, firstLog.ToString());
            Assert.Equal(thirdLogString, thirdLog.ToString());
        }
    }
}