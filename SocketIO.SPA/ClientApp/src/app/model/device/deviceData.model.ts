import { DiskDriver } from './diskDriver.model';
import { Antivirus } from './antivirus.model';
import { Firewall } from './firewall.model';

export class DeviceData {
    ID: string;
    HostName: string;
    HostIP: string;
    WindowsVersion: string;
    DotNetVersion: string;
    DiskDrivers: DiskDriver[];
    Antivirus: Antivirus[];
    Firewall: Firewall[];
    
    DiskDriversFormatted: string;
    AntivirusFormatted: string;
    FirewallFormatted: string;

    Serialize() : string {
        return JSON.stringify(this);
    }
}