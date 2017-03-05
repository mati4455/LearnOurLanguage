import './langswitch.scss';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService } from 'lol/services';
let store = require('store2');
let $ = require('jquery');

@Component({
    selector: 'lang-switch',
    templateUrl: 'langswitch.component.html',
    providers: [
        DictionariesService
    ]
})

export class LangSwitchComponent implements OnInit {

    // private variables

    private dictionaryId: number;
    private reverseLanguages: boolean;
    private reverseFlag: boolean;

    // end of variables

    // variables for html

    dictionaries: Array<DictionaryModel> = [];
    selectedDictionary: DictionaryModel;
    userId: number = 0;
    firstLang: string = '';
    secondLang: string = '';

    // end of variables


    // Input, Output for dictionary

    @Input()
    get dictionary():number {
        let me = this;
        return me.dictionaryId;
    }

    @Output() dictionaryChange = new EventEmitter();
    @Output() selDictionaryChange = new EventEmitter();

    set dictionary(value: number) {
        let me = this;
        if (value && value > 0) {
            me.dictionaryId = value;
            me.dictionaryChange.emit(me.dictionaryId);
            me.selectedDictionary = $.extend(true, {}, me.dictionaries.find((item) => item.id == me.dictionaryId));
            me.reverseFlag = false;
            me.setLanguages(false);
            me.selDictionaryChange.emit(me.selectedDictionary);
            me.reverseFlag = true;
        }
    }

    // end of dictionary

    // Input, Output for reverse languages

    @Input()
    get reverseLangs() {
        let me = this;
        return me.reverseLanguages;
    }

    @Output() reverseLangsChange = new EventEmitter();

    set reverseLangs(value) {
        let me = this;

        if (!me.selectedDictionary) return;

        if (me.reverseLanguages != value && me.reverseFlag) {
            let temp = me.selectedDictionary.firstLanguage;
            me.selectedDictionary.firstLanguage = me.selectedDictionary.secondLanguage;
            me.selectedDictionary.secondLanguage = temp;
        }

        me.reverseLanguages = value;
        me.reverseLangsChange.emit(me.reverseLanguages);

        me.firstLang = me.selectedDictionary.firstLanguage.name;
        me.secondLang = me.selectedDictionary.secondLanguage.name;
    }

    // end of reverse

    constructor(private dictionariesService: DictionariesService) { }

    ngOnInit() {
        let me = this;
        me.userId = +store('userId');
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    setNewDictionaryValue(event: any) {
        let me = this;
        me.dictionary = event.target.value;
    }

    setLanguages(reverse: boolean) {
        let me = this;
        me.reverseLangs = reverse;
    }

    switchLanguages() {
        let me = this;
        me.reverseLangs = !me.reverseLangs;
    }
}