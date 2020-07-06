import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, Page } from '@app/shared';

export interface SuCompany {
  companyCode: string,
  companyNameTha: string,
  companyNameEng: string,
  addressTha: string,
  addressEng: string,
  moo: string,
  soi: string,
  road: string,
  subDistrictId: number,
  districtId: number,
  provinceId: number,
  postalCodeId: number,
  countryId: number,
  telephoneNo: string,
  faxNo: string,
  email: string,
  personalTaxType: string,
  taxId: string,
  socailSecurityNo: string,
  socailSecurityBranch: string,
  receiptBranchCode: string,
  receiptBranchName: string,
  active: boolean,
  rowVersion: number
  divisions: SuDivision[]
}

export class SuDivision extends BaseList {
  companyCode: string;
  divCode: string;
  divNameTha: string;
  divNameEng: string;
  divParent: string;
  divType: string;
  locationCode: string;
  facCode: string;
  programCode: string;
  rowVersion: number;
}

@Injectable()
export class Surt01Service {

  constructor(private http: HttpClient) { }

  getCompanies(keyword: string, page: any) {
    const filter = Object.assign({ Keyword: keyword }, page);
    filter.sort = page.sort || 'companyCode'
    return this.http.get<any>('surt01/company', { params: filter });
  }

  getDivisions(keyword: string, companyCode: string, page: any) {
    const filter = Object.assign({ Keyword: keyword, CompanyCode: companyCode }, page);
    filter.sort = page.sort || 'divCode'
    return this.http.get<any>('surt01/division', { params: filter });
  }

  getCompany(companyCode) {
    return this.http.get<SuCompany>('surt01/company/detail', { params: { CompanyCode: companyCode } });
  }

  getDivision(companyCode, divCode) {
    return this.http.get<SuDivision>('surt01/division/detail', { params: { CompanyCode: companyCode, DivCode: divCode } });
  }

  getMaster(params) {
    return this.http.get<any>('surt01/master', { params: params });
  }

  saveCompany(company: SuCompany) {
    if (company.rowVersion) {
      return this.http.put('surt01/company', company);
    }
    else {
      return this.http.post('surt01/company', company);
    }
  }

  saveDivision(division: SuDivision) {
    if (division.rowVersion) {
      return this.http.put('surt01/division', division);
    }
    else {
      return this.http.post('surt01/division', division);
    }
  }
  
  deleteCompany(code, version) {
    return this.http.delete('surt01/company', { params: { companyCode: code, rowVersion: version } })
  }

  deleteDivision(companyCode, divCode, version) {
    return this.http.delete('surt01/division', { params: { companyCode: companyCode, divCode: divCode, rowVersion: version } })
  }
}