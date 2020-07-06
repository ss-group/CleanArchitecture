import { Injectable } from '@angular/core';
import {
    Router, Resolve, RouterStateSnapshot,
    ActivatedRouteSnapshot, ActivatedRoute
} from '@angular/router';
import { pipe, Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt12Service, DbProgram, DbProgramDetail } from './dbrt12.service';

@Injectable()
export class Dbrt12ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt12Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code ? this.gs.getProgramType(code.code) : of({csCurriculum: []} as DbProgramDetail);
        const master = this.gs.getMaster();
        return forkJoin(master,detail).pipe(map((result) => {
            let master = result[0];
            let detail = result[1];
            return {master,detail}
        }))

        return;
    }
}
