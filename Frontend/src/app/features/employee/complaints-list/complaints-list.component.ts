import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PriorityBadgeComponent } from '../../../shared/components/badge/priority-badge.component';
import { StatusBadgeComponent } from '../../../shared/components/badge/status-badge.component';
import { ComplaintService } from '../../../core/services/complaint.service';
import { AuthService } from '../../../core/services/auth.service';
import { ComplaintListItem, PagedResult } from '../../../core/models/models';
import { ComplaintStatus, ComplaintStatusLabels, PriorityLabels, PriorityLevel } from '../../../core/models/enums';

@Component({
  selector: 'app-complaints-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, SidebarComponent, PriorityBadgeComponent, StatusBadgeComponent],
  templateUrl: './complaints-list.component.html',
  styleUrl: './complaints-list.component.scss'
})
export class ComplaintsListComponent implements OnInit {
  result = signal<PagedResult<ComplaintListItem> | null>(null);
  statusFilter: ComplaintStatus | 'all' = 'all';
  priorityFilter: PriorityLevel | 'all' = 'all';
  searchTerm = '';

  statusOptions = Object.entries(ComplaintStatusLabels).map(([value, label]) => ({ value: Number(value), label }));
  priorityOptions = Object.entries(PriorityLabels).map(([value, label]) => ({ value: Number(value), label }));

  constructor(private complaintService: ComplaintService, public auth: AuthService) {}

  ngOnInit() { this.load(); }

  load() {
    const isAdmin = this.auth.isAdmin();
    const service$ = this.complaintService.getList({
      status: this.statusFilter === 'all' ? null : this.statusFilter,
      priority: this.priorityFilter === 'all' ? null : this.priorityFilter,
      searchTerm: this.searchTerm
    });
    service$.subscribe(r => this.result.set(r));
    void isAdmin;
  }
}
