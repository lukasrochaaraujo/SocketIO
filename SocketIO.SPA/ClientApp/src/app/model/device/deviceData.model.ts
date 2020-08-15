export class DeviceData {
    HostName: string;
    HostIP: string;
    WindowsVersion: string;
    DotNetVersion: string;
    DiskDrivers: string;
    Antivirus: string;
    Firewall: string;

    public Serialize() : string {
        return JSON.stringify(this);
    }
}