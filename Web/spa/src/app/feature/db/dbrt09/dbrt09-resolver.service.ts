import { Injectable } from '@angular/core';
import { Resolve, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt09Service, DbEducationType } from './dbrt09.service';

@Injectable()
export class Dbrt09ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt09Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getEducationType(code.code) : of({} as DbEducationType);
        const master = this.gs.getMaster();
        return forkJoin(master, detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return { master, detail }
        }))
    }
}
