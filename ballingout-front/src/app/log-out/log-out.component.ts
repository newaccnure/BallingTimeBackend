import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
@Component({
  selector: 'app-log-out',
  templateUrl: './log-out.component.html',
  styleUrls: ['./log-out.component.css', '../header/header.component.css']
})
export class LogOutComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  
  authorized(): boolean {
    return this.authService.isAuthorized();
  }

  logOut():void{
    this.authService.logOut();
  }
}
