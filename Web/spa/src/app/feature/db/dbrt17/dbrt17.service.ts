import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';

export interface DbBuilding {
  buildingId: number,
  companyCode: string,
  locationCode: string,
  buildingCode: string,
  buildingNameTha: string,
  buildingNameEng: string,
  active: boolean,
  rowVersion: number

}
export interface DbBuildingDetail {
  buildingId: number,
  companyCode: string,
  locationCode: string,
  buildingCode: string,
  buildingNameTha: string,
  buildingNameEng: string,
  active: boolean,
  rowVersion: number
  dbRoom: DbRoom[];
  dbPrivilegeBuilding: DbPrivilegeBuilding[];
}
export class DbRoom extends BaseList {
  roomId: number;
  buildingId: number;
  roomNo: string;
  roomNameTha: string;
  roomNameEng: string;
  floorNo: number;
  capacity: number;
  roomType: string;
  active: boolean;
  roomDDL: any[];
}
export class DbPrivilegeBuilding extends BaseList {
  privilegeBuildingId: number;
  buildingId: number;
  divCode: string;
  active: boolean;
}

@Injectable()
export class Dbrt17Service extends BaseService {

  constructor(private http: HttpClient) { super() }

  getBuilding(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'buildingCode'
    return this.http.get<any>('dbrt17', { params: filter });
  }

  getBuildingDetail(code) {
    return this.http.get<DbBuilding>('dbrt17/detail', { params: { buildingId: code } });
  }

  save(status: any, detail: DbBuildingDetail, dbRoomDelete: DbRoom[], dbPrivilegeBuildingDelete: DbPrivilegeBuilding[]) {
    const groupform = Object.assign({}, detail, status);
    if (groupform.rowVersion) {
      groupform.dbRoom = this.prepareSaveList(groupform.dbRoom, dbRoomDelete);
      groupform.dbPrivilegeBuilding = this.prepareSaveList(groupform.dbPrivilegeBuilding, dbPrivilegeBuildingDelete);
      return this.http.put('dbrt17', groupform);
    }
    else {
      return this.http.post('dbrt17', groupform);
    }
  }

  getRoom(code) {
    return this.http.get<DbBuildingDetail>('dbrt17/detail', { params: { buildingId: code } });
  }
  getMaster() {
    return this.http.get<any>('dbrt17/master');
  }

  delete(id, version) {
    return this.http.delete('dbrt17', { params: { buildingId: id, rowVersion: version } })
  }

}
