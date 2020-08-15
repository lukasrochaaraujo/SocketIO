import { Injectable } from '@angular/core';
import { SocketPackage } from './model/socketPackage.model';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket: WebSocket;

  constructor() { }
 
  startSocket(callBackOpen: Function, callBackClose: Function, callBackMessage: Function) {
    this.socket = new WebSocket('ws://localhost:5000/ws');
    this.socket.addEventListener("open", (ev => callBackOpen('connection established')));    
    this.socket.addEventListener("close", (ev => callBackClose('connection finalized')));
    this.socket.addEventListener("message", (ev => {
      callBackMessage(this.convertPackageMessage(ev.data));
    }));
  }

  sendMessage(message: string) {
    this.socket.send(message);
  }

  convertPackageMessage(data: string): SocketPackage {
    let extracted: string[] = data.split('@');
    
    if (extracted.length != 3)
      return null;

    let originId = extracted[0];
    if (originId != null && originId.startsWith('origin'))
      originId = originId.replace('origin', '');
    else
      originId = '';

    let targetId = extracted[1];
    if (targetId != null && targetId.startsWith('target'))
      targetId = targetId.replace('target', '');
    else
      targetId = '';


    let message = extracted[2];
    if (message != null && message.startsWith('message'))
      message = message.replace('message', '');
    else
      message = '';

    let socketPackage = new SocketPackage();
    socketPackage.SocketOriginID = originId;
    socketPackage.SocketTargetID = targetId;
    socketPackage.Message = message;

    if (socketPackage.Message.indexOf('{') >= 0) {
      socketPackage.DeviceData = JSON.parse(socketPackage.Message);
      socketPackage.Message = null;
    }

    return socketPackage;
  }
}
