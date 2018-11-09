import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.model';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { MatListOption } from '@angular/material';

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
  typesOfShoes: string[] = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  user: User;
  confirmPassword: string;
  array: any;
  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.user = new User();
    this.confirmPassword = "";
  }

  signUp() {
    // let key = 'Item 1';
    // localStorage.setItem(key, 'Value');
    //console.log(this.user.Email + ' ' + this.user.Name + ' ' + this.user.Password + ' ' + this.confirmPassword);
    
    console.log(this.array.selectedOptions);
    // this.authService.addUser(this.user, this.confirmPassword).subscribe(data => {
    //   if (data == true) {
    //     console.log('Signed up');
    //   }
    //  });
    // console.log(localStorage.getItem(key));
  }

}
