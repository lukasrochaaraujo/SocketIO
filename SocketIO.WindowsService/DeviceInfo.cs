using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace SocketIO.WindowsService
{
    public static class DeviceInfo
    {
        public static string CollectData()
        {
            var builder = new StringBuilder();
            builder.Append($"hostName={Environment.MachineName}");
            builder.Append($"&");
            builder.Append($"hostIp={CollectLocalIPv4()}");
            builder.Append($"&");
            builder.Append($"disks={CollectDisks()}");
            builder.Append($"&");
            builder.Append($"winver={Environment.OSVersion.VersionString}");
            builder.Append($"&");
            builder.Append($"netver={Environment.Version}");
            builder.Append($"&");
            builder.Append($"firewall={GetFirewallInstalled()}");
            builder.Append($"&");
            builder.Append($"antivirus={GetAntivirusInstalled()}");

            return builder.ToString();
        }

        private static string CollectLocalIPv4()
        {
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                                                   .FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                                                        c.OperationalStatus == OperationalStatus.Up);

            if (firstUpInterface != null)
            {
                var props = firstUpInterface.GetIPProperties();
                var firstIpV4Address = props.UnicastAddresses
                                            .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                                            .Select(c => c.Address)
                                            .FirstOrDefault();

                return firstIpV4Address?.ToString() ?? "UNDEFINED";
            }

            return "UNDEFINED";
        }

        private static string CollectDisks()
        {
            return "";

            //return DriveInfo.GetDrives()
            //                .Select(d => new
            //                {
            //                    d.Name,
            //                    d.TotalSize,
            //                    d.TotalFreeSpace
            //                })
            //                .ToArray()
            //                .ToString();
        }

        private static string GetFirewallInstalled()
        {
            //var firewallList = new List<Firewall>();

            //var wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM FirewallProduct");
            //ManagementObjectCollection data = wmiData.Get();

            //foreach (ManagementObject WmiObject in data)
            //    firewallList.Add(new Firewall() { Name = WmiObject["displayName"].ToString(), Status = WmiObject["enabled"].ToString() });

            //return firewallList;

            return "";
        }

        private static string GetAntivirusInstalled()
        {
            //var antivirusList = new List<Antivirus>();

            //var wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            //ManagementObjectCollection data = wmiData.Get();

            //foreach (ManagementObject virusChecker in data)
            //    antivirusList.Add(new Antivirus() { Name = virusChecker["displayName"].ToString() });

            //return antivirusList;

            return "";
        }
    }
}
