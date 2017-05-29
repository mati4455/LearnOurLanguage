import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'game-chooser',
    templateUrl: 'chooser.component.html',
    styleUrls: ['./chooser.scss']
})

export class ChooserComponent {

    constructor() {
        let me = this;
    }

    getGameUrl(gameName: string): string {
        return '/user/games/' + gameName;
    }
}
