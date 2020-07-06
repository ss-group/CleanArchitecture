import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt16Service, DbLocation } from './dbrt16.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable()
export class Dbrt16ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt16Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getLocationDetail(code.code) : of({} as DbLocation);
      return forkJoin(detail).pipe(map((result) => {
          let detail = result[0];
          return {detail}
      }))
  }

}