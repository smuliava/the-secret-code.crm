import {Component, OnInit} from "@angular/core";
import {Menu} from "./menu";
import {MenuService} from "./menu.service";

@Component({
    selector: 'sid-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss'],
    providers: [MenuService]
})
export class MenuComponent implements OnInit {
    rootMenu: Menu[];

    constructor(private menuService: MenuService) {
    }

    ngOnInit() {
        this.getRootMenu();
    }

    getRootMenu() {
        const vm = this;
        vm.menuService.getMenu().subscribe(data => {
            vm.rootMenu = data;
        });
    }

    addMenu(menuTitle: string) {
        const vm = this;
        vm.menuService.create(menuTitle).subscribe(data => {
            vm.rootMenu.push(data);
        })
    }
}
