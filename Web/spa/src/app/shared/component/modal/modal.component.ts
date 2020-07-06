import { Component, OnInit, Input, EventEmitter, Output, HostListener } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {

  @Input() header: string;
  @Output() onClose = new EventEmitter();
  constructor(

  ) { }
  close() {
    this.onClose.emit();
  }

  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler(event: KeyboardEvent) {
    this.onClose.emit();
  }
}
