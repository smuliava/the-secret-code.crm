import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MdButtonModule, MdCheckboxModule, MdSidenavModule} from "@angular/material";
import {SystemMenuComponent} from './components/system-menu/system-menu.component';

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
        MdSidenavModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
