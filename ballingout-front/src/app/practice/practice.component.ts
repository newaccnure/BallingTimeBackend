import { Component, OnInit } from '@angular/core';
import { PracticeService } from '../services/practice.service';
import { AuthService } from '../services/auth.service';
import { Router } from "@angular/router";
import { Drill } from '../models/drill.model';
import { MatDialog, MatSnackBar } from '@angular/material';
import { DrillResultDialogComponent } from '../drill-result-dialog/drill-result-dialog.component';
import { CurrentDrillService } from '../services/current-drill.service';
import { TranslationService, LocaleService } from 'angular-l10n';

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
    private currentDrillService: CurrentDrillService,
    private router: Router,
    public dialog: MatDialog,
    public locale: LocaleService,
    public translation: TranslationService,
    public snackBar: MatSnackBar
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

  viewDrillResult(drill: Drill) {
    this.dialog.open(DrillResultDialogComponent, {
      width: '350px',
      height: '400px',
      data: drill
    });
  }

  startDrillPractice(drill: Drill) {
    this.practiceService.startDrillPractice(drill).subscribe(data => console.log(data))
    this.currentDrillService.changeDrill(drill);
    this.router.navigate(['/drill-practice']);
  }
}
