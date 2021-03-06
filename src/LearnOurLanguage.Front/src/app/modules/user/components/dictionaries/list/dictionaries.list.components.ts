import { DictionaryModel } from 'lol/models/dictionary';
import { DictionariesService } from 'lol/services';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

declare let $: any;
declare let store: any;

@Component({
    selector: 'dictionaries-list',
    templateUrl: 'dictionaries.list.component.html',
    styleUrls: ['./dictionaries.list.scss'],
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
    public dictionaryId: number = 0;
    public currTab: string;

    constructor(
        private dictionariesService: DictionariesService,
        private router: Router,
        private route: ActivatedRoute,
        private toast: ToastsManager) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.loadPublicDictionaries();
        me.loadOwnDictionaries();
        me.currTab = me.route.snapshot.params['type'];
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

    loadPublicDictionaries() {
        let me = this;
        me.dictionariesService.getAllPublic(me.loadedPublicDictionaries, me);
    }

    loadedPublicDictionaries(data: any) {
        let me = this;
        me.dictionariesPublic = data;
        me.dictionariesPublicBase = data;
    }

    selectTab(tab: any) {
        let me = this;
        if (me.route.snapshot.params['type'] != tab) {
            me.router.navigate(['/user/dictionaries/', tab == 'public' ? 'public' : 'own']);

            if (tab == 'public') {
                me.loadPublicDictionaries();
            } else {
                me.loadOwnDictionaries();
            }
        }
    }

    isEditing() {
        let me = this;
        return me.router.url.indexOf('new') !== -1;
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

    setTabAndScroll() {
        let me = this;
        let id = me.currTab == 'own' ? '#ownDictionaries' : '#publicDictionaries';
        setTimeout(() => {
            var container = $(id + ' .list'),
                scrollTo = $(id + ' .list a.active');
            if (!container || !scrollTo || !scrollTo.offset()) return;

            container.scrollTop(
                scrollTo.offset().top - container.offset().top + container.scrollTop() - 15
            );
        }, 200);

    }
}