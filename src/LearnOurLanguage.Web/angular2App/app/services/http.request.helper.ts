import {Http, Headers, RequestOptions, URLSearchParams, BaseRequestOptions} from '@angular/http';

/**
* Klasa pomocnicza dla żądań HTTP.
*/
export class HttpRequestHelper {

    static getErrorMessage(status: number): string {
        switch (status) {
            case 403:
                return 'Brak uprawnień do wykonania operacji';
            case 404:
            case 500:
                return 'Wystąpił błąd podczas komunikacji z serwerem';
            default:
                return 'Błąd';
        }
    }
}

export class ODataConfig extends RequestOptions { // BaseRequestOptions
    private options: RequestOptions;
    private params: URLSearchParams;

    // parametry wyszukiwania
    public top: number;
    public skip: number;
    public filterEquals: Array<any>;
    public filterContains: Array<any>;
    public filterDate: Array<any>;

    public orderByParam: string;
    public orderByType: OrderType;
    public count: boolean;

    constructor() {
        super();
        this.filterEquals = new Array();
        this.filterContains = new Array();
        this.filterDate = new Array();

        this.top = 0; // do usunięcia
        this.skip = 0;
    }

    public clearFilters() {
        this.filterEquals = new Array();
        this.filterContains = new Array();
        this.filterDate = new Array();
    }

    public get requestOptions() {
        this.options = new RequestOptions();
        this.params = new URLSearchParams();

        // filtrowanie
        let filterString = '';
        let filters = new Array<string>();

        //filters.push(this.filterEqualsString);
        //filters.push(this.filterContainsString);
        //filters.push(this.filterDateString);

        filterString = filters.filter(item => item !== '').join(' and ');

        if (filterString !== '') {
            this.params.append('$filter', filterString);
        }

        // top
        if (this.top !== undefined && this.top !== null && this.top > 0) {
            if (!this.params.has('$top')) {
                this.params.append('$top', this.top.toString());
            } else {
                this.params.set('$top', this.top.toString());
            }
        }

        // skip
        if (this.skip !== undefined && this.skip !== null) {
            if (!this.params.has('$skip')) {
                this.params.append('$skip', this.skip.toString());
            } else {
                this.params.set('$skip', this.skip.toString());
            }
        }

        // dzięki temu mamy ilość wszystkich rekordów (przed filtrowaniem, topem, skipem itd.!)
        if (this.count) {
            this.params.append('$count', 'true');
        }

        // sortowanie
        if (this.orderByParam) {
            this.params.append('$orderby',
                (this.orderByType !== undefined ? `${this.orderByParam} ${OrderType[this.orderByType]}` : `${this.orderByParam}`));
        }

        this.options.search = this.params;
        return this.options;
    }
}

export enum OrderType {
    desc,
    asc
}
