import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { OrderDetails, OrderItem } from '../../shared/models/order-details';
import { OrderService } from '../../shared/services/order.service';
import { MatTableDataSource } from '@angular/material';
import { Address } from '../../shared/models/customer-details';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  orderDetails: OrderDetails;
  orderItems: OrderItem[] = [];
  firstName: string;
  lastName: string;
  gender: string;
  email: string;
  mobilePhone: string;
  homePhone: string;
  facebookId: string;
  addresses: Address[];
  orderId: string;
  orderDate: Date;
  isCompleted: boolean;
  private param: any;
  displayedColumns = ['productName', 'productCategory', 'description', 'price', 'quantity', 'amount'];
  dataSource = new MatTableDataSource<OrderItem>(this.orderItems);

  constructor(private _router: Router, private _route: ActivatedRoute, private _orderService: OrderService) { }

  ngOnInit() {
    this.param = this._route.params.subscribe(p => {
      this.orderId = p['id'];
      this.getOrderById(this.orderId);     
    });
  }

  getOrderById(id: string) {
    this._orderService.getOrderById(id)
      .subscribe(order => {
        this.orderDetails = order;
        for (var i = 0; i < order.orderItems.length; i++) {
          this.orderItems.push(order.orderItems[i]);
        }
        this.dataSource.data = this.orderItems;
        this.firstName = order.customer.firstName;
        this.lastName = order.customer.lastName;
        this.gender = order.customer.gender;
        this.email = order.customer.email;
        this.mobilePhone = order.customer.mobilePhone;
        this.homePhone = order.customer.homePhone;
        this.facebookId = order.customer.facebookId;
        this.addresses = order.customer.addresses;
        this.orderId = order.orderId;
        this.orderDate = order.orderDate;
        this.isCompleted = order.isCompleted;
        //console.log(this.orderDetails);
      },
      error => {
        console.log(error);
      });
  }

}

