import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt05Service, SuParameter } from './surt05.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Surt05Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt05Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.groupCode && code.code ? this.su.getParameter(code.groupCode, code.code) : of({} as SuParameter);
    return forkJoin(detail).pipe(map((result) => {
      let detail = result[0];
      return { detail }
    }))
  }
}
