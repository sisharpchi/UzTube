import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../../core/services/api.service';
import { Subscription } from '../../../../core/models/types';

@Component({
  selector: 'app-subscriptions',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="subscriptions-page">
      <h2>My Subscriptions</h2>

      <div *ngIf="loading" class="loading">Loading...</div>

      <div *ngIf="!loading && subscriptions.length === 0" class="empty">
        You are not subscribed to any channels yet.
      </div>

      <div class="channel-list" *ngIf="subscriptions.length > 0">
        <div *ngFor="let sub of subscriptions" class="channel-card">
          <div class="avatar-placeholder">📺</div>
          <div class="channel-info">
            <h3>{{ sub.channelDto?.name }}</h3>
            <p>{{ sub.channelDto?.description }}</p>
            <div class="stats">
              <span>{{ sub.channelDto?.videoCount }} videos</span>
              <span>{{ sub.channelDto?.subscriberCount }} subscribers</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .subscriptions-page {
      padding: 32px;
      color: white;
      font-family: Arial, sans-serif;
      background-color: #121212;
      min-height: 100vh;
    }

    h2 {
      margin-bottom: 24px;
      font-size: 28px;
    }

    .loading, .empty {
      text-align: center;
      margin-top: 40px;
      font-size: 18px;
      color: #aaa;
    }

    .channel-list {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .channel-card {
      display: flex;
      gap: 16px;
      background-color: #1e1e1e;
      padding: 16px;
      border-radius: 8px;
      align-items: flex-start;
    }

    .avatar-placeholder {
      width: 60px;
      height: 60px;
      font-size: 30px;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #333;
      border-radius: 50%;
    }

    .channel-info h3 {
      margin: 0;
      font-size: 20px;
    }

    .channel-info p {
      margin: 6px 0;
      color: #ccc;
    }

    .stats {
      display: flex;
      gap: 20px;
      color: #aaa;
      font-size: 14px;
    }
  `]
})
export class SubscriptionsPage implements OnInit {
  subscriptions: Subscription[] = [];
  loading = true;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getMySubscriptions().subscribe({
      next: (res) => {
        this.subscriptions = res.data;
        this.loading = false;
      },
      error: () => {
        this.subscriptions = [];
        this.loading = false;
      }
    });
  }
}
