import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { OrderDetails, OrderItem } from '../../shared/models/order-details';
import { Address } from '../../shared/models/customer-details';
import { OrderService } from '../../shared/services/order.service';
import { NgForm } from '@angular/forms';
import { ProductDetails } from '../../shared/models/product-details';
import { UUID } from 'angular2-uuid';
import { ToastrServices } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.css']
})
export class EditOrderComponent implements OnInit {
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
  displayedColumns = ['productName', 'productCategory', 'description', 'price', 'quantity', 'amount', 'delete'];
  dataSource = new MatTableDataSource<OrderItem>(this.orderItems);

  productNames: string[];
  productN: string;
  product: ProductDetails;

  productCategory: string;
  description: string;
  unitPrice: number;
  availability: string;
  quantity: number;

  constructor(private _router: Router, private _route: ActivatedRoute, private _orderService: OrderService, private _toastrService: ToastrServices) { }

  ngOnInit() {
    this.param = this._route.params.subscribe(p => {
      this.orderId = p['id'];
      this.getOrderById(this.orderId);
      this.getAllProductNames();
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

  getAllProductNames() {
    this._orderService.getAllProductNames()
      .subscribe(productNames => {
        this.productNames = productNames;
        //console.log(this.productNames);
      },
        error => {
          console.log(error);
        });
  }

  onProductSelected() {
    this._orderService.GetProductByName(this.productN)
      .subscribe(product => {
        this.product = product;
        this.productCategory = product.productCategoryName;
        this.description = product.description;
        this.unitPrice = product.price;
        this.availability = (product.isAvailable == true ? "In Stock" : "Out of Stock");
        //console.log(this.product);
      },
        error => {
          console.log(error);
        });
  }

  addOrderItems() {
    var item: OrderItem = new OrderItem();
    let uuid = UUID.UUID();
    item.orderItemId = uuid;
    item.productId = this.product.productId;
    item.productName = this.product.productName;
    item.productCategoryId = this.product.productCategoryId;
    item.productCategoryName = this.product.productCategoryName;
    item.description = this.product.description;
    item.price = this.product.price;
    item.quantity = this.quantity;
    item.isAvailable = true;
    this.orderItems.push(item);
    this.dataSource.data = this.orderItems;
    //console.log(this.orderItems);
  }

  removeOrderItems(id) {
    var filtered = this.orderItems.filter(function (el) { return el.orderItemId != id; });
    this.orderItems = filtered;
    this.dataSource.data = this.orderItems;
    //console.log(this.orderItems);
  }

  onEditButtonClick(orderForm: NgForm) {
    var editedOrder = new OrderDetails();
    editedOrder.isCompleted = orderForm.controls['isCompleted'].value;
    editedOrder.orderItems = this.orderItems;

    this._orderService.UpdateOrder(this.orderId, editedOrder)
      .subscribe(
        result => {
          //console.log(editedOrder);
          this._toastrService.success('Order Edited Successfully.', '');
          this._router.navigate(['orders']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Order Edition Failed.', 'Error');
        }); 
  }

}
