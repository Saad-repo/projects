import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from '../app.component';
import { environment } from '../../environments/environment';
import { LoginService } from '../login.service';
import {CookieService} from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private baseUrl = environment.apiUrl;
  loginForm: any;
  registerForm: any;
  loading = false;
  submitted = false;
  isLoginMode: boolean = true; // [ Login | Register ]
  registrationMsg: any;
  showLoginForm: boolean = true;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginService,
    private route: ActivatedRoute,
    private cookieService: CookieService
  ) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(10)]]
    });

    // console.log('inside ngOnInit of login component');
    // console.log(this.route.snapshot.paramMap);
    
    const action = this.route.snapshot.paramMap.get('action');
    if(action && action == "logout") {
      this.deleteCookie('uid');
      return;
    }

    const actionId = this.route.snapshot.paramMap.get('id');
    if(actionId) {
      if(actionId == "zx5TK") {
        this.registrationMsg = "Account activated. Please login.";
      }
      else if(actionId == "zx5Te") {
        this.registrationMsg = "Account activation failed.";
        this.showLoginForm = false;
      }
      else if(actionId == "Xp5TK") {
        this.registrationMsg = "Session expired.";
        this.showLoginForm = true;
      }
    }
  }
  

  onSubmit() {
    this.submitted = true;
    // console.log('signing in...');
    
    // reset alerts on submit
    // this.alertService.clear();
    // stop here if form is invalid
    //console.log(this.loginForm.controls.username.error);
    //this.loginForm.controls.username.error = "test";
    //console.log(this.loginForm.controls.username.error);
    // console.log(this.loginForm);


    if (this.loginForm.invalid) {
        return;
    }

    this.loading = true;  

    this.loginService.login(this.loginForm.controls.username.value, this.loginForm.controls.password.value)
                 .subscribe(u=>{
                  // console.log(u);
                  LoginService.isSignedIn = true;
                  this.setCookieUid(u);
                  this.router.navigate(['/dashboard',{id:"Kx5TK"}]);
      }, error=> {
          LoginService.isSignedIn = false;
          this.registrationMsg = error.error;
      });


  }

  setCookieUid(u: any){
    let expDt = new Date();
    expDt.setMinutes(expDt.getMinutes() + 45);
    this.cookieService.set('uid', (u as ProjectUsers).activationCode, expDt);
  }

  deleteCookie(name: string){
    this.cookieService.delete(name);
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }



}

export interface ProjectUsers {
	userId: number;
	email: string;
	fullName: string;
	userName: string;
	description: string;
	roles: string;
	createDt: Date;
	activationCode: string;
	isActive: boolean;
}
