import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
   selector: 'app-shop',
   templateUrl: './shop.component.html',
   styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
   // como el input siempre va a estar ( no esta envuelto en un *ngIf que condiciona si esta o no ) se le puede poner { static: true }
   // como lo cambie dentro de un *ngIf => ahora es false
   @ViewChild('search', { static: false }) searchTerm: ElementRef;

   products: IProduct[];
   brands: IBrand[];
   types: IType[];
   totalCount: number;
   shopParams = new ShopParams();

   sortOptions = [
      { name: 'Alphabetical', value: 'name' },
      { name: 'Price: Low to High', value: 'priceAsc' },
      { name: 'Price: High to Low', value: 'priceDesc' },
   ];

   constructor(private shopService: ShopService) {}

   ngOnInit(): void {
      this.getProducts();
      this.getBrands();
      this.getTypes();
   }

   getProducts() {
      this.shopService.getProducts(this.shopParams).subscribe({
         next: (res) => {
            this.products = res.data;
            this.shopParams.pageNumber = res.pageIndex;
            this.shopParams.pageSize = res.pageSize;
            this.totalCount = res.count;
         },
         error: (error) => console.log(error),
      });
   }

   getBrands() {
      this.shopService.getBrands().subscribe({
         next: (res) => (this.brands = [{ id: 0, name: 'all' }, ...res]),
         error: (error) => console.log(error),
      });
   }

   getTypes() {
      this.shopService.getTypes().subscribe({
         next: (res) => (this.types = [{ id: 0, name: 'all' }, ...res]),
         error: (error) => console.log(error),
      });
   }

   onBrandSelected(brandId: number) {
      this.shopParams.brandId = brandId;
      this.shopParams.pageNumber = 1;
      this.getProducts();
   }

   onTypeSelected(typeId: number) {
      this.shopParams.typeId = typeId;
      this.shopParams.pageNumber = 1;
      this.getProducts();
   }

   onSortSelected(sort: string) {
      this.shopParams.sort = sort;
      this.shopParams.pageNumber = 1;
      this.getProducts();
   }

   onPageChanged(e: number) {
      if (this.shopParams.pageNumber !== e) {
         // xq al cambiar totalCount el pager llama el pageChanged q va a terminar llamando otra vez a este, q a su vez llama a getProducts() . Como lo llama con el mismo pageNumber => asi discrimino
         this.shopParams.pageNumber = e;
         this.getProducts();
      }
   }

   onSearch() {
      this.shopParams.search = this.searchTerm.nativeElement.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
   }

   onReset() {
      this.searchTerm.nativeElement.value = '';
      this.shopParams = new ShopParams();
      this.getProducts();
   }
}
