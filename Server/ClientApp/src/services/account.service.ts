import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Router } from "@angular/router";
import jwtDecode from "jwt-decode";

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentUserSource = new BehaviorSubject<User>(null);

  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.setCurrentUser(user);
  }

  public get currentUser(): User {
    return this.currentUserSource.value;
  }

  login(model: any): Observable<void> {
    return this.http.post('/api/korisnik/login', model, { responseType: 'text' }).pipe(
      map((token: string) => {
        const _user: any = jwtDecode(token);
        const user = new User();
        user.Tip = _user.tip;
        user.Username = _user.username;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);
        }
      }),
    );
  }

  register(model: any): Observable<void> {
    return this.http.post('/api/korisnik/register', model, { responseType: 'text' }).pipe(
      map((token: string) => {
        const _user: any = jwtDecode(token);
        const user = new User();
        user.Tip = _user.tip;
        user.Username = _user.username;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);
        }
      }),
    );
  }

  setCurrentUser(user: User): void {
    this.currentUserSource.next(user);
  }

  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.router.navigate([''])
  }
}
