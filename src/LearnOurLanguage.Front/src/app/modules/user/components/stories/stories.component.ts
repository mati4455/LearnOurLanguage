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

    constructor() { }

    ngOnInit() {
    }

    selectTab(tab_id: number) {
        this.staticTabs.tabs[tab_id].active = true;
    }

    checkClass(tab_id: number): boolean {
        return this.staticTabs.tabs[tab_id].active;
    }

    disableEnable() {
        this.staticTabs.tabs[2].disabled = !this.staticTabs.tabs[2].disabled;
    }
}
