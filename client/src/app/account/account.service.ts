import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
   providedIn: 'root',
})
export class AccountService {
   baseUrl = environment.apiUrl;
   // private currentUserSource = new BehaviorSubject<IUser>(null);
   // 👆 si estoy logeado al darle reload a la pagina el 1er valor q emite es null xlo q me patea al login 👇 va a guardar el ultimo valor emitido y va a entregar ese ( (1) p'q guarde solo un ultimo valor )
   // la clave esta en q NO emite un valor inicial, xlo q el authGuard en lugar de encontrar null y patearme, se va a esperar a q tenga un valor al darle reload
   private currentUserSource = new ReplaySubject<IUser>(1);
   currentUser$ = this.currentUserSource.asObservable();

   constructor(private http: HttpClient, private router: Router) {}

   loadCurrentUser(token: string) {
      if (token === null) {
         this.currentUserSource.next(null);

         // xq tiene q devolver un observable p'los q se suscriben a este
         return of(null);
      }

      let headers = new HttpHeaders();
      headers = headers.set('Authorization', `Bearer ${token}`);

      return this.http.get(this.baseUrl + 'account', { headers }).pipe(
         map((user: IUser) => {
            if (user) {
               // recibo un nuevo token
               localStorage.setItem('token', user.token);
               this.currentUserSource.next(user);
            }
            // this.currentUserSource.next(user);
         })
      );
   }

   login(values: any) {
      return this.http.post(this.baseUrl + 'account/login', values).pipe(
         map((user: IUser) => {
            if (user) {
               localStorage.setItem('token', user.token);
            }

            this.currentUserSource.next(user);
         })
      );
   }

   register(values: any) {
      return this.http.post(this.baseUrl + 'account/register', values).pipe(
         map((user: IUser) => {
            if (user) {
               localStorage.setItem('token', user.token);
            }

            this.currentUserSource.next(user);
         })
      );
   }

   logout() {
      localStorage.removeItem('token');
      this.currentUserSource.next(null);
      this.router.navigateByUrl('/');
   }

   checkEmailExists(email: string) {
      return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
   }
}
