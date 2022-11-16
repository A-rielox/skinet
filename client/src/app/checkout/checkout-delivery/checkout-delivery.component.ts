import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';

@Component({
   selector: 'app-checkout-delivery',
   templateUrl: './checkout-delivery.component.html',
   styleUrls: ['./checkout-delivery.component.scss'],
})
export class CheckoutDeliveryComponent implements OnInit {
   @Input() checkoutForm: FormGroup;
   deliveryMethods: IDeliveryMethod[];

   constructor(private checkoutService: CheckoutService) {}

   ngOnInit(): void {
      this.checkoutService.getDeliveryMethods().subscribe(
         (res) => {
            this.deliveryMethods = res;
            console.log(this.deliveryMethods);
         },
         (error) => console.log(error)
      );
   }

   setShippingPrice(method: any) {}
}
