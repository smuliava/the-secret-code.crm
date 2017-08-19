import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable()
export class MenuService {

    private menuUrl = 'api/menu';
    private headers = new HttpHeaders({'Content-Type': 'application/json'});

    public constructor(private http: HttpClient) {
    }

    public getMenu() {
        return this.http.get<IMenu[]>(this.menuUrl);
    }

    public create(menuTitle: string) {
        return this.http.post<IMenu>(this.menuUrl, JSON.stringify({title: menuTitle}), {headers: this.headers});
    }
}
