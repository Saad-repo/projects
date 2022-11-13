import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from '../app.component';
import {matchPasswordValidator} from '../password.validator'
import { LoginService } from '../login.service';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../login/login.component.css']
})
export class RegisterComponent implements OnInit {

  loginForm: any;
  registerForm: any;
  loading = false;
  submitted = false;
  isLoginMode: boolean = true; // [ Login | Register ]
  activatedUser: any;
  registrationSubmitMsg: any;
  sentToEmailAddr: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private loginService: LoginService,
  ) { }
  

  ngOnInit(): void {
    // this.loginForm = this.formBuilder.group({
    //   fullName: ['', Validators.required],
    //   username: ['', [Validators.required, Validators.email]],
    //   password: ['', [Validators.required, Validators.minLength(10)]],
    //   confirm:  ['', [Validators.required, Validators.minLength(10)]]
    // });

    //console.log("DEBUG-info: get activation code...");

    this.route.params.subscribe(p=>{
      //console.log("DEBUG: register comp url parameter: ", p);
      if(p.activationCode != "form") { 
        this.loginService.activateAccount(p.activationCode).subscribe(q => {
           //console.log("DEBUG-info: out of activate service",q);
          if(q && q.fullName){
             //console.log("DEBUG-info: settting to true");
            this.activatedUser = q.fullName;
            this.router.navigate(['/login',{id:"zx5TK"}]);
            return;
          }
          else {
            //console.log("DEBUG-info: else block");
            this.activatedUser = null;
            this.router.navigate(['/login',{id:"zx5Te"}]);
          }
      });
    }
  });
   

    this.loginForm = new FormGroup({
      fullName: new FormControl('', Validators.required),
      username: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(10)]),
      confirm:  new FormControl('', [Validators.required, Validators.minLength(10)])
    }, 
    { 
      validators: matchPasswordValidator
    });

  }
  
  onSubmit() {
    this.submitted = true;
    // reset alerts on submit
    // this.alertService.clear();
    //console.log(this.loginForm.controls.confirm.errors);
    // stop here if form is invalid
    if (this.loginForm.invalid) {
        return;
    }
    this.loading = true;

    //console.log("registering...");
    const email: string = this.loginForm.controls.username.value;
    this.loginService.register(this.loginForm.controls.fullName.value, email, this.loginForm.controls.password.value)
                 .subscribe(u=>{
                  //console.log("DEBUG: respns frm loginSrvc.register", u);
                    //this.router.navigate(['/dashboard',{id:"zx5TK"}]);
                    //console.log("registering2...", u);
                  if(u){
                    //console.log("DEBUG: Yes, API.singup returned 200 u", u);
                    this.sentToEmailAddr = email;
                    this.registrationSubmitMsg = `Account activation link sent to `;
                    LoginService.isSignedIn = false;
                    this.startTimer();
                  }
                  else 
                    this.loading = false;
        }, e => {
          //console.log("DEBUG: Error in register function", e);
          this.sentToEmailAddr = null;
          if(e.status = 300) {
            this.registrationSubmitMsg = e.error;
          }
          else 
            console.log("loginsrvc returned error",e);
        });


  }

  timerMins: number = 45;
  timeLeft: number = this.timerMins * 60;
  interval: any;
  timeLeftMsg: string = `expires in ${this.timerMins} minutes`;

  startTimer() {
    this.interval = setInterval(() => {
      if(this.timeLeft > 0) {
        this.timeLeft--;
        const mins = Math.floor(this.timeLeft / 60);
        const secs = this.timeLeft % 60;
        let andPart = `and`;
        if(mins == 1)
          this.timeLeftMsg = `expires in ${mins} minute`;
        else  if(mins == 0) {
          andPart = '';
          this.timeLeftMsg = `expires in `;
        }
        else 
          this.timeLeftMsg = `expires in ${mins} minutes`;

        if(secs == 1)
          this.timeLeftMsg += ` ${andPart} ${secs} second`;
        else if(secs > 0)
          this.timeLeftMsg += ` ${andPart} ${secs} seconds`;
        else if (secs == 0)
          this.timeLeftMsg = `expired`;
        
      } else {
        // this.timeLeft = 60;
        clearInterval(this.interval);
        this.timeLeftMsg = `expired`;
      }
    },1000)
  }  


  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

}
