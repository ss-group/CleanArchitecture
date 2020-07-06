import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt15Service } from './dbrt15.service';
import { MessageService, SaveDataService } from '@app/core';

@Component({
    selector: 'app-dbrt15',
    templateUrl: './dbrt15.component.html',
    styleUrls: ['./dbrt15.component.scss']
})
export class Dbrt15Component implements OnInit {

    page = new Page();
    subdegreeForm: FormGroup;
    keyword: string = '';
    subdegree = [];

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private cs: Dbrt15Service,
        private modal:ModalService,
        private as:MessageService,
        private util:FormUtilService,
        private saver:SaveDataService,
    ) { }
    ngOnInit() {
      this.search();

    }

    search() {
        this.cs.getSubDegree(this.keyword, this.page)
          .subscribe(res => {
            this.subdegree = res.rows;
            this.page.totalElements = res.count;
          });
    }
    enter(value) {
        this.keyword = value;
        this.page.index = 0;
        this.search();
    }
    
    clear() {
      this.subdegreeForm.reset();
    }

    remove(id, version) {
        this.modal.confirm("message.STD00003").subscribe(
          (res) => {
            if (res) {
              this.cs.delete(id, version)
                .subscribe(() => {
                  this.as.success("message.STD00014");
                  this.page = this.util.setPageIndex(this.page);
                  this.search();
                });
            }
          })
    }
    
    add() {
        this.router.navigate(['/db/dbrt15/detail']);
    }
}
