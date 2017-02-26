import { DictionaryModel, LanguageModel } from 'lol/models/dictionary';
import { TranslationsService, DictionariesService } from 'lol/services';
import { TranslationModel } from 'lol/models/dictionary';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import './dictionaries.form.scss';
let $ = require('jquery');
let store = require('store2');

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
    public isCollapsed: boolean = true;

    constructor(
        private translationsService: TranslationsService,
        private dictionaryService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute) {
        let me = this;
        me.dictionary.firstLanguage = new LanguageModel();
        me.dictionary.secondLanguage = new LanguageModel();
    }

    ngOnInit() {
        let me = this;
        let userId = +store('userId');

        me.route.params.forEach((params: Params) => {
            let id = +params['dictionaryId'];
            if (userId > 0 && id > 0) {
                me.isCollapsed = true;
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

    sendFile(event: any) {
        let me = this;
        // todo: do poprawki
        // let files = event.target.files;
        // let formData = new FormData();
        // formData.append("files", formData);

        $('#loader').css('display', 'flex');
        $('#uploadSpace form').submit();

        // me.dictionaryService.importDictionary(me.dictionary.id, formData, me.finishImport, me);
    }

    finishImport(data: any) {
        let me = this;
    }

    exportDictionary(data: any) {
        let me = this;
        window.location.href = '/api/DataExchange/Export/' + me.dictionary.id;
    }
}
