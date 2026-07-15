import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email = '';
  password = '';
  loading = signal(false);
  errorMsg = signal('');

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    if (!this.email || !this.password) { this.errorMsg.set('من فضلك أدخل البريد الإلكتروني وكلمة المرور'); return; }
    this.loading.set(true);
    this.errorMsg.set('');

    this.auth.login(this.email, this.password).subscribe({
      next: (auth) => {
        this.loading.set(false);
        this.router.navigate([auth.role === 'Admin' ? '/admin/dashboard' : '/employee/dashboard']);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMsg.set(err?.error?.message || 'بيانات الدخول غير صحيحة');
      }
    });
  }
}
