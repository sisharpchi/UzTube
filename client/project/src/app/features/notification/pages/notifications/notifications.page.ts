import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notifications-page">
      <h2>Notifications Page</h2>
      <p>This page will display user notifications.</p>
    </div>
  `,
  styles: [`
    .notifications-page {
      padding: 20px;
      color: var(--text-primary);
    }
  `]
})
export class NotificationsPage {}