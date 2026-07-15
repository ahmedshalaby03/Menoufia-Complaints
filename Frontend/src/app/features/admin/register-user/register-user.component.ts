import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { AuthService, RegisterRequest } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register-user',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.scss'
})
export class RegisterUserComponent {
  form: RegisterRequest = {
    fullName: '',
    email: '',
    password: '',
    role: 'Employee'
  };

  submitting = signal(false);
  successMsg = signal('');
  errorMsg = signal('');

  constructor(private authService: AuthService) {}

  submit() {
    this.successMsg.set('');
    this.errorMsg.set('');

    if (!this.form.fullName || !this.form.email || !this.form.password) {
      this.errorMsg.set('من فضلك املأ جميع الحقول المطلوبة');
      return;
    }

    this.submitting.set(true);
    this.authService.register(this.form).subscribe({
      next: () => {
        this.submitting.set(false);
        this.successMsg.set(`تم تسجيل المستخدم "${this.form.fullName}" بنجاح`);
        this.form = { fullName: '', email: '', password: '', role: 'Employee' };
      },
      error: (err) => {
        this.submitting.set(false);
        this.errorMsg.set(err?.error?.message || 'حدث خطأ أثناء تسجيل المستخدم');
      }
    });
  }
}