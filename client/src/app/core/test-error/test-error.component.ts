import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
   selector: 'app-test-error',
   templateUrl: './test-error.component.html',
   styleUrls: ['./test-error.component.scss'],
})
export class TestErrorComponent implements OnInit {
   validationErrors: any;
   baseUrl = environment.apiUrl;

   constructor(private http: HttpClient) {}

   ngOnInit(): void {}

   get404Error() {
      console.log('get404Error');

      this.http.get(this.baseUrl + 'products/777').subscribe({
         next: (res) => console.log(res),
         error: (err) => console.log(err),
      });
   }

   get500Error() {
      console.log('get500Error');

      this.http.get(this.baseUrl + 'Buggy/ServerError').subscribe({
         next: (res) => console.log(res),
         error: (err) => console.log(err),
      });
   }

   get400Error() {
      console.log('get400Error');

      this.http.get(this.baseUrl + 'Buggy/BadRequest').subscribe({
         next: (res) => console.log(res),
         error: (err) => console.log(err),
      });
   }

   get400ValidationError() {
      console.log('get400ValidationError');

      this.http.get(this.baseUrl + 'products/jajajajaj').subscribe({
         next: (res) => console.log(res),
         error: (err) => {
            this.validationErrors = err.errors;
            // el error que mando del if en el interceptor
            // console.log(this.validationErrors);
            // {errors: Array(1), statusCode: 400, message: 'A bad request, you have made'}
         },
      });
   }
}

//  yellow 🟡 los errores
//  los agarro con interceptor : ErrorInterceptor
//  yellow 🟡
