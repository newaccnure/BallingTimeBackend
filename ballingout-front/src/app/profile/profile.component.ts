import { Component, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service';
import { ProfileInfoService } from '../services/profile-info.service';
import { Router } from "@angular/router";
import * as Highcharts from 'highcharts';
import { UserStats } from '../models/user-stats.model';



@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  Highcharts = Highcharts;
  averageSpeedUpdateFlag: boolean;
  repsPerSecondUpdateFlag: boolean;
  accuracyUpdateFlag: boolean;

  averageSpeedChartOptions = {
    plotOptions: {
      series: {
        cursor: 'pointer',
        point: {
          events: {
            click: function () {
              alert('Category: ' + this.category + ', value: ' + this.y);
            }
          }
        }
      }
    },
    series: [
      { name: 'Average speed', data: [] }
    ],
    title: {
      text: 'Average speed during practice'
    },
    xAxis: {
      categories:
        []
      ,
      title: {
        text: 'Practice day'
      }
    },
    yAxis: {
      title: {
        text: 'Speed, m/s'
      }
    }
  };

  repsPerSecondChartOptions = {
    plotOptions: {
      series: {
        cursor: 'pointer',
        point: {
          events: {
            click: function () {
              alert('Category: ' + this.category + ', value: ' + this.y);
            }
          }
        }
      }
    },
    series: [
      { name: 'Repetitions per second', data: [] }
    ],
    title: {
      text: 'Repetitions per second during practice'
    },
    xAxis: {
      categories:
        []
      ,
      title: {
        text: 'Practice day'
      }
    },
    yAxis: {
      title: {
        text: 'Reps per sec'
      }
    }
  };

  accuracyChartOptions = {
    plotOptions: {
      series: {
        cursor: 'pointer',
        point: {
          events: {
            click: function () {
              alert('Category: ' + this.category + ', value: ' + this.y);
            }
          }
        }
      }
    },
    series: [
      { name: 'Accuracy', data: [] }
    ],
    title: {
      text: 'Accuracy during practice'
    },
    xAxis: {
      categories:
        []
      ,
      title: {
        text: 'Practice day'
      }
    },
    yAxis: {
      title: {
        text: 'Accuracy, m/s'
      }
    }
  };
  stats: Array<UserStats>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private profileInfoService: ProfileInfoService) {

  }


  ngOnInit() {
    this.profileInfoService.getUserStatsById().subscribe(stats => {
      stats.forEach(stat => {

        this.accuracyChartOptions.series[0].data.push(stat.averageAccuracy);
        this.repsPerSecondChartOptions.series[0].data.push(stat.averageRepsPerSec);
        this.averageSpeedChartOptions.series[0].data.push(stat.averageSpeed);
        console.log(new Date(stat.practiceDay).toDateString())
        this.accuracyChartOptions.xAxis.categories.push(new Date(stat.practiceDay).toDateString());
        this.repsPerSecondChartOptions.xAxis.categories.push(new Date(stat.practiceDay).toDateString());
        this.averageSpeedChartOptions.xAxis.categories.push(new Date(stat.practiceDay).toDateString());

      })
      this.averageSpeedUpdateFlag = true;
      this.accuracyUpdateFlag = true;
      this.repsPerSecondUpdateFlag = true;
    });
  }

  unAuthorized() {
    if (!this.authService.isAuthorized()) {
      this.router.navigate(['/home']);
    }
  }
  getData() {
    console.log(this.accuracyChartOptions.series[0].data)
  }
}
