import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SharedConstantsService } from '../services/shared.constants.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }

  private getHttpRequestOptions(token = null) {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        // 'Authorization': 'Bearer ' + token,
      })
    };
  }

  public get(url): Observable<any> {

    const httpOptions = this.getHttpRequestOptions();
    return this.http.get(url, httpOptions);
  }

  public post(url, body = null): Observable<any> {

    const httpOptions = this.getHttpRequestOptions();

    return this.http.post(url, body, httpOptions);
  }

  public put(url, body): Observable<any> {

    const httpOptions = this.getHttpRequestOptions();

    return this.http.put(url, body, httpOptions);
  }

  public delete(url): Observable<any> {

    const httpOptions = this.getHttpRequestOptions();

    return this.http.delete(url, httpOptions);
  }
}
