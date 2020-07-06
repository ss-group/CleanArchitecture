import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';

export interface DbMajorDetail {
  companyCode: string,
  majorCode: string,
  facCode: string,
  facName: string,
  programCode: string,
  programName: string,
  majorCodeMua: string,
  majorNameTha: string,
  majorNameEng: string,
  majorShortNameTha: string,
  majorShortNameEng: string,
  active: boolean,
  rowVersion: number,
  dbProfessional: DbProfessional[]
}

export class DbProfessional extends BaseList {
  companyCode: string;
  majorCode: string;
  facCode: string;
  programCode: string;
  proCode: string;
  proNameTha: string;
  proNameEng: string;
  proShortNameTha: string;
  proShortNameEng: string;
  formatCode: string;
  active: boolean;
}

@Injectable()
export class Dbrt13Service extends BaseService{

  constructor(private http: HttpClient) { super() }

  getMajor(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    return this.http.get<any>('dbrt13', { params: filter });
  }

  getProfessional(majorCode, facCode, programCode) {
    return this.http.get<DbMajorDetail>('dbrt13/detail', { params: { majorCode: majorCode, facCode: facCode, programCode: programCode} });
  }

  getMaster(facCode) {
    return this.http.get<any>('dbrt13/master', { params: { facCode: facCode }});
  }

  getMasterProgram(facCode) {
    return this.http.get<any>('dbrt13/program', { params: { facCode: facCode }});
  }

  save(dbMajorDetail: DbMajorDetail, status : any, dbProfessionalDelete: DbProfessional[]) {
    const groupform = Object.assign({}, dbMajorDetail, status);
    
    if (groupform.rowVersion) {
      groupform.dbProfessional = this.prepareSaveList(groupform.dbProfessional, dbProfessionalDelete);
      return this.http.put('dbrt13', groupform);
    }
    else {
      return this.http.post('dbrt13', groupform);
    }
  }

  delete(majorCode, facCode, programCode, version) {
    return this.http.delete('dbrt13', { params: { majorCode: majorCode, facCode: facCode
      , programCode: programCode, rowVersion: version } })
  }

}
