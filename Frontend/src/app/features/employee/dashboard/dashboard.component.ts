import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { StatCardComponent } from '../../../shared/components/stat-card/stat-card.component';
import { PriorityBadgeComponent } from '../../../shared/components/badge/priority-badge.component';
import { StatusBadgeComponent } from '../../../shared/components/badge/status-badge.component';
import { DashboardService } from '../../../core/services/dashboard.service';
import { AuthService } from '../../../core/services/auth.service';
import { DashboardSummary } from '../../../core/models/models';

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, SidebarComponent, StatCardComponent, PriorityBadgeComponent, StatusBadgeComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class EmployeeDashboardComponent implements OnInit {
  summary = signal<DashboardSummary | null>(null);

  constructor(private dashboardService: DashboardService, public auth: AuthService) {}

  ngOnInit() {
    this.dashboardService.getSummary().subscribe(s => this.summary.set(s));
  }

  pct(value: number, s: DashboardSummary): number {
    const max = Math.max(s.priorityDistribution.urgent, s.priorityDistribution.high, s.priorityDistribution.medium, s.priorityDistribution.low, 1);
    return (value / max) * 100;
  }
}
