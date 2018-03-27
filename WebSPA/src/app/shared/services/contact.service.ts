import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ContactDetails } from '../models/contact-details';
import { ContactsConfigurationService } from './contacts-configurations.service';

@Injectable()
export class ContactService {
    headers: Headers;
    options: RequestOptions;
    private contactsUrl: string;
    constructor(private http: Http, private _configuration: ContactsConfigurationService) {
        this.contactsUrl = _configuration.ServerWithApiUrl + 'contacts';
        this.headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        });
        this.options = new RequestOptions({ headers: this.headers });
    }

    public getAllContacts(): Observable<ContactDetails[]> {
        return this.http.get(this.contactsUrl)
            .map((res: Response) => <ContactDetails[]>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public GetContactById(id: string): Observable<ContactDetails> {
        const url = `${this.contactsUrl}/${id}`;
        return this.http.get(url)
            .map((res: Response) => <ContactDetails>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public SearchContact(tag: string): Observable<ContactDetails[]> {
        const url = `${this.contactsUrl}/search/${tag}`;
        return this.http.get(url)
            .map((res: Response) => <ContactDetails[]>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public getAllContactNames(): Observable<string[]> {
        const url = `${this.contactsUrl}/names`;
        return this.http.get(url)
            .map((res: Response) => <string[]>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public AddContact(addedContact: ContactDetails): Observable<ContactDetails> {
        let bodyString = JSON.stringify(addedContact);
        return this.http.post(this.contactsUrl, bodyString, this.options)
            .map((res: Response) => <ContactDetails>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public DeleteContact(id: string): Observable<ContactDetails> {
        const url = `${this.contactsUrl}/${id}`;
        return this.http.delete(url, this.options)
            .map((res: Response) => <ContactDetails>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }

    public EditContact(id: string, editedContact: ContactDetails): Observable<ContactDetails> {
        let bodyString = JSON.stringify(editedContact);
        const url = `${this.contactsUrl}/${id}`;
        return this.http.put(url, bodyString, this.options)
            .map((res: Response) => <ContactDetails>res.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }
}    
