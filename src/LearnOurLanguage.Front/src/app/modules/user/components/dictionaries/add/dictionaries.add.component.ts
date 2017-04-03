import { DictionaryModel, TranslationModel, DictionaryVoModel } from 'lol/models/dictionary';
import { DictionariesService, LanguageService, TranslationsService } from 'lol/services';
import { LanguageModel } from 'lol/models/dictionary';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

declare let store: any;

@Component({
    selector: 'dictionaries-add',
    templateUrl: 'dictionaries.add.component.html',
    styleUrls: ['./dictionaries.add.component.scss'],
    providers: [
        DictionariesService, LanguageService, TranslationsService
    ]
})

export class DictionariesAddComponent {

    dictionaryVo: DictionaryVoModel = new DictionaryVoModel();
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
        private route: ActivatedRoute,
        private toast: ToastsManager) {
        let me = this;
        me.dictionary.firstLanguage = new LanguageModel();
        me.dictionary.secondLanguage = new LanguageModel();
    }

    ngOnInit() {
        let me = this;
        me.userId = +store('userId');

        me.route.params.forEach((params: Params) => {
            let id = +params['dictionaryId'];

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

    deleteTranslation(id: any) {
        let me = this;
        me.translations.splice(id, 1);
    }

    updateDictionary() {
        let me = this;
        me.dictionaryVo.userId = me.userId;
        me.dictionaryVo.id = me.dictionary.id;
        me.dictionaryVo.date = me.dictionary.date;
        me.dictionaryVo.name = me.dictionary.name;
        me.dictionaryVo.isPublic = me.dictionary.isPublic;
        me.dictionaryVo.description = me.dictionary.description;
        me.dictionaryVo.translationList = me.translations;
        me.dictionaryVo.firstLanguageId = me.dictionary.firstLanguage.id;
        me.dictionaryVo.secondLanguageId = me.dictionary.secondLanguage.id;
        me.dictionariesService.post(me.dictionaryVo, me.savedDictionary, me);
    }

    savedDictionary(data: any) {
        let me = this;
        if (data > 0) {
            me.toast.success('Operacja przebiegła pomyślnie.');
            me.router.navigate(['dictionaries']); // przeladowanie listy slownikow
            setTimeout(() => me.router.navigate(['/user', 'dictionaries', 'own', data]), 100);
        } else {
            me.toast.warning('Wystąpił błąd podczas zapisywania słownika.');
        }
    }

    updateFromParentDictionary() {
        let me = this;

        me.dictionariesService.updateDictionary(me.dictionary.id, me.finishUpdateDictionary, me);
    }

    finishUpdateDictionary(data: any) {
        let me = this;
        if (data) {
            me.toast.success("Słownik został zaktuałizownay");
            me.router.navigate(['/user', 'dictionaries', 'own', me.dictionary.id]);
        } else {
            me.toast.warning('Wystąpił błąd podczas aktualizowania słownika.');
        }
    }
}
