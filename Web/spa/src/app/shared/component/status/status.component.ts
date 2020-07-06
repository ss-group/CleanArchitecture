import { Component, OnInit, Input } from '@angular/core';
import { StatusColor } from './color';


@Component({
  selector: 'status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.scss']
})
export class StatusComponent implements OnInit {
  @Input() hasLabel:boolean = true;
  @Input() small:boolean = false;
  @Input() status:string = null;
  @Input() color:StatusColor = StatusColor.NORMAL;
  constructor() { }

  ngOnInit() {

  }

}
