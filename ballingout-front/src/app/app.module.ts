import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { DemoMaterialModule } from './material.module';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { CountdownTimerModule } from 'ngx-countdown-timer';

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
import { DrillPracticeCountdownComponent } from './drill-practice-countdown/drill-practice-countdown.component';

import { AuthService } from './services/auth.service';
import { PracticeService } from './services/practice.service';
import { CurrentDrillService } from './services/current-drill.service';
import { ProfileInfoService } from './services/profile-info.service';

import { L10nConfig, L10nLoader, LocalizationModule, StorageStrategy, ProviderType, LogLevel } from 'angular-l10n';

const l10nConfig: L10nConfig = {
  logger: {
    level: LogLevel.Warn
  },
  locale: {
    languages: [
      { code: 'en', dir: 'ltr' },
      { code: 'ua', dir: 'ltr' }
    ],
    language: 'en',
    storage: StorageStrategy.Cookie
  },
  translation: {
    providers: [
      { type: ProviderType.Static, prefix: './assets/locale-' }
    ],
    caching: true,
    missingValue: 'No key'
  }
};

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
    CountdownTimerModule,
    LocalizationModule.forRoot(l10nConfig)
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
export class AppModule {
  constructor(public l10nLoader: L10nLoader) {
    this.l10nLoader.load();
  }
}
