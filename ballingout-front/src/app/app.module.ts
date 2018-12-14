import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { DemoMaterialModule } from './material.module';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AuthMenuComponent } from './auth-menu/auth-menu.component';
import { LogInMenuComponent } from './log-in-menu/log-in-menu.component';
import { ProfileComponent } from './profile/profile.component';
import { LogOutComponent } from './log-out/log-out.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { PracticeComponent } from './practice/practice.component';
import { DrillResultDialogComponent } from './drill-result-dialog/drill-result-dialog.component';
import { HighchartsChartComponent } from 'highcharts-angular';

import { AuthService } from './services/auth.service';
import { PracticeService } from './services/practice.service';
import { CountdownTimerModule } from 'ngx-countdown-timer';
import { DrillPracticeCountdownComponent } from './drill-practice-countdown/drill-practice-countdown.component';
import { CurrentDrillService } from './services/current-drill.service';
import { ProfileInfoService } from './services/profile-info.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    AuthMenuComponent,
    LogInMenuComponent,
    ProfileComponent,
    HighchartsChartComponent,
    LogOutComponent,
    SignUpComponent,
    PracticeComponent,
    DrillResultDialogComponent,
    DrillPracticeCountdownComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DemoMaterialModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    HttpModule,
    ReactiveFormsModule,
    CountdownTimerModule
  ],
  providers: [
    AuthService, 
    PracticeService, 
    CurrentDrillService,
    ProfileInfoService
  ],
  entryComponents: [DrillResultDialogComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
