import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import {finalize, delay} from 'rxjs/operators';


@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!req.url.includes('emailexists')){      // that will turn off the laoding spinner when we do the async valiation for email exists but we also want to show sth when we are handling an async request so we go to the input-text.html and check the control status
            this.busyService.busy();
        }
        return next.handle(req).pipe(
            delay(1000),
            finalize(() => {
                this.busyService.idle();
            })
        );
    }
}
