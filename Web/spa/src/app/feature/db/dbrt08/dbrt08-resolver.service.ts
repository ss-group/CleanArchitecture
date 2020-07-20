import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Dbrt08Service, DbEmployee } from './dbrt08.service';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Dbrt08ResolverService implements Resolve<any> {
  constructor(private router: Router, private db: Dbrt08Service) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;;
    const detail = code && code.code ? this.db.getEmployee(code.code) : of({} as DbEmployee);
    const url: string = route.url.join('/');
    let master;
    if (url.includes('detail')) {
      master = this.db.getMaster("detail");
    } else {
      master = this.db.getMaster("search");
    }
    return forkJoin(master,detail).pipe(map((result) => {
      let master = result[0];
      let detail = result[1];
      return { master,detail }
    }))
  }
}
