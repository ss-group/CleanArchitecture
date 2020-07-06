import { Injectable } from '@angular/core';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, map, first, switchMap, filter } from 'rxjs/operators';
import { Category } from './category';
import { ParameterService } from '@app/shared/service/parameter.service';

export enum Type {
    Image = "Image",
    File = "File"
}

export interface Content {
    id: number,
    name: string,
    path: string | ArrayBuffer
}

export interface Path {
    contentUrl: string
}
@Injectable({
    providedIn: 'root'
})

export class ContentUploadService {

    private pathSub = new BehaviorSubject<Path>(null);
    pathObserv = this.pathSub.pipe(first(value => value != null));
    constructor(private http: HttpClient, private ps: ParameterService) {
        this.ps.getParameter("Path", "ShareContent").subscribe(path => {
            const result: Path = { contentUrl: path.ShareContent }
            this.pathSub.next(result);
        })
    }

    getContent(id) {
        return this.pathObserv.pipe(
            switchMap(path => this.http.disableApiPrefix().skipErrorHandler().disableLoading().get<Content>(`${path.contentUrl}api/content`, { params: { id: id } })),
            map(content => content || {} as Content),
            catchError(err => {
                return of({} as Content)
            })
        )
    }

    upload(file: File, type: Type, category: Category): Observable<any> {
        var formData: any = new FormData();
        formData.append("file", file);
        formData.append("category", category || Category.Example);
        return this.pathObserv.pipe(
            switchMap(path => this.http.disableApiPrefix().skipErrorHandler().disableLoading().post<Content>(`${path.contentUrl}api/content/${type}`, formData, {
                reportProgress: true,
                observe: 'events'
            })),
            map(content => content || {} as Content),
            catchError(err => {
                return of({} as Content)
            })
        )
    }
}