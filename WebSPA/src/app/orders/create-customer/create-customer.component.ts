import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { ToastrServices } from '../../shared/services/toastr.service';
import { NgForm } from '@angular/forms';
import { CustomerDetails, Address } from '../../shared/models/customer-details';
import { Router } from '@angular/router';

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
  addresses: Address[] = [];
  provinceNames: string[] = ['Northern', 'North Western', 'Western', 'North Central', 'Central', 'Sabaragamuwa', 'Eastern', 'Uva','Southern'];

  constructor(private _orderService: OrderService, private _router: Router, private _toastrService: ToastrServices) { }

  ngOnInit() {
  }

  onAddButtonClick(customerForm: NgForm) {
    var address = new Address();
    address.street = customerForm.controls['street'].value;
    address.city = customerForm.controls['city'].value;
    address.province = customerForm.controls['province'].value;
    address.zipCode = customerForm.controls['zipcode'].value;

    this.addresses.push(address);

    var addedCustomer = new CustomerDetails();
    addedCustomer.firstName = customerForm.controls['firstName'].value;
    addedCustomer.lastName = customerForm.controls['lastName'].value;
    addedCustomer.gender = customerForm.controls['gender'].value;
    addedCustomer.email = customerForm.controls['email'].value;
    addedCustomer.mobilePhone = customerForm.controls['mobilePhone'].value;
    addedCustomer.homePhone = customerForm.controls['homePhone'].value;
    addedCustomer.facebookId = customerForm.controls['facebookId'].value;
    addedCustomer.addresses = this.addresses;

    this._orderService.AddCustomer(addedCustomer)
      .subscribe(
        result => {
            //console.log(addedCustomer);
            this._toastrService.success('Customer Added Successfully.', '');
            this._router.navigate(['orders']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Customer Addition Failed.', 'Error');
        });
  }

}
