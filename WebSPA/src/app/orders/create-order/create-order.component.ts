import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { Router } from '@angular/router';
import { CustomerDetails, Address } from '../../shared/models/customer-details';

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
  productCategory: string[] = ["A", "B"];
  products: string[] = ["A", "B"];

  constructor(private _orderService: OrderService, private _router: Router) { }

  ngOnInit() {
    this.getAllCustomerNames();
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

  onProductCategorySelected(event) {
    console.log(event);
  }
}
