import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Dbrt13Service, DbMajorDetail } from './dbrt13.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Dbrt13ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt13Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const master = code && code.majorCode ? this.cs.getMaster(code.facCode): this.cs.getMaster('');
      // const master = this.cs.getMaster(code.facCode);
      const detail = code && (code.majorCode && code.facCode && code.programCode)
         ? this.cs.getProfessional(code.majorCode, code.facCode, code.programCode) : of({} as DbMajorDetail);
      return forkJoin(master,detail).pipe(map((result) => {
          let master = result[0];
          let detail = result[1];
          return {master,detail}
      }))
  }
}