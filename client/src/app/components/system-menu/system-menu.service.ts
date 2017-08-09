import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import 'rxjs/add/operator/toPromise';

import {SystemMenu} from "./system-menu";

interface ItemsResponse {
    results: SystemMenu[];
}

@Injectable()
export class SystemMenuService {

    private systemMenuUrl = 'api/system-menu';

    constructor(private http: HttpClient) {
    }

    getRootMenu() {
        const vm = this;
        return vm.http.get<ItemsResponse>(vm.systemMenuUrl)
    }
}
