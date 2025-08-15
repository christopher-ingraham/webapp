import { Injectable } from '@angular/core';
import { HttpHeaders, HttpParams, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, finalize, map } from 'rxjs/operators';

import { OidcSecurityService } from 'angular-auth-oidc-client';

import { HttpStatusCode, ListRequest, PagingRequest, SortRequestDto, DownloadResponse } from './model';
import { HttpVerb } from '../api/model';
import { LogService } from '../log';
import { ConfigurationService } from '../configuration';
import { NotifierService } from '../notifier';
import {
    ApiCallFailError,
    BadRequestError,
    ForbiddenError,
    InternalServerError,
    NotFoundError,
    UnauthorizedError,
} from './errors';
import { JsonHelperService, UrlHelperService } from '../helpers';


@Injectable({
    providedIn: 'root'
})
export class ApiService {

    constructor(
        private http: HttpClient,
        private config: ConfigurationService,
        private urlHelper: UrlHelperService,
        private notifier: NotifierService,
        private oidcService: OidcSecurityService,
        protected log: LogService,
        private jsonHelperService: JsonHelperService
    ) { }

    public createListRequest<TListFilter>(filter: TListFilter): ListRequest<TListFilter> {
        const page: PagingRequest = {
            skip: 0,
            take: 10,
        };
        const sort: SortRequestDto = {
            items: [],
        };
        const listRequest: ListRequest<TListFilter> = { filter, page, sort, };
        return listRequest;
    }

    private logExec(methodName: HttpVerb, url: string, body?: any) {
        const message = `ApiService: executing ${methodName} "${url}"`;
        if (body) {
            this.log.debug(message, body);
        } else {
            this.log.debug(message);
        }
    }

    private logExecResult(methodName: HttpVerb, url: string, body?: any) {
        const message = `ApiService: executed ${methodName} "${url}" result`;
        if (body) {
            this.log.debug(message, body);
        } else {
            this.log.debug(message);
        }
    }

    public get<T>(relativeUrl: string, parameters?: any): Observable<T> {

        const url = this.buildUrl(relativeUrl);

        const options = { headers: new HttpHeaders(), params: new HttpParams() };
        options.headers = this.appendApiHeaders(options.headers);
        options.params = this.appendParams(options.params, parameters);


        this.logExec(HttpVerb.GET, url);
        this.notifier.busy();

        return this.http
            .get<T>(url, options)
            .pipe(
                map(res => res),
                // for tap reference: https://angular.io/tutorial/toh-pt6#tap-into-the-observable
                tap(_ => this.logExecResult(HttpVerb.GET, url, _)),
                // Observable.throw is obsolete, replaced with throwError operator
                catchError(_ => throwError(this.handleError(url, HttpVerb.GET, _))),
                finalize(() => this.notifier.unbusy())
            );
    }

    public download(relativeUrl: string, parameters?: any): Observable<DownloadResponse> {
        const url = this.buildUrl(relativeUrl);
        const options = {
            headers: this.appendApiHeaders(),
            observe: 'response' as 'body',
            params: this.appendParams(new HttpParams(), parameters),
            responseType: 'blob' as 'json',
        };
        this.logExec(HttpVerb.GET, url);
        this.notifier.busy();
        return this.http.get<HttpResponse<Blob>>(url, options)
            .pipe(
                map((response: HttpResponse<Blob>) => ({
                    name: this.getHttpResponseFileName(response, 'report.xlsx'),
                    data: response.body,
                } as DownloadResponse)),
                catchError((err) => throwError(this.handleError(url, HttpVerb.GET, err))),
                finalize(() => this.notifier.unbusy())
            );
    }

    public post<T>(relativeUrl: string, body?: any, parameters?: any): Observable<T> {

        const url = this.buildUrl(relativeUrl);

        const options = { headers: new HttpHeaders(), params: new HttpParams() };
        options.headers = this.appendApiHeaders(options.headers);
        options.params = this.appendParams(options.params, parameters);

        this.logExec(HttpVerb.POST, url, body);
        this.notifier.busy();

        return this.http
            .post<T>(url, body, options)
            .pipe(
                map(res => this.jsonHelperService.clone(res)),
                // for tap reference: https://angular.io/tutorial/toh-pt6#tap-into-the-observable
                tap(result => this.logExecResult(HttpVerb.POST, url, result)),
                // Observable.throw is obsolete, replaced with throwError operator
                catchError(_ => throwError(this.handleError(url, HttpVerb.POST, _))),
                finalize(() => this.notifier.unbusy())
            );
    }

    public put<T>(relativeUrl: string, body?: any, parameters?: any): Observable<T> {

        const url = this.buildUrl(relativeUrl);

        const options = { headers: new HttpHeaders(), params: new HttpParams() };
        options.headers = this.appendApiHeaders(options.headers);
        options.params = this.appendParams(options.params, parameters);

        this.logExec(HttpVerb.PUT, url, body);
        this.notifier.busy();

        return this.http
            .put<T>(url, body, options)
            .pipe(
                map(res => this.jsonHelperService.clone(res)),
                // for tap reference: https://angular.io/tutorial/toh-pt6#tap-into-the-observable
                tap(result => this.logExecResult(HttpVerb.PUT, url, result)),
                // Observable.throw is obsolete, replaced with throwError operator
                catchError(_ => throwError(this.handleError(url, HttpVerb.PUT, _))),
                finalize(() => this.notifier.unbusy())
            );
    }

    public delete(relativeUrl: string, parameters?: any): Observable<Object> {

        const url = this.buildUrl(relativeUrl);

        const options = { headers: new HttpHeaders(), params: new HttpParams() };
        options.headers = this.appendApiHeaders(options.headers);
        options.params = this.appendParams(options.params, parameters);

        this.logExec(HttpVerb.DELETE, url);
        this.notifier.busy();

        return this.http
            .delete(url, options)
            .pipe(
                // for tap reference: https://angular.io/tutorial/toh-pt6#tap-into-the-observable
                tap(result => this.logExecResult(HttpVerb.DELETE, url, result)),
                // Observable.throw is obsolete, replaced with throwError operator
                catchError(_ => throwError(this.handleError(url, HttpVerb.DELETE, _))),
                finalize(() => this.notifier.unbusy())
            );
    }

    private buildUrl = (relativeUrl: string): string => `${this.config.get().apiUrl}/${this.urlHelper.trimLeadingSlash(relativeUrl)}`;

    private appendApiHeaders(headers?: HttpHeaders): HttpHeaders {
        const token = this.oidcService.getToken();
        return ((headers as HttpHeaders) || new HttpHeaders())
            // set is immutable
            .set('Authorization', `Bearer ${token}`)
            // Prevent IE11 caching GET call in Angular 2
            // ref: https://stackoverflow.com/a/44561162
            .set('Cache-Control', 'no-cache')
            .set('Pragma', 'no-cache')
            .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT');
    }

    private appendParams(httpParams: HttpParams, parameters?: any, prefix?: string): HttpParams {

        if (!parameters) { return httpParams; }

        Object.keys(parameters).forEach(key => {

            const value = parameters[key];
            const paramName = `${prefix || ''}${key}`;

            if (!value) { return; }

            if (this.isSimpleType(value)) {

                httpParams = httpParams.append(paramName, this.formatParamValue(value));
                return;
            }
            if (value instanceof Array) {
                Object.keys(value).forEach(arrayKey => {
                    httpParams = httpParams.append(paramName, this.formatParamValue(value[arrayKey]));
                });

                return;
            }

            httpParams = this.appendParams(httpParams, value, `${paramName}.`);
        });

        return httpParams;
    }

    private isSimpleType(value: any): boolean {

        if ((-1 !== ['number', 'string', 'boolean', 'number'].indexOf(typeof value)) ||
            (value instanceof Date)) {
            return true;
        }

        return false;
    }

    private formatParamValue(value: any) {

        if (value instanceof Date) {
            return value.toISOString();
        }

        // todo: format date, etc... for querystring
        return `${value}`;
    }


    private handleError(url: string, verb: string, err: any): Error {

        this.log.debug(`ApiService: error ${err.status} executing ${verb} ${url}`, err);
        const message = this.getErrorMessage(err);
        const error = err.error || {};

        switch (err.status) {

            // An HTTP response code of 0 indicates that the AJAX request was cancelled.
            // This can happen either from a timeout, XHR abortion or a firewall stomping on the request.
            // A timeout is common, it means the request failed to execute within a specified time.
            case 0: {
                this.log.debug(`ApiService: error converted to ApiCallFailError`);
                return new ApiCallFailError(message);
            }

            case HttpStatusCode.BAD_REQUEST: {
                this.log.debug(`ApiService: error converted to BadRequestError`);
                return new BadRequestError(error.key, err.data || err.error, message);
            }

            case HttpStatusCode.UNAUTHORIZED: {
                this.log.debug(`ApiService: error converted to UnauthorizedError`);
                return new UnauthorizedError(message);
            }

            case HttpStatusCode.FORBIDDEN: {
                this.log.debug(`ApiService: error converted to ForbiddenError`);
                return new ForbiddenError(message);
            }

            case HttpStatusCode.NOT_FOUND: {
                this.log.debug(`ApiService: error converted to NotFoundError`);
                return new NotFoundError(message);
            }

            case HttpStatusCode.INTERNAL_SERVER_ERROR: {
                this.log.debug(`ApiService: error converted to InternalServerError`);
                return new InternalServerError(message);
            }

            default: {
                this.log.debug(`ApiService: unknown error not converted to known error type`);
                return err;
            }
        }
    }

    private getErrorMessage(err: any): string {

        const error = err.error;

        if (!error) {
            return err.message;
        }

        if (typeof (error) === 'string') {
            return error;
        }

        return error.message || err.message;
    }


    private getHttpResponseFileName<T>(response: HttpResponse<T>, defaultFileName = 'file') {
        let fileName = defaultFileName;
        const contentDisposition = response.headers.get('Content-Disposition');
        if (contentDisposition) {
            const fileNameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
            const matches = fileNameRegex.exec(contentDisposition);
            if (matches != null && matches[1]) {
                fileName = matches[1].replace(/['"]/g, '');
            }
        }
        return fileName;
    }

}

