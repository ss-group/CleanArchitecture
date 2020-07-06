import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService, StatusColor } from '@app/shared';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';
import { first, tap, switchMap } from 'rxjs/operators';
import { Surt06Service, UserType } from './surt06.service';

@Component({
  selector: 'app-surt06-search',
  templateUrl: './surt06-search.component.html',
  styleUrls: ['./surt06-search.component.scss']
})
export class Surt06SearchComponent implements OnInit {
  color = StatusColor;
  users = [];
  master = { statuses: [], userTypes: [] };
  userTypeParam: UserType;
  page = new Page();
  searchForm: FormGroup;
  personLabel = '';
  personPlaceholder = '';
  personCode = '';
  personName = '';
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private modal: ModalService,
    private ms: MessageService,
    public ss: Surt06Service,
    private saver: SaveDataService,
    public util: FormUtilService
  ) { }

  ngOnInit() {
    if( this.ss.userType.code == this.ss.userTypeParam.Student ){
      this.personLabel =  'label.SURT06.Student';
      this.personPlaceholder =  'label.SURT06.StudentPlace' ;
      this.personCode =  'label.SURT06.StudentCode' ;
      this.personName =  'label.SURT06.StudentName';
    }
    else if(this.ss.userType.code == this.ss.userTypeParam.VisitingProfessor){
      this.personLabel =  'label.SURT06.VisitingProfessor';
      this.personPlaceholder =  'label.SURT06.VisitingProfessorPlace' ;
      this.personCode =  'label.SURT06.VisitingProfessorCode' ;
      this.personName =  'label.SURT06.VisitingProfessorName';
    }
    else{
      this.personLabel =  'label.SURT06.Employee';
      this.personPlaceholder =  'label.SURT06.EmployeePlace';
      this.personCode = 'label.SURT06.EmployeeCode';
      this.personName =  'label.SURT06.EmployeeName';
    }

    this.createSearchForm();
    this.route.data.subscribe((data) => {
      this.master = data.surt06;
      this.userTypeParam = this.ss.userTypeParam;
    });
    const saveData = this.saver.retrive('SURT06');
    if (saveData) this.searchForm.patchValue(saveData);
    const savePage = this.saver.retrive('SURT06Page');
    if (savePage) this.page = savePage;
    this.search(false);
  }

  ngOnDestroy(): void {
    this.saver.save(this.searchForm.value, 'SURT06');
    this.saver.save(this.page, 'SURT06Page');
  }

  createSearchForm() {
    this.searchForm = this.fb.group({
      username: null,
      employee: null,
      status: null
    })
  }

  search(resetIndex) {
    if (resetIndex) this.page.index = 0;
    this.ss.getUsers(this.searchForm.value,this.ss.userType.code, this.page)
      .subscribe(res => {
        this.users = res.rows;
        this.page.totalElements = res.count;
      });
  }

  clear() {
    this.searchForm.reset();
  }

  add() {
    this.router.navigate(['/su/surt06/detail'], { state: { keepUserType: true } });
  }

  remove(userId, version) {
    this.modal.confirm("message.STD00003").pipe(first(confirm => confirm))
      .subscribe(() => {
        this.ss.delete(userId, version).subscribe(() => {
          this.ms.success("message.STD00014");
          this.page = this.util.setPageIndex(this.page);
          this.search(false);
        });
      })
  }
}
