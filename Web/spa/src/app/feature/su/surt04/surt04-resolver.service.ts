import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt04Service, SuProfile } from './surt04.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Surt04Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt04Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.code ? this.su.getProfile(code.code) : of({menuProfiles: []} as SuProfile);
    return forkJoin(detail).pipe(map((result) => {
      let detail = result[0];
      return { detail }
    }))
  }
}
