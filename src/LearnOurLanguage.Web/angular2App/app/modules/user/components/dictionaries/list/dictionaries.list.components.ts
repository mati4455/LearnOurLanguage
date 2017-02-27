import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService } from 'lol/services';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import './dictionaries.list.scss';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
let store = require('store2');
let $ = require('jquery');

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
    public flagAdd: boolean = false;
    public flagEdit: boolean = true;
    public dictionaryId: number = 0;

    public publicTabActive: boolean = false;

    constructor(
        private dictionariesService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute,
        private toast: ToastsManager) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.dictionariesService.getAllPublic(me.loadPublicDictionaries, me);
        me.loadOwnDictionaries();
    }

    loadOwnDictionaries() {
        let me = this;
        let userId = +store('userId');
        if (userId > 0) {
            me.dictionariesService.getForUser(userId, me.loadDictionaries, me);
        }
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
        me.dictionariesBase = data;

        me.setTabAndScroll();
    }

    loadPublicDictionaries(data: any) {
        let me = this;
        me.dictionariesPublic = data;
        me.dictionariesPublicBase = data;
    }

    deleteDictionary(id: any) {
        let me = this;
        console.log(id[0]);
        me.dictionariesService.delete(id[0], me.deleteSuccessfully, me);
    }

    deleteSuccessfully(data: any) {
        let me = this;
        if (data) {
            me.toast.success('Słownik został usunięty poprawnie');
            me.router.navigate(['dictionaries']);
            me.loadOwnDictionaries();
        } else {
            me.toast.warning('Wystąpił problem podczas usuwania słownika');
        }
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

    setAddFlag() {
        let me = this;
        me.flagAdd = !me.flagAdd;
    }

    setEditFlag() {
        let me = this;
        me.flagEdit = !me.flagEdit;
    }

    setDictionaryId(id: any) {
        console.log(id);
        let me = this;
        me.dictionaryId = id;
        me.flagAdd = false;
        me.flagEdit = false;
    }

    copyDictionary() {
        let me = this;
        let userId = +store('userId');
        me.dictionariesService.copyDictionary(userId, me.dictionaryId, me.finalizeCopy, me);
    }

    finalizeCopy(data: any) {
        let me = this;

        if (data > 0) {
            me.toast.success('Słownik został skopiowany poprawnie');
            me.router.navigate(['dictionaries', data]);
            me.loadOwnDictionaries();
        } else {
            me.toast.warning('Wystąpił problem podczas kopiowania słownika');
        }
    }

    setTabAndScroll() {
        let me = this;

        // setTimeout(() => {
        //     var container = $('#ownDictionaries .list, #publicDictionaries .list'),
        //         scrollTo = $('.dictionariesTab .list a.active');
        //     if (!container || !scrollTo) return;

        //     container.scrollTop(
        //         scrollTo.offset().top - container.offset().top + container.scrollTop()
        //     );
        // }, 200);

    }
}
