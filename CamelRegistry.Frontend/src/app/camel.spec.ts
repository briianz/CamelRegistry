import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

//Camel adatmodell, amely a backend API-tól érkező adatokat reprezentálja.
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

  // Ez a függvény meghívja a backend API-t, hogy megkapja a tevéket.
  getCamels(): Observable<Camel[]> {
    return this.http.get<Camel[]>(this.apiUrl);
  }
}