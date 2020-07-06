import { Injectable } from '@angular/core';
import { BaseService } from '@app/shared';
import { HttpClient } from '@angular/common/http';

export interface SuPasswordPolicy {
  passwordPolicyCode: string;
  passwordPolicyName: string;
  passwordPolicyDescription: string;
  passwordMinimumLength: number;
  passwordMaximumLength: number;
  failTime: number;
  passwordAge: number;
  maxDupPassword: number;
  usingUppercase: boolean;
  usingLowercase: boolean;
  usingNumericChar: boolean;
  usingSpecialChar: boolean;
  active: boolean;
  rowVersion: number;
}

@Injectable()
export class Surt07Service extends BaseService {

  constructor(private http: HttpClient) { super() }

  getPasswordPolicies(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'passwordPolicyCode'
    return this.http.get<any>('surt07', { params: filter });
  }

  getPasswordPolicy(code) {
    return this.http.get<SuPasswordPolicy>('surt07/detail', { params: { passwordPolicyCode: code } });
  }

  save(passwordPolicy: SuPasswordPolicy) {
    if (passwordPolicy.rowVersion)
      return this.http.put('surt07', passwordPolicy);
    else
      return this.http.post('surt07', passwordPolicy);
  }

  delete(code, version) {
    return this.http.delete('surt07', { params: { passwordPolicyCode: code, rowVersion: version } });
  }
}
