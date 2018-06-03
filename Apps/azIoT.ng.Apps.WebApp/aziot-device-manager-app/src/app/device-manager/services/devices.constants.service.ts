import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DevicesConstantsService {

  constructor() { }

public static API_ROUTES={
  get: 'api/devices/get/',
  sendMessage: 'api/devices/sendto/',
  callOperation: 'api/devices/call/{0}/on/{1}'
}
}
