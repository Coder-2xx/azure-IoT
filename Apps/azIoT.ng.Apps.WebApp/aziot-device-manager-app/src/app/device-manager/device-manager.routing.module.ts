import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PageNotFoundComponent} from '../shared/components/page-not-found/page-not-found.component';
import { DevicesComponent } from './components/devices/devices.component';
import { DevicesDashboardComponent } from './components/devices-dashboard/devices-dashboard.component';
import { DevicesTableComponent } from './components/devices-table/devices-table.component';

const deviceManagerRoutes: Routes = [
  {
    path: '',
    pathMatch:'full',
    redirectTo:'all'
  },
  {
    path:'all',
    component: DevicesDashboardComponent
  },
  {
    path:'**',
    component:PageNotFoundComponent
  }
];

@NgModule({
  imports: [
RouterModule.forChild(deviceManagerRoutes)
  ],
  exports:[RouterModule]
})
export class DeviceManagerRoutingModule { }
