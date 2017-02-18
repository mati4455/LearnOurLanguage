import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import './user.layout.scss';

@Component({
    selector: 'user-layout',
    templateUrl: 'layoutv2.component.html',
    providers: [

    ]
})

export class LayoutComponent {

    constructor(private router: Router) {
        let me = this;
        let access = localStorage.getItem('accessLevel') == '100' && +localStorage.getItem('userId') > 0;
        if (!access) {
            me.router.navigate(['/auth/login']);
        }
    }

}
