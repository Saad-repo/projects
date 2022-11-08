import { Injectable } from '@angular/core';
import {CookieService} from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UtilService {

  constructor(    private cookieService: CookieService    ) { }


  public static resetButtonColors() {
    var x = document.getElementsByClassName("activeAppButton");
    var i;
    for (i = 0; i < x.length; i++) {
      (<HTMLElement>x[i]).style.backgroundColor = "#c9ccc5";
    }
  }

  public static setButtonColorActive(elemID: string) {
    let b: any = document.getElementById(elemID);
    if(b)
      b.style.backgroundColor = "chartreuse";
  }

  public getCookieUid(){
      // console.log('Util.getting uid cookie...');
      let uid = this.cookieService.get('uid');
      // console.log('uid', uid);
      return uid;
  }  

  public deleteCookie(name: string){
    this.cookieService.delete(name);
  }


}
