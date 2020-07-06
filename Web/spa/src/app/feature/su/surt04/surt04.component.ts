import { Component, OnInit } from '@angular/core';
import { Surt04Service } from './surt04.service';
import { Page, ModalService, FormUtilService, Size, Pattern } from '@app/shared';
import { Router } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-surt04',
  templateUrl: './surt04.component.html'
})
export class Surt04Component implements OnInit {

  page = new Page();
  keyword: string = '';
  profiles = [];
  processPopup: BsModalRef;
  copyForm: FormGroup;

  constructor(
    private su: Surt04Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private util: FormUtilService,
    private saver: SaveDataService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    const saveData = this.saver.retrive('surt04');
    if (saveData) this.keyword = saveData;
    const savePage = this.saver.retrive('surt04Page');
    if (savePage) this.page = savePage;
    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.keyword, 'surt04');
    this.saver.save(this.page, 'surt04Page');
  }

  search() {
    this.su.getProfiles(this.keyword, this.page)
      .pipe()
      .subscribe(res => {
        this.profiles = res.rows;
        this.page.totalElements = res.count;
      });
  }

  add() {
    this.router.navigate(['/su/surt04/detail']);
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

  remove(code, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.su.delete(code, version)
            .subscribe(() => {
              this.ms.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  openModal(row, content) {
    this.copyForm = this.fb.group({    
      profileCodeFrom: { value: row.profileCode, disabled: true },
      profileDescFrom: { value: row.profileDesc, disabled: true },
      profileCodeTo: [null, [Validators.required, Validators.maxLength(10), Validators.pattern(Pattern.UpperOnly)]],
      profileDescTo: [null, [Validators.required, Validators.maxLength(50)]]
    });

    this.processPopup = this.modal.open(content, Size.large); 
  }

  closeModal() {
    this.processPopup.hide();
  }

  copy() {
    if (this.copyForm.invalid) {
      this.util.markFormGroupTouched(this.copyForm);
      return;
    }
    if (this.copyForm.controls.profileCodeFrom.value === this.copyForm.controls.profileCodeTo.value) {
      this.ms.warning("message.STD00040", ['label.SURT04.CopyFrom', 'label.SURT04.CopyTo']);
      return;
    }
    this.su.copy(Object.assign({}, 
      this.copyForm.value, 
      { 
        profileCodeFrom: this.copyForm.controls.profileCodeFrom.value,
        profileDescFrom: this.copyForm.controls.profileDescFrom.value
      })).subscribe(() => {
      this.ms.success("message.STD00020");
      this.closeModal();
      this.search();
    });
  }
}
