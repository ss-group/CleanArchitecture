import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt11Service, DbFac } from './dbrt11.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()

export class Dbrt11ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt11Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getFacType(code.code) : of({} as DbFac);
      return forkJoin(detail).pipe(map((result) => {
          let detail = result[0];
          return {detail}
      }))
  }

}