import { Component, OnInit, ViewChild } from '@angular/core';
import { IDevice } from '../../models/device';
import { DevicesService } from '../../services/devices.service';
import { MatPaginator, MatSort } from '@angular/material';
import { DevicesTableDataSource1 } from './devices-table-datasource';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'devices-dashboard',
  templateUrl: './devices-dashboard.component.html',
  styleUrls: ['./devices-dashboard.component.css']
})
export class DevicesDashboardComponent {

  message: string;
  operation: string;
  allowedMethods: string[];
  dataSource: DevicesTableDataSource1;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  IsDataLoading: boolean;
  IsTableVisible: boolean;

  private _hubConnection: HubConnection | undefined;
  public messages: string[] = [];

  displayedColumns = ['id-connectionstate', 'send-message', 'call-operation'];

  constructor(private deviceService: DevicesService) { }

  ngOnInit() {

    this.IsDataLoading = true;
    this.IsTableVisible = false;
    this.message = '';
    this.allowedMethods = ['LOCK', 'LOGOFF', 'CAPTURE_SCREEN'];

    this.dataSource = new DevicesTableDataSource1(this.paginator, this.sort, []);

    this.deviceService.getDevices('')
      .subscribe(data => {

        this.IsDataLoading = false;

        if (data && 0 < data.length) {
          this.IsTableVisible = true;
          this.dataSource = new DevicesTableDataSource1(this.paginator, this.sort, data);
        }
      },
        error => {

          this.IsDataLoading = false;
        });

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:57065/deviceshub')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    this._hubConnection.on('NewMessage', (data: any) => {
      this.messages.push(data);
    });
  }

  sendMessageClicked(deviceId) {

    this.deviceService.sendMessage(deviceId, this.message, { 'type': 'ping' })
      .subscribe(data => {

      });
  }

  callOperationClicked(deviceId) {

    this.deviceService.callOperation(this.operation, deviceId)
      .subscribe(data => {

      });
  }
}
