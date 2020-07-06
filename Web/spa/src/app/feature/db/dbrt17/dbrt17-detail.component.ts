import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt17Service, DbBuilding, DbBuildingDetail, DbRoom, DbPrivilegeBuilding } from './dbrt17.service';
import { FormUtilService, ModalService, RowState } from '@app/shared';
import { MessageService } from '@app/core';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt17-detail',
  templateUrl: './dbrt17-detail.component.html',
  styleUrls: ['./dbrt17-detail.component.scss']
})
export class Dbrt17DetailComponent implements OnInit {

  dbrt17Form: FormGroup;
  groupForm: FormGroup;
  building: DbBuilding = {} as DbBuilding;
  submited: boolean = false;
  saving: boolean = false;
  state: Observable<object>;
  detail: DbBuildingDetail = {} as DbBuildingDetail;
  dbRoomDelete: DbRoom[] = [];
  dbPrivilegeBuildingDelete: DbPrivilegeBuilding[] = [];
  disBtnSave: boolean;
  masterRoom: [];
  master = {
    location: [],
    roomType: [],
    privilegeBuilding: []
  };

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt17Service
  ) {
  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.detail = data.dbrt17.detail;
      this.master = data.dbrt17.master;
      if (this.detail.locationCode != null) {
        this.master.location = this.util.getActive(data.dbrt17.master.location, this.detail.locationCode);
      } else {
        this.master.location = this.util.getActive(data.dbrt17.master.location, null);
      }
      this.masterRoom = data.dbrt17.master.roomType;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt17Form.markAsPristine();
    if (this.detail.buildingId) {
      this.dbrt17Form.patchValue(this.detail);
      this.detail.dbRoom.map(detail => detail.form = this.createRoomForm(detail))
      this.detail.dbPrivilegeBuilding.map(detail => detail.form = this.createPrivilegeBuildingForm(detail))
      this.dbrt17Form.controls.buildingCode.disable();
    }
  }

  createForm() {
    this.dbrt17Form = this.fb.group({
      buildingCode: [null, [Validators.required, Validators.maxLength(10)]],
      locationCode: [null, [Validators.required, Validators.maxLength(10)]],
      buildingNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      buildingNameEng: [null, [Validators.maxLength(100)]],
      active: true
    });

  }
  createRoomForm(dbRoom: DbRoom) {
    const fg = this.fb.group({
      roomNo: [null, [Validators.required, Validators.maxLength(10)]],
      roomNameTha: [null, [Validators.maxLength(100)]],
      roomNameEng: [null, [Validators.maxLength(100)]],
      floorNo: [null, Validators.required,],
      capacity: [null, Validators.required,],
      roomType: [null, [Validators.required, Validators.maxLength(20)]],
      active: true,
      assigned: false
    });
    fg.valueChanges.subscribe((control) => {
      if (control.assigned && dbRoom.rowState == RowState.Normal) {
        dbRoom.rowState = RowState.Edit;
      }
    })
    fg.patchValue(dbRoom);
    if (dbRoom.roomType) {
      dbRoom.roomDDL = this.util.getActive(this.masterRoom, dbRoom.roomType);
    }
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }
  addRoom() {
    if (this.detail.buildingId != null) {
      const newGroup = new DbRoom();
      newGroup.roomDDL = this.util.getActive(this.masterRoom, null);
      newGroup.form = this.createRoomForm(newGroup);
      this.detail.dbRoom.push(newGroup);
      this.detail.dbRoom = [...this.detail.dbRoom];
      this.dbrt17Form.markAsDirty();
    } else {
      this.ms.warning("message.STD00035");
      return;
    }
  }
  removeRoom(dbRoom: DbRoom) {
    if (dbRoom.rowState !== RowState.Add) {
      dbRoom.rowState = RowState.Delete;
      this.dbRoomDelete.push(dbRoom);
    }
    this.detail.dbRoom = this.detail.dbRoom.filter(item => item.guid !== dbRoom.guid);
    this.dbrt17Form.markAsDirty();
  }

  createPrivilegeBuildingForm(dbPrivilegeBuilding: DbPrivilegeBuilding) {
    const fg = this.fb.group({
      divCode: [null, [Validators.required, Validators.maxLength(20)]],
      active: true,
      assigned: false
    });
    fg.valueChanges.subscribe((control) => {
      if (control.assigned && dbPrivilegeBuilding.rowState == RowState.Normal) {
        dbPrivilegeBuilding.rowState = RowState.Edit;
      }
    })
    fg.patchValue(dbPrivilegeBuilding);
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }
  addPrivilegeBuilding() {
    if (this.detail.buildingId != null) {
      const newGroup = new DbPrivilegeBuilding();
      newGroup.form = this.createPrivilegeBuildingForm(newGroup);
      this.detail.dbPrivilegeBuilding.push(newGroup);
      this.detail.dbPrivilegeBuilding = [...this.detail.dbPrivilegeBuilding];
      this.dbrt17Form.markAsDirty();
    } else {
      this.ms.warning("message.STD00035");
      return;
    }
  }
  removePrivilegeBuilding(dbPrivilegeBuilding: DbPrivilegeBuilding) {
    if (dbPrivilegeBuilding.rowState !== RowState.Add) {
      dbPrivilegeBuilding.rowState = RowState.Delete;
      this.dbPrivilegeBuildingDelete.push(dbPrivilegeBuilding);
    }
    this.detail.dbPrivilegeBuilding = this.detail.dbPrivilegeBuilding.filter(item => item.guid !== dbPrivilegeBuilding.guid);
    this.dbrt17Form.markAsDirty();
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt17Form);
    if (this.dbrt17Form.invalid) {
      return;
    }
    if (this.detail.dbRoom != null && this.detail.dbRoom != []) {
      this.detail.dbRoom.map(item => this.util.markFormGroupTouched(item.form));
      if (this.detail.dbRoom.some(item => item.form.invalid)) {
        return;
      }
    }
    if (this.detail.dbPrivilegeBuilding != null && this.detail.dbPrivilegeBuilding != []) {
      this.detail.dbPrivilegeBuilding.map(item => this.util.markFormGroupTouched(item.form));
      if (this.detail.dbPrivilegeBuilding.some(item => item.form.invalid)) {
        return;
      }
    }
    Object.assign(this.detail, this.dbrt17Form.value);
    this.saving = true;
    this.cs.save(this.detail, this.dbrt17Form.getRawValue(), this.dbRoomDelete, this.dbPrivilegeBuildingDelete).pipe(
      finalize(() => this.saving = false)
    ).subscribe((buildingId) => {
      this.dbrt17Form.markAsPristine();
      this.rebuildForm();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt17/detail'], { replaceUrl: true, state: { code: buildingId } })
    })
  }

  validate() {
    if (this.validate()) return;
    let invalid = false;
    if (this.dbrt17Form.invalid) {
      this.util.markFormGroupTouched(this.dbrt17Form);
      invalid = true;
    }
    if (this.detail.dbPrivilegeBuilding.some(item => item.form.invalid)) {
      this.detail.dbPrivilegeBuilding.map(item => this.util.markFormGroupTouched(item.form));
      invalid = true;
    }
    const seen = new Set();
    const detail = this.detail.dbPrivilegeBuilding.some((item) => {
      return seen.size === seen.add(item.form.controls.divCode.value).size && item.form.controls.divCode.value;
    });
    if (detail) {
      this.ms.error("message.STD00004", ['label.DBRT17.divCode']);
      invalid = true;
    }
    return invalid;
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt17Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
