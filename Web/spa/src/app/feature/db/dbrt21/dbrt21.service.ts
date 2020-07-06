import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';

export interface DbFacProgram {
  companyCode: string;
  no: number;
  facCode: string;
  programCode: string;
  // refGroupId: string;
  // currId: string;
  // programId: string;
  // iscedId: string;
  // programCodeMua: string;
  active: boolean;
  rowVersion: number;
  // departmentDDL: any[],
  dbFacProgramDetail: DbFacProgramDetail[];
}

export class DbFacProgramDetail extends BaseList {
  companyCode: string;
  facCode: string;
  programCode: string;
  departmentCode: string;
  active: boolean;
  departmentDDL: any[];
  // rowVersion: number;
}

@Injectable()
export class Dbrt21Service extends BaseService {

  constructor(private http: HttpClient) { super(); }

  getFacProgram(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort;
    return this.http.get<any>('dbrt21', { params: filter });
  }

  getMaster() {
    return this.http.get<any>('dbrt21/master');
  }

  getFacProgramDetail(facCode, programCode) {
    return this.http.get<DbFacProgram>('dbrt21/detail', { params: { facCode: facCode, programCode: programCode } });
  }

  save(dbFacProgram: DbFacProgram, status: any, dbFacProgramDelete: DbFacProgramDetail[]) {
    const groupform = Object.assign({}, dbFacProgram, status);

    if (groupform.rowVersion) {
      groupform.dbFacProgramDetail = this.prepareSaveList(groupform.dbFacProgramDetail, dbFacProgramDelete);
      return this.http.put('dbrt21', groupform);
    }
    else {
      return this.http.post('dbrt21', groupform);
    }
  }

  delete(facCode, programCode, version) {
    return this.http.delete('dbrt21', {
      params: {
        facCode: facCode
        , programCode: programCode, rowVersion: version
      }
    })
  }

}
