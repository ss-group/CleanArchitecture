import { Component, Input, Output, EventEmitter, ViewChild, QueryList, ContentChildren, ContentChild} from '@angular/core';
import { ColumnMode, DatatableComponent, DataTableColumnDirective, DatatableMergeHeaderDirective, DatatableRowDetailDirective } from 'ss-group-datatable';
import { Page } from '../page.model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['../table.scss']
})
export class TableComponent {
  ColumnMode = ColumnMode;
  @Input() rows: any[];
  @Input() page = new Page();
  @Input() select: any[] = [];
  @Input() selectOnePage: boolean = false;
  @Output() onTableEvent = new EventEmitter();
  @Input() rowIdentity: (x: any) => any = ((x: any) => x);
  @Input() trackByProp: string = "guid";
  @Input() summaryRow: boolean = false;
  hasGroupHeader:boolean = false;
  pageSizes = [5,...Array.from(Array(20).keys()).map((v, i) => 10 + i * 10)];
  messages = {
    emptyMessage: `
      <div class="text-center text-secondary">
        <span>ไม่พบข้อมูล</span>
      </div>
    `
  }
  columnMode = ColumnMode;

  @ViewChild(DatatableComponent) datatable: DatatableComponent;
  @ContentChildren(DatatableMergeHeaderDirective) mergeHeaders : QueryList<DatatableMergeHeaderDirective>;
  @ContentChildren(DataTableColumnDirective) columns: QueryList<DataTableColumnDirective>;
  @ContentChild(DatatableRowDetailDirective) rowDetail: DatatableRowDetailDirective;

  ngAfterContentInit() {
    this.datatable.mergeHeaders = this.mergeHeaders;
    this.datatable.columnTemplates = this.columns;
    this.datatable.rowDetail = this.rowDetail;
  }

  ngOnDestroy() {
    this.onTableEvent.complete();
  }
  onSort(event): void {
    this.datatable.offset = this.page.index;
    let prop: string = event.sorts[0].prop;
    const props = prop.split(",");
    this.page.sort = props.map(prop=>`${prop} ${event.sorts[0].dir}`).join(",");
    this.onTableEvent.emit();
  }

  onPage(event): void {
    this.page.index = event.offset;
    this.onTableEvent.emit();
  }

  onPageSize(value) {
    this.page.index = 0;
    this.page.size = Number(value);
    this.onTableEvent.emit();
  }
  onChecked({ selected }) {
    this.select.splice(0, this.select.length);
    this.select.push(...selected);
  }

  toggleExpandRow(row) {
    this.datatable.rowDetail.toggleExpandRow(row);
  }

  expandAllRows(){
    setTimeout(() => {
    this.datatable.rowDetail.expandAllRows();
      
    }, 100);
  }

  collapseAllRows(){
    this.datatable.rowDetail.collapseAllRows();
  }
}
