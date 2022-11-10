import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

// { alias: 'productDetails' } en el product-details le doy el alias p' lo que va a desplegar en el breadcrumb
const routes: Routes = [
   { path: '', component: ShopComponent },
   {
      path: ':id',
      component: ProductDetailsComponent,
      data: { breadcrumb: { alias: 'productDetails' } },
   },
];

@NgModule({
   declarations: [],
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule],
})
export class ShopRoutingModule {}

// yellow 🟡
// imports: [RouterModule.forChild(routes)],
// en lugar de:
// imports: [RouterModule.forRoot(routes)]

// p'q no esten disponibles en app.module y SOLO en el shop.module
// yellow
