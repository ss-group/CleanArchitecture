import { Injectable, Component, ComponentFactoryResolver, Type, Injector } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmComponent } from './confirm/confirm.component';
import { ModalResolve } from './modal-resolver';
import { switchMap } from 'rxjs/operators';


export enum Size {
  small = "modal-sm",
  medium = "",
  large = "modal-lg",
  xlarge = "modal-xl"
}

@Injectable()
export class ModalService {
  private openModal: BsModalRef;
  constructor(
    private injector: Injector,
    private bsModalService: BsModalService
  ) { }

  confirm(message: string, size?: Size): Observable<boolean> {
    const initialState = {
      message: message
    };
    this.openModal = this.bsModalService.show(ConfirmComponent, { initialState, ignoreBackdropClick: true, class: size });
    return this.openModal.content.selected;
  }

  //templateRef
  open(content, size?: Size, initialState?: Object) {
    this.openModal = this.bsModalService.show(content, { initialState, ignoreBackdropClick: true, class: size });
    return this.openModal;
  }

  //component
  openComponent(content, size?: Size, initialObject?: Object, resolver?: any, resolverParam?: any): Observable<any> {
    const initialState = initialObject || {};
    if (resolver) {
      const instant = this.injector.get(resolver) as ModalResolve<any>;
      return instant.resolve(resolverParam).pipe(
        switchMap((result) => {
          initialState['resolved'] = result;
          this.openModal = this.bsModalService.show(content, { initialState, ignoreBackdropClick: true, class: size });
          return this.openModal.content.selected.asObservable();
        })
      )
    }
    else {
      this.openModal = this.bsModalService.show(content, { initialState, ignoreBackdropClick: true, class: size });
      return this.openModal.content.selected.asObservable();
    }
  }
  
  destroy(){
    if(this.openModal){
      this.openModal.hide();
    }
  }
}