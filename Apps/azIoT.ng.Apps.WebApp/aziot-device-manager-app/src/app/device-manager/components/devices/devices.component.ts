import { Component, OnInit } from '@angular/core';
import { IDevice } from '../../models/device';
import { DevicesService } from '../../services/devices.service';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit {

  public devices: IDevice[];
  public message: string;

  constructor(private deviceService: DevicesService) { }

  ngOnInit() {

    this.message='';

    this.deviceService.getDevices('')
      .subscribe(data => {

        this.devices = data;

      });
  }

  sendMessageClicked(index) {
debugger
    this.deviceService.sendMessage(this.devices[index].id, this.message, { 'type': 'ping' })
      .subscribe(data => {

      });
  }
}

