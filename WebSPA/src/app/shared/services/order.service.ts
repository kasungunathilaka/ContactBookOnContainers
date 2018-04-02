import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { OrdersConfigurationService } from './orders-configurations.service';
import { CustomerDetails } from '../models/customer-details';
import { OrderDetails } from '../models/order-details';
import { ProductDetails } from '../models/product-details';

@Injectable()
export class OrderService {
  headers: Headers;
  options: RequestOptions;
  private ordersUrl: string;
  constructor(private http: Http, private _configuration: OrdersConfigurationService) {
    this.ordersUrl = _configuration.ServerWithApiUrl + 'orders';
    this.headers = new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    this.options = new RequestOptions({ headers: this.headers });
  }

  public getAllOrders(): Observable<OrderDetails[]> {
    return this.http.get(this.ordersUrl)
      .map((res: Response) => <OrderDetails[]>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public getOrderById(id: string): Observable<OrderDetails> {
    const url = `${this.ordersUrl}/${id}`;
    return this.http.get(url)
      .map((res: Response) => <OrderDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public CreateCustomer(addedContact: CustomerDetails): Observable<CustomerDetails> {
    let bodyString = JSON.stringify(addedContact);
    const url = `${this.ordersUrl}/createCustomer`;
    return this.http.post(url, bodyString, this.options)
      .map((res: Response) => <CustomerDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public getAllCustomerNames(): Observable<string[]> {
    const url = `${this.ordersUrl}/names`;
    return this.http.get(url)
      .map((res: Response) => <string[]>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public SearchCustomer(tag: string): Observable<CustomerDetails> {
    const url = `${this.ordersUrl}/search/${tag}`;
    return this.http.get(url)
      .map((res: Response) => <CustomerDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public GetProductByName(productName: string): Observable<ProductDetails> {
    const url = `${this.ordersUrl}/product/${productName}`;
    return this.http.get(url)
      .map((res: Response) => <ProductDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public getAllProductNames(): Observable<string[]> {
    const url = `${this.ordersUrl}/productNames`;
    return this.http.get(url)
      .map((res: Response) => <string[]>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public getAllProductCategoryNames(): Observable<string[]> {
    const url = `${this.ordersUrl}/productCategoryNames`;
    return this.http.get(url)
      .map((res: Response) => <string[]>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public CreateOrder(addedOrder: OrderDetails): Observable<OrderDetails> {
    let bodyString = JSON.stringify(addedOrder);
    const url = `${this.ordersUrl}/createOrder`;
    return this.http.post(url, bodyString, this.options)
      .map((res: Response) => <OrderDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public DeleteOrder(id: string): Observable<OrderDetails> {
    const url = `${this.ordersUrl}/${id}`;
    return this.http.delete(url, this.options)
      .map((res: Response) => <OrderDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public UpdateOrder(id: string, editedOrder: OrderDetails): Observable<OrderDetails> {
    let bodyString = JSON.stringify(editedOrder);
    const url = `${this.ordersUrl}/${id}`;
    return this.http.put(url, bodyString, this.options)
      .map((res: Response) => <OrderDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  public CreateProduct(addedProduct: ProductDetails): Observable<ProductDetails> {
    let bodyString = JSON.stringify(addedProduct);
    const url = `${this.ordersUrl}/createProduct`;
    return this.http.post(url, bodyString, this.options)
      .map((res: Response) => <ProductDetails>res.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

}    
