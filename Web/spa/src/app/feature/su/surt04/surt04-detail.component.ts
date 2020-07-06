import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Surt04Service, SuProfile, SuMenuProfile } from './surt04.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, RowState, Size, Page, LookupMultipleResult, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap';
import { Surt04LookupComponent } from './surt04-lookup.component';

@Component({
  selector: 'app-surt04-detail',
  templateUrl: './surt04-detail.component.html'
})
export class Surt04DetailComponent implements OnInit {

  profileForm: FormGroup;
  profile: SuProfile = {} as SuProfile;
  saving: boolean = false;
  keyword: string = '';
  menuProfiles = [];
  menuProfilesDelete = [];
  lookupMultiple = Surt04LookupComponent;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt04Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.profile = data.surt04.detail;
      this.rebuildForm();
    });
  }

  createForm() {
    this.profileForm = this.fb.group({
      profileCode: [null, [Validators.required, Validators.maxLength(10), Validators.pattern(Pattern.UpperOnly)]],
      profileDesc: [null, [Validators.required, Validators.maxLength(50)]],
      active: true
    });
  }

  rebuildForm() {
    this.profileForm.markAsPristine();
    if (this.profile.profileCode) {
      this.profileForm.patchValue(this.profile);
      this.menuProfiles = [...this.profile.menuProfiles.map(item => { item.rowState = RowState.Normal; return item; })];
      this.profileForm.controls.profileCode.disable();
    }
  }

  addItems() {
    if (this.profileForm.invalid) {
      this.util.markFormGroupTouched(this.profileForm);
      return;
    }
    const initial = { selectedList: [...this.menuProfiles] };
    this.modal.openComponent(this.lookupMultiple, Size.large, initial).subscribe((list: LookupMultipleResult) => {
      if (list) {
        this.menuProfiles = list.selected;
        this.menuProfilesDelete.push(...list.deleting);
        this.profile.menuProfiles = [...this.menuProfiles];
      }
    });
  }

  save() {
    if (this.validate()) return;

    this.saving = true;
    const profile = Object.assign({}, this.profile, this.profileForm.value);
    this.su.save(profile, this.menuProfilesDelete).pipe(
      switchMap(() => this.su.getProfile(this.profileForm.controls.profileCode.value)),
      finalize(() => this.saving = false)
    ).subscribe(result => {
      this.profile = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
      this.menuProfilesDelete = [];
    })
  }

  validate() {
    let invalid = false;
    if (this.profileForm.invalid) {
      this.util.markFormGroupTouched(this.profileForm);
      invalid = true;
    }
    if (this.menuProfiles.length === 0) {
      this.ms.warning("message.STD00012", ['label.SURT04.Menu']);
      invalid = true;
    }
    let seen = new Set();
    let hasDuplicates = this.menuProfiles.some((item) => {
      return seen.size === seen.add(item.menuCode.toLowerCase()).size && item.menuCode;
    });

    if (hasDuplicates) {
      this.ms.error("message.STD00004", ['label.SURT04.MenuCode']);
      invalid = true;
    }
    return invalid;
  }

  canDeactivate(): Observable<boolean> {
    if (!this.profileForm.dirty && !this.profile.menuProfiles.some(item => item.rowState != RowState.Normal)) {
      return of(true);
    }

    return this.modal.confirm("message.STD00002");
  }

  remove(row) {
    if (row.rowState !== RowState.Add) {
      row.rowState = RowState.Delete;
      this.menuProfilesDelete.push(row);
    }
    this.menuProfiles = this.menuProfiles.filter(menu => menu.guid !== row.guid);
    this.profile.menuProfiles = [...this.menuProfiles];
    this.profileForm.markAsDirty();
  }
}
