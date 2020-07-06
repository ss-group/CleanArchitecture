import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt21Service, DbFacProgram } from './dbrt21.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Dbrt21ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt21Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const master = this.cs.getMaster();
      const detail = code && (code.facCode && code.programCode)
         ? this.cs.getFacProgramDetail(code.facCode, code.programCode) : of({} as DbFacProgram);
      return forkJoin(master,detail).pipe(map((result) => {
          let master = result[0];
          let detail = result[1];
          return {master,detail}
      }))
  }
}