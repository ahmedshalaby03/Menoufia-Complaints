import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CenterLookupDto, LookupDto, ServiceLookupDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LookupService {
  private base = `${environment.apiUrl}/lookups`;

  constructor(private http: HttpClient) {}

  getGovernorates(): Observable<LookupDto[]> { return this.http.get<LookupDto[]>(`${this.base}/governorates`); }
  getCenters(governorateId?: number): Observable<CenterLookupDto[]> {
    return this.http.get<CenterLookupDto[]>(`${this.base}/centers`, { params: governorateId ? { governorateId } : {} });
  }
  getSectors(): Observable<LookupDto[]> { return this.http.get<LookupDto[]>(`${this.base}/sectors`); }
  getServices(sectorId?: number): Observable<ServiceLookupDto[]> {
    return this.http.get<ServiceLookupDto[]>(`${this.base}/services`, { params: sectorId ? { sectorId } : {} });
  }
  getGovernmentEntities(): Observable<LookupDto[]> { return this.http.get<LookupDto[]>(`${this.base}/government-entities`); }
}
