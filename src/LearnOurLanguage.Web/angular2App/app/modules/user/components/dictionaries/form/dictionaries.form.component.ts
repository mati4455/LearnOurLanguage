import { DictionaryModel, LanguageModel } from 'lol/models/dictionary';
import { TranslationsService, DictionariesService } from 'lol/services';
import { TranslationModel } from 'lol/models/dictionary';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import './dictionaries.form.scss';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
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
    public ownDictionary: boolean = true;

    constructor(
        private translationsService: TranslationsService,
        private dictionariesService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute,
        private toast: ToastsManager) {
        let me = this;
        me.dictionary.firstLanguage = new LanguageModel();
        me.dictionary.secondLanguage = new LanguageModel();
    }

    ngOnInit() {
        let me = this;
        let userId = +store('userId');
        me.ownDictionary = me.route.snapshot.parent.params['type'] != 'public';
        me.route.params.forEach((params: Params) => {
            let id = +params['dictionaryId'];
            if (userId > 0 && id > 0) {
                me.isCollapsed = true;
                me.dictionariesService.get(id, me.loadDictionary, me);
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

    deleteDictionary() {
        let me = this;
        if (confirm('Czy na pewno chcesz usunąć słownik?')) {
            me.dictionariesService.delete(me.dictionary.id, me.deleteSuccessfully, me);
        }
    }

    deleteSuccessfully(data: any) {
        let me = this;
        if (data) {
            me.toast.success('Słownik został usunięty poprawnie');
            me.router.navigate(['dictionaries', 'own']);
        } else {
            me.toast.warning('Wystąpił problem podczas usuwania słownika');
        }
    }

    copyDictionary() {
        let me = this;
        let userId = +store('userId');
        me.dictionariesService.copyDictionary(userId, me.dictionary.id, me.finalizeCopy, me);
    }

    finalizeCopy(data: any) {
        let me = this;

        if (data > 0) {
            me.toast.success('Słownik został skopiowany poprawnie');
            me.router.navigate(['dictionaries', 'own', data]);
        } else {
            me.toast.warning('Wystąpił problem podczas kopiowania słownika');
        }
    }
}
