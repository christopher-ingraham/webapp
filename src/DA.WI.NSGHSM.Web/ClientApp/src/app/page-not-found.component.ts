import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
    selector: 'app-page-not-found',
    template: `
<div>
  <h2>{{ appPageNotFoundTitle | translate }}</h2>
  <div *ngIf="page">
    <p>Page <code>{{page}}</code> was not found on this server.</p>
  </div>
  <hr />
  <p>
  <button kendoButton class="k-button k-primary" routerLink="/">{{ appPageNotFoundBackToHome | translate }}</button>
  </p>
</div>`
})
export class PageNotFoundComponent implements OnInit {
    public readonly appPageNotFoundTitle = 'APP.PAGE_NOT_FOUND.TITLE|Page not found';
    public readonly appPageNotFoundBackToHome = 'APP.PAGE_NOT_FOUND.BACK_TO_HOME|Back to Home';

    public page?: string;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.page = params['page'];
        });
    }

}
