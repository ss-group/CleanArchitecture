import { Injectable } from '@angular/core';
import {
    Router, Resolve, RouterStateSnapshot,
    ActivatedRouteSnapshot, ActivatedRoute
} from '@angular/router';
import { pipe, Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt03Service, DbProvince } from './dbrt03.service';

@Injectable()
export class Dbrt03ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt03Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getProvinceType(code.code) : of({} as DbProvince);
        const master = this.gs.getMaster();
        return forkJoin(master,detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return {master,detail}
        }))

        return;
    }
}
