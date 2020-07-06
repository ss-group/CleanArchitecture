import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt01Service, DbCountry } from './dbrt01.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()

export class Dbrt01ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt01Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getCountryDetail(code.code) : of({} as DbCountry);
      return forkJoin(detail).pipe(map((result) => {
          let detail = result[0];
          return {detail}
      }))
  }

}