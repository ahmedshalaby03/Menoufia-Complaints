import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DashboardSummary, ReportsSummary } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private http: HttpClient) {}

  getSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${environment.apiUrl}/dashboard`);
  }

  getReports(): Observable<ReportsSummary> {
    return this.http.get<ReportsSummary>(`${environment.apiUrl}/reports`);
  }
}
