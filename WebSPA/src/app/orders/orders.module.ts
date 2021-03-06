import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule, MatInputModule } from '@angular/material';
import { TagInputModule } from 'ngx-chips';
import { OrdersRoutingModule } from './orders-routing.module';
import { MatTableModule } from '@angular/material';

import { OrdersComponent } from './orders.component';
import { IndexComponent } from './index/index.component';
import { CreateCustomerComponent } from './create-customer/create-customer.component';
import { CreateProductComponent } from './create-product/create-product.component';

import { OrdersConfigurationService } from '../shared/services/orders-configurations.service';
import { OrderService } from '../shared/services/order.service';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { CreateOrderComponent } from './create-order/create-order.component';
import { EditOrderComponent } from './edit-order/edit-order.component';

@NgModule({
  declarations: [
    OrdersComponent,
    IndexComponent,
    CreateCustomerComponent,
    CreateProductComponent,
    OrderDetailsComponent,
    CreateOrderComponent,
    EditOrderComponent
  ],
  imports: [
    OrdersRoutingModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,
    TagInputModule,
    CommonModule,
    MatTableModule
  ],
  providers: [
    OrdersConfigurationService,
    OrderService
  ],
  bootstrap: [OrdersComponent]
})
export class OrdersModule { }
