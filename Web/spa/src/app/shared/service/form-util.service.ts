import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Page } from '../component/datatable/page.model';
import { BaseList } from './base.service';
import { RowState } from '../component/datatable/state.enum';

@Injectable()
export class FormUtilService {
    markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.markAsTouched();

            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }

    focusInvalid(el) {
        const invalidElements = el.querySelectorAll('.ng-invalid');
        if (invalidElements.length > 0) {
            invalidElements[0].scrollIntoView(false);
        }
    }

    setPageIndex(page: Page, deletedCount: number = 1) {
        const index = Math.min(Math.ceil((page.totalElements - deletedCount) / page.size) - 1, page.index);
        page.index = index < 0 ? 0 : index;
        return page;
    }

    //hidden from dropdownlist 
    getActive(originals: any[], baseValue: any): any[] {
        let items = [];
        if (baseValue) {
            items = originals.reduce((result,row)=>{
                if(row.active || baseValue == row.value){
                   result.push(row);
                }
                return result;
            },[])
        }
        else {
            items = originals.reduce((result,row)=>{
                if(row.active){
                   result.push(row);
                }
                return result;
            },[])
        }
        return items;
    }
}