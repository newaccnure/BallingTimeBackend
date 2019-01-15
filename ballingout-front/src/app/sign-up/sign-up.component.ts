import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.model';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { MatListOption } from '@angular/material';
import { FormControl } from '@angular/forms';
import { Router } from "@angular/router";
import { MatSnackBar } from '@angular/material';
import { Language } from 'angular-l10n';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: [
    './sign-up.component.css',
    '../log-in-menu/log-in-menu.component.css'
  ],
  providers: [AuthService]
})
export class SignUpComponent implements OnInit {

  daysOfWeek: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  daysOfWeekNumbers: number[] = [1, 2, 3, 4, 5, 6, 0];
  user: User;
  confirmPassword: string;
  array: any;
  @Language() lang: string;
  
  constructor(
    private authService: AuthService,
    private router: Router,
    public snackBar: MatSnackBar) { }

  ngOnInit() {
    this.user = new User();
    this.confirmPassword = "";
  }

  signUp() {
    
    var indexArray = [];
    this.array.forEach((element: string) => {
      indexArray.push(this.daysOfWeekNumbers[this.daysOfWeek.indexOf(element)])
    });
    this.user.PracticeDays = indexArray;
    console.log(this.user.PracticeDays);
    this.authService.addUser(this.user, this.confirmPassword).subscribe(data => {
      if (data == true) {
        this.authService.getUserIdByEmail(this.user.Email).subscribe(id => {
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
