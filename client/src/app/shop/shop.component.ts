import { IType } from './../shared/models/productType';
import { ShopService } from './shop.service';
import { IProduct } from './../shared/models/product';
import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brands';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: IProduct[];
  brands: IBrand[];
  types: IType[];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  private getProducts(): void {
      this.shopService.getProducts().subscribe(response => {
        this.products = response.data;
      }, error => {
        console.log(error);
      });
  }

  private getBrands(): void {
    this.shopService.getBrands().subscribe(response => {
      this.brands = response;
    }, error => {
      console.log(error);
    });
  }

  private getTypes(): void {
    this.shopService.getTypes().subscribe(response => {
      this.types = response;
    }, error => {
      console.log(error);
    });
  }
}
