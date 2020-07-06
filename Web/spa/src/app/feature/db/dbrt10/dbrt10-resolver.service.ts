import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt10Service, DbEducationTypes } from './dbrt10.service';

@Injectable()
export class Dbrt10ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt10Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getEducationTypie(code.code, code.fac) : of({} as DbEducationTypes);
        const master = this.gs.getMaster();
        return forkJoin(master, detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return { master, detail }
        }))

        return;
    }
}
