import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Project, ProjectsService, ProjectSummary } from '../projects.service';
import { UtilService } from '../util.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];
  enteringNew: boolean = false;
  newPrj : Project = {
    //submitId: null,
    submitUser: 1,
    projectName: '',
    description: '',
    projectLinks: '',
    projectDate: new Date(),
    projectCost: 123,
    projectStates: '',
    createDt: new Date(),
    updateDt: new Date(),
    updateUser: 1,
    status: 'New',
    activationCode: this.utilService.getCookieUid()
  };

  constructor(private projectService: ProjectsService,
    private router: Router,
    private route: ActivatedRoute,
    private utilService: UtilService,
    ) { }


  ngOnInit(): void {


    const actionId = this.route.snapshot.paramMap.get('action');
    if(actionId) {
      if(actionId == "new") {
        this.enteringNew = true;
      }
      else if(actionId == "zx5Te") {
      }
    }

    this.LoadProjects();
  }

  LoadProjects() {
    let uid = this.utilService.getCookieUid();
    if(!uid) {  // session expired
      this.router.navigate(['/login',{id:"Xp5TK"}]);
      return;
    }
    
    this.projectService.getProjects(uid)
    .subscribe(p=>this.projects = p, err => alert(err));
  }

  save(): void {

    console.log(this.newPrj);
    this.projectService.saveProject(this.newPrj).subscribe(
      r => {
         // console.log('saveproject response', r);
        this.router.navigate(['/projects']);
      });
  }

  canSubmit(prj: Project): boolean {
    // TtEjjioM5k62w

    if(prj.status == "Submitted" || prj.status == "Approved")
      return false;

    return true;
  }
  canEdit(prj: Project): boolean {
    // TtEjjioM5k62w
    if(prj.status == "Submitted" || prj.status == "Approved")
      return false;

    return true;
  }
  canView(prj: Project): boolean {
    // TtEjjioM5k62w

    return true;
  }
  canApprove(prj: Project): boolean {
    // TtEjjioM5k62w
    if(prj.status =="Submitted")
      return true;
    
    return false;
  }
  canDelete(prj: Project): boolean {
    // TtEjjioM5k62w
    if(prj.status == "Submitted" || prj.status == "Approved")
      return false;

    return true;
  }

  submit(prj: Project): void {
    if(window.confirm(`Submit ${prj.projectName}?`)){
      //put your delete method logic here
      this.projectService.submitPrj(this.utilService.getCookieUid(), prj.submitId!)
      .subscribe(() => {
        this.LoadProjects();
        // const index: number = this.sdkEvents.indexOf(prj, 0);
        // if (index > -1)
        // {
        //     this.sdkEvents.splice(index, 1);
        // }
      },
      err => alert(err));
    }  
    else {
      // console.log('Approve event cancelled');
    }
  }

  approve(prj: Project): void {
    if(window.confirm(`Approve ${prj.projectName}?`)){
      //put your delete method logic here
      this.projectService.approvePrj(this.utilService.getCookieUid(), prj.submitId!)
      .subscribe(() => {
        this.LoadProjects();
        // const index: number = this.sdkEvents.indexOf(prj, 0);
        // if (index > -1)
        // {
        //     this.sdkEvents.splice(index, 1);
        // }
      },
      err => alert(err));
    }  
    else {
      // console.log('Approve event cancelled');
    }
  }

  edit(prj: Project): void {
    this.newPrj = prj;
    this.enteringNew = true;
  }

  delete(prj: Project): void {

    if(window.confirm(`Delete ${prj.projectName}?`)){
      //put your delete method logic here
      this.projectService.deletePrj(this.utilService.getCookieUid(), prj.submitId!)
      .subscribe(() => {
        this.LoadProjects();
        // const index: number = this.sdkEvents.indexOf(prj, 0);
        // if (index > -1)
        // {
        //     this.sdkEvents.splice(index, 1);
        // }
      },
      err => alert(err));
    }  
    else {
      //console.log('Delete event cancelled');
    }
  }  


  cancel(){
    this.enteringNew = false;
    this.router.navigate(['/projects']);
  }
}
