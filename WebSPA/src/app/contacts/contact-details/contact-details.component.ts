import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ContactDetails } from '../../shared/models/contact-details';
import { ContactService } from '../../shared/services/contact.service';
import { ToastrServices } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})
export class ContactDetailsComponent implements OnInit {
  private param: any;
  customerId: string;
  contactDetails: ContactDetails;
  firstName: string;
  lastName : string;
  gender : string;
  email : string;
  mobilePhone : string;
  homePhone : string;
  facebookId : string; 
  street : string;
  city : string;
  province : string;
  zipCode : string;

  constructor(private _contactService: ContactService, 
    private _toastrService: ToastrServices,
    private _route: ActivatedRoute,
    private _router: Router) { }

  ngOnInit() {
    this.param = this._route.params.subscribe(p =>{
      this.customerId = p['id'];   
      this.getContactById(this.customerId);       
    });    
  }

  getContactById(id: string) {
    this._contactService.GetContactById(id)
      .subscribe(
        contact => {
          this.contactDetails = contact;
          this.firstName = this.contactDetails.firstName;
          this.lastName = this.contactDetails.lastName;
          this.gender = this.contactDetails.gender;
          this.email = this.contactDetails.email;
          this.facebookId = this.contactDetails.facebookId;
          this.mobilePhone = this.contactDetails.mobilePhone;
          this.homePhone = this.contactDetails.homePhone;
          this.street = this.contactDetails.street;
          this.city = this.contactDetails.city;
          this.province = this.contactDetails.province;
          this.zipCode = this.contactDetails.zipCode;
          //console.log(this.contactDetails);
        },
      error => {
          console.log(error);
    });
  }

  editDetails(id) {
    this._router.navigate(['contacts/editContact/', id]);
  }

  deleteContact(id) {
    this._contactService.DeleteContact(id)
      .subscribe(
        result => {
          //console.log('Contact Deleted.');           
          this._toastrService.success('Contact Deleted Successfully.', '');
          this._router.navigate(['contacts']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Contact Deletion Failed.', 'Error');
    });     
  }

}
