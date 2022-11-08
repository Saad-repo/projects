import { Component } from '@angular/core';
import {environment} from '../environments/environment';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginService } from './login.service';
import { UtilService } from './util.service';
import {CookieService} from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Projects Registry';
  imgLogo = environment.cdnUrl + "logo-university.png";
  uidExists: boolean = false;

  constructor(private route: ActivatedRoute,
    private loginService: LoginService,
    private cookieService: CookieService
    ) {}

  ngOnInit(): void {

  }

  toggleSelectedButtonVisual(elemID: string): void {
    if(elemID == "bar3") {
      this.loginService.logOut();
      this.deleteCookie('uid');
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


  isLoggedIn(): boolean {
    if(!this.uidExists){
      // console.log('getting uid cookie...');
      let uid = this.cookieService.get('uid');
      if(uid)
        return true;
    }

    return false;
  }

  deleteCookie(name: string){
    this.cookieService.delete(name);
  }

  
}
