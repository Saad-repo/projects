import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  private baseUrl = environment.apiUrl;
  private prjsUrl = this.baseUrl + "projects";

  constructor(private http: HttpClient) { }


  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.prjsUrl)
    .pipe(
      tap(_ => this.log("fetched projects from "+ this.prjsUrl)),
      catchError(this.handleError<Project[]>('getProjects', undefined))
    );
  }

  saveProject(prj: Project) {

    this.log(this.prjsUrl + "/save");

    const headers= new HttpHeaders()
              .set(environment.headerNameIp, "skip ip fetch");

    return this.http.post(this.prjsUrl + "/save", prj, undefined)
    .pipe(
      tap(_ => this.log("saving projects at "+ this.prjsUrl+ "/save")),
      catchError(this.handleError<Project[]>('saveProjects', undefined))
    );
  }

  /** Log a EventsService message with the MessageService */
  private log(message: string) {
    console.log(`EventsService: ${message}`);
  }


  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
   private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }  

}



export interface Project {
	submitId?: number;
	submitUser: number;
	projectName: string;
	description: string;
	projectLinks: string;
	projectDate: Date;
	projectCost: number;
	projectStates: string;
	createDt: Date;
	updateDt: Date;
	updateUser: number;
	status: string;
}
