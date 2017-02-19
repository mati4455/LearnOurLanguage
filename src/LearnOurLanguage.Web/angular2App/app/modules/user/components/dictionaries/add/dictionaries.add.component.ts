import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService } from 'lol/services';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'dictionaries-add',
    templateUrl: 'dictionaries.add.component.html',
    providers: [
        DictionariesService
    ]
})

export class DictionariesAddComponent {

parameters: DictionaryModel = new DictionaryModel();
dictionaries: Array<DictionaryModel> = [];
userId: number;


    constructor(private dictionariesService: DictionariesService) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
       // me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
    }

    createDictionary() {
        let me = this;
        me.parameters.userId = me.userId;
        me.dictionariesService.post(me.parameters,me.initializeDictionary,me);
    }

       initializeDictionary(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

}
