import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, switchMap } from 'rxjs/operators';
import { AuthService } from '@app/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

export enum ScreenSize {
    Small = 768,
    Large = 1400
}

export interface Menu {
    title?: string;
    icon?: string;
    active?: boolean;
    type?: string;
    url?: string;
    subMenus?: Menu[];
    badge?: any;
}

@Injectable({
    providedIn: 'root'
})
export class SidebarService {
    toggled = false;
    _hasBackgroundImage = false;
    menus = [] as Menu[];

    isMediumScreenObserv: Observable<boolean>;

    constructor(public breakpointObserver: BreakpointObserver, private http: HttpClient) {
        this.isMediumScreenObserv = this.breakpointObserver
            .observe([`(max-width: ${ScreenSize.Large - 1}px)`])
            .pipe(map(state => state.matches))
    }

    toggle() {
        this.toggled = !this.toggled;
    }

    getSidebarState() {
        return this.toggled;
    }

    setSidebarState(state: boolean) {
        this.toggled = state;
    }

    getMenuList() {
        const demos = [{
            title: 'Demo',
            icon: 'fas fa-laptop-code',
            active: false,
            type: 'dropdown',
            subMenus: [
                { title: 'Layout', icon: 'far fa-circle fa-xs', url: '/demo/layout' },
                { title: 'Input', icon: 'far fa-circle fa-xs', url: '/demo/input' },
                { title: 'Table', icon: 'far fa-circle fa-xs', url: '/demo/datatable' },
                { title: 'Input-Table', icon: 'far fa-circle fa-xs', url: '/demo/input-datatable' },
                { title: 'Input-Lookup-Table', icon: 'far fa-circle fa-xs', url: '/demo/input-lookup' },
                { title: 'Report', icon: 'far fa-circle fa-xs', url: '/demo/report' },
                { title: 'Reportssru', icon: 'far fa-circle fa-xs', url: '/demo/reportssru' }
            ]
        } as Menu];
        if(environment.production){
            return this.http.get<Menu[]>('menu');
        }
        else return this.http.get<Menu[]>('menu').pipe(
            map(menu => demos.concat(menu))
        )
    }

    get hasBackgroundImage() {
        return this._hasBackgroundImage;
    }

    set hasBackgroundImage(hasBackgroundImage) {
        this._hasBackgroundImage = hasBackgroundImage;
    }
}
