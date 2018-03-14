import { Component, OnInit } from '@angular/core';
import { ContactDetails } from '../../shared/models/contact-details';
import { ContactService } from '../../shared/services/contact.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-search-contact',
  templateUrl: './search-contact.component.html',
  styleUrls: ['./search-contact.component.css']
})
export class SearchContactComponent implements OnInit {
  param: any;
  contacts: ContactDetails[];
  searchTag: string;
  constructor(private _contactService: ContactService, private _route: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    this.param = this._route.params.subscribe(p =>{
      this.searchTag = p['tag'];   
      this.searchContacts(this.searchTag);       
    });    
  }

  searchContacts(searchTag: string): void {
    this._contactService.SearchContact(searchTag)
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
