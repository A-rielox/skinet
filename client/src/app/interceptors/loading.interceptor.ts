import { Injectable } from '@angular/core';
import {
   HttpRequest,
   HttpHandler,
   HttpEvent,
   HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators';
import { BusyService } from '../core/services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
   constructor(private busyService: BusyService) {}

   intercept(
      request: HttpRequest<any>,
      next: HttpHandler
   ): Observable<HttpEvent<any>> {
      this.busyService.busy();

      return next.handle(request).pipe(
         delay(2000),
         finalize(() => {
            this.busyService.idle();
         })
      );
   }
}

// con next agarro la respuesta q llega

// para usarlo en app.module :
// providers: [
//    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
// ],

// spinner configurado en

// BusyService y BusyService
