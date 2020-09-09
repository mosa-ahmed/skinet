import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import {Router, NavigationExtras} from '@angular/router';
import {catchError} from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router, private toastr: ToastrService){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error){
                    if (error.status === 400){
                        if (error.error.errors){
                            throw error.error;      // we throw it back to the component
                        } else {
                            this.toastr.error(error.error.message, error.error.statusCode);
                        }
                    }

                    if (error.status === 401){
                        this.toastr.error(error.error.message, error.error.statusCode);
                    }

                    if (error.status === 404){
                        this.router.navigateByUrl('/not-found');
                    }

                    if (error.status === 500){
                                                                                                // tslint:disable-next-line: max-line-length
                        const navigationExtras: NavigationExtras = {state: {error: error.error}};   // we are passing the error object of the httpResponse to the ServerErrorComponent
                        this.router.navigateByUrl('/server-error', navigationExtras);
                    }
                }
                return throwError(error);
            }) // in order to make use of this httpInterceptor we need to add this as a provider inside our app.module
        );
    }
}

