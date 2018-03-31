import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { NgForm } from '@angular/forms';
import { ProductDetails } from '../../shared/models/product-details';
import { Router } from '@angular/router';
import { ToastrServices } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {
  productCategoryNames: string[];
  productName: string;
  productCategory: any[];
  description: string;
  price: number;
  isAvailable: boolean = false;

  constructor(private _orderService: OrderService, private _router: Router, private _toastrService: ToastrServices) { }

  ngOnInit() {
    this.getAllProductCategoryNames();
  }

  getAllProductCategoryNames(): void {
    this._orderService.getAllProductCategoryNames()
      .subscribe(productCategoryNames => {
        this.productCategoryNames = productCategoryNames;
        //console.log(this.productCategoryNames);
      },
      error => {
        console.log(error);
      });
  }

  onAddButtonClick(productForm: NgForm) {
    var addedProduct = new ProductDetails();
    addedProduct.productName = productForm.controls['productName'].value;
    addedProduct.productCategoryName = this.productCategory[0].value;
    addedProduct.description = productForm.controls['description'].value;
    addedProduct.price = productForm.controls['price'].value;
    addedProduct.isAvailable = productForm.controls['isAvailable'].value;

    this._orderService.CreateProduct(addedProduct)
      .subscribe(
        result => {
          //console.log(addedProduct);
          this._toastrService.success('Product Added Successfully.', '');
          this._router.navigate(['orders']);
        },
        error => {
          console.log(error);
          this._toastrService.error('Product Addition Failed.', 'Error');
        });
  }
}
