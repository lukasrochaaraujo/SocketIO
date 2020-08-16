using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SocketIO.Service.Logger
{
    public static class LoggerService
    {
        public static readonly string LogPath = AppDomain.CurrentDomain.BaseDirectory + "commands.log";

        public static void Log(LogCommand command)
        {
            command.Date = DateTime.Now;
            File.AppendAllText(LogPath, $"{JsonConvert.SerializeObject(command)},\r\n");
        }

        public static string ReadFirst()
        {
            return ReadAllAsList().FirstOrDefault()?.ToString() ?? "empty";
        }

        public static string ReadLast()
        {
            return ReadAllAsList().LastOrDefault()?.ToString() ?? "empty";
        }

        public static string ReadAll()
        {
            var log = new StringBuilder();

            ReadAllAsList().Select(l => l.ToString())
                           .ToList()
                           .ForEach(l => log.AppendLine(l));

            return log.ToString();
        }

        public static List<LogCommand> ReadAllAsList()
        {
            string logJson = ReadAsString();
            return JsonConvert.DeserializeObject<List<LogCommand>>($"[{logJson}]", new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddTHH:mm:ss" });
        }

        public static string ReadAsString()
        {
            return File.ReadAllText(LogPath);
        }
    }
}
