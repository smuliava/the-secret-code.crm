import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Menu} from "./menu";

@Injectable()
export class MenuService {

    private menuUrl = 'api/menu';
    private headers = new HttpHeaders({'Content-Type': 'application/json'});

    constructor(private http: HttpClient) {
    }

    getMenu() {
        const vm = this;
        return vm.http.get<Menu[]>(vm.menuUrl);
    }

    create(menuTitle: string) {
        const vm = this;
        return vm.http.post<Menu[]>(vm.menuUrl, JSON.stringify({title: menuTitle}), {headers: new HttpHeaders({'Content-Type': 'application/json'})})
    }
}
