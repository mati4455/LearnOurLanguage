import { DictionaryModel, TranslationModel } from 'lol/models/dictionary';
import { DictionariesService, LanguageService, TranslationsService } from 'lol/services';
import { LanguageModel } from 'lol/models/dictionary';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';

let store = require('store2');

@Component({
    selector: 'dictionaries-add',
    templateUrl: 'dictionaries.add.component.html',
    providers: [
        DictionariesService, LanguageService, TranslationsService
    ]
})

export class DictionariesAddComponent {

    dictionary: DictionaryModel = new DictionaryModel();
    dictionaries: Array<DictionaryModel> = [];
    translations: Array<TranslationModel> = [];
    languages: Array<LanguageModel> = [];
    userId: number;


    constructor(
        private dictionariesService: DictionariesService,
        private languageService: LanguageService,
        private translationsService: TranslationsService,
        private router: Router,
        private route: ActivatedRoute) {
        let me = this;
        me.dictionary.firstLanguage = new LanguageModel();
        me.dictionary.secondLanguage = new LanguageModel();
    }

    ngOnInit() {
        let me = this;
        me.userId = +store('userId');

        me.route.params.forEach((params: Params) => {
            let id = +params['dictionaryId'];
            console.log(id);
            if (me.userId > 0 && id > 0) {
                me.dictionariesService.get(id, me.loadDictionary, me);
                me.translationsService.getForDictionary(id, me.loadTranslations, me);
            }
        });

        me.languageService.getAll(me.loadLanguages, me);
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    loadDictionary(data: any) {
        let me = this;
        me.dictionary = data;
    }

    loadLanguages(data: any) {
        let me = this;
        me.languages = data;
    }

    loadTranslations(data: any) {
        let me = this;
        me.translations = data;
    }

    addNewTranslation() {
        let me = this;
        me.translations.push(new TranslationModel());
    }

    updateDictionary() {
        let me = this;
        me.dictionary.userId = me.userId;
        console.log(me.dictionary);
        console.log(me.translations);
    }
}
