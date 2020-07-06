import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbRegion {
  regionId: number,
  countryId: number,
  regionCode: string,
  regionCodeMua: string,
  regionNameTha: string,
  regionNameEng: string,
  regionShortNameTha: string,
  regionShortNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt02Service {

  constructor(private http: HttpClient) { }

  getRegion(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    return this.http.get<any>('dbrt02', { params: filter });
  }

  getRegionDetail(code) {
    return this.http.get<DbRegion>('dbrt02/detail', { params: { regionCode: code } });
  }
  save(region: DbRegion) {
    if (region.rowVersion) {
      return this.http.put('dbrt02', region);
    }
    else {
      return this.http.post('dbrt02', region);
    }
  }
  
  getMaster() {
    return this.http.get<any>('dbrt02/master');
}

  delete(id, version) {
    return this.http.delete('dbrt02', { params: { regionId: id, rowVersion: version } })
  }

}
