using System;

namespace SocketIO.Service.Logger
{
    public class LogCommand
    {
        public DateTime Date { get; set; }
        public string Command { get; set; }
        public string Output { get; set; }

        public override string ToString()
        {
            return $"{Date}: {Command} => {Output}";
        }
    }
}
