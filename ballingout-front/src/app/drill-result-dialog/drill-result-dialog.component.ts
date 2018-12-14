import { Component, OnInit } from '@angular/core';
import { Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Drill } from '../models/drill.model';
import { DrillStats } from '../models/drill-stats.model';
import { TIPS } from '../constants/tips.model';
import { PracticeService } from '../services/practice.service';

@Component({
  selector: 'app-drill-result-dialog',
  templateUrl: './drill-result-dialog.component.html',
  styleUrls: ['./drill-result-dialog.component.css']
})
export class DrillResultDialogComponent implements OnInit {
  tips = TIPS;
  currentTip: string;
  drillStats: DrillStats;
  drillName: string;

  constructor(
    public dialogRef: MatDialogRef<DrillResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public drill: Drill,
    private practiceService: PracticeService) { }

  ngOnInit() {
    this.drillName = this.drill.name;

    this.practiceService.getDrillStatsById(this.drill.drillId).subscribe(stats => {
      this.drillStats = stats;
      var tipsLength = TIPS.length;
      var randomIndex = Math.floor(Math.random() * tipsLength);
      this.currentTip = TIPS[randomIndex];
    });

  }

  onOkClick(): void {
    this.dialogRef.close();
  }

}