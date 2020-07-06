import { Component, OnInit } from '@angular/core';
import { Page, BaseLookupMultipleComponent } from '@app/shared';
import { Surt04Service } from './surt04.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-surt04-lookup',
  templateUrl: './surt04-lookup.component.html'
})
export class Surt04LookupComponent extends BaseLookupMultipleComponent {

  masterMenus = [];
  page = new Page();

  constructor(
    private su: Surt04Service,
    public modal: BsModalRef) { super(modal); }

  ngOnInit() {
    this.search();
  }

  enter(value) {
    this.page.index = 0;
    this.keyword = value;
    this.search();
  }

  search() {
    this.su.getMaster(this.keyword, this.page).pipe().subscribe(data => {
      this.masterMenus = this.excludeRows(data.rows);
      this.page.totalElements = data.count;
    });
  }

  identity(row){
    return row.menuCode;
  }

}
