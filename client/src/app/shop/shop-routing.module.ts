import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {Routes, RouterModule} from '@angular/router';
import {ShopComponent} from './shop.component';
import {ProductDetailsComponent} from './product-details/product-details.component';

const routes: Routes = [
  {path: '', component: ShopComponent},
  {path: ':id', component: ProductDetailsComponent, data: {breadcrumb: {alias: 'productDetails'}}},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)   // this means that these routes are not available in our app.module.ts and only available in shop.module
  ],
  exports: [RouterModule]   // we added this to be able to use it insisde shop.module
})
export class ShopRoutingModule { }
