import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { OrdersConfigurationService } from './orders-configurations.service';
import { CustomerDetails } from '../models/customer-details';
import { OrderDetails } from '../models/order-details';

@Injectable()
export class OrderService {
  headers: Headers;
  options: RequestOptions;
  private contactsUrl: string;
  constructor(private http: Http, private _configuration: OrdersConfigurationService) {
    this.contactsUrl = _configuration.ServerWithApiUrl + 'orders';
    this.headers = new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    this.options = new RequestOptions({ headers: this.headers });
  }

  public getAllOrders(): Observable<OrderDetails[]> {
    return this.http.get(this.contactsUrl)
      .map((res: Response) => <OrderDetails[]>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public getOrderById(id: string): Observable<OrderDetails> {
    const url = `${this.contactsUrl}/${id}`;
    return this.http.get(url)
      .map((res: Response) => <OrderDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public AddContact(addedContact: CustomerDetails): Observable<CustomerDetails> {
    let bodyString = JSON.stringify(addedContact);
    return this.http.post(this.contactsUrl, bodyString, this.options)
      .map((res: Response) => <CustomerDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

}    
