export abstract class BaseService {
    
    constructor(public service: any, private apiUrl: string) { }

    protected api(id: any = ''):string {
        let me = this;
        return +id > 0 ? me.apiUrl + '/' + id : me.apiUrl;
    }

    getAll(callback: Function, scope: any):any {
        let me = this;
        me.service.get(me.api(), callback, scope);
    }

    get(id: number, callback: Function, scope: any):any {
        let me = this;
        me.service.get(me.api(id), callback, scope);
    }

    post(model: any, callback: Function, scope: any): any {
        let me = this;
        me.service.post(me.api(), model, callback, scope);
    }

    put(model: any, callback: Function, scope: any): any {
        let me = this;
        me.service.put(me.api(model.id), model, callback, scope);
    }

    delete(id: number, callback: Function, scope: any): any {
        let me = this;
        me.service.delete(me.api(id), callback, scope);
    }

}