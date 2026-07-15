import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { StatCardComponent } from '../../../shared/components/stat-card/stat-card.component';
import { DashboardService } from '../../../core/services/dashboard.service';
import { AuthService } from '../../../core/services/auth.service';
import { DashboardSummary } from '../../../core/models/models';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, SidebarComponent, StatCardComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: '../../employee/dashboard/dashboard.component.scss'
})
export class AdminDashboardComponent implements OnInit {
  summary = signal<DashboardSummary | null>(null);
  constructor(private dashboardService: DashboardService, public auth: AuthService) {}
  ngOnInit() { this.dashboardService.getSummary().subscribe(s => this.summary.set(s)); }

  pct(value: number, s: DashboardSummary): number {
    const max = Math.max(s.priorityDistribution.urgent, s.priorityDistribution.high, s.priorityDistribution.medium, s.priorityDistribution.low, 1);
    return (value / max) * 100;
  }
}
