import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, Page, BaseService } from '@app/shared';
import { FormGroup } from '@angular/forms';

export interface SuProfile {
  profileCode: string,
  profileDesc: string,
  active: boolean,
  rowVersion: number,
  menuProfiles: SuMenuProfile[]
}

export class SuMenuProfile extends BaseList {
  profileCode: string;
  menuCode: string;
  rowVersion: number;
}

@Injectable()
export class Surt04Service extends BaseService {

  constructor(private http: HttpClient) { super() }

  getProfiles(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'profileCode'
    return this.http.get<any>('surt04', { params: filter });
  }

  getProfile(code) {
    return this.http.get<SuProfile>('surt04/detail', { params: { profileCode: code } });
  }

  getMaster(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'menuCode'
    return this.http.get<any>('surt04/master', { params: filter });
  }

  save(profile: SuProfile, menuProfilesDelete: SuMenuProfile[]) {
    profile.menuProfiles = this.prepareSaveList(profile.menuProfiles, menuProfilesDelete);
    if (profile.rowVersion) {
      return this.http.put('surt04', profile);
    } else {
      return this.http.post('surt04', profile);
    }
  }
  
  delete(code, version) {
    return this.http.delete('surt04', { params: { profileCode: code, rowVersion: version } })
  }

  copy(value) {
    return this.http.post('surt04/copy', value);
  }
}