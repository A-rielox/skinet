import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
   providedIn: 'root',
})
export class ShopService {
   baseUrl = 'https://localhost:5001/api/';

   constructor(private http: HttpClient) {}

   getProducts(shopParams: ShopParams) {
      const { brandId, typeId, sort, pageNumber, pageSize, search } =
         shopParams;
      let params = new HttpParams();

      // 0 es FALSY => SI VIENE COMO brandId o typeId => no pone params
      // => es solo if(brandId)
      if (brandId !== 0) {
         params = params.append('brandId', brandId.toString());
      }

      if (typeId !== 0) {
         params = params.append('typeId', typeId.toString());
      }

      if (search) {
         params = params.append('search', search);
      }

      params = params.append('sort', sort);
      params = params.append('pageIndex', pageNumber.toString());
      params = params.append('pageIndex', pageSize.toString());

      return this.http
         .get<IPagination>(this.baseUrl + 'products', {
            observe: 'response',
            params,
         })
         .pipe(
            map((res) => {
               return res.body;
            })
         );
   }

   getBrands() {
      return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
   }

   getTypes() {
      return this.http.get<IType[]>(this.baseUrl + 'products/types');
   }
}
