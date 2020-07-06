import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt15Service, DbDegreeSubDetail } from './dbrt15.service';


@Injectable()

export class Dbrt15ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt15Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getSubDegreeId(code.code) : of({dbDegreeSubEduGroup: []} as DbDegreeSubDetail);
      const master = this.cs.getMaster();
      return forkJoin(detail,master).pipe(map((result) => {
          let detail = result[0];
          let master = result[1];
          return {detail,master}
      }))
  }
}