import { Component, OnInit } from '@angular/core';
import './user.layout.scss';

@Component({
    selector: 'user-layout',
    templateUrl: 'layoutv2.component.html',
    styles: [
        String(require('./user.layout.scss'))
    ],
    providers: [

    ]
})

export class LayoutComponent {

    constructor() {
        let me = this;
    }

}
