import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt07Service } from './surt07.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Surt07Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt07Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.code ? this.su.getPasswordPolicy(code.code) : of({});
    return forkJoin(detail).pipe(map((result) => {
      let detail = result[0];
      return { detail };
    }))
  }
}
