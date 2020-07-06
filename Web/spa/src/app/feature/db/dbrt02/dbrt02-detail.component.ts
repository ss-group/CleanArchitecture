import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of} from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DbRegion, Dbrt02Service } from './dbrt02.service';
import { FormUtilService, ModalService} from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt02-detail',
  templateUrl: './dbrt02-detail.component.html',
  styleUrls: ['./dbrt02-detail.component.scss']
})
export class Dbrt02DetailComponent implements OnInit {
  
  dbrt02Form: FormGroup;
  master = { country: [] };
  region:DbRegion = {} as DbRegion;
  submited:boolean=false;
  saving:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt02Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.region = data.dbrt02.detail
      if (this.region.countryId != null) {
        this.master.country = this.util.getActive(data.dbrt02.master.country, this.region.countryId);
      } else {
        this.master.country = this.util.getActive(data.dbrt02.master.country, null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt02Form.markAsPristine();
    if (this.region.regionId) {
      this.dbrt02Form.patchValue(this.region);
      this.dbrt02Form.controls.regionCode.disable();
    }
  }

  createForm(){
    this.dbrt02Form = this.fb.group({
      regionCode: [null, [Validators.required, Validators.maxLength(10)]],
      regionCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      countryId: [null, Validators.required],
      regionNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      regionNameEng: [null,  Validators.maxLength(100)],
      regionShortNameTha:[null, [Validators.required, Validators.maxLength(100)]],
      regionShortNameEng: [null,  Validators.maxLength(100)],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt02Form);
    if (this.dbrt02Form.invalid) {
      return;
    }
    Object.assign(this.region, this.dbrt02Form.value);
    this.saving = true;
    this.cs.save(this.region).pipe(
      switchMap(() => this.cs.getRegionDetail(this.region.regionCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbRegion) => {
      this.region = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  canDeactivate(): Observable<boolean> {
    if (!this.dbrt02Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
