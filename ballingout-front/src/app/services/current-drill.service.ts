import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Drill } from '../models/drill.model';

@Injectable({
  providedIn: 'root'
})
export class CurrentDrillService {
  private messageSource = new BehaviorSubject(new Drill());
  currentDrill = this.messageSource.asObservable();

  constructor() { }

  changeDrill(drill: Drill) {
    this.messageSource.next(drill);
  }
}

