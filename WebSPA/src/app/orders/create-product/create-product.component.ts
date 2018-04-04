import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { NgForm } from '@angular/forms';
import { ProductDetails } from '../../shared/models/product-details';
import { Router } from '@angular/router';
import { ToastrServices } from '../../shared/services/toastr.service';
import { MatTableDataSource } from '@angular/material';

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

  isVisible: boolean = false;
  productDetails: ProductDetails[] = [];
  displayedColumns = ['productName', 'productCategory', 'description', 'unitPrice', 'availability', 'delete'];
  dataSource = new MatTableDataSource<ProductDetails>(this.productDetails);

  constructor(private _orderService: OrderService, private _router: Router, private _toastrService: ToastrServices) { }

  ngOnInit() {
    this.getAllProductCategoryNames();
    this.getAllProducts();
  }

  getAllProducts(): void {
    this._orderService.getAllProducts()
      .subscribe(productDetails => {
        this.productDetails = productDetails;
        this.dataSource.data = this.productDetails;
        //console.log(this.productDetails);
      },
      error => {
        console.log(error);
      });
  }

  onAdd() {
    this.isVisible = true;
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
    productForm.reset();

    this._orderService.CreateProduct(addedProduct)
      .subscribe(
        result => {
          //console.log(addedProduct);
          this._toastrService.success('Product Added Successfully.', '');
          this.isVisible = false;

          this._orderService.getAllProducts()
            .subscribe(productDetails => {
              this.productDetails = productDetails;
              this.dataSource.data = this.productDetails;
              //console.log(this.productDetails);
            },
            error => {
              console.log(error);
            });
        },
        error => {
          console.log(error);
          this._toastrService.error('Product Addition Failed.', 'Error');
        });
  }

  removeProduct(id) {
    this._orderService.DeleteProduct(id)
      .subscribe(
        result => {
          //console.log('Product Deleted.');
          this._orderService.getAllProducts()
            .subscribe(productDetails => {
              this.productDetails = productDetails;
              this.dataSource.data = this.productDetails;
              //console.log(this.productDetails);
            },
              error => {
                console.log(error);
              });
          this._toastrService.success('Product Deleted Successfully.', '');
        },
        error => {
          console.log(error);
          this._toastrService.error('Product Deletion Failed.', 'Error');
        });
  }
}
