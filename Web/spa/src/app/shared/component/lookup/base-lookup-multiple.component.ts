
import { Output, EventEmitter, Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { RowState } from '../datatable/state.enum';
import { Guid } from 'guid-typescript';
import { SubscriptionDisposer } from '../subscription-disposer';

export interface LookupMultipleResult {
    selected: any[],
    deleting: any[]
}

export interface HasIdentity {
    identity(row: any): any;
}

@Component({
    selector: 'app-base-lookup-multiple',
    template: 'no template'
})
export class BaseLookupMultipleComponent extends SubscriptionDisposer implements HasIdentity {

    keyword: string = '';
    parameter: any = {};
    selectedList: any[] = [];
    deleteList: any[] = [];
    alreadySelected = [];
    isFirst: boolean = true;
    @Output() selected = new EventEmitter<LookupMultipleResult>();
    constructor(
        public bsModalRef: BsModalRef
    ) {
        super()

    }

    onAccept(): void {
        this.prepareSelect();
        this.selected.next({ selected: this.selectedList, deleting: this.deleteList } as LookupMultipleResult);
        this.bsModalRef.hide();
    }

    onClose(): void {
        this.selected.next(null);
        this.bsModalRef.hide();
    }

    identity(row: any) {
        throw new Error("Please add identity function.");
    }

    headDisabled(rows:any[]){
        return !rows.some(row=>!row.disabled)
    }

    rowEnable(row){
        return !row.disabled;
    }

    excludeRows(rows: any[]) {
        if (this.isFirst) {
            this.alreadySelected = [...this.selectedList];
            this.selectedList = [];
            this.isFirst = false;
        }
        rows.forEach(row => {
            if (this.alreadySelected.some(select => this.identity(select) === this.identity(row))) {
                row.disabled = true;
            }
            else {
                row.rowState = RowState.Add;
                row.guid = Guid.raw();
            }

        })
        return rows;
    }

    onSelectMultiple(row: any, checked: boolean) { }
    onSelectAll(checked: boolean) { }

    prepareSelect() {
        this.selectedList = this.selectedList.filter(selected => !selected.disabled).concat(this.alreadySelected);
    }
}