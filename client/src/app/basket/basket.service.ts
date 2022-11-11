import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
   Basket,
   IBasket,
   IBasketItem,
   IBasketTotals,
} from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
   providedIn: 'root',
})
export class BasketService {
   // en app.component agarro la basket de local storage
   baseUrl = environment.apiUrl;
   private basketSource = new BehaviorSubject<IBasket>(null);
   basket$ = this.basketSource.asObservable();
   private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
   basketTotals$ = this.basketTotalSource.asObservable();

   constructor(private http: HttpClient) {}

   // los componentes agarran el basket del observable basket$, los metodos hacen el request y lo meten ahi
   // voy a ocupar el async pipe " | " para hacer la suscripcion y traer el valor
   getBasket(id: string) {
      return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
         map((basket: IBasket) => {
            this.basketSource.next(basket);

            // calcula el valor del basket
            this.calculateTotals();
         })
      );
   }

   // hago la suscripcion, p'q al llamar al metodo haga el req
   setBasket(basket: IBasket) {
      return this.http.post(this.baseUrl + 'basket', basket).subscribe({
         next: (res: IBasket) => {
            this.basketSource.next(res);
            // calcula el valor del basket
            this.calculateTotals();
         },
         error: (err) => this.basketSource.next(err),
      });
   }

   // para obtener el valor sin tener q subscibirme a algo
   getCurrentBasketValue() {
      return this.basketSource.value;
   }

   ////////////////////////////
   ///////////////////////
   addItemToBasket(item: IProduct, quantity = 1) {
      const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(
         item,
         quantity
      );

      const basket = this.getCurrentBasketValue() ?? this.createBasket();

      basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

      this.setBasket(basket);
   }

   private addOrUpdateItem(
      items: IBasketItem[],
      itemToAdd: IBasketItem,
      quantity: number
   ): IBasketItem[] {
      const index = items.findIndex((i) => i.id === itemToAdd.id);

      if (index === -1) {
         itemToAdd.quantity = quantity; //segun yo esta ya esta con el map
         items.push(itemToAdd);
      } else {
         items[index].quantity += quantity;
      }

      return items;
   }

   private createBasket(): IBasket {
      const basket = new Basket();
      localStorage.setItem('basket_id', basket.id);

      return basket;
   }

   private mapProductItemToBasketItem(
      item: IProduct,
      quantity: number
   ): IBasketItem {
      return {
         id: item.id,
         productName: item.name,
         price: item.price,
         pictureUrl: item.pictureUrl,
         quantity,
         brand: item.productBrand,
         type: item.productType,
      };
   }

   ////////////////////////////
   ///////////////////////
   private calculateTotals() {
      const basket = this.getCurrentBasketValue();
      const shipping = 0;
      const subtotal = basket.items.reduce((acc, curr) => {
         acc = curr.price * curr.quantity + acc;

         return acc;
      }, 0);

      const total = subtotal + shipping;

      this.basketTotalSource.next({ shipping, subtotal, total });
   }
}
