import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { Router } from '@angular/router';
import { CustomerDetails, Address } from '../../shared/models/customer-details';
import { OrderItem, OrderDetails } from '../../shared/models/order-details';
import { ProductDetails } from '../../shared/models/product-details';
import { MatTableDataSource } from '@angular/material';
import { UUID } from 'angular2-uuid';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent implements OnInit {
  names: string[];
  searchTag: any[];

  customer: CustomerDetails;
  addresses: Address[];
  customerId: string;
  firstName: string;
  lastName: string;
  gender: string;
  email: string;
  mobilePhone: string;
  homePhone: string;
  facebookId: string;
  street: string;
  city: string;
  province: string;
  zipCode: string;

  products: ProductDetails[];
  productNames: string[];
  productN: string;
  product: ProductDetails;

  orderItems: OrderItem[] = [];
  
  productCategory: string;
  description: string;
  unitPrice: number;
  availability: string;
  quantity: number;

  displayedColumns = ['productName', 'productCategory', 'description', 'price', 'quantity', 'amount', 'delete'];
  dataSource = new MatTableDataSource<OrderItem>(this.orderItems);

  constructor(private _orderService: OrderService, private _router: Router) { }

  ngOnInit() {
    this.getAllCustomerNames();
    this.getAllProductNames();
  }

  getAllCustomerNames(): void {
    this._orderService.getAllCustomerNames()
      .subscribe(names => {
        this.names = names;
        //console.log(this.names);
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

  onItemAdded() {
    //console.log(this.searchTag[0].value);
    this.searchCustomer(this.searchTag[0].value);
  }

  onItemRemoved() {
    this.customer = null;
    this.customerId = null;
    this.firstName = null;
    this.firstName = null;
    this.lastName = null;
    this.gender = null;
    this.email = null;
    this.facebookId = null;
    this.mobilePhone = null;
    this.homePhone = null;
  }

  searchCustomer(searchTag: string): void {
    this._orderService.SearchCustomer(searchTag)
      .subscribe(customer => {
        this.customer = customer;
        this.addresses = customer.addresses;
        this.customerId = customer.customerId;
        this.firstName = customer.firstName;
        this.lastName = customer.lastName;
        this.gender = customer.gender;
        this.email = customer.email;
        this.facebookId = customer.facebookId;
        this.mobilePhone = customer.mobilePhone;
        this.homePhone = customer.homePhone;
        //console.log(this.customer);
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

  onAddButtonClick() {
    var addedOrder = new OrderDetails();
    addedOrder.customerDetails = this.customer;
    addedOrder.orderItems = this.orderItems;
    console.log(addedOrder);
  }

}
