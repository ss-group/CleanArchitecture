import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';

export interface DbProgram {
  programCode: string,
  companyCode: string,
  programNameTha: string,
  programNameEng: string,
  programShortNameTha: string,
  programShortNameEng: string,
  facCode: string,
  active: boolean,
  programCodeMua: string,
  rowVersion: number
}

export interface DbProgramDetail{
  programCode:string,
  programNameTha:string,
  programNameEng:string,
  programShortNameTha:string,
  programShortNameEng:string,
  formatCode:string,
  fnPayinCode:string,
  subjectGroupCode:string,
  courseDescriptionCode:string,
  cardName:string,
  currId:string,
  programId:string,
  iscedId:string,
  refGroupId:string,
  branch:boolean,
  active:boolean,
  programCodeMua: string,
  rowVersion: number
  csCurriculum: CsCurriculum[];
}
export class CsCurriculum extends BaseList {
  ohecCclCode: string;
}

@Injectable()
export class Dbrt12Service extends BaseService{

  constructor(private http: HttpClient) { super()}

  getProgram(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'programCode'
    return this.http.get<any>('dbrt12', { params: filter });
  }

  getProgramType(code) {
    return this.http.get<DbProgramDetail>('dbrt12/detail', { params: { programCode: code } });
  }

  save(detail: DbProgramDetail) {
    if (detail.rowVersion) {
      return this.http.put('dbrt12', detail);
    }
    else {
      return this.http.post('dbrt12', detail);
    }
  }
  
  getProgramCode(code) {

    return this.http.get<DbProgramDetail>('dbrt12/detail', { params: { programCode: code} });
}

  getMaster() {
    return this.http.get<any>('dbrt12/master');
}

  delete(code,version) {
    return this.http.delete('dbrt12', { params: { programCode: code,rowVersion: version } })
  }
}

