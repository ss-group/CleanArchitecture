import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbProject {
  projectId: number,
  companyCode: string,
  projectCode: string,
  projectNameTha: string,
  projectNameEng: string,
  projectDescription: string,
  educationTypeCode: string,
  educationTypeNameTha: string,
  facCode: string,
  facNameTha: string,
  studentTypeCode: string,
  studentTypeName: string,
  erpCode: string,
  erpActivity: string,
  erpProduct: string,
  erpProject: string,
  bypassRegister: boolean,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt14Service {

  constructor(private http: HttpClient) { }

  getProject(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'projectCode'
    return this.http.get<any>('dbrt14', { params: filter });
  }

  getProjectDetail(code) {
    return this.http.get<DbProject>('dbrt14/detail', { params: { projectCode: code } });
  }
  save(project: DbProject) {
    if (project.rowVersion) {
      return this.http.put('dbrt14', project);
    }
    else {
      return this.http.post('dbrt14', project);
    }
  }
  
  getMaster() {
    return this.http.get<any>('dbrt14/master');
}

  delete(id,code,version) {
    return this.http.delete('dbrt14', { params: { projectId: id, projectCode: code ,rowVersion: version } })
  }

}
