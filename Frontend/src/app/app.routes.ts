import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },

  {
    path: 'employee',
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', loadComponent: () => import('./features/employee/dashboard/dashboard.component').then(m => m.EmployeeDashboardComponent) },
      { path: 'complaints', loadComponent: () => import('./features/employee/complaints-list/complaints-list.component').then(m => m.ComplaintsListComponent) },
      { path: 'complaints/new', loadComponent: () => import('./features/employee/new-complaint/new-complaint.component').then(m => m.NewComplaintComponent) },
      { path: 'complaints/:id', loadComponent: () => import('./features/complaint-details/complaint-details.component').then(m => m.ComplaintDetailsComponent) },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    children: [
      { path: 'dashboard', loadComponent: () => import('./features/admin/dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent) },
      { path: 'complaints', loadComponent: () => import('./features/employee/complaints-list/complaints-list.component').then(m => m.ComplaintsListComponent) },
      { path: 'complaints/:id', loadComponent: () => import('./features/complaint-details/complaint-details.component').then(m => m.ComplaintDetailsComponent) },
      { path: 'inbox', loadComponent: () => import('./features/admin/inbox/inbox.component').then(m => m.InboxComponent) },
      { path: 'reports', loadComponent: () => import('./features/admin/reports/reports.component').then(m => m.ReportsComponent) },
      { path: 'register-user', loadComponent: () => import('./features/admin/register-user/register-user.component').then(m => m.RegisterUserComponent) },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
