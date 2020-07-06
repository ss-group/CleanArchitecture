import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt02Service, SuProgram } from './surt02.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class Surt02Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt02Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const detail = code && code.programCode ? this.su.getProgram(code.programCode, 'th') : of({ programLabels: [] } as SuProgram);
    const master = this.su.getMaster();
    return forkJoin(master, detail).pipe(map((result) => {
      let master = result[0];
      let detail = result[1];
      return { master, detail }
    }))
  }
}
