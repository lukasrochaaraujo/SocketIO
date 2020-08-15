import { Component, OnInit } from '@angular/core';
import { WebSocketService } from './web-socket.service';
import { SocketPackage } from './model/socketPackage.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'SocketIOSPA';
  
  constructor(private webSocketService: WebSocketService) { 

  }

  ngOnInit(): void {
    this.webSocketService.startSocket(this.onSocketOpenEvent, this.onSocketCloseEvent, this.onSocketMessageEvent);
  }

  onSocketOpenEvent(data: any) {
    console.log(data);
  }

  onSocketCloseEvent(data: any) {
    console.log(data);
  }

  onSocketMessageEvent(data: SocketPackage) {
    console.log(data);
  }
}
