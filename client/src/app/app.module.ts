import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MdButtonModule, MdCheckboxModule, MdSidenavModule} from '@angular/material';
import {SystemMenuComponent} from './components/system-menu/system-menu.component';
import {RouterModule} from "@angular/router";
import {routes} from "./app.routes";

@NgModule({
    declarations: [
        AppComponent,
        SystemMenuComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MdButtonModule,
        MdCheckboxModule,
        MdSidenavModule,
        RouterModule.forRoot(routes, {enableTracing: true})
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
