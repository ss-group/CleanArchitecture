import { Injectable } from '@angular/core';
import {Router, Resolve, RouterStateSnapshot,ActivatedRouteSnapshot, ActivatedRoute} from '@angular/router';
import { pipe, Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt04Service, DbDistrict } from './dbrt04.service';

@Injectable()
export class Dbrt04ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt04Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getDistrictType(code.code) : of({} as DbDistrict);
        const master = this.gs.getMaster();
        return forkJoin(master, detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return { master, detail }
        }))

        return;
    }
}
