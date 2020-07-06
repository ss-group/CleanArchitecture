import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt03Service, SuMenu } from './surt03.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Surt03Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt03Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.menuCode ? this.su.getMenu(code.menuCode) : of({ menuLabels: [] } as SuMenu);
    const master = this.su.getMaster({ FieldName: 'SystemCodeAndLanguage' });
    return forkJoin(master, detail).pipe(map((result) => {
      let master = result[0];
      let detail = result[1];
      return { master, detail }
    }))
  }
}
