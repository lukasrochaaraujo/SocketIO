using Newtonsoft.Json;
using System.Collections.Generic;

namespace SocketIO.WindowsService.DeviceInfo
{
    public class DeviceData
    {
        public string HostName { get; set; }
        public string HostIP { get; set; }
        public string WindowsVersion { get; set; }
        public string DotNetVersion { get; set; }
        public ICollection<DiskDriver> DiskDrivers { get; set; }
        public ICollection<Antivirus> Antivirus { get; set; }
        public ICollection<Firewall> Firewall { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
