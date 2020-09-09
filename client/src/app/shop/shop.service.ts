import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {IPagination} from '../shared/models/pagination';
import {IBrand} from '../shared/models/brand';
import {IType} from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if (shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);

    params = params.append('pageIndex', shopParams.pageNumber.toString());

    params = params.append('pageSize', shopParams.pageSize.toString());

    // tslint:disable-next-line: max-line-length
    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})  // using this {observe: 'response', params} makes an error and it tells us that the response doesn't have data property. What we're doing is we're observing a response. And this is going to give us the http response instead of the body of the response which is what it does automatically if we use this way of getting the data: get('') because we're saying we're observing the response here. We actually need to project this data into our actual response. We need to extract the body out of this.
                                                   // tslint:disable-next-line: max-line-length
      .pipe(                                      // in order to use rxjs methods, then we need to use pipe()
        map(response => {
                                                  // tslint:disable-next-line: max-line-length
          return response.body;                  // we are getting an Observable back fro the http rquest, we can manipulate this Observable and project it into IPagination object, so we get the body of the rsponse and projects that into IPagination object
        })
      );
  }

  // tslint:disable-next-line: typedef
  getProduct(id: number){
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  // tslint:disable-next-line: typedef
  getBrands(){
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  // tslint:disable-next-line: typedef
  getTypes(){
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }
}
