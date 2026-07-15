import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, map, tap } from 'rxjs';
import { AuthResponse } from '../models/models';
import { environment } from '../../../environments/environment';

const STORAGE_KEY = 'complaint_system_auth';

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: 'Admin' | 'Employee';
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private authState = signal<AuthResponse | null>(this.loadFromStorage());

  currentUser = computed(() => this.authState());
  isLoggedIn = computed(() => !!this.authState());
  isAdmin = computed(() => this.authState()?.role === 'Admin');

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<{ data: AuthResponse }>(`${environment.apiUrl}/auth/login`, { email, password })
      .pipe(
        map(res => res.data),
        tap(auth => this.setSession(auth))
      );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<{ data: AuthResponse }>(`${environment.apiUrl}/auth/register`, request)
      .pipe(map(res => res.data));
  }

  private setSession(auth: AuthResponse) {
    this.authState.set(auth);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(auth));
  }

  logout() {
    this.authState.set(null);
    localStorage.removeItem(STORAGE_KEY);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return this.authState()?.token ?? null;
  }

  private loadFromStorage(): AuthResponse | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return null;
    try {
      const parsed: AuthResponse = JSON.parse(raw);
      if (new Date(parsed.expiresAt) < new Date()) { localStorage.removeItem(STORAGE_KEY); return null; }
      return parsed;
    } catch { return null; }
  }
}
