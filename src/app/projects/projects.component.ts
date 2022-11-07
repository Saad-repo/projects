import { Component, OnInit } from '@angular/core';
import { Project, ProjectsService } from '../projects.service';


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
    projectName: 'string;',
    description: 'project description;',
    projectLinks: '<a href="test">test</a>',
    projectDate: new Date(),
    projectCost: 123,
    projectStates: 'string;',
    createDt: new Date(),
    updateDt: new Date(),
    updateUser: 1,
    status: 'new'
  };

  constructor(private projectService: ProjectsService) { }


  ngOnInit(): void {
    this.LoadProjects();
  }

  LoadProjects() {
    this.projectService.getProjects()
    .subscribe(p=>this.projects = p, err => alert(err));
  }

  save(): void {

    console.log(this.newPrj);
    this.projectService.saveProject(this.newPrj).subscribe(
      r => console.log(r)
    );
  }
}
