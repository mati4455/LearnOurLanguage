import { DictionaryModel } from 'lol/models/dictionary';
import { TranslationsService, DictionariesService } from 'lol/services';
import { TranslationModel } from 'lol/models/dictionary';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import './dictionaries.form.scss';
@Component({
    selector: 'dictionaries-form',
    templateUrl: 'dictionaries.form.component.html',
    providers: [
        DictionariesService,
        TranslationsService
    ]
})

export class DictionariesFormComponent {

    public dictionary: DictionaryModel = new DictionaryModel();
    public translations: Array<TranslationModel> = [];

    constructor(
        private translationsService: TranslationsService,
        private dictionaryService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        let me = this;
        let userId = +localStorage.getItem('userId');

        me.route.params.forEach((params: Params) => {
            let id = +params['dictionaryId'];
            if (userId > 0 && id > 0) {
                me.dictionaryService.get(id, me.loadDictionary, me);
                me.translationsService.getForDictionary(id, me.loadTranslations, me);
            }
        });
    }

    loadTranslations(data: any) {
        let me = this;
        me.translations = data;
    }

    loadDictionary(data: any) {
        let me = this;
        me.dictionary = data;
    }
}
