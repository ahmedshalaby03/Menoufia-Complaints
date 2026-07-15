import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { StatCardComponent } from '../../../shared/components/stat-card/stat-card.component';
import { DashboardService } from '../../../core/services/dashboard.service';
import { ReportsSummary } from '../../../core/models/models';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, SidebarComponent, StatCardComponent],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
  reports = signal<ReportsSummary | null>(null);

  constructor(private dashboardService: DashboardService) {}

  ngOnInit() { this.dashboardService.getReports().subscribe(r => this.reports.set(r)); }
}
