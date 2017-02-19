import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService, LanguageService } from 'lol/services';
import { LanguageModel } from 'lol/models/dictionary';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'dictionaries-add',
    templateUrl: 'dictionaries.add.component.html',
    providers: [
        DictionariesService, LanguageService
    ]
})

export class DictionariesAddComponent {

parameters: DictionaryModel = new DictionaryModel();
dictionaries: Array<DictionaryModel> = [];
languages: Array<LanguageModel> = [];
userId: number;


    constructor(private dictionariesService: DictionariesService, private languageService:LanguageService) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
        me.languageService.getAll(me.loadLanguages,me);
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
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

    loadLanguages(data: any){
        let me = this;
        me.languages = data;
    }

}
