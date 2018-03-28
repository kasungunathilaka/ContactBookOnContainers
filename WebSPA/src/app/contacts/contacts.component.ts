import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { ContactService } from '../shared/services/contact.service';


@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnInit {
  names: string[];
  searchTag: any[];

  constructor(private _contactService: ContactService, private _router: Router) { }

  ngOnInit() {
    this.getAllContactNames();
  }

  onItemAdded() {
    this._router.navigate(['contacts/searchContact/', this.searchTag[0].value]);
    //console.log(this.searchTag[0].value);
  }

  onItemRemoved() {
    this._router.navigate(['contacts']);
  }

  getAllContactNames(): void {
    this._contactService.getAllContactNames()
      .subscribe(names => {
        this.names = names;
        //console.log(this.names);
      },
      error => {
        console.log(error);
      });
  }

}
