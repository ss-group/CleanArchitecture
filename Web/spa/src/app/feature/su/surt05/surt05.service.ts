import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface SuParameter {
  parameterGroupCode: string,
  parameterCode: string,
  parameterValue: string,
  remark: string,
  rowVersion: number
}

@Injectable()
export class Surt05Service {

  constructor(private http: HttpClient) { }

  getParameters(criteria, page: any) {
    const filter = Object.assign(criteria, page);
    filter.sort = page.sort || 'parameterGroupCode', 'parameterCode';
    return this.http.get<any>('surt05', { params: filter });
  }

  getParameter(groupCode, code) {
    return this.http.get<SuParameter>('surt05/detail', { params: { parameterGroupCode: groupCode, parameterCode: code } });
  }

  save(parameter: SuParameter) {
    return this.http.put('surt05', parameter);
  }

  getMaster() {
    return this.http.get<any>('surt05/master');
  }
}
