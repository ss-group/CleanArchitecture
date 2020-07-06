import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Location } from "@angular/common";
import { ModalService } from '@app/shared';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Surt06Service, UserType } from './surt06.service';
import { BsModalRef } from 'ngx-bootstrap';
import { tap, switchMap, first } from 'rxjs/operators';

@Component({
  selector: 'app-surt06',
  templateUrl: './surt06.component.html',
  styleUrls: ['./surt06.component.scss']
})
export class Surt06Component implements OnInit {
  master = { statuses: [], userTypes: [] };
  userTypeParam: UserType;
  userTypeForm: FormGroup;
  userTypePopup: BsModalRef;
  @ViewChild('userTypeRef') userTypeRef: TemplateRef<any>;
  private isOpen = false;
  constructor(
    private location:Location,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private modal: ModalService,
    public ss: Surt06Service,
  ) { }

  ngOnInit() {
  
    this.route.data.subscribe((data) => {
      this.master = data.surt06;
      this.userTypeParam = this.ss.userTypeParam
      if (!this.ss.userType) {
        setTimeout(() => {
          this.openUserType();
        });
      }
    });
   
  }

  ngOnDestroy(): void {
    if (this.userTypePopup && this.isOpen){
      this.close();
    }
  }

  createUserTypeForm() {
    this.userTypeForm = this.fb.group({
      type: this.userTypeParam.Employee
    });
  }

  openUserType() {
    this.createUserTypeForm();
    this.userTypePopup = this.modal.open(this.userTypeRef);
    this.isOpen = true;
  }

  close() {
    this.userTypePopup.hide();
    this.isOpen = false;
    if(!this.ss.userType)
    this.location.back();
  }

  onSelectUserType() {
    const code = this.userTypeForm.controls.type.value;
    const name = this.master.userTypes.find(type => type.value == code).text;
    this.ss.userType = { code: code, name: name };
    this.close()
    this.router.navigate(['su/surt06/search']);
  }
}
