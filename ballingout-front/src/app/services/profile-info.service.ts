import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { UserStats } from '../models/user-stats.model';
@Injectable({
  providedIn: 'root'
})
export class ProfileInfoService {

  constructor(private http:HttpClient) { }

  getUserStatsById(): Observable<Array<UserStats>>{
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/getUserStatsById";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);

      return this.http.post<Array<UserStats>>(requestUrl, body, { headers: myheader })
    }
    return null;
  }
}
