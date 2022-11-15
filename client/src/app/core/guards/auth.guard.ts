import { Injectable } from '@angular/core';
import {
   ActivatedRouteSnapshot,
   CanActivate,
   Router,
   RouterStateSnapshot,
   UrlTree,
} from '@angular/router';
import { map, Observable, of } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
   providedIn: 'root',
})
export class AuthGuard implements CanActivate {
   constructor(
      private accountService: AccountService,
      private router: Router
   ) {}

   canActivate(
      next: ActivatedRouteSnapshot,
      state: RouterStateSnapshot
   ): Observable<boolean> {
      // 🟡 cuando estoy dentro del router (como en esta guard) no necesito hacer ni subscribe ni unsubscribe, lo hace el router

      return this.accountService.currentUser$.pipe(
         map((auth) => {
            if (auth) {
               return true;
            }

            this.router.navigate(['account/login'], {
               // lo namdo al "login" con un queryParam "?returnUrl=la-url-en-la-q-está"
               queryParams: { returnUrl: state.url },
            });
         })
      );
   }
}
