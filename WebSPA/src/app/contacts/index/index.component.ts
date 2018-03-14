import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ContactDetails } from '../../shared/models/contact-details';
import { ContactService } from '../../shared/services/contact.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  contacts: ContactDetails[];

  constructor(private _contactService: ContactService, private _router: Router ) { }

  ngOnInit() {
    this.getAllContacts();
  }

  getAllContacts(): void {
    this._contactService.getAllContacts()
        .subscribe(contacts => {
          this.contacts = contacts;
          //console.log(this.contacts);
        },
        error => {
          console.log(error);
        });        
  }

  getDetails(id) {
    this._router.navigate(['contacts/contactDetails/', id]);
  }  
  
}
