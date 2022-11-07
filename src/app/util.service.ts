import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilService {

  constructor() { }


  public static resetButtonColors() {
    var x = document.getElementsByClassName("activeAppButton");
    var i;
    for (i = 0; i < x.length; i++) {
      (<HTMLElement>x[i]).style.backgroundColor = "#c9ccc5";
    }
  }

  public static setButtonColorActive(elemID: string) {
    let b: any = document.getElementById(elemID);
    b.style.backgroundColor = "chartreuse";
  }

}
