import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Language, TranslationService, LocaleService } from 'angular-l10n';

@Component({
  selector: 'app-auth-menu',
  templateUrl: './auth-menu.component.html',
  styleUrls: ['../header/header.component.css', './auth-menu.component.css']
})

export class AuthMenuComponent implements OnInit {

  @Language() lang: string;
  
  constructor(
    private authService: AuthService,
    public locale: LocaleService, 
    public translation: TranslationService) { }

  ngOnInit() {

  }
  unAuthorized(): boolean {
    return !this.authService.isAuthorized();
  }
}
