import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseList, BaseService } from '@app/shared';
import { FormGroup } from '@angular/forms';

export interface SuProgram {
  programCode: string,
  programName: string,
  programPath: string,
  systemCode: string,
  moduleCode: string,
  rowVersion: number,
  programLabels: SuProgramLabel[]
}

export class SuProgramLabel extends BaseList {
  programCode: string;
  fieldName: string;
  langCode: string;
  labelName: string;
  systemCode: string;
  moduleCode: string;
  rowVersion: number;
}

@Injectable()
export class Surt02Service extends BaseService {

  constructor(private http: HttpClient) { super() }

  getPrograms(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'system,moduleCode,programCode';
    return this.http.get<any>('surt02', { params: filter });
  }

  getProgram(code, language) {
    return this.http.get<SuProgram>('surt02/detail', { params: { programCode: code, language: language } });
  }

  checkDuplicateProgramLabel(programCode) {
    return this.http.get<boolean>('surt02/checkDuplicate', { params: { programCode: programCode } });
  }

  getMaster() {
    return this.http.get<any>('surt02/master');
  }

  save(suProgram: SuProgram, formGroup: FormGroup, programLabelDelete: SuProgramLabel[]) {
    const program = Object.assign({}, suProgram, formGroup);
    program.programLabels = this.prepareSaveList(program.programLabels, programLabelDelete);
    if (program.rowVersion) {
      return this.http.put('surt02', program);
    }
    else {
      return this.http.post('surt02', program);
    }
  }
  
  deleteProgram(code, version) {
    return this.http.delete('surt02/program', { params: { programCode: code, rowVersion: version } })
  }

  deleteProgramLabel(code) {
    return this.http.delete('surt02/programlabel', { params: { programCode: code } })
  }

  copy(suProgram: SuProgram, formGroup: FormGroup, programLabelDelete: SuProgramLabel[]) {
    const program = Object.assign({}, suProgram, formGroup);
    program.programLabels = this.prepareSaveList(program.programLabels, programLabelDelete);
    return this.http.put('surt02/copy',  program);
  }
}
