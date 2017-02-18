import {Injectable} from '@angular/core';
import {ToastsManager} from 'ng2-toastr/ng2-toastr';
import {SlimLoadingBarService} from 'ng2-slim-loading-bar';
import {Http, Headers, RequestOptions, URLSearchParams, BaseRequestOptions} from '@angular/http';
import {HttpRequestHelper, ODataConfig} from './http.request.helper';
import 'rxjs/Rx';
let $ = require('jquery');
let interval: any;

/**
* Bazowy serwis Http odpowiedzialny za wykonywanie żądań GET, POST, PUT oraz DELETE.
*/
@Injectable()
export class BaseHttpService {
    constructor(private _http: Http, private _toast: ToastsManager, private _loader: SlimLoadingBarService) { }

    public get(url: string, paramObject: any, callback: Function, scope: any, odataConfig: ODataConfig = null): void {
        let me = this;
        let headers = me.getHeaderSettings();

        me.showLoader();
        if (odataConfig === null) {
            odataConfig = new ODataConfig();
        }
        url = me.getUrl(url, paramObject);

        odataConfig.headers = headers;
        me._http.get(url, odataConfig).map(responseData => {
            return responseData.json();
        }).subscribe(
            data => me.processResponse(data, callback, scope),
            error => me.processError(error)
        );
    }

    public post(url: string, object: any, callback: Function, scope: any): void {
        let me = this;
        let headers = me.getHeaderSettings();

        me.showLoader();
        me._http.post(url, JSON.stringify(object), {headers: headers}).map(responseData => {
            return responseData.json();
        }).subscribe(
            data => me.processResponse(data, callback, scope),
            error => me.processError(error)
        );
    }

    public put(url: string, object: any, callback: Function, scope: any): void {
        let me = this;
        let headers = me.getHeaderSettings();

        me.showLoader();
        me._http.put(url, JSON.stringify(object), {headers: headers}).map(responseData => {
            return responseData.json();
        }).subscribe(
            data => me.processResponse(data, callback, scope),
            error => me.processError(error)
        );
    }

    public delete(url: string, callback: Function, scope: any): void {
        let me = this;
        let headers = me.getHeaderSettings();

        me.showLoader();
        me._http.delete(url, {headers: headers}).map(responseData => {
            return responseData.json();
        }).subscribe(
            data => me.processResponse(data, callback, scope),
            error => me.processError(error)
        );
    }

    private processResponse(data: any, callback: Function, scope: any): void {
        let me = this;
        me.hideLoader();

        if ((data !== undefined || data !== null) && data.success === true) {
            if (callback.call) {
                callback.call(scope, data.data, data.count);
            }
        } else {
            me.processError(data.data);
        }
        // todo: obsługa błędów - success w json result
    }

    private processError(error: any): void {
        let me = this;
        me.hideLoader();
        let message: string = HttpRequestHelper.getErrorMessage(error.status);
        me._toast.error(error);
        console.error(error);
    }

    private getHeaderSettings(): Headers {
        let headers = new Headers();

        headers.append('Content-Type', 'application/json');
        headers.append('Accept', 'application/json');
        headers.append('X-Requested-With', 'XMLHttpRequest');

        return headers;
    }

    private showLoader() {
        let me = this;
        //me._loader.start();
        $('#loader').css('display', 'flex');
    }

    private hideLoader() {
        let me = this;
        if (interval) {
            clearInterval(interval);
        }
        interval = setTimeout(function() {
            $('#loader').fadeOut(100);
        }, 100);
        //me._loader.complete();
    }

    private getUrl(url: string, params: any) {
        let me = this;
        let str = '';
        for (let key in params) {
            if (str != '') {
                str += '&';
            }
            str += key + '=' + encodeURIComponent(params[key]);
        }
        return str.length > 0 ? url + '?' + str : url;
    }
}
