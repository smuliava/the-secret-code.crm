import {Component, OnInit} from '@angular/core';

import {Menu} from './menu';
import {MenuService} from "./menu.service";

@Component({
    selector: 'sid-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
    private menu: Menu[];

    constructor(private menuService: MenuService) {
    }

    ngOnInit() {
    }

    private gerRootMenu() {
        const vm = this;
        vm.menuService()
    }
}
