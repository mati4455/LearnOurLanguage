import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'game-chooser',
    templateUrl: 'chooser.component.html',
    styleUrls: ['./chooser.scss'],
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
