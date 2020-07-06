import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';
import { FormGroup } from '@angular/forms';

export interface SuMenu {
  menuCode: string,
  programCode: string,
  mainMenu: string,
  systemCode: string,
  icon: string,
  active: boolean,
  rowVersion: number,
  menuLabels: SuMenuLabel[]
}

export class SuMenuLabel extends BaseList {
  menuCode: string;
  langCode: string;
  menuName: string;
  systemCode: string;
  rowVersion: number;
}

@Injectable()
export class Surt03Service extends BaseService {

  constructor(private http: HttpClient) { super() }

  getMenus(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'systemCode,menuCode';
    return this.http.get<any>('surt03', { params: filter });
  }

  getMenu(code) {
    return this.http.get<SuMenu>('surt03/detail', { params: { menuCode: code } });
  }

  getMaster(params) {
    return this.http.get<any>('surt03/master', { params: params });
  }

  save(suMenu: SuMenu, formGroup: FormGroup, menuLabelDelete: SuMenuLabel[]) {
    const menu = Object.assign({}, suMenu, formGroup);
    menu.menuLabels = this.prepareSaveList(menu.menuLabels, menuLabelDelete);
    if (menu.rowVersion) {
      return this.http.put('surt03', menu);
    }
    else {
      return this.http.post('surt03', menu);
    }
  }
  
  delete(code, version) {
    return this.http.delete('surt03', { params: { menuCode: code, rowVersion: version } })
  }
}
