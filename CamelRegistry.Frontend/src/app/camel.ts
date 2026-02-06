import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// A Camel adatmodell, amely a backend API-tól érkező adatokat reprezentálja.
export interface Camel {
  id?: number;
  name: string;
  color: string;
  humpCount: number;
  lastFed: string;
}

@Injectable({ providedIn: 'root' })
export class CamelService {

  private apiUrl = 'https://localhost:7264/api/camels';

  constructor(private http: HttpClient) { }

  //Lekéri a tevéket
  getCamels(): Observable<Camel[]> {
    return this.http.get<Camel[]>(this.apiUrl);
  }
  addCamel(camel: Camel): Observable<Camel> {
    return this.http.post<Camel>(this.apiUrl, camel);
  }
  updateCamel(id: number, camel: Camel) {
    return this.http.put(`${this.apiUrl}/${id}`, camel);
  }
  deleteCamel(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}