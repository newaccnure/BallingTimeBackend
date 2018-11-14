import { Component, OnInit } from '@angular/core';
import { PracticeService } from '../services/practice.service';
import { AuthService } from '../services/auth.service';
import { Router } from "@angular/router";
import { Drill } from '../models/drill.model';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-practice',
  templateUrl: './practice.component.html',
  styleUrls: ['./practice.component.css']
})
export class PracticeComponent implements OnInit {

  drills: Array<Drill>;

  constructor(
    private practiceService: PracticeService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getDrills();
  }

  unAuthorized(): void {
    if (!this.authService.isAuthorized()) {
      this.router.navigate(['/home']);
    }
  }

  getDrills(): void {
    this.practiceService.getDrills().subscribe(data => this.drills = data);
  }

  addDrillToCompleted(drill: Drill): void {
    var isCompleted = false;
    this.practiceService.addDrillToCompleted(drill.drillId).subscribe(data =>{
      drill.isCompleted = data;
    });

  }
}
