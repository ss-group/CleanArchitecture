import { Injectable } from '@angular/core';
import {
    Router, Resolve, RouterStateSnapshot,
    ActivatedRouteSnapshot, ActivatedRoute
} from '@angular/router';
import { pipe, Observable, of, forkJoin, merge } from 'rxjs';
import { map, switchMap, concatMap, first } from 'rxjs/operators';
import { Surt06Service, User } from './surt06.service';
import { ParameterService } from '@app/shared';

@Injectable()
export class Surt06Resolver implements Resolve<any> {
    constructor(private router: Router, private ss: Surt06Service, private ps: ParameterService) { }
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {

        const extra = this.router.getCurrentNavigation().extras.state
        let detail: Observable<any> = of({});
        let permission: Observable<any> = of({});
        const url: string = route.url.join('/');
        let master;
        if (url.includes('permission')) {
            if (!this.ss.userType) {
                this.router.navigate(["/su/surt06"]);
                return of({});
            }
            if (!extra || !extra.id) {
                this.router.navigate(["/su/surt06"]);
                return of({});
            }
            master = this.ss.getMaster("permission");
            permission = this.ss.getUserPermission(extra.id, extra.companyCode ? extra.companyCode : null);
            return forkJoin(master, permission).pipe(map((result) => {
                let master = result[0];
                let permission = result[1];

                return { master, permission, id: extra.id }
            }))
        }
        if (url.includes('detail')) {
            if (!this.ss.userType) {
                this.router.navigate(["/su/surt06"]);
                return of({});
            }
            master = extra && extra.id ? this.ss.getMaster("detail", this.ss.userType.code, extra.id) : this.ss.getMaster("detail", this.ss.userType.code)
            detail = this.ss.getUser(extra && extra.id ? extra.id : null);

            const userTypeInfo = detail.pipe(
                switchMap(detail => {
                    if (detail.id) return this.ss.getEmployee(detail.id, this.ss.userType.code);
                    else return of({})
                }, (d, e) => ({ d, e })),
                map(result => {
                    return { detail: result.d, employee: result.e }
                })
            )
            const userType = this.ps.getParameter('SuUserType');
            return forkJoin(master, userTypeInfo, userType).pipe(map((result) => {
                let master = result[0];
                let userTypeInfo = result[1];
                return { master, userTypeInfo }
            }))
        }
        else if (url.includes('search')) {
            if (!this.ss.userType) {
                this.router.navigate(["/su/surt06"]);
                return of({});
            }
            master = this.ss.getMaster("search");
            return master;
        }
        else {

            this.ss.userType = null;
            master = this.ss.parameterReady.pipe(
                first(ready => ready),
                switchMap(() => this.ss.getMaster("search"))
            );
            return master;
        }


    }
}