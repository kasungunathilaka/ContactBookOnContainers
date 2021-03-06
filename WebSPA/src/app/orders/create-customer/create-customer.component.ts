import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { ToastrServices } from '../../shared/services/toastr.service';
import { NgForm } from '@angular/forms';
import { CustomerDetails, Address } from '../../shared/models/customer-details';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-create-customer',
  templateUrl: './create-customer.component.html',
  styleUrls: ['./create-customer.component.css']
})
export class CreateCustomerComponent implements OnInit {
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
  provinceNames: string[] = ['Northern', 'North Western', 'Western', 'North Central', 'Central', 'Sabaragamuwa', 'Eastern', 'Uva', 'Southern'];

  isVisible: boolean = false;
  customerDetails: CustomerDetails[] = [];
  displayedColumns = ['customerName', 'mobilePhone', 'homePhone', 'email', 'billingAddress', 'delete'];
  dataSource = new MatTableDataSource<CustomerDetails>(this.customerDetails);

  constructor(private _orderService: OrderService, private _router: Router, private _toastrService: ToastrServices) { }

  ngOnInit() {
    this.getAllCustomers();
  }

  getAllCustomers(): void {
    this._orderService.getAllCustomers()
      .subscribe(customerDetails => {
        this.customerDetails = customerDetails;
        this.dataSource.data = this.customerDetails;
        //console.log(this.customerDetails);
      },
      error => {
        console.log(error);
      });
  }

  onAdd() {
    this.isVisible = true;
  }

  onAddButtonClick(customerForm: NgForm) {
    let addresses = new Array<Address>();
    let address = new Address();
    address.street = customerForm.controls['street'].value;
    address.city = customerForm.controls['city'].value;
    address.province = customerForm.controls['province'].value;
    address.zipCode = customerForm.controls['zipcode'].value;

    addresses.push(address);

    var addedCustomer = new CustomerDetails();
    addedCustomer.firstName = customerForm.controls['firstName'].value;
    addedCustomer.lastName = customerForm.controls['lastName'].value;
    addedCustomer.gender = customerForm.controls['gender'].value;
    addedCustomer.email = customerForm.controls['email'].value;
    addedCustomer.mobilePhone = customerForm.controls['mobilePhone'].value;
    addedCustomer.homePhone = customerForm.controls['homePhone'].value;
    addedCustomer.facebookId = customerForm.controls['facebookId'].value;
    addedCustomer.addresses = addresses;
    customerForm.reset();

    this._orderService.CreateCustomer(addedCustomer)
      .subscribe(
        result => {
          //console.log(addedCustomer);
          this._toastrService.success('Customer Added Successfully.', '');
          this.isVisible = false;

          this._orderService.getAllCustomers()
            .subscribe(customerDetails => {
              this.customerDetails = customerDetails;
              this.dataSource.data = this.customerDetails;
              //console.log(this.customerDetails);
            },
            error => {
              console.log(error);
            });
        },
        error => {
          console.log(error);
          this._toastrService.error('Customer Addition Failed.', 'Error');
        });
  }

  removeCustomer(id) {
    this._orderService.DeleteCustomer(id)
      .subscribe(
        result => {
          //console.log('Customer Deleted.');
          this._orderService.getAllCustomers()
            .subscribe(customerDetails => {
              this.customerDetails = customerDetails;
              this.dataSource.data = this.customerDetails;
              //console.log(this.customerDetails);
            },
            error => {
              console.log(error);
            });
          this._toastrService.success('Customer Deleted Successfully.', '');          
        },
        error => {
          console.log(error);
          this._toastrService.error('Customer Deletion Failed.', 'Error');
        });
  }

}
