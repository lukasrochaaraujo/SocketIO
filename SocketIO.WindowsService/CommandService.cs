using SocketIO.WindowsService.Logger;
using System.Diagnostics;

namespace SocketIO.WindowsService
{
    public static class CommandService
    {
        private const string CMD_TIP = "cmd";
        private const string PS_TIP = "ps";
        private const string LOG_TIP = "log";

        public static bool IsACommand(string command)
        {
            return !string.IsNullOrWhiteSpace(command) &&
                    command.StartsWith(CMD_TIP) ||
                    command.StartsWith(PS_TIP) ||
                    command.StartsWith(LOG_TIP);
        }

        public static bool IsALogReaderCommand(string command)
        {
            return !string.IsNullOrWhiteSpace(command) &&
                    command.StartsWith(LOG_TIP);
        }

        public static string ExecuteCommand(string command)
        {
            if (command.StartsWith(CMD_TIP))
                return ExecuteCMDCommand(command.Split(CMD_TIP)[1]);

            if (command.StartsWith(PS_TIP))
                return ExecutepowerShellCommand(command.Split(PS_TIP)[1]);

            if (command.StartsWith(LOG_TIP))
                return LoggerService.Read();

            return "Unrecognized command (use cmd [commands] or ps [commands])";
        }

        private static string ExecuteCMDCommand(string args)
        {
            var cmd = CreateProcess("cmd.exe");
            cmd.StartInfo.Arguments = "/C" + args;
            cmd.Start();

            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();

            return output;
        }

        private static string ExecutepowerShellCommand(string args)
        {
            var ps = CreateProcess("powershell.exe");
            ps.StartInfo.Arguments = args;
            ps.Start();

            string output = ps.StandardOutput.ReadToEnd();
            ps.WaitForExit();

            return output;
        }

        private static Process CreateProcess(string executable)
        {
            return new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    FileName = executable
                }
            };
        }
    }
}
