import {Component, OnInit} from '@angular/core';

import {SystemMenuService} from './system-menu.service';
import {SystemMenu} from "./system-menu";

@Component({
    selector: 'sid-system-menu',
    templateUrl: './system-menu.component.html',
    styleUrls: ['./system-menu.component.scss'],
    providers: [SystemMenuService]
})
export class SystemMenuComponent implements OnInit {
    systemMenu: SystemMenu[];

    constructor(private systemMenuService: SystemMenuService) {
    }

    ngOnInit() {
        this.getRootSystemMenu();
    }


    getRootSystemMenu() {
        const vm = this;
        vm.systemMenuService.getRootMenu().subscribe(
            data => {
                console.log(data);
                this.systemMenu = data.results
            });
    }
}
