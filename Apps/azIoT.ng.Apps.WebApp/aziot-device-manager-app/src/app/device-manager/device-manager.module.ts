import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DeviceManagerRoutingModule } from './device-manager.routing.module';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { DevicesComponent } from './components/devices/devices.component';
import { DataService } from '../shared/services/data.service';
import { DevicesDashboardComponent } from './components/devices-dashboard/devices-dashboard.component';
import { MatGridListModule, MatCardModule, MatMenuModule, MatIconModule, MatButtonModule, MatTableModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatProgressBarModule, MatBadgeModule, MatListModule } from '@angular/material';
import { DevicesTableComponent } from './components/devices-table/devices-table.component';

@NgModule({
  imports: [
    DeviceManagerRoutingModule,
    CommonModule,
    FormsModule,
    SharedModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressBarModule,
    MatBadgeModule,
    MatListModule
  ],
  providers: [DataService],
  declarations: [DevicesComponent, DevicesDashboardComponent, DevicesTableComponent]
})
export class DeviceManagerModule { }
