import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

import { User } from '../models/user.model';
@Injectable()
export class AuthService {

  constructor(private http: HttpClient) { }

  isAuthorized(): boolean {
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let currentDate = new Date()
      return currentUser.logInDate == currentDate.toDateString();
    }
    return false;
  }

  addUser(user: User, confirmPassword: string): Observable<Object> {
    let requestUrl: string = environment.apiUrl + "/user/addUser";
    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

    let body = new HttpParams();
    body = body.set('email', user.Email);
    body = body.set('password', user.Password);
    body = body.set('name', user.Name);
    body = body.set('practiceDays', JSON.stringify(user.PracticeDays));
    body = body.set('checkPassword', confirmPassword);

    return this.http
      .post(requestUrl, body, { headers: myheader });
  }

  checkUser(email: string, password: string): Observable<Object> {
    let requestUrl: string = environment.apiUrl + "/user/checkUser";

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

    let body = new HttpParams();
    body = body.set('email', email);
    body = body.set('password', password);

    return this.http
      .post(requestUrl, body, { headers: myheader });
  }

  getUserIdByEmail(email: string): Observable<Object> {
    let requestUrl: string = environment.apiUrl + "/user/getUserIdByEmail";

    const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

    let body = new HttpParams();
    body = body.set('email', email);

    return this.http
      .post(requestUrl, body, { headers: myheader });
  }

  logOut() {
    localStorage.removeItem('currentUser');
  }
}