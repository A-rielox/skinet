import { Injectable } from '@angular/core';
import {
   HttpRequest,
   HttpHandler,
   HttpEvent,
   HttpInterceptor,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
   constructor(private router: Router, private toastr: ToastrService) {}

   intercept(
      request: HttpRequest<any>,
      next: HttpHandler
   ): Observable<HttpEvent<any>> {
      return next.handle(request).pipe(
         catchError((error) => {
            if (error) {
               if (error.status === 400) {
                  if (error.error.errors) {
                     // p' mandar MI error al componente
                     // {errors: Array(1), statusCode: 400, message: 'A bad request, you have made'}
                     // alla agarro el array de errores de validacion
                     throw error.error;
                  } else {
                     this.toastr.error(
                        error.error.message,
                        error.error.statusCode
                     );
                  }
               }

               if (error.status === 401) {
                  this.toastr.error(
                     error.error.message,
                     error.error.statusCode
                  );

                  // this.toastr.error(error.statusText === 'OK' ? 'Unauthorised' : error.statusText, error.status);
               }

               if (error.status === 404) {
                  this.router.navigateByUrl('/not-found');
               }

               if (error.status === 500) {
                  // yellow 🟡
                  // p' poder pasar un state a la ruta a la q se va a navegar
                  // NavigationExtras SOLO se pueden "sacar" en el constructor ( en este caso en el de ServerErrorComponent )
                  // error.error tiene el error ( object ) que yo mando del back
                  const navigationExtras: NavigationExtras = {
                     state: { error: error.error },
                  };
                  this.router.navigateByUrl('/server-error', navigationExtras);
               }
            }

            // return throwError(error);
            return throwError(() => error);
         })
      );
   }
}

// con next agarro la respuesta q llega

// para usarlo en app.module :
// providers: [
//    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
// ],
