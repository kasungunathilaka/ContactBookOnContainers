import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { ContactsRoutingModule } from './contacts-routing.module';
import { MatAutocompleteModule, MatInputModule } from '@angular/material';
import { TagInputModule } from 'ngx-chips';

import { ContactsComponent } from './contacts.component';
import { IndexComponent } from './index/index.component';
import { AddContactComponent } from './add-contact/add-contact.component';
import { EditContactComponent } from './edit-contact/edit-contact.component';
import { ContactDetailsComponent } from './contact-details/contact-details.component';
import { SearchContactComponent } from './search-contact/search-contact.component';

import { ContactService } from '../shared/services/contact.service';
import { ConfigurationService } from '../shared/services/configurations.service';
import { ToastrServices } from '../shared/services/toastr.service';


@NgModule({
  declarations: [
    ContactsComponent,
    IndexComponent,
    AddContactComponent,
    EditContactComponent,
    ContactDetailsComponent,
    SearchContactComponent
  ],
  imports: [
    ContactsRoutingModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,
    TagInputModule,
    CommonModule,
    ToastrModule.forRoot()
  ],
  providers: [
    ContactService,
    ConfigurationService,
    ToastrServices
  ]
})
export class ContactsModule { }
