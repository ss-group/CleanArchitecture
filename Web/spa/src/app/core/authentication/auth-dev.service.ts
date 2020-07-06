import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthDevService {
 
  constructor(
  ) {}

  init(){
  
  }

  destory(){
   
  }

  get isLoggedIn(): Observable<boolean> {
    return of(true);
  }

  get claims(): Observable<any> {
    return of({ name : 'admin-dev',role:[],super:true});
  }

  get authorizationHeader(): Observable<string> {
    return of('auth-header');
  }

  signin() {
    
  }
  signout() {
     
  }

  completeAuthentication() {
    
  }

  get redirectUrl() {
    return '/';
  }
  set redirectUrl(value) {
     
  }

  get personalInfo(): Observable<any> {
    return of({ personalName:'administrator-dev'});
  }

}
