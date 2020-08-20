import { ShopParams } from './../shared/models/shopParams';
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
  maxPages = 3;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams();
  totalCount: number;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price Low to High', value: 'priceAsc'},
    {name: 'Price High to Low', value: 'priceDesc'}
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  private getProducts(): void {
      this.shopService.getProducts(this.shopParams)
        .subscribe(response => {
          this.products = response.data;
          this.shopParams.pageNumber = response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        }, error => {
          console.log(error);
        });
  }

  private getBrands(): void {
    this.shopService.getBrands().subscribe(response => {
      this.brands = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  private getTypes(): void {
    this.shopService.getTypes().subscribe(response => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  /* Handlers */
  onBrandSelected(brandId: number): void {
    this.shopParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number): void {
    this.shopParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(sort: string): void {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any): void {
    this.shopParams.pageNumber = event;
    this.getProducts();
  }
}
