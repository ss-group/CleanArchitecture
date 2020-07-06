import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt07Service, DbPreNamae } from './dbrt07.service';



@Injectable()

export class Dbrt07ResolverService implements Resolve<any> {
  constructor(private router: Router, private cs: Dbrt07Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
      const code = this.router.getCurrentNavigation().extras.state;
      const detail = code && code.code ? this.cs.getPreName(code.code) : of({} as DbPreNamae);
      const master = this.cs.getMaster();
      return forkJoin(master,detail).pipe(map((result) => {
        let master = result[0];
        let detail = result[1];
        return {master,detail}
      }))
  }

}