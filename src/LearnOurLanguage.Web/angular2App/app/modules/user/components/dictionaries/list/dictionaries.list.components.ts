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
    public dictionariesPublic: Array<DictionaryModel> = [];
    public dictionariesPublicBase: Array<DictionaryModel> = [];
    public flagEdit: boolean = false;
    public flagAdd: boolean = true;
    public dictionaryId: number = 0;

    constructor(
        private dictionariesService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        let userId = +localStorage.getItem('userId');

        me.dictionariesService.getAllPublic(me.loadPublicDictionaries, me);

        if (userId > 0) {
            me.dictionariesService.getForUser(userId, me.loadDictionaries, me);
        }

    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
        me.dictionariesBase = data;
    }

    loadPublicDictionaries(data: any) {
        let me = this;
        me.dictionariesPublic = data;
        me.dictionariesPublicBase = data;
    }

    deleteDictionary(id : any) {
        let me = this;
        console.log(id[0]);
        me.dictionariesService.delete(id[0], me.loadDictionaries, me);
    }

    filterList(event: any) {
        let me = this;
        me.queryString = event.target.value;

        me.dictionaries = me.dictionariesBase.filter((element) => element.name.includes(me.queryString));
    }

    filterListPublic(event: any) {
        let me = this;
        me.queryString = event.target.value;

        me.dictionariesPublic = me.dictionariesPublicBase.filter((element) => element.name.includes(me.queryString));
    }
}
