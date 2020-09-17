import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {AccountService} from '../../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router){}

  // this is a router functionality
  canActivate(
    next: ActivatedRouteSnapshot,       // next: is the routing that is trying to be activated
    state: RouterStateSnapshot): Observable<boolean> {   // state: we use it to know where the user is coming from
      // when we are in a context of a router which we are going to be because this is a router functionality. then when we activate the route and we want to observe sth or check what's inside an observable, we don't actually need to subscribe because the router is going to subscribe for us and then unsubscribe
      // if currentUser$ doesn't have a value, then it won't do neither if() or the else()
      return this.accountService.currentUser$.pipe(
        map(auth => {
          if (auth){
            return true;
          }
          this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
        })
      );

    }

}
