import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class AuthService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/auth'); }

    refreshSession() {
        let me = this;
        me.service.get(me.api() + '/GetAccess', null, me.updateLocalStorage, me);
    }

    private updateLocalStorage(data: any): void {
        let me = this;
        localStorage.setItem('accessLevel', data.accessLevel);
        localStorage.setItem('userId', data.userId);
        localStorage.setItem('loggedIn', (data.accessLevel > 0) ? '1' : '0');
    }

}