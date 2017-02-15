import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class TranslationsService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/translations'); }
    
    getForDictionary(dictionaryId: number, callback: Function, scope: any) {
        let me = this;
        me.service.get(me.api() + '/GetForDictionary/' + dictionaryId, callback, scope);
    }
}