import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PriorityBadgeComponent } from '../../../shared/components/badge/priority-badge.component';
import { StatusBadgeComponent } from '../../../shared/components/badge/status-badge.component';
import { ComplaintService } from '../../../core/services/complaint.service';
import { ComplaintListItem, PagedResult } from '../../../core/models/models';
import { ComplaintStatus } from '../../../core/models/enums';

@Component({
  selector: 'app-inbox',
  standalone: true,
  imports: [CommonModule, RouterLink, SidebarComponent, PriorityBadgeComponent, StatusBadgeComponent],
  templateUrl: './inbox.component.html',
  styleUrl: './inbox.component.scss'
})
export class InboxComponent implements OnInit {
  result = signal<PagedResult<ComplaintListItem> | null>(null);
  activeFilter: ComplaintStatus | 'all' = 'all';

  filters = [
    { key: 'all' as const, label: 'الكل' },
    { key: ComplaintStatus.New, label: 'جديدة' },
    { key: ComplaintStatus.Assigned, label: 'معينة' },
    { key: ComplaintStatus.UnderInvestigation, label: 'قيد التحقيق' }
  ];

  constructor(private complaintService: ComplaintService) {}

  ngOnInit() { this.load(); }

  setFilter(key: ComplaintStatus | 'all') { this.activeFilter = key; this.load(); }

  load() {
    this.complaintService.getInbox({ status: this.activeFilter === 'all' ? null : this.activeFilter })
      .subscribe(r => this.result.set(r));
  }
}
