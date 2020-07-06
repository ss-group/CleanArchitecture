import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';

export interface DbDegreeSub {
  subDegreeId: number,
  subDegreeNameTha: string,
  subDegreeNameEng: string,
  subDegreeShortNameTha: string,
  subDegreeShortNameEng: string,
  degreeId : number
  active: boolean,
  rowVersion: number
}

export interface DbDegreeSubDetail{
  subDegreeId: number,
  subDegreeNameTha: string,
  subDegreeNameEng: string,
  subDegreeShortNameTha: string,
  subDegreeShortNameEng: string,
  degreeId : number
  active: boolean,
  groupLevel: string;
  rowVersion: number
  
  dbDegreeSubEduGroup: DbDegreeSubEduGroup[];
}

export class DbDegreeSubEduGroup extends BaseList {
  subDegreeId: number;
  groupLevel: string;
  seq:number;
  active:boolean;
  subDegreeDDL: any[];
}


@Injectable()
export class Dbrt15Service extends BaseService{

  constructor(private http: HttpClient) { super()}

  getSubDegree(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'subDegreeId'
    return this.http.get<any>('dbrt15', { params: filter });
  }

  getSubDegreeDetail(code) {
    return this.http.get<DbDegreeSub>('dbrt15/detail', { params: { subdegreeId: code } });
  }
  
  save(detail: DbDegreeSubDetail,status : any,dbDegreeSubEduGroupDelete: DbDegreeSubEduGroup[]) {
    const groupform = Object.assign({}, detail,status);
    if (groupform.rowVersion) {
      groupform.dbDegreeSubEduGroup = this.prepareSaveList(groupform.dbDegreeSubEduGroup, dbDegreeSubEduGroupDelete);
      return this.http.put('dbrt15', groupform);
    }
    else {
      return this.http.post('dbrt15', groupform);
    }
  }

  getSubDegreeId(code) {

    return this.http.get<DbDegreeSubDetail>('dbrt15/detail', { params: { subDegreeId: code} });
}

  delete(id,version) {
    return this.http.delete('dbrt15', { params: { subdegreeId: id,rowVersion: version } })
  }
  
  getMaster() {
    return this.http.get<any>('dbrt15/master');
}


}
