import { Component, Input, Output, EventEmitter, ContentChild, TemplateRef, SimpleChanges } from '@angular/core';
import { BaseFormField } from '../base-form';
import { Subject, BehaviorSubject, Subscription } from 'rxjs';
import { first, switchMap, filter } from 'rxjs/operators';

@Component({
  selector: 'select-input',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss']
})
export class SelectComponent extends BaseFormField {

  @Input() items = [];
  @Input() bindLabel = "text";
  @Input() bindValue = "value";
  @Input() bindDesc = "description";
  @Input() modelChange: Subject<any>;
  @Input() showDescription = false;
  @Input() customLabel = false;
  @Input() customTemplate = false;
  @Input() multiple = false;
  @Input() labelTemplate: TemplateRef<any>;
  @Input() optionTemplate: TemplateRef<any>;
  @Output() returnValue = new EventEmitter<any>();

  appendTo: string;
  customSearch: (term: string, item: any) => boolean = null;
  private ready = new BehaviorSubject<boolean>(false);
  private readySub: Subscription;
  @ContentChild(TemplateRef) defaultTemplate: TemplateRef<any>;
  ngOnInit() {
    this.appendTo = this.small ? 'Body' : null
    this.placeholder = this.placeholder ? this.placeholder : 'label.ALL.PleaseSelect'
    if (this.customTemplate && this.optionTemplate == null) {
      this.optionTemplate = this.defaultTemplate;
    }
    if (this.showDescription) {
      this.customSearch = this.searchFn;
    }
    this.ready.next(true);
  }

  ngOnDestroy() {
    if (this.readySub) this.readySub.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.items) {
      if (this.modelChange) {
        const item = changes.items.currentValue.find(item => item[this.bindValue] == this.value);
        this.modelChange.next(item || {});
      }
    }
  }

  writeValue(value: any): void {
    this.value = value;
    this.readySub = this.ready.pipe(
      first(ready => ready)
    ).subscribe(() => {
      if (this.modelChange) {
        const item = this.items.find(item => item[this.bindValue] == this.value);
        this.modelChange.next(item || {});
      }
    });
  }

  onSelected(event) {
    this.onChange(event);
    this.value = event;
  }

  searchFn = (term: string, item: any) => {
    term = term.toLowerCase();
    return item[this.bindLabel].toLowerCase().indexOf(term) > -1 || item[this.bindDesc].toLowerCase().indexOf(term) > -1;
  }

  onItemChange(model) {
    if (this.modelChange) {
      this.modelChange.next(model || {});
    }
    if (this.returnValue) {
      setTimeout(() => {
        this.returnValue.emit(model);
      }, 100);
    }
  }
}
