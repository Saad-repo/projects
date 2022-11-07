import { Component } from '@angular/core';
import {environment} from '../environments/environment';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginService } from './login.service';
import { UtilService } from './util.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Projects Registry';
  imgLogo = environment.cdnUrl + "logo-university.png";
  get showPageBottons():boolean {return LoginService.isSignedIn;}

  constructor(private route: ActivatedRoute,
    private loginService: LoginService,
    ) {}

  ngOnInit(): void {

  }

  toggleSelectedButtonVisual(elemID: string): void {
    if(elemID == "bar3") {
      this.loginService.logOut();
    }
    // else if (elemID == "bar5") {
    //   this.settingsButtonsSectionActive = true;
    // }
    // else if (elemID == "bar10") {
    //   this.settingsButtonsSectionActive = false;
    // }
    
    UtilService.resetButtonColors();
    UtilService.setButtonColorActive(elemID);
  }

  
  
}
