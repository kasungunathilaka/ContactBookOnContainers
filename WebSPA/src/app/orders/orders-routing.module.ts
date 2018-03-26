import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersComponent } from './orders.component';
import { IndexComponent } from './index/index.component';
import { CreateCustomerComponent } from './create-customer/create-customer.component';
import { CreateProductComponent } from './create-product/create-product.component';

const routes: Routes = [
  {
    path: '', component: OrdersComponent, children: [
      { path: '', component: IndexComponent },
      { path: 'addCustomer', component: CreateCustomerComponent },
      { path: 'addProduct', component: CreateProductComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
