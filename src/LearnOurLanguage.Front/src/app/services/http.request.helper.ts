import {Http, Headers, RequestOptions, URLSearchParams, BaseRequestOptions} from '@angular/http';

/**
* Klasa pomocnicza dla żądań HTTP.
*/
export class HttpRequestHelper {

    static getErrorMessage(status: number): string {
        switch (status) {
            case 401:
            case 403:
                return 'Brak wymaganych uprawnień';
            case 404:
            case 500:
                return 'Wystąpił błąd podczas komunikacji z serwerem';
            default:
                return 'Błąd';
        }
    }
}

export class ODataConfig extends RequestOptions { // BaseRequestOptions
    private options: RequestOptions = new RequestOptions();
    public params: URLSearchParams = new URLSearchParams();

    public get requestOptions() {
        let me = this;

        me.options.search = me.params;
        return me.options;
    }
}