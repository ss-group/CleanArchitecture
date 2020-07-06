import { Injectable } from '@angular/core';
import {
    Router, Resolve, RouterStateSnapshot,
    ActivatedRouteSnapshot, ActivatedRoute
} from '@angular/router';
import { pipe, Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dbrt18Service, DbListItem } from './dbrt18.service';

@Injectable()
export class Dbrt18ResolverService implements Resolve<any> {
    constructor(private router: Router, private gs: Dbrt18Service) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
        const code = this.router.getCurrentNavigation().extras.state;
        const code2 = this.router.getCurrentNavigation().extras.state;
        const detail = code && code.code? this.gs.getListCode(code.code) : of({} as DbListItem);
        const detail2 = code && code.code? this.gs.getListTypeCode(code.code) : of({} as DbListItem);
        const manage = code2 && code2.code2? this.gs.getListType(code2.code,code2.code2) : of({} as DbListItem);
        return forkJoin(detail,manage,detail2).pipe(map((result) => {
            let detail = result[0];
            let manage = result[1];
            let detail2 = result[2];
            return {detail,manage,detail2}
        }))

        return;
    }
}
