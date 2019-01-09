import { Component, OnInit } from '@angular/core';
import { TranslationService, LocaleService, Language } from 'angular-l10n';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  
  constructor(
    public locale: LocaleService, 
    public translation: TranslationService) { }

  ngOnInit() {
  }
  
  onValChange(value:string){
    localStorage.setItem('locale', value);
    this.locale.setCurrentLanguage(value);
  }
}
