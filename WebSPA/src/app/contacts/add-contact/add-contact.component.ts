import { Component, OnInit } from '@angular/core';
import { NgForm } from "@angular/forms";
import { ContactService } from '../../shared/services/contact.service';
import { ToastrServices } from '../../shared/services/toastr.service';
import { ContactDetails } from '../../shared/models/contact-details';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit {
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

  constructor(private _contactService: ContactService,
    private _router: Router,
    private _toastrService: ToastrServices) { }

  ngOnInit() {}

  onAddButtonClick(contactForm: NgForm){
    var addedContact = new ContactDetails();
    addedContact.firstName = contactForm.controls['firstName'].value;
    addedContact.lastName = contactForm.controls['lastName'].value;
    addedContact.gender = contactForm.controls['gender'].value;
    addedContact.email = contactForm.controls['email'].value;
    addedContact.mobilePhone = contactForm.controls['mobilePhone'].value;
    addedContact.homePhone = contactForm.controls['homePhone'].value;
    addedContact.facebookId = contactForm.controls['facebookId'].value;
    addedContact.street = contactForm.controls['street'].value;
    addedContact.city = contactForm.controls['city'].value;
    addedContact.province = contactForm.controls['province'].value;
    addedContact.zipCode = contactForm.controls['zipcode'].value;

    this._contactService.AddContact(addedContact)
      .subscribe(
        result => {
          //console.log('Contact Added.');
          //console.log(addedContact);            
          this._toastrService.success('Contact Added Successfully.', '');
          this._router.navigate(['contacts']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Contact Addition Failed.', 'Error');
    }); 
  }

}
