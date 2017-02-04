import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';

@Injectable()
export class UserService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/users')}
    
}