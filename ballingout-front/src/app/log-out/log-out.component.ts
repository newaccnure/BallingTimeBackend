import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { TranslationService, LocaleService, Language } from 'angular-l10n';
import { Router } from '@angular/router';
@Component({
  selector: 'app-log-out',
  templateUrl: './log-out.component.html',
  styleUrls: ['./log-out.component.css', '../header/header.component.css']
})
export class LogOutComponent implements OnInit {
  
  @Language() lang: string;

  constructor(
    private authService: AuthService,
    public locale: LocaleService, 
    public translation: TranslationService,
    private router : Router) { }

  ngOnInit() {
  }
  
  authorized(): boolean {
    return this.authService.isAuthorized();
  }

  logOut():void{
    this.authService.logOut();
    this.router.navigate(['/home']);
  }
}
