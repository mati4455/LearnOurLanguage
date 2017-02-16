import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService } from 'lol/services';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import './dictionaries.list.scss';

@Component({
    selector: 'dictionaries-list',
    templateUrl: 'dictionaries.list.component.html',
    providers: [
        DictionariesService        
    ]
})

export class DictionariesListComponent {

    public queryString: string = '';
    public dictionaries: Array<DictionaryModel> = [];
    public dictionariesBase: Array<DictionaryModel> = [];
    
    constructor(
        private dictionariesService: DictionariesService,
        private router: Router, 
        private route: ActivatedRoute) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        let userId = +localStorage.getItem('userId');

        if (userId > 0) {
            me.dictionariesService.getForUser(userId, me.loadDictionaries, me);
        }
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
        me.dictionariesBase = data;
    }

    filterList(event: Event) {
        let me = this;
        //me.queryString = event.target.value;
        
        me.dictionaries = me.dictionariesBase.filter((element) => element.name.includes(me.queryString));
    }

}
