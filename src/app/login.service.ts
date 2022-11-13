import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private baseUrl = environment.apiUrl;
  private loginUrl = this.baseUrl + "users/authenticate"
  private signUpUrl = this.baseUrl + "users/signup/"
  private activateUrl = this.baseUrl + "users/signup/confirm/"
  private static userIP = "N/A";
  public static isSignedIn: boolean = false;

  constructor(private http: HttpClient) { }



  activateAccount(activationCode: string): Observable<any> {
    const headers= new HttpHeaders()
              .set(environment.headerNameIp, this.getIpAddress());

    return (this.http.post(this.activateUrl + activationCode, { 'headers': headers})
      .pipe(
        tap(_ => {
          // this.log('activate user');
                  }),
        catchError(this.handleError<any>('activateAccount', undefined))
      ));
  }  


  register(fullName: string, userName: string, pwd: string) {
    const headers= new HttpHeaders()
              .set(environment.headerNameIp, this.getIpAddress());

              //this.log('register1');
    let authRequestModel = {
      fullName: fullName,
      email: userName,
      username: userName,
      password: pwd
    };

    return (this.http.post(this.signUpUrl, authRequestModel, { 'headers': headers})
      .pipe(
        tap(r => {
          //console.log('singupurl', this.signUpUrl);
          console.log('response from signup endpoint:',r);
          
           //this.log('Registering user');
                LoginService.isSignedIn = true;
          })
        //,catchError(this.handleError<any>('RegisterUser', undefined))
      ));
  } 


  login(userName: string, pwd: string) {
    const headers= new HttpHeaders()
              .set(environment.headerNameIp, this.getIpAddress());

    let authRequestModel = {
      username: userName,
      password: pwd
    };

    return (this.http.post(this.loginUrl, authRequestModel, { 'headers': headers})
      .pipe(
        tap(_ => {
                // this.log('authenticate user');
                LoginService.isSignedIn = true;
                  })
           //,catchError(this.handleError<any>('AuthUsers', undefined))
      ));
  } 

  logOut(){
    LoginService.isSignedIn = false;
  }


  private log(message: string) {
    console.log(`LoginService: ${message}`);
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

  getIpAddress(): string {
    if(LoginService.userIP != "N/A")
      return LoginService.userIP;

    this.http.get('https://jsonip.com').subscribe(
      (value:any) => {
        // console.log("ip fetched", value);
        LoginService.userIP = value.ip;
      },
      (error) => {
        console.log(error);
      }
    );

    return LoginService.userIP;
  }


}
