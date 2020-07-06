import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from '@env/environment';
import { OrganizationService, I18nService, AuthService } from '@app/core';
import { combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

export enum ContentType {
    PDF = 'PDF',
    EXCEL = 'XLSX'
}

export interface ReportParam {
    paramsJson: any;
    module: string;
    reportName: string;
    exportType: string;
    autoLoadLabel?: string;
}

@Injectable({ providedIn: 'root' })
export class ReportService {

    constructor(
        private http: HttpClient,
        private org: OrganizationService,
        private lang: I18nService,
        private authService: AuthService) { }
    serverUrl = `${environment['reportUrl']}`;


    download(resp: HttpResponse<Blob>) {
        const fileName = this.getFileNameFromResponseContentDisposition(resp);
        const downloadedFile = new Blob([resp.body], { type: this.getContentType(resp) });
        var downloadUrl = URL.createObjectURL(downloadedFile);
        var anchor = document.createElement("a");
        anchor.href = downloadUrl;
        anchor.target = '_blank';
        anchor.download = fileName;
        document.body.appendChild(anchor);
        anchor.click();

        document.body.removeChild(anchor);
        URL.revokeObjectURL(downloadUrl);
    }

    private getFileNameFromResponseContentDisposition(response) {
        const contentDisposition = response.headers.get('content-disposition') || '';
        const matches = /filename=([^;]+)/ig.exec(contentDisposition);
        const fileName = (matches[1] || 'untitled').replace(/"/g, '').trim();
        return fileName;
    };

    private getContentType(response) {
        return response.headers.get('content-type') || '';
    }

    generateReportBase64(param: ReportParam) {
        let company = "";
        this.org.company.subscribe(res => {
            company = res
        });
        let userName = "";
        this.authService.claims.subscribe(claim => {
            userName = claim.name;
        })
        var objParam = {};
        objParam = param.paramsJson;
        objParam["company_code"] = company;
        objParam["lin_id"] = this.lang.language.toLowerCase();
        objParam["user_name"] = userName;
        objParam["ip_address"] = "";
        param.paramsJson = objParam;
        console.log("paramReport ");
        console.log(param);
        return this.http.disableApiPrefix().post(this.serverUrl + '/exportReportBase64', param, { responseType: 'text' });
    }

    generateReport(param: ReportParam) {
        return combineLatest(this.org.company, this.authService.claims).pipe(
            switchMap(([company, claims]) => {
                var system = {
                    company_code: company,
                    lin_id: this.lang.language,
                    user_name: claims.name,
                    ip_address: ''
                }
                Object.assign(param.paramsJson, system);
                return this.http.post(`${this.serverUrl}/exportReport`, param, { responseType: 'blob', observe: "response" });
            })
        )
    }
}
