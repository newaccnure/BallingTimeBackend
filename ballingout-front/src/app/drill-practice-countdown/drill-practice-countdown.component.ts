import { Component, OnInit, Input } from '@angular/core';
import { Drill } from '../models/drill.model';
import { PracticeService } from '../services/practice.service';
import { CurrentDrillService } from '../services/current-drill.service';
import { Router } from "@angular/router";
@Component({
  selector: 'app-drill-practice-countdown',
  templateUrl: './drill-practice-countdown.component.html',
  styleUrls: ['./drill-practice-countdown.component.css']
})
export class DrillPracticeCountdownComponent implements OnInit {

  time: number;
  drill: Drill;
  myTimer: any;

  constructor(
    private currentDrillService: CurrentDrillService,
    private practiceService: PracticeService,
    private router: Router,
  ) {
    this.addDrillToCompleted = this.addDrillToCompleted.bind(this);

    this.time = 45;
  }

  ngOnInit() {
    this.currentDrillService.currentDrill.subscribe(drill => this.drill = drill);
    var test = this.changeTime.bind(this);
    this.myTimer = setInterval(test, 1000);
  }

  addDrillToCompleted(drill: Drill): void {
    this.practiceService.addDrillToCompleted(drill.drillId).subscribe(data => {
      drill.isCompleted = data;
    });
  }
  changeTime() {
    if (this.time != 0) {
      this.time -= 1
    } else {
      console.log("done")
      clearInterval(this.myTimer)
      this.addDrillToCompleted(this.drill)
      this.router.navigate(['/practice']);
    }
  }
}
