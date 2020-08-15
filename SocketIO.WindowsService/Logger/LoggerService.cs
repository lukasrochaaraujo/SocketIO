using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SocketIO.WindowsService.Logger
{
    public static class LoggerService
    {
        private static readonly string LogPath = AppDomain.CurrentDomain.BaseDirectory + "commands.log";

        public static void Log(LogCommand command)
        {
            File.AppendAllText(LogPath, $"{DateTimeOffset.Now}: {JsonConvert.SerializeObject(command)}\r\n");
        }

        public static List<LogCommand> Read()
        {
            string logString = File.ReadAllText(LogPath);
            return JsonConvert.DeserializeObject<List<LogCommand>>(logString);
        }
    }
}
