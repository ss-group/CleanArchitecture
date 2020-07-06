import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Surt01Service, SuCompany, SuDivision } from './surt01.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Page } from '@app/shared';

@Injectable()
export class Surt01Resolver implements Resolve<any> {

  constructor(private router: Router, private su: Surt01Service) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    const code = this.router.getCurrentNavigation().extras.state;
    const url: string = route.url.join('/');
    let detail = of({});
    let master = of({});

    if (url.includes('division/detail')) {
      detail = code && code.companyCode && code.divCode ? this.su.getDivision(code.companyCode, code.divCode) : of({ companyCode: code.companyCode } as SuDivision);
      master = this.su.getMaster({ FormName: 'divParent, divType, location, faculty, program', CompanyCode: code.companyCode, DivCode: code.divCode });
    }
    else if (url.includes('division')) {
      detail = code && code.companyCode ? of({ companyCode: code.companyCode }) : of({});
    }
    else if (url.includes('company/detail')) {
      detail = code && code.companyCode ? this.su.getCompany(code.companyCode) : of({} as SuCompany);
      master = this.su.getMaster({ FormName: 'personalTaxType, country' });
    }

    return forkJoin(master, detail).pipe(map((result) => {
      let master = result[0];
      let detail = result[1];
      return { master, detail }
    }))
  }
}
