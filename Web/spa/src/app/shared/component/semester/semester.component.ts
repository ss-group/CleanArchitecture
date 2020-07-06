import { Component, ViewChild, ElementRef, SimpleChanges } from '@angular/core';
import IMask from 'imask';
import { BaseFormField } from '../base-form';
import { Subscription, BehaviorSubject } from 'rxjs';
import { FormControl, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import * as Big from 'big.js';

@Component({
  selector: 'semester',
  templateUrl: './semester.component.html',
  styleUrls: ['./semester.component.scss']
})
export class SemesterComponent extends BaseFormField {
  imask = {
    mask: Number,
    scale: 0,
    signed: false,
    thousandsSeparator: '',
    min: 1,
    max: 99
  };
  maskRef?: IMask.InputMask<IMask.AnyMaskedOptions>;

  @ViewChild('semester') semester: ElementRef;
  private _writing: boolean = false;
  private _writingValue: any;
  ready: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  readySub: Subscription;

  private initMask() {
    this.maskRef = IMask(this.element, this.imask)
      .on('accept', this._onAccept.bind(this))
  }
  get element() {
    return this.semester.nativeElement;
  }

  ngOnInit() {
    if (this.control) {
      const validatorMin = this.control.validator && this.control.validator(new FormControl(-1))
      const hasMin = Boolean(validatorMin && validatorMin.hasOwnProperty('min'));
      if(hasMin){
        this.imask.signed = false;
        this.imask.min = 0;
      }
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.value) {
      this._assignValue(changes.value.currentValue);
    }
  }

  ngAfterViewInit() {
    this.initMask();
    this.ready.next(true);
    if (this.value) {
      this._assignValue(this.value);
    }
  }

  destroyMask() {
    if (this.maskRef) {
      this.maskRef.destroy();
      delete this.maskRef;
    }
  }

  ngOnDestroy() {
    this.destroyMask();
    if (this.readySub) this.readySub.unsubscribe();
  }

  _assignValue(value){
    value = value != null && value != undefined ? Number(new Big(value).toFixed(0)) : null;
    if (this.maskRef) {
      this.beginWrite(value);
      this.maskValue = value;
    }
  }

  writeValue(value: any): void {
    this.readySub = this.ready.pipe(first(ready => ready)).subscribe(() => {
      this._assignValue(value);
    })
  }

  get maskValue(): any {
    return this.maskRef.value ? this.maskRef.typedValue : null;
  }

  set maskValue(value: any) {
    this.maskRef.typedValue = value;
  }


  beginWrite(value: any): void {
    this._writing = true;
    this._writingValue = value;
  }

  endWrite(): any {
    this._writing = false;
    return this._writingValue;
  }

  _onAccept() {
    const value = this.maskValue;
    if (this._writing && value === this.endWrite()) return;
    this.onChange(value);
  }

}
