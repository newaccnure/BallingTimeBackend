import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { jsonpCallbackContext } from '@angular/common/http/src/module';

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

  addUser(user: User, confirmPassword: number): Observable<Object> {
    let requestUrl: string = environment.apiUrl + "/user/addUser";

    const body = {
      email: user.Email,
      password: user.Password,
      name: user.Name,
      checkPassword: user.Password,
      practiceDays: user.PracticeDays
    };
    return this.http
      .post(requestUrl, body);
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