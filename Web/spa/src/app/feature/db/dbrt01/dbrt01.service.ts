import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbCountry {
  countryId: number,
  countryCode: string,
  countryCodeMua: string,
  countryNameTha: string,
  countryNameEng: string,
  countryShortNameTha: string,
  countryShortNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt01Service {

  constructor(private http: HttpClient) { }

  getCountry(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    return this.http.get<any>('dbrt01', { params: filter });
  }

  getCountryDetail(id) {
    return this.http.get<DbCountry>('dbrt01/detail', { params: { countryid: id } });
  }
  save(country: DbCountry) {
    if (country.rowVersion) {
      return this.http.put('dbrt01', country);
    }
    else {
      return this.http.post('dbrt01', country);
    }
  }

  delete(id,version) {
    return this.http.delete('dbrt01', { params: { countryId: id, rowVersion: version } })
  }

}
