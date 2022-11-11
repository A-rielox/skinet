import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
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
   quantity = 1;

   constructor(
      private shopService: ShopService,
      private activatedRoute: ActivatedRoute,
      private bcService: BreadcrumbService,
      private basketService: BasketService
   ) {
      // console.log(this.activatedRoute);
      // this.activatedRoute.params.subscribe((res) => {
      //    this.productId = +res.id;
      // }); 📌

      // p'q al cargar la pag salga este nombre de item xdefecto en el header
      this.bcService.set('@productDetails', '... Cargando Producto');
   }

   ngOnInit(): void {
      this.loadProduct();
   }

   addItemToBasket() {
      this.basketService.addItemToBasket(this.product, this.quantity);
   }

   incrementQuantity() {
      this.quantity++;
   }

   decrementQuantity() {
      if (this.quantity > 1) this.quantity--;
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
