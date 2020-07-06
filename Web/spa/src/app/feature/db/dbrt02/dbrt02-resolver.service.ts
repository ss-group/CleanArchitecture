import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt02Service, DbRegion } from './dbrt02.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()

export class Dbrt02ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt02Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getRegionDetail(code.code) : of({} as DbRegion);
      const master = this.cs.getMaster();
      return forkJoin(detail, master).pipe(map((result) => {
          let detail = result[0];
          let master = result[1];
          return {detail,master}
      }))
  }


}