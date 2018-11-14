import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Drill } from '../models/drill.model';
@Injectable({
  providedIn: 'root'
})
export class PracticeService {

  constructor(private http:HttpClient) { }

  checkDayOfPractice(){
    //var id = 
  }

  getDrills(): Observable<Array<Drill>>{
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/getFullTrainingProgramById";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);

      return this.http.post<Array<Drill>>(requestUrl, body, { headers: myheader })
    }
    return null;
  }

  addDrillToCompleted(drillId:number): Observable<boolean>{
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/addDrillToCompleted";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);
      body = body.set('drillId', drillId.toString());
      body = body.set('averageSpeed', "10");
      body = body.set('averageAccuracy', "0.8");
      body = body.set('repeatitionsPerSecond', "0.8");

      return this.http.post<boolean>(requestUrl, body, { headers: myheader })
    }
    return null;
  }
}
