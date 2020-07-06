import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbListItem {
  listItemGroupCode: string,
  listItemGroupName: string,
  systemCode: string,
  listItemCode: string,
  listItemCodeMua: string,
  listItemNameTha:string,
  listItemNameEng:string,
  listItemShortNameTha: string,
  listItemShortNameEng: string,
  active:boolean,
  rowVersion: number
}


@Injectable()
export class Dbrt18Service {

  constructor(private http: HttpClient) { }

  getListItem(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'listItemGroupCode'
    return this.http.get<any>('dbrt18', { params: filter });
  }

  getListCode(code){
    return this.http.get<DbListItem>('dbrt18/detail', { params: { listItemGroupCode: code }});
  }

  getListItemDetail(code: any ,search: string, page: any) {
    const filter = Object.assign({ listItemGroupCode: code },{ keyword: search }, page);
    filter.sort = page.sort || 'listItemCode'
    return this.http.get<any>('dbrt18/detail', { params: filter });
  }

  getListType(code,code2) {
    const filter = Object.assign({ listItemGroupCode: code },{listItemCode: code2});
    return this.http.get<DbListItem>('dbrt18/detail/management', { params: filter });
  }

  getListSave(code) {
    const filter = Object.assign({ listItemCode: code });
    return this.http.get<DbListItem>('dbrt18/detail/management', { params: filter });
  }



  getListTypeCode(code) {
    return this.http.get<DbListItem>('dbrt18/detail/management2', { params: {listItemGroupCode: code} });
  }

  save(listtype: DbListItem) {
    if (listtype.rowVersion) {
      return this.http.put('dbrt18/detail/management', listtype);
    }
    else {
      return this.http.post('dbrt18/detail/management', listtype);
    }
  }

  delete(code,code2, version) {
    return this.http.delete('dbrt18/detail', { params: { listItemGroupCode: code,listItemCode: code2, rowVersion: version } })
  }

}
