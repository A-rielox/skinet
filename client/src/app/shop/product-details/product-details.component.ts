import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
   selector: 'app-product-details',
   templateUrl: './product-details.component.html',
   styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
   product: IProduct;
   // productId: number; 📌

   constructor(
      private shopService: ShopService,
      private activatedRoute: ActivatedRoute
   ) {
      // console.log(this.activatedRoute);
      // this.activatedRoute.params.subscribe((res) => {
      //    this.productId = +res.id;
      // }); 📌
   }

   ngOnInit(): void {
      this.loadProduct();
   }

   loadProduct() {
      this.shopService
         .getProduct(+this.activatedRoute.snapshot.paramMap.get('id'))
         .subscribe({
            next: (product) => {
               this.product = product;
            },
            error: (err) => console.log(err),
         });
   }
}
