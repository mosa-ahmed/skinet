import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './home/home.component';
import {TestErrorComponent} from './core/test-error/test-error.component';
import {ServerErrorComponent} from './core/server-error/server-error.component';
import {NotFoundComponent} from './core/not-found/not-found.component';
import {AuthGuard} from './core/guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  {path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'}},
  // all below are lazy loaded routes
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule), data: {breadcrumb: 'Shop'}},  // this is how we deel with lazy loading. Now our ShopModule is going to be activated and loaded when we access the shop path What this also means is that we can go to our app.module and we no longer need to add the ShopModule to the imports here and this also means that we no longer need in our ShopModule to export our shop component because our app.module is no longer responsible for loading this particular component.It's our ShopModule that's now gonna be responsible for this and it already has it in its declarations
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule), data: {breadcrumb: 'Basket'}},
  {path: 'checkout', canActivate: [AuthGuard], loadChildren: () => import('./checkout/checkout.module')
          .then(mod => mod.CheckoutModule), data: {breadcrumb: 'Checkout'}},
  {path: 'account', loadChildren: () => import('./account/account.module')
          .then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}}}, // {skip: true} this will prevent our Breadcrumb service from giving this a breadcrumb automatically, we just tell it to skip this one because we are not goona have aacount route we will have account/login or account/register but not just account/
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
