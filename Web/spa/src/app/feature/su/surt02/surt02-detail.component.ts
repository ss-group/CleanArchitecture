import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Surt02Service, SuProgram, SuProgramLabel } from './surt02.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, RowState, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt02-detail',
  templateUrl: './surt02-detail.component.html'
})

export class Surt02DetailComponent implements OnInit {

  language: string;
  master = { systemCodes: [], moduleCodes: [], languages: [] };
  programForm: FormGroup;
  suProgram: SuProgram = {} as SuProgram;
  saving: boolean = false;
  programLabelDelete: SuProgramLabel[] = [] as SuProgramLabel[];
  programLabels = {}
  btnCopy: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt02Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.suProgram = data.surt02.detail;
      this.master = data.surt02.master;
      this.language = this.master.languages[0].language;
      this.rebuildForm();
    });
  }

  createForm() {
    this.programForm = this.fb.group({
      systemCode: [null, Validators.required],
      moduleCode: [null, Validators.required],
      programCode: [null, [Validators.required, Validators.maxLength(50), Validators.pattern(Pattern.UpperOnly)]],
      programName: [null, [Validators.required, Validators.maxLength(200)]],
      programPath: [null, [Validators.required, Validators.maxLength(200)]]
    });
  }

  rebuildForm() {
    this.programForm.markAsPristine();
    if (this.suProgram.systemCode || this.suProgram.moduleCode || this.suProgram.programCode || this.suProgram.programName || this.suProgram.programPath) {
      this.programForm.patchValue(this.suProgram);
      this.programForm.controls.systemCode.disable();
      this.programForm.controls.moduleCode.disable();
      this.programForm.controls.programCode.disable();
      this.suProgram.programLabels.map(value => value.form = this.createProgramLabelForm(value));
    }
  }

  createProgramLabelForm(programLabel: SuProgramLabel) {
    const fg = this.fb.group({
      fieldName: [null, [Validators.required, Validators.maxLength(50)]],
      langCode: this.language,
      labelName: [null, [Validators.required, Validators.maxLength(1000)]],
      systemCode: this.programForm.controls.systemCode.value,
      moduleCode: this.programForm.controls.moduleCode.value,
    });
    fg.valueChanges.subscribe((control) => {
      if (programLabel.rowState == RowState.Normal) {
        programLabel.rowState = RowState.Edit;
      }
    })
    fg.patchValue(programLabel);
    if (programLabel.fieldName) {
      fg.controls.fieldName.disable();
    }

    return fg;
  }

  add() {
    const programLabel = new SuProgramLabel();
    programLabel.form = this.createProgramLabelForm(programLabel);
    this.suProgram.programLabels.push(programLabel);
    this.suProgram.programLabels = [...this.suProgram.programLabels];
    this.programForm.markAsDirty();
  }

  save() {
    try {
      this.saving = true;
      if (this.validate()) return;
      this.su.save(this.suProgram, this.programForm.value, this.programLabelDelete).pipe(
        switchMap(() => this.su.getProgram(this.programForm.controls.programCode.value, this.language)),
        finalize(() => this.saving = false)
      ).subscribe(result => {
        this.suProgram = result;
        this.rebuildForm();
        this.ms.success("message.STD00006");
        this.programLabelDelete = [];
      })
    } finally {
      this.saving = false
    }
  }

  validate() {
    let invalid = false;

    if (this.programForm.invalid) {
      this.util.markFormGroupTouched(this.programForm);
      invalid = true;
    }

    if (this.suProgram.programLabels.some(item => item.form.invalid)) {
      this.suProgram.programLabels.map(item => this.util.markFormGroupTouched(item.form));
      invalid = true;
    }

    let seen = new Set();
    let hasDuplicates = this.suProgram.programLabels.some((item) => {
      return seen.size === seen.add(item.form.controls.fieldName.value.toLowerCase()).size && item.form.controls.fieldName.value;
    });

    if (hasDuplicates) {
      this.ms.error("message.STD00004", ['label.SURT02.FieldName']);
      invalid = true;
    }

    return invalid;
  }

  canDeactivate(): Observable<boolean> {
    if (!this.programForm.dirty && !this.suProgram.programLabels.some(item => item.form.dirty)) {
      return of(true);
    }

    return this.modal.confirm("message.STD00002");
  }

  changeLanguage(language) {
    if (language === this.language)
      return;

    if (this.suProgram.programLabels.some(item => item.form.dirty) || this.programLabelDelete.length > 0) {
      this.modal.confirm("message.STD00002").subscribe(res => {
        if (res) {
          this.language = language;
          this.programLabelDelete = [];
          if (this.suProgram.rowVersion)
            this.getProgram();
          else
            this.suProgram.programLabels = [];
        }
      });
    } else {
      this.language = language;
      if (this.suProgram.rowVersion)
        this.getProgram();
      else
        this.suProgram.programLabels = [];
    }
  }

  getProgram() {
    this.suProgram.programLabels = [];
    this.su.getProgram(this.programForm.controls.programCode.value, this.language).subscribe(value => {
      this.suProgram = value;
      this.rebuildForm();
    });
  }

  remove(programLabel: SuProgramLabel) {
    if (programLabel.rowState !== RowState.Add) {
      programLabel.rowState = RowState.Delete;
      this.programLabelDelete.push(programLabel);
    }
    this.suProgram.programLabels = this.suProgram.programLabels.filter(item => item.guid !== programLabel.guid);
    this.programForm.markAsDirty();
  }

  checkCopy() {
    if (this.validateCopy()) {
      this.ms.warning("message.STD00035");
    } else {
      this.checkDuplicate();
    }
  }

  validateCopy() {
    let invalid = false;
    if (this.programForm.invalid || this.programForm.dirty) {
      this.util.markFormGroupTouched(this.programForm);
      invalid = true;
    }
    if (this.suProgram.programLabels.some(item => item.form.invalid || item.form.dirty)) {
      this.suProgram.programLabels.map(item => this.util.markFormGroupTouched(item.form));
      invalid = true;
    }
    return invalid;
  }

  checkDuplicate() {
    this.su.checkDuplicateProgramLabel(this.programForm.controls.programCode.value).subscribe(isDuplicate => {
      if (isDuplicate) {
        this.modal.confirm("message.SU00031").subscribe(res => { if (res) this.copy(); });
      } else {
        this.copy();
      }
    });
  }

  copy() {
    this.su.deleteProgramLabel(this.programForm.controls.programCode.value).pipe(
      switchMap(() => this.su.copy(this.suProgram, this.programForm.value, this.programLabelDelete)),
      switchMap(() => this.su.getProgram(this.programForm.controls.programCode.value, this.language))
    ).subscribe(result => {
      this.suProgram = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
      this.programLabelDelete = [];
    });
  }
}
