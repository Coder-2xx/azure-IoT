import { Injectable } from '@angular/core';
import { DataService } from '../../shared/services/data.service';
import { DevicesConstantsService } from '../services/devices.constants.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { IDevice } from '../models/device';
import { IDeviceMessage } from '../models/device-message';
import { SharedConstantsService } from '../../shared/services/shared.constants.service';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(private data: DataService, private sharedConstants: SharedConstantsService) { }

  getDevices(searchTerm: string): Observable<IDevice[]> {
    return this.data
      .get(SharedConstantsService.ENDPOINTS.DEVICE_MANAGER_API + DevicesConstantsService.API_ROUTES.get + searchTerm)
      .pipe(map(data => <IDevice[]>data),
      catchError((error: any) => Observable.throw("error")));
  }

  sendMessage(deviceId: string, message: string, properties: {}): Observable<boolean> {

    let deviceMessage: IDeviceMessage = {
      Text: message,
      Properties: properties
    };

    return this.data
      .post(SharedConstantsService.ENDPOINTS.DEVICE_MANAGER_API + DevicesConstantsService.API_ROUTES.sendMessage + deviceId, deviceMessage)
      .pipe(map(data => <boolean>data));
  }

  callOperation(operation: string, deviceId: string): Observable<IDevice[]> {
    return this.data
      .post(SharedConstantsService.ENDPOINTS.DEVICE_MANAGER_API + DevicesConstantsService.API_ROUTES.callOperation
        .replace('{0}', operation)
        .replace('{1}', deviceId))
      .pipe(map(data => <IDevice[]>data));
  }
}
