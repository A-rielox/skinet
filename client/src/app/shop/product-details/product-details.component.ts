import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
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
      private activatedRoute: ActivatedRoute,
      private bcService: BreadcrumbService
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
               this.bcService.set('@productDetails', product.name);
            },
            error: (err) => console.log(err),
         });
   }
}

// yellow 🟡 en los estilos globales sobreescribo los que tiene breadcrumb xdefecto ( en style.scss )
