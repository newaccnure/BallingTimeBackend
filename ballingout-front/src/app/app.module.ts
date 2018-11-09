import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { DemoMaterialModule } from './material.module';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AuthMenuComponent } from './auth-menu/auth-menu.component';
import { LogInMenuComponent } from './log-in-menu/log-in-menu.component';
import { ProfileComponent } from './profile/profile.component';
import { LogOutComponent } from './log-out/log-out.component';
import { SignUpComponent } from './sign-up/sign-up.component';

import { AuthService } from './services/auth.service';
import { PracticeService } from './services/practice.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    AuthMenuComponent,
    LogInMenuComponent,
    ProfileComponent,
    LogOutComponent,
    SignUpComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DemoMaterialModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    HttpModule
  ],
  providers: [AuthService, PracticeService],
  bootstrap: [AppComponent]
})
export class AppModule { }
