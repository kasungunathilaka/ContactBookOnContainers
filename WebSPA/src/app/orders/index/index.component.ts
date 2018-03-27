import { Component, OnInit } from '@angular/core';
import { OrderDetails } from '../../shared/models/order-details';
import { OrderService } from '../../shared/services/order.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  orders: OrderDetails[];

  constructor(private _orderService: OrderService, private _router: Router ) { }

  ngOnInit() {
    this.getAllOrders();
  }

  getAllOrders() {
    this._orderService.getAllOrders()
      .subscribe(orders => {
        this.orders = orders;
        console.log(this.orders);
      },
      error => {
        console.log(error);
      });
  }

  getOrderDetails(id) {
    this._router.navigate(['orders/orderDetails/', id]);
  }
}
