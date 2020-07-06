import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuCompany, Surt01Service, SuDivision } from './surt01.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt01-division-detail',
  templateUrl: './surt01-division-detail.component.html'
})
export class Surt01DivisionDetailComponent implements OnInit {

  division: SuDivision = {} as SuDivision;
  divisionForm: FormGroup;
  master = { divParents: [], divTypes: [], locations: [], faculties: [], programs: [] };
  saving: boolean = false;
  locations = [];
  faculties = [];
  programs = [];
  facultiesOfPrograms = [];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt01Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.division = data.surt01.detail;
      this.master = data.surt01.master;
      this.rebuildForm();
    });
  }

  createForm() {
    this.divisionForm = this.fb.group({
      divCode: [null, [Validators.required, Validators.maxLength(20), Validators.pattern(Pattern.UpperOnly)]],
      divNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      divNameEng: [null, Validators.maxLength(200)],
      divParent: null,
      divType: 'N',
      locationCode: [null, Validators.required],
      facCode: [null, Validators.required],
      programCode: [null, Validators.required],
    });

    this.divisionForm.controls.divType.valueChanges.subscribe(() => {
      if (!this.division.divCode) {
        this.divisionForm.controls.locationCode.setValue(null);
        this.divisionForm.controls.facCode.setValue(null);
        this.divisionForm.controls.programCode.setValue(null);
      }
    });

    this.divisionForm.controls.programCode.valueChanges.subscribe(value => {
      if (this.divisionForm.controls.divType.value === 'P') {
        this.divisionForm.controls.facCode.setValue(null);
        this.facultiesOfPrograms = [];
        if (value) {
          this.su.getMaster({ formName: 'facultyOfProgram', companyCode: this.division.companyCode, programCode: value }).subscribe(master => {
            if (this.division.divCode) {
              this.facultiesOfPrograms = this.util.getActive(master.facultiesOfPrograms, this.division.facCode);
              this.divisionForm.controls.facCode.setValue(this.division.facCode);
            } else {
              this.facultiesOfPrograms = this.util.getActive(master.facultiesOfPrograms, null);
              if (this.facultiesOfPrograms.length === 1) this.divisionForm.controls.facCode.setValue(this.facultiesOfPrograms[0].value);
            }
          });
        }
      }
    });
  }

  rebuildForm() {
    this.divisionForm.markAsPristine();
    if (this.division.divCode) {
      this.divisionForm.patchValue(this.division);
      this.divisionForm.controls.divCode.disable();
      this.divisionForm.controls.divType.disable();
      this.locations = this.util.getActive(this.master.locations, this.division.locationCode);
      this.faculties = this.util.getActive(this.master.faculties, this.division.facCode);
      this.programs = this.util.getActive(this.master.programs, this.division.programCode);
      this.divisionForm.controls.locationCode.disable();
      this.divisionForm.controls.facCode.disable();
      this.divisionForm.controls.programCode.disable();
    } else {
      this.locations = this.util.getActive(this.master.locations, null);
      this.faculties = this.util.getActive(this.master.faculties, null);
      this.programs = this.util.getActive(this.master.programs, null);
    }
  }

  save() {
    if (this.divisionForm.controls.divType.value == 'L') {
      this.divisionForm.controls.facCode.setErrors(null);
      this.divisionForm.controls.programCode.setErrors(null);
    } else if (this.divisionForm.controls.divType.value == 'F') {
      this.divisionForm.controls.locationCode.setErrors(null);
      this.divisionForm.controls.programCode.setErrors(null);
    } else if (this.divisionForm.controls.divType.value == 'P') {
      this.divisionForm.controls.locationCode.setErrors(null);
    } else {
      this.divisionForm.controls.locationCode.setErrors(null);
      this.divisionForm.controls.facCode.setErrors(null);
      this.divisionForm.controls.programCode.setErrors(null);
    }

    this.util.markFormGroupTouched(this.divisionForm);
    if (this.divisionForm.invalid) return;

    Object.assign(this.division, this.divisionForm.value);
    this.su.saveDivision(this.division).pipe(
      switchMap(() => this.su.getDivision(this.division.companyCode, this.division.divCode))
    ).subscribe((result: SuDivision) => {
      this.division = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.divisionForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
