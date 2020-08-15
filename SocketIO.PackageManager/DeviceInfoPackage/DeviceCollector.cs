using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SocketIO.PackageManager.DeviceInfoPackage
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
                Firewall = GetFirewallInstalled()
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

            var wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();

            foreach (ManagementObject virusChecker in data)
                antivirusList.Add(new Antivirus() { Name = virusChecker["displayName"].ToString() });

            return antivirusList;
        }

        private static List<Firewall> GetFirewallInstalled()
        {
            var firewallList = new List<Firewall>();

            var wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM FirewallProduct");
            ManagementObjectCollection data = wmiData.Get();

            foreach (ManagementObject WmiObject in data)
                firewallList.Add(new Firewall() { Name = WmiObject["displayName"].ToString(), Status = WmiObject["enabled"].ToString() });

            return firewallList;
        }
    }
}
