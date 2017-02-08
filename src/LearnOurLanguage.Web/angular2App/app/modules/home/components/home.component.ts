import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services';
import { AuthLoginComponent } from '../../shared/components/auth/auth-login.component';

@Component({
    selector: 'home-page',
    templateUrl: 'home.component.html',
    providers: [AuthService, AuthLoginComponent]
})

export class HomeComponent {
    public isLogged: boolean;
    public accessLevel: number;

    constructor(private auth: AuthLoginComponent) {
        let me = this;
        me.accessLevel = me.auth.getAccessLevel();
        me.isLogged = me.auth.getLoggedIn();
    }

}
