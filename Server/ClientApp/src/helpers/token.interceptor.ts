import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from "../services/account.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public accountService: AccountService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.accountService.currentUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.accountService.currentUser.Token}`
        }
      });
    }
    return next.handle(request);
  }
}
