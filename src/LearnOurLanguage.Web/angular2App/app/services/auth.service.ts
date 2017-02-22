import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';

let store = require('store2');

@Injectable()
export class AuthService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/auth'); }

    refreshSession() {
        let me = this;
        me.service.get(me.api() + '/GetAccess', null, me.updateLocalStorage, me);
    }

    private updateLocalStorage(data: any): void {
        let me = this;
        store('accessLevel', data.accessLevel);
        store('userId', data.userId);
        store('loggedIn', (data.accessLevel > 0) ? '1' : '0');
    }

}