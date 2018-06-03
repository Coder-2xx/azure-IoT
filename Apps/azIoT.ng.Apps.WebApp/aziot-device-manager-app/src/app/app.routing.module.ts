import { NgModule } from '@angular/core';
import { RouterModule, Routes  } from '@angular/router';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

const appRoutes: Routes = [
  {
    path: '',
    pathMatch:'full',
    redirectTo:'devices'
  },
  {
    path: 'devices',
    loadChildren: './device-manager/device-manager.module#DeviceManagerModule'
  },
  {
    path:'**',
    component:PageNotFoundComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [RouterModule]
})

export class AppRoutingModule { }
