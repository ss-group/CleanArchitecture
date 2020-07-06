import { Injectable } from '@angular/core';
import { BaseService, BaseList, RowState, ParameterService } from '@app/shared';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, switchMap, tap, first, delay } from 'rxjs/operators';
import { environment } from '@env/environment';
import { of, Observable, BehaviorSubject, forkJoin, Subscription } from 'rxjs';

export interface User {
  id: number,
  userName: string,
  email: string,
  passwordPolicyCode: string,
  studentId:number,
  employeeCode: string | number,
  defaultLang: string,
  password: string,
  forceChangePassword: string,
  startEffectiveDate: Date,
  endEffectiveDate: Date,
  updatedBy: string,
  updatedDate: Date,
  lastChangePassword: Date,
  passwordExpire: Date,
  active: boolean,
  rowVersion: number,
  profiles: UserProfile[],
  permissions: UserPermission[],
  userType:string
}

export class UserProfile extends BaseList {
  id: number;
  profileCode: string;
  rowVersion: number;
}

export class UserPermission extends BaseList {
  id: number;
  companyCode: string;
  rowVersion: number;
  divisions: UserDivisions[];
  eduLevels : UserEduLevel[];
}

export class UserDivisions extends BaseList {
  userId: number;
  companyCode: string;
  divCode: string;
  rowState: RowState;
  rowVersion: number;
}

export class UserEduLevel extends BaseList {
  userId: number;
  companyCode: string;
  educationTypeLevel: string;
  rowVersion: number;
}

export interface UserType{
   Employee: string, Student: string, Professor: string, VisitingProfessor: string 
}
export interface UserGroup{
   Employee: string, Professor: string, VisitingProfessor: string 
}
@Injectable()
export class Surt06Service extends BaseService {

  userInfo:{username:string,employeeName:string};
  userTypeParam:UserType;
  private currentUserType: { code: string, name: string } = null;
  userGroupsParam:UserGroup;
  private userTypeGroupMapping:{ [index: string]: string; } = {};
  policyStudent:string;

  parameterReady = new BehaviorSubject<boolean>(false);
  constructor(private http: HttpClient,private ps:ParameterService) { 
    super();
    this.init();
  }

  init(){
    forkJoin(this.ps.getParameter('SuUserType'),this.ps.getParameter('SuEmpGroupType'),this.ps.getParameter('SuSetDefault','PolicyStudent')).pipe(
      tap(result=>{
          this.userTypeGroupMapping[result[0].Employee] = result[1].Employee;
          this.userTypeGroupMapping[result[0].Professor] = result[1].Professor;
          this.userTypeGroupMapping[result[0].VisitingProfessor] = result[1].VisitingProfessor;
      })
    ).subscribe(result=>{
       this.userTypeParam = result[0];
       this.userGroupsParam = result[1];
       this.policyStudent = result[2].PolicyStudent;
       this.parameterReady.next(true);
    })
  }

  get userType() {
    return this.currentUserType;
  }

  set userType(value) {
    this.currentUserType = value;
  }

  getUsers(search: any,userType:any, page: any) {
    const filter = Object.assign(search,{ userType:userType,userGroup:this.userTypeGroupMapping[userType]}, page);
    filter.sort = page.sort || 'username'
    return this.http.get<any>('surt06', { params: filter });
  }

  getUser(id) {
    if (id)
      return this.http.get<User>('surt06/detail', { params: { id: id } })
    else return of({ profiles: [], permissions: [] } as User);
  }

  getUserPermission(id, companyCode) {
    if (companyCode)
      return this.http.get<User>('surt06/permission', { params: { id: id, companyCode: companyCode } });
    else return of({ divisions: [],eduLevels:[] } as UserPermission);
  }

  getAutoComplete(term,value,personCompany){
    const isStudent = this.userType.code == this.userTypeParam.Student;
    return this.http.get<any>('surt06/auto',{params:{ personCompany : personCompany, studentId:isStudent ? value : null,employeeCode:isStudent ? null:value,keyword:term,isStudent: String(isStudent),userGroup:this.userTypeGroupMapping[this.userType.code] }});
  }

  getMaster(page, userType?, id?) {
    return this.http.get<any>('surt06/master', { params: { page: page, userType: userType, id: id } });
  }

  getMasterPermission(page, company?: string) {
    return this.http.get<any>('surt06/master-permission-detail/' + page + '/' + company);
  }

  save(user: User, userForm: any, profileDelete: UserProfile[], permissionDelete: UserPermission[]) {
    const userTable = Object.assign({}, user, userForm);
    userTable.profiles = this.prepareSaveList(userTable.profiles, profileDelete);
    userTable.permissions = this.prepareSaveList(userTable.permissions, permissionDelete);
    if (userTable.rowVersion) {
      return this.http.put('surt06', userTable);
    }
    else {
      return this.http.post('surt06', userTable);
    }
  }

  delete(id, version) {
    return this.http.delete('surt06', { params: { id: id, rowVersion: version } })
  }

  getEmployee(userId,userType) {
    return this.http.get<any>('surt06/employee', { params: { userId: userId,userType:userType } })
  }

  resetPassword(email,isStudent) {
    return this.http.post<any>(`${environment.authUrl}/account/forgetpassword`, { email:email,isStudent:isStudent});
  }

  savePermission(permission: UserPermission, permissionForm: any,eduLevelDelete:UserEduLevel[]) {
    const per = Object.assign({}, permission, permissionForm);
    per.eduLevels = this.prepareSaveList(per.eduLevels, eduLevelDelete);
    if (per.rowVersion) {
      return this.http.put('surt06/permission', per);
    }
    else {
      return this.http.post('surt06/permission', per);
    }
  }
}
