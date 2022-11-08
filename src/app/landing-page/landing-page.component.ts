import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectSummary, ProjectsService } from '../projects.service';
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
  summary: ProjectSummary[] = [];

  constructor(private router: Router,  
    private route: ActivatedRoute,
    private projectService: ProjectsService,
    private utilService: UtilService,
    ) { 
  }

  ngOnInit(): void {
    //console.log(this.route.snapshot.paramMap);
    // console.log('query params stuff', heroId);

    const actionId = this.route.snapshot.paramMap.get('id');
    if(actionId) {
      // console.log("getCurrentNavig");
      console.log('actionId', actionId);
      if(actionId == "Kx5TK") {
        this.toggleSelectedButtonVisual("bar1");
      }
    }

    this.LoadProjectsSummary();
  }

  LoadProjectsSummary() {
    let uid = this.utilService.getCookieUid();
    if(!uid) {  // session expired
      this.router.navigate(['/login',{id:"Xp5TK"}]);
      return;
    }
    
    this.projectService.getProjectsSummary(uid)
    .subscribe(p=> {
      this.summary = p;
      //console.log(p);
      this.summary.forEach( (element) => {
        //console.log(element);
        if(element.status == "New")
            this.countPending = element.count;
          else if(element.status == "Submitted")
            this.countSubmitted = element.count;
          if(element.status == "Approved")
            this.countApproved = element.count;
      });
  }, 
      err => alert(err));
  }



  toggleSelectedButtonVisual(elemID: string): void {
    if(elemID == "bar3") {
      // this.loginService.logOut();
    }
    
    UtilService.resetButtonColors();
    UtilService.setButtonColorActive(elemID);
  }

  

}
