using System;
using Topshelf;

namespace SocketIO.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(h =>
            {
                h.Service<WorkerService>(s =>
                {
                    s.ConstructUsing(serviceWorker => new WorkerService());
                    s.WhenStarted(async serviceWorker => await serviceWorker.StartAsync());
                    s.WhenStopped(async serviceWorker => await serviceWorker.StopAsync());
                });

                h.RunAsLocalService();
                h.SetServiceName("SocketIOService");
                h.SetDisplayName("SocketIO Service");
                h.SetDescription("SocketIO service to monitoring and execute remote commands");
            });

            int exitCodevalue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodevalue;
        }
    }
}
