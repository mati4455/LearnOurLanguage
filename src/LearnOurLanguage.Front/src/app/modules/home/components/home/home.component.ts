import { Component, OnInit } from '@angular/core';
import { AuthLoginComponent } from 'shared/components/auth/auth-login.component';
import { AuthService } from 'lol/services/auth.service';

declare let $: any;

@Component({
    selector: 'home-page',
    templateUrl: 'home.component.html',
    styleUrls: ['./home.scss'],
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

    togglePreview(event: any) {
        let me = this;
        let tmp = $(event.target).parents('.image');
        if (tmp.hasClass('active')) {
            tmp.removeClass('active');
            return;
        }

        if (tmp) {
            $('#home .image').removeClass('active');
            tmp.addClass('active');
        }
    }

}
