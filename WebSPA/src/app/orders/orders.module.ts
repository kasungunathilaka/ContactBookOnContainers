import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule, MatInputModule } from '@angular/material';
import { TagInputModule } from 'ngx-chips';
import { OrdersRoutingModule } from './orders-routing.module';

import { OrdersComponent } from './orders.component';
import { IndexComponent } from './index/index.component';
import { CreateCustomerComponent } from './create-customer/create-customer.component';
import { CreateProductComponent } from './create-product/create-product.component';

@NgModule({
  declarations: [
    OrdersComponent,
    IndexComponent,
    CreateCustomerComponent,
    CreateProductComponent
  ],
  imports: [
    OrdersRoutingModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,
    TagInputModule,
    CommonModule
  ],
  providers: [

  ]
})
export class OrdersModule { }
