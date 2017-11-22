import { Component, OnInit, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ng2-bootstrap';

@Component({
    selector: 'app-stories',
    templateUrl: './stories.component.html',
    styleUrls: ['./stories.component.scss']
})
export class StoriesComponent implements OnInit {

    @ViewChild('staticTabs') staticTabs: TabsetComponent;

    active: boolean[];

    answers: boolean[][] = [
        [false, true, false],
        [true, false, false],
        [true, false, false]
    ];

    constructor() { }

    ngOnInit() {
    }

    selectTab(tab_id: number) {
        const buttons = document.querySelectorAll('.answers button');
        [].forEach.call(buttons, function (button) {
            button.classList = ['btn'];
            button.style.pointerEvents = 'auto';
        });

        this.staticTabs.tabs[tab_id].active = true;
    }

    checkClass(tab_id: number): boolean {
        return this.staticTabs.tabs[tab_id].active;
    }

    disableEnable() {
        this.staticTabs.tabs[2].disabled = !this.staticTabs.tabs[2].disabled;
    }

    checkAnswer(input: any, question, answer) {
        const me = this;
        const containerId = '#question' + question;
        const className = me.answers[question][answer] ? 'correct' : 'wrong';
        input.target.classList.add(className);

        const correctIndex = me.answers[question].indexOf(true);
        const correctButton: any = document.querySelectorAll(containerId + ' button')[correctIndex];
        correctButton.classList.add('correct');

        const buttons = document.querySelectorAll(containerId + ' button');

        [].forEach.call(buttons, function(button) {
            button.style.pointerEvents = 'none';
        });

    }
}
