import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Connection } from '../types';

@Injectable({
  providedIn: 'root'
})
export class ConnectionsService {
  baseUrl = environment.baseApiUrl;
  private http = inject(HttpClient);
  allConnections = signal<Connection[]>([]);
  randomConnections = signal<Connection[]>([]);

  public getAllConnections() {
    return this.http.get<Connection[]>(
      `${this.baseUrl}/connections`
    ).subscribe({
      next: connections => this.allConnections.set(connections)
    });
  }

  public getRandomConnections() {
    return this.http.get<Connection[]>(
      `${this.baseUrl}/connections/random`
    ).subscribe({
      next: connections => this.randomConnections.set(connections)
    });
  }
}
