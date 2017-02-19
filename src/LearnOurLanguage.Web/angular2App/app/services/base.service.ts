export abstract class BaseService {

    constructor(public service: any, private apiUrl: string) { }

    protected api(method: string = ''): string {
        let me = this;
        return method.length > 0 ? me.apiUrl + '/' + method : me.apiUrl;
    }

    getAll(callback: Function, scope: any): any {
        let me = this;
        me.service.get(me.api(), null, callback, scope);
    }

    get(id: number, callback: Function, scope: any): any {
        let me = this;
        if (id > 0) {
            me.service.get(me.api() + '/' + id, null, callback, scope);
        }
    }

    post(model: any, callback: Function, scope: any): any {
        let me = this;
        me.service.post(me.api(), model, callback, scope);
    }

    put(model: any, callback: Function, scope: any): any {
        let me = this;
        me.service.put(me.api(), model, callback, scope);
    }

    delete(id: number, callback: Function, scope: any): any {
        let me = this;
        me.service.delete(me.api() + '/' + id, callback, scope);
    }

}