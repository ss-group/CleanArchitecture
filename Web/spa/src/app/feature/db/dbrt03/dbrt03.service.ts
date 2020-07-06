import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbProvince {
  provinceId: number,
  provinceCode: string,
  provinceCodeMua: string,
  countryId: number,
  provinceNameTha: string,
  provinceNameEng: string,
  provinceShortNameTha:string,
  provinceShortNameEng:string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt03Service {

  constructor(private http: HttpClient) { }

  getProvince(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    return this.http.get<any>('dbrt03', { params: filter });
  }

  getProvinceType(code) {
    return this.http.get<DbProvince>('dbrt03/detail', { params: { provinceCode: code} });
  }

  save(provincetype: DbProvince) {
    if (provincetype.rowVersion) {
      return this.http.put('dbrt03', provincetype);
    }
    else {
      return this.http.post('dbrt03', provincetype);
    }
  }

  getMaster() {
    return this.http.get<any>('dbrt03/master');
}

  delete(id, version) {
    return this.http.delete('dbrt03', { params: { provinceId: id, rowVersion: version } })
  }

}
