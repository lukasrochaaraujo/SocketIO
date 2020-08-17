using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SocketIO.Service.DeviceInfo
{
    public static class DeviceCollector
    {
        public static DeviceData CollectData()
        {
            return new DeviceData()
            {
                HostName = Environment.MachineName,
                HostIP = CollectLocalIPv4(),
                WindowsVersion = Environment.OSVersion.VersionString,
                DotNetVersion = Environment.Version.ToString(),
                DiskDrivers = CollectDisks(),
                Antivirus = GetAntivirusInstalled(),
                Firewall = GetFirewallStatus()
            };
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

        private static List<DiskDriver> CollectDisks()
        {
            return DriveInfo.GetDrives()
                            .Where(d => d.IsReady)
                            .Select(d => new DiskDriver
                            {
                                Letter = d.Name,
                                TotalSpace = d.TotalSize,
                                TotalSpaceFree = d.TotalFreeSpace
                            })
                            .ToList();
        }

        private static List<Antivirus> GetAntivirusInstalled()
        {
            var antivirusList = new List<Antivirus>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
                ManagementObjectCollection data = wmiData.Get();

                foreach (ManagementObject virusChecker in data)
                    antivirusList.Add(new Antivirus() { Name = virusChecker["displayName"].ToString() });
            }

            return antivirusList;
        }

        private static List<Firewall> GetFirewallStatus()
        {
            var listFirewall = new List<Firewall>();
            string outputCommand = CommandService.ExecuteCommand("ps Get-NetFirewallProfile");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string[] outputCommandSplited = outputCommand.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                                                         .Where(l => !string.IsNullOrWhiteSpace(l))
                                                         .Where(l => l.StartsWith("Name") || l.StartsWith("Enabled"))
                                                         .ToArray();

                for (int i = 0; i < 5; i += 2)
                    listFirewall.Add(new Firewall()
                    {
                        Name = outputCommandSplited[i].Split(':')[1].Trim(),
                        Status = bool.TryParse(outputCommandSplited[i + 1].Split(':')[1].Trim(), out bool isEnable) ? "Enable" : "Disable"
                    });
            }

            return listFirewall;
        }
    }
}
