import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from "@angular/router";
import { MatSnackBar } from '@angular/material';
import { LocaleService, TranslationService, Language } from 'angular-l10n';

@Component({
  selector: 'app-log-in-menu',
  templateUrl: './log-in-menu.component.html',
  styleUrls: ['./log-in-menu.component.css']
})

export class LogInMenuComponent implements OnInit {
  email: string;
  password: string;

  constructor(
    private authService: AuthService,
    private router: Router,
    public snackBar: MatSnackBar,
    public locale: LocaleService, 
    public translation: TranslationService) { }
 

  ngOnInit() {
  }

  selectLanguage(language: string): void {
    if ('locale' in localStorage) {
      let localeLanguage = JSON.parse(localStorage.getItem('locale'));
      this.locale.setCurrentLanguage(localeLanguage);
    } else {
      localStorage.setItem('locale', 'en');
      this.locale.setCurrentLanguage('en');
    }
  }

  logIn() {

    console.log(this.email + ' ' + this.password);

    this.authService.checkUser(this.email, this.password).subscribe(loggedIn => {
      if (loggedIn == true) {
        this.authService.getUserIdByEmail(this.email).subscribe(id => {
          let key = 'currentUser';
          let currentDate = new Date();
          localStorage.setItem(key, JSON.stringify({
            userId: id,
            logInDate: currentDate.toDateString()
          }));
          this.router.navigate(['/home']);
        });
      } else {
        this.snackBar.open("Failed to login. Please try again!", "", {
          duration: 5000,
        });
      }
    });
  }
}

