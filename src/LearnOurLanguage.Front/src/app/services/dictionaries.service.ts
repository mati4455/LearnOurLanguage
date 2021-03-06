import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class DictionariesService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/dictionaries'); }

    getAllPublic(callback: Function, scope: any) {
        let me = this;
        me.service.get(me.api() + '/GetAllPublic', null, callback, scope);
    }

    getForUser(userId: number, callback: Function, scope: any) {
        let me = this;
        let params = {
            userId: userId
        };
        me.service.get(me.api() + '/GetForUser', params, callback, scope);
    }

    copyDictionary(userId: number, dictionaryId: number, callback: Function, scope: any) {
        let me = this;
        let params = {
            userId: userId,
            dictionaryId: dictionaryId
        };
        me.service.get(me.api() + '/CopyDictionary', params, callback, scope);
    }

    updateDictionary(dictionaryId: number, callback: Function, scope: any) {
        let me = this;
        let params = {
            dictionaryId: dictionaryId
        };
        me.service.get(me.api() + '/UpdateDictionary', params, callback, scope);
    }

    importDictionary(dictionaryId: number, formData: any, callback: Function, scope: any) {
        let me = this;
        me.service.postFile('/api/DataExchange/Import/' + dictionaryId, formData, callback, scope);
    }

}