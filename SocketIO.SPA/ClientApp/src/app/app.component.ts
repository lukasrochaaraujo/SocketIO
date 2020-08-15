import { Component, OnInit, ApplicationRef, OnChanges, SimpleChanges, Input, ChangeDetectorRef } from '@angular/core';
import { WebSocketService } from './web-socket.service';
import { SocketPackage } from './model/socketPackage.model';
import { DeviceData } from './model/device/deviceData.model';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	title = 'SocketIO SPA';
	DevicesConnected: DeviceData[];

	constructor(private webSocketService: WebSocketService) { }

	ngOnInit(): void {
		this.DevicesConnected = [];
		this.webSocketService.startSocket(this.onSocketOpenEvent, this.onSocketCloseEvent, this.onSocketMessageEvent.bind(this));
	}

	onSocketOpenEvent(data: any) {
		console.log(data);
	}

	onSocketCloseEvent(data: any) {
		console.log(data);
	}

	onSocketMessageEvent(socketPackage: SocketPackage) {
		if (socketPackage.DeviceData != null && socketPackage.Message == 'connected') {			
			this.addDevice(socketPackage.SocketOriginID, socketPackage.DeviceData);
		} else if (socketPackage.Message == 'disconnected') {
			this.DevicesConnected = this.DevicesConnected.filter(d => 
				d.ID != socketPackage.SocketOriginID);
		} else if (socketPackage.Message != 'connected') {
			let output = document.getElementById(socketPackage.SocketOriginID);
			output.textContent += socketPackage.Message;
		}
	}

	public sendMessage(to: string, command: string) {
		this.webSocketService.sendMessage(to, command);
	}

	addDevice(id: string, device: DeviceData) {
		device.ID = id;
		
		device.DiskDriversFormatted = device.DiskDrivers.length > 0 ? '' : '-';
		device.DiskDrivers.map(d => {
			device.DiskDriversFormatted += `[${d.Letter} ${((d.TotalSpaceFree/d.TotalSpace)*100).toFixed(2)}% Free]`;
		});

		device.AntivirusFormatted = device.Antivirus.length > 0 ? '' : '-';
		device.Antivirus.map(a => {
			device.AntivirusFormatted += `[${a.Name}]`;
		});

		device.FirewallFormatted = device.Firewall.length > 0 ? '' : '-';
		device.Firewall.map(f => {
			device.FirewallFormatted += `[${f.Name}(${f.Status})]`;
		});
	
		if (this.DevicesConnected.length == 0 || this.DevicesConnected.find(d => d.ID === device.ID) == null)
			this.DevicesConnected.push(device);
	}
}
