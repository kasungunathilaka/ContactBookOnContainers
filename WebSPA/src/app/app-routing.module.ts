import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';

const routes: Routes = [
  { path: 'contacts', loadChildren: 'app/contacts/contacts.module#ContactsModule' },
  { path: 'orders', loadChildren: 'app/orders/orders.module#OrdersModule' },
  { path: '', redirectTo: 'orders', pathMatch: 'full' }
  //{ path: '**', redirectTo: 'contacts', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
