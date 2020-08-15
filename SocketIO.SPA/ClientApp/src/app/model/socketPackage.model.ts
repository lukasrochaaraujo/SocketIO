import { DeviceData } from './device/deviceData.model';

export class SocketPackage {
    SocketOriginID: string;
    SocketTargetID: string
    Message: string;
    DeviceData: DeviceData;
}