import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ComplaintDetails, ComplaintListItem, DuplicateCheckResult, PagedResult } from '../models/models';
import { ComplaintStatus, PriorityLevel } from '../models/enums';

export interface ComplaintFilter {
  status?: ComplaintStatus | null;
  priority?: PriorityLevel | null;
  searchTerm?: string;
  pageNumber?: number;
  pageSize?: number;
}

@Injectable({ providedIn: 'root' })
export class ComplaintService {
  private base = `${environment.apiUrl}/complaints`;

  constructor(private http: HttpClient) {}

  checkDuplicates(description: string): Observable<DuplicateCheckResult> {
    return this.http.post<{ data: DuplicateCheckResult }>(`${this.base}/check-duplicates`, { description })
      .pipe(map(r => r.data));
  }

  create(formData: FormData): Observable<ComplaintDetails> {
    return this.http.post<{ data: ComplaintDetails }>(this.base, formData).pipe(map(r => r.data));
  }

  getById(id: number): Observable<ComplaintDetails> {
    return this.http.get<{ data: ComplaintDetails }>(`${this.base}/${id}`).pipe(map(r => r.data));
  }

  getList(filter: ComplaintFilter): Observable<PagedResult<ComplaintListItem>> {
    return this.http.get<PagedResult<ComplaintListItem>>(this.base, { params: this.toParams(filter) });
  }

  getInbox(filter: ComplaintFilter): Observable<PagedResult<ComplaintListItem>> {
    return this.http.get<PagedResult<ComplaintListItem>>(`${environment.apiUrl}/inbox`, { params: this.toParams(filter) });
  }

  updateStatus(id: number, newStatus: ComplaintStatus, notes?: string): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}/status`, { newStatus, notes });
  }

  assign(id: number, governmentEntityId?: number, assignedToUserId?: string): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}/assign`, { governmentEntityId, assignedToUserId });
  }

  private toParams(filter: ComplaintFilter): Record<string, string> {
    const params: Record<string, string> = {};
    if (filter.status) params['status'] = String(filter.status);
    if (filter.priority) params['priority'] = String(filter.priority);
    if (filter.searchTerm) params['searchTerm'] = filter.searchTerm;
    params['pageNumber'] = String(filter.pageNumber ?? 1);
    params['pageSize'] = String(filter.pageSize ?? 20);
    return params;
  }
}
