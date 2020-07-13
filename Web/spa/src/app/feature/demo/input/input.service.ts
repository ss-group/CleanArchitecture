import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InputService {

  constructor(private http:HttpClient) { }

  getAutoComplete(keyword,value){
    return this.http.disableLoading().get<any>('csrt01/typeLevel', { params: { keyword: keyword,value:value } });
  }
}
