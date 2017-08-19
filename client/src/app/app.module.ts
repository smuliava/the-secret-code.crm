import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MdButtonModule, MdCheckboxModule, MdSidenavModule, MdListModule} from '@angular/material';
import {MenuComponent} from './components/menu/menu.component';
import {RouterModule} from "@angular/router";
import {routes} from "./app.routes";
import {HttpClientModule} from "@angular/common/http";

@NgModule({
    declarations: [
        AppComponent,
        MenuComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MdButtonModule,
        HttpClientModule,
        MdCheckboxModule,
        MdSidenavModule,
        MdListModule,
        RouterModule.forRoot(routes, {enableTracing: true})
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
