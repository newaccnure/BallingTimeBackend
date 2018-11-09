import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth-menu',
  templateUrl: './auth-menu.component.html',
  styleUrls: ['../header/header.component.css', './auth-menu.component.css']
})

export class AuthMenuComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {

  }

  unAuthorized(): boolean {
    return !this.authService.isAuthorized();
  }
}
