import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UtilService } from '../util.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit {
  public countSubmitted: number = 0;
  public countApproved: number = 0;
  public countPending: number = 0;

  constructor(private router: Router,  
    private route: ActivatedRoute,) { 
    
    
  }

  ngOnInit(): void {
    const heroId = this.route.snapshot.paramMap.get('id');
    // console.log('query params stuff', heroId);

    const actionId = this.route.snapshot.paramMap.get('id');
    if(actionId) {
      // console.log("getCurrentNavig");
      console.log('actionId', actionId);
      if(actionId == "Kx5TK") {
        this.toggleSelectedButtonVisual("bar1");
      }
    }

  }

  toggleSelectedButtonVisual(elemID: string): void {
    if(elemID == "bar3") {
      // this.loginService.logOut();
    }
    
    UtilService.resetButtonColors();
    UtilService.setButtonColorActive(elemID);
  }

  

}
