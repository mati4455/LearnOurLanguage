import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class DictionariesService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/dictionaries'); }

    getForUser(userId: number, callback: Function, scope: any) {
        let me = this;
        let params = {
            userId: userId
        };
        me.service.get(me.api() + '/GetForUser', params, callback, scope);
    }
}