import { Injectable } from '@angular/core';
import {
    Router, Resolve, RouterStateSnapshot,
    ActivatedRouteSnapshot, ActivatedRoute
} from '@angular/router';
import { pipe, Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt05Service, DbSubDistrict } from './dbrt05.service';

@Injectable()
export class Dbrt05ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt05Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getSubDistrictType(code.code) : of({} as DbSubDistrict);
        const master = this.gs.getMaster();
        return forkJoin(master,detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return {master,detail}
        }))

        return;
    }
}
