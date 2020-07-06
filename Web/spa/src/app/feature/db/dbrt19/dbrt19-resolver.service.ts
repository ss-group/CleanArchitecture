import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt19Service, DbDegree } from './dbrt19.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Dbrt19ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt19Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.code ? this.cs.getDegreeDetail(code.code) : of({} as DbDegree);
    return forkJoin(detail).pipe(map((result) => {
      let detail = result[0];
      return { detail }
    }))
  }

}