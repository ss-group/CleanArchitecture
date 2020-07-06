import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt20Service, DbDepartment } from './dbrt20.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Dbrt20ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt20Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.code ? this.cs.getDepartmentDetail(code.code) : of({} as DbDepartment);
    return forkJoin(detail).pipe(map((result) => {
      let detail = result[0];
      return { detail }
    }))
  }

}