import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedConstantsService {

  constructor() { }

  public static ENDPOINTS = {
    DEVICE_MANAGER_API: 'http://localhost:57065/'
  };
}
