import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Drill } from '../models/drill.model';
import { DrillStats } from '../models/drill-stats.model';

@Injectable({
  providedIn: 'root'
})
export class PracticeService {

  constructor(private http: HttpClient) { }

  checkDayOfPractice() {
    //var id = 
  }

  startDrillPractice(drill:Drill):Observable<boolean> {
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/startDrillPractice";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);
      body = body.set('drillId', drill.drillId.toString());

      return this.http.post<boolean>(requestUrl, body, { headers: myheader })
    }
    return null;
  }

  getDrills(): Observable<Array<Drill>> {
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

  addDrillToCompleted(drillId: number): Observable<boolean> {
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/addDrillToCompleted";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);
      body = body.set('drillId', drillId.toString());

      body = body.set('averageSpeed', (Math.random() * 5 + 10).toFixed(2).toString());
      body = body.set('averageAccuracy', (Math.random() * 0.5 + 0.5).toFixed(2).toString());
      body = body.set('repeatitionsPerSecond', (Math.random() * 0.5 + 0.5).toFixed(2).toString());

      return this.http.post<boolean>(requestUrl, body, { headers: myheader })
    }
    return null;
  }

  getDrillStatsById(drillId: number): Observable<DrillStats> {
    if ('currentUser' in localStorage) {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      let requestUrl: string = environment.apiUrl + "/practice/getDrillStatsById";
      const myheader = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

      let body = new HttpParams();
      body = body.set('userId', currentUser.userId);
      body = body.set('drillId', drillId.toString());

      return this.http.post<DrillStats>(requestUrl, body, { headers: myheader })
    }
    return null;
  }
}
