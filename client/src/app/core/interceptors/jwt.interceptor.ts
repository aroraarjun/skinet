import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  token?: string;

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // we used take(1) because we want to unsubscribe to it and once we have one these current user observables
    // then our request is completed 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.token = user?.token
    })

    if(this.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.token}`
        }
      })
    }
    return next.handle(request);
  }
}
