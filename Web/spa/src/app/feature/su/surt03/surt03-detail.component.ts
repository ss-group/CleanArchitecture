import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuMenu, Surt03Service, SuMenuLabel } from './surt03.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, RowState, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt03-detail',
  templateUrl: './surt03-detail.component.html'
})
export class Surt03DetailComponent implements OnInit {

  master = { systemCodes: [], mainMenus: [], programCodes: [], langCodes: [] };
  menuForm: FormGroup;
  suMenu: SuMenu = {} as SuMenu;
  saving: boolean = false;
  menuLabelDelete: SuMenuLabel[] = [] as SuMenuLabel[];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt03Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.suMenu = data.surt03.detail;
      this.master.systemCodes = data.surt03.master.systemCodes;
      this.master.langCodes = data.surt03.master.langCodes;
      this.rebuildForm();
    });
  }

  createForm() {
    this.menuForm = this.fb.group({
      systemCode: [null, [Validators.required]],
      mainMenu: null,
      menuCode: [null, [Validators.required, Validators.maxLength(20), Validators.pattern(Pattern.UpperOnly)]],
      programCode: null,
      icon: [null, [Validators.required, Validators.maxLength(50)]],
      active: true
    });

    this.menuForm.controls.systemCode.valueChanges.subscribe(systemCode => {
      this.master.mainMenus = [];
      this.master.programCodes = [];
      this.menuForm.controls.mainMenu.setValue(null);
      this.menuForm.controls.programCode.setValue(null);
      if (systemCode) {
        this.su.getMaster({ FieldName: 'MainMenusAndProgramCodes', SystemCode: systemCode, MenuCode: this.menuForm.controls.menuCode.value }).subscribe(master => {
          if (this.suMenu.systemCode && this.suMenu.menuCode) {
            this.master.mainMenus = this.util.getActive(master.mainMenus, this.suMenu.mainMenu);
          } else {
            this.master.mainMenus = this.util.getActive(master.mainMenus, null);
          }
            
          this.master.programCodes = master.programCodes;
        });
      }
    });
  }

  rebuildForm() {
    this.menuForm.markAsPristine();
    if (this.suMenu.systemCode && this.suMenu.menuCode) {
      this.menuForm.controls.systemCode.disable();
      this.menuForm.controls.menuCode.disable();
      this.menuForm.patchValue(this.suMenu);
      this.suMenu.menuLabels.map(value => value.form = this.createMenuLabelForm(value));
    }
    this.menuLabelDelete = [];
  }


  validate() {
    let invalid = false;
    if (this.menuForm.invalid) {
      this.util.markFormGroupTouched(this.menuForm);
      invalid = true;
    }
    if (this.suMenu.menuLabels.some(item => item.form.invalid)) {
      this.suMenu.menuLabels.map(item => this.util.markFormGroupTouched(item.form));
      invalid = true;
    }
    if (this.suMenu.menuLabels.length === 0) {
      this.ms.warning("message.STD00012", ['label.SURT03.MenuName']);
      invalid = true;
    }
    let seen = new Set();
    let hasDuplicates = this.suMenu.menuLabels.some(item=> {
      return  item.form.controls.langCode.value != null && seen.size === seen.add(item.form.controls.langCode.value).size;
    });
    if (hasDuplicates) {
      this.ms.error("message.STD00004", ['label.SURT03.Language']);
      invalid = true;
    }

    return invalid;
  }

  save() {
    if (this.validate()) return;
    this.saving = true;
    Object.assign(this.suMenu, this.menuForm.value);
    this.su.save(this.suMenu, this.menuForm.value, this.menuLabelDelete).pipe(
      switchMap(code => this.su.getMenu(this.suMenu.menuCode)),
      finalize(() => this.saving = false)
    ).subscribe((result: SuMenu) => {
      this.suMenu = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  add() {
    const menuLabel = new SuMenuLabel();
    menuLabel.form = this.createMenuLabelForm(menuLabel);
    this.suMenu.menuLabels.push(menuLabel);
    this.suMenu.menuLabels = [...this.suMenu.menuLabels];
    this.menuForm.markAsDirty();
  }

  createMenuLabelForm(menuLabel: SuMenuLabel) {
    const fg = this.fb.group({
      menuCode: null,
      langCode: [null, Validators.required],
      menuName: [null, [Validators.required, Validators.maxLength(200)]],
      systemCode: this.menuForm.controls.systemCode.value,
      assigned: false
    });
    fg.valueChanges.subscribe((control) => {
      if (control.assigned && menuLabel.rowState == RowState.Normal) {
        menuLabel.rowState = RowState.Edit;
      }
    });
    fg.patchValue(menuLabel);
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }

  remove(menuLabel: SuMenuLabel) {
    if (menuLabel.rowState !== RowState.Add) {
      menuLabel.rowState = RowState.Delete;
      this.menuLabelDelete.push(menuLabel);
    }
    this.suMenu.menuLabels = this.suMenu.menuLabels.filter(item => item.guid !== menuLabel.guid);
    this.menuForm.markAsDirty();
  }


  canDeactivate(): Observable<boolean> {
    if (!this.menuForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
