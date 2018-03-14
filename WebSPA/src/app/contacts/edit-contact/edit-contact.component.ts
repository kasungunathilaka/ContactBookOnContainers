import { Component, OnInit, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrServices } from '../../shared/services/toastr.service';
import { ContactService } from '../../shared/services/contact.service';
import { ContactDetails } from '../../shared/models/contact-details';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.css']
})
export class EditContactComponent implements OnInit {
  private param: any;
  customerId: string;
  contactToEdit: ContactDetails;
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

  ngOnDestroy() {
    this.param.unsubscribe();
  }

  getContactById(id: string) {
    this._contactService.GetContactById(id)
      .subscribe(
        contact => {
          this.contactToEdit = contact;
          this.firstName = this.contactToEdit.firstName;
          this.lastName = this.contactToEdit.lastName;
          this.gender = this.contactToEdit.gender;
          this.email = this.contactToEdit.email;
          this.facebookId = this.contactToEdit.facebookId;
          this.mobilePhone = this.contactToEdit.mobilePhone;
          this.homePhone = this.contactToEdit.homePhone;
          this.street = this.contactToEdit.street;
          this.city = this.contactToEdit.city;
          this.province = this.contactToEdit.province;
          this.zipCode = this.contactToEdit.zipCode;
          //console.log(this.contactToEdit);
        },
      error => {
          console.log(error);
      })
  }

  onEditButtonClick(contactForm: NgForm){
    var editedContact = new ContactDetails();

    editedContact.firstName = contactForm.controls['firstName'].value;
    editedContact.lastName = contactForm.controls['lastName'].value;
    editedContact.gender = contactForm.controls['gender'].value;
    editedContact.email = contactForm.controls['email'].value;
    editedContact.mobilePhone = contactForm.controls['mobilePhone'].value;
    editedContact.homePhone = contactForm.controls['homePhone'].value;
    editedContact.facebookId = contactForm.controls['facebookId'].value;
    editedContact.street = contactForm.controls['street'].value;
    editedContact.city = contactForm.controls['city'].value;
    editedContact.province = contactForm.controls['province'].value;
    editedContact.zipCode = contactForm.controls['zipcode'].value;

    this._contactService.EditContact(this.contactToEdit.customerId, editedContact)
      .subscribe(
        result => {
          //console.log('Contact Edited.');
          //console.log(editedContact);            
          this._toastrService.success('Contact Edited Successfully.', '');
          this._router.navigate(['contacts']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Contact Edition Failed.', 'Error');
    });     
  }

}
