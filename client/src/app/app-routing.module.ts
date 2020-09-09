import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './home/home.component';
import { ShopComponent } from './shop/shop.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
  // tslint:disable-next-line: max-line-length
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule)},  // this is how we deel with lazy loading. Now our ShopModule is going to be activated and loaded when we access the shop path What this also means is that we can go to our app.module and we no longer need to add the ShopModule to the imports here and this also means that we no longer need in our ShopModule to export our shop component because our app.module is no longer responsible for loading this particular component.It's our ShopModule that's now gonna be responsible for this and it already has it in its declarations
  {path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
