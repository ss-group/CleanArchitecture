import { Component} from '@angular/core';
import { BaseFormField } from '../base-form';

@Component({
  selector: 'textbox',
  templateUrl: './textbox.component.html',
  styleUrls: ['./textbox.component.scss']
})
export class TextboxComponent extends BaseFormField  {
 
  ngOnInit(){
    this.value = this.value || '';
  }

  writeValue(value: any): void {
    this.value = value
  }

  onTextChange(value){
    this.onChange(value);
    if(this.required){
      const notEmpty = new RegExp(/\S+/);
      if(notEmpty.test(value)){
        this.control.setErrors({ empty:null}, { emitEvent: false });
        this.control.updateValueAndValidity({ onlySelf: false, emitEvent: false });
      }
      else{
        this.control.setErrors({ empty:true}, { emitEvent: false });
      }
    }
    this.value = value;
  }
}
