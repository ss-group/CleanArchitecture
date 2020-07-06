import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt14Service, DbProject } from './dbrt14.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()

export class Dbrt14ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt14Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getProjectDetail(code.code) : of({} as DbProject);
      const master = this.cs.getMaster();
      return forkJoin(detail, master).pipe(map((result) => {
        let detail = result[0];
        let master = result[1];
        return {detail,master}
      }))
  }


}