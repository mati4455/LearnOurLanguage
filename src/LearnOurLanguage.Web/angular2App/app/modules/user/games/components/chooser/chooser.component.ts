import { Component, OnInit } from '@angular/core';
import './chooser.scss';

@Component({
    selector: 'game-chooser',
    templateUrl: 'chooser.component.html',
    providers: [

    ]
})

export class ChooserComponent {

    constructor() {
        let me = this;
    }

    getGameUrl(gameName: string): string {
        return '/user/games/' + gameName;
    }
}
