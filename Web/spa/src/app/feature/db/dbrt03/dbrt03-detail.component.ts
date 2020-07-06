import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Dbrt03Service ,DbProvince} from './dbrt03.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt03-detail',
  templateUrl: './dbrt03-detail.component.html',
  styleUrls: ['./dbrt03-detail.component.scss']
})
export class Dbrt03DetailComponent implements OnInit {
  dbrt03Form: FormGroup;
  master = { country: [] };
  provinceList: DbProvince = {} as DbProvince;
  saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt03Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.provinceList = data.dbrt03.detail;
      if (this.provinceList.countryId != null) {
        this.master.country = this.util.getActive(data.dbrt03.master.country, this.provinceList.countryId);
      } else {
        this.master.country = this.util.getActive(data.dbrt03.master.country, null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt03Form.markAsPristine();
    if (this.provinceList.provinceNameTha) {
      this.dbrt03Form.patchValue(this.provinceList);
      this.dbrt03Form.controls.provinceCode.disable();
  }
  }

  createForm() {
    this.dbrt03Form = this.fb.group({
      provinceCode: [null, [Validators.required, Validators.maxLength(10)]],
      provinceCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      countryId: [null, Validators.required],
      provinceNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      provinceNameEng: [null, [Validators.maxLength(100)]],
      provinceShortNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      provinceShortNameEng: [null, [Validators.maxLength(100)]],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt03Form);
    if (this.dbrt03Form.invalid) {
      return;
    }
    Object.assign(this.provinceList, this.dbrt03Form.value);
    this.saving = true;
    this.cs.save(this.provinceList).pipe(
      switchMap(() => this.cs.getProvinceType(this.provinceList.provinceCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbProvince) => {
      this.provinceList = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
}
canDeactivate(): Observable<boolean> {
  if (!this.dbrt03Form.dirty) {
    return of(true);
  }
  return this.modal.confirm("message.STD00002");
}
}
