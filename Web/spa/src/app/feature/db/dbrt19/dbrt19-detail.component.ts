import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';
import { DbDegree, Dbrt19Service } from './dbrt19.service';

@Component({
  selector: 'app-dbrt19-detail',
  templateUrl: './dbrt19-detail.component.html',
  styleUrls: ['./dbrt19-detail.component.scss']
})
export class Dbrt19DetailComponent implements OnInit {

  dbrt19Form: FormGroup;
  degree: DbDegree = {} as DbDegree;
  submited: boolean = false;
  saving: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt19Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.degree = data.dbrt19.detail;
      this.rebuildForm();
    });

  }

  rebuildForm() {
    this.dbrt19Form.markAsPristine();
    if (this.degree.degreeId) {
      this.dbrt19Form.patchValue(this.degree);
    }
  }

  createForm() {
    this.dbrt19Form = this.fb.group({
      degreeNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      degreeNameEng: [null, [Validators.maxLength(200)]],
      degreeShortNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      degreeShortNameEng: [null, [Validators.maxLength(200)]],
      active: true
    });
  }
  
  save() {
    this.util.markFormGroupTouched(this.dbrt19Form);
    if (this.dbrt19Form.invalid) {
      return;
    }
    Object.assign(this.degree, this.dbrt19Form.value);
    this.saving = true;
    this.cs.save(this.degree).pipe(
      finalize(() => this.saving = false)
    ).subscribe((degreeId) => {
      this.dbrt19Form.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt19/detail'], { replaceUrl: true, state: { code: degreeId } })
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt19Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
