import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { OrderDetails } from '../../shared/models/order-details';
import { OrderService } from '../../shared/services/order.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  orderDetails: OrderDetails;
  orderId: string;
  private param: any;

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
        console.log(this.orderDetails);
      },
      error => {
        console.log(error);
      });
  }

}
