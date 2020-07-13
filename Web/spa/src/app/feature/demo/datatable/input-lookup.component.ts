import { Component } from '@angular/core';
import { ModalService, Size, RowState, LookupMultipleResult, BaseList } from '@app/shared';
import { LookupMultipleComponent } from './lookup-multiple/lookup-multiple.component';

@Component({
  selector: 'app-input-lookup',
  templateUrl: './input-lookup.component.html',
  styleUrls: ['./input-lookup.component.scss']
})
export class InputLookupComponent{

  demoItems = [];
  demoItemsDelete = [];
  lookupMultiple = LookupMultipleComponent
  constructor(private modal:ModalService) { }

  ngOnInit(){
    let item = new BaseList();
    item = Object.assign({ guid:'', categoryCode:'11', categoryNameTha:'11',rowState:RowState.Normal});
    this.demoItems.push(item);
  }
  addItems(){
     const initial = {
      selectedList: [...this.demoItems]
     }
     this.modal.openComponent(this.lookupMultiple,Size.large,initial).subscribe((list:LookupMultipleResult)=>{
       if(list){
         this.demoItems = [...list.selected]
       }
     })
  }

  removeItem(row) {
    if (row.rowState !== RowState.Add) {
      row.rowState = RowState.Delete;
      this.demoItemsDelete.push(row);
    }
    this.demoItems = this.demoItems.filter(item => item.guid !== row.guid);
  }
}
