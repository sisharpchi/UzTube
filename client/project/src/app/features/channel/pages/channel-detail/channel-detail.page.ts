import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { VideoCardComponent } from '../../../video/components/video-card/video-card.component';
import { ChannelWithVideosDto, Video } from '../../../../core/models/types';
import { ApiService } from '../../../../core/services/api.service';

@Component({
  selector: 'app-channel-detail',
  standalone: true,
  imports: [CommonModule, VideoCardComponent],
  template: `
<div class="channel-container" *ngIf="!loading; else loadingBlock">

  <!-- 1. Banner -->
  <div class="banner-wrapper">
    <ng-container *ngIf="channel?.bannerUrl; else bannerPlaceholder">
      <img class="banner" [src]="channel.bannerUrl" alt="Banner" />
    </ng-container>
    <ng-template #bannerPlaceholder>
      <div class="banner banner-placeholder">
        <span class="banner-text">{{ channel?.name }}</span>
      </div>
    </ng-template>
  </div>

  <!-- 2. Channel Info -->
  <div class="channel-info-block">
    <div class="avatar-wrapper">
      <ng-container *ngIf="channel?.avatarUrl; else avatarInitial">
        <img class="avatar" [src]="channel.avatarUrl" alt="Avatar" />
      </ng-container>
      <ng-template #avatarInitial>
        <div class="avatar avatar-initial">
          {{ channel?.name?.[0] | uppercase }}
        </div>
      </ng-template>
    </div>
    <div class="channel-meta">
      <h2 class="channel-name">{{ channel?.name }}</h2>
      <p class="channel-description">{{ channel?.description }}</p>
      <div class="subscribe-row">
        <span class="subscriber-count">{{ subscriberCount }} subscribers</span>
        <button (click)="onSubscribe()" [class.subscribed]="isSubscribed">
          {{ isSubscribed ? 'Unsubscribe' : 'Subscribe' }}
        </button>
      </div>
    </div>
  </div>

  <!-- 3. Tab Bar -->
  <div class="channel-tabs">
    <span class="tab selected">Home</span>
    <span class="tab">Videos</span>
    <span class="tab">Playlists</span>
    <span class="tab">About</span>
  </div>

  <!-- 4. Videos -->
  <div class="channel-videos">
    <h3>Videos</h3>
    <div class="video-grid">
      <app-video-card *ngFor="let video of videos" [video]="video"></app-video-card>
    </div>
  </div>

</div>

<ng-template #loadingBlock>
  <div class="loading">Loading channel...</div>
</ng-template>
`,
  styles: [ `
.channel-container {
  font-family: Arial, sans-serif;
  color: white;
  background-color: #121212;
  padding-bottom: 40px;
  border-radius: 12px;
  height: 100%;
}

.banner-wrapper {
  position: relative;
}
.banner {
  width: 100%;
  height: 250px;
  object-fit: cover;
  border-radius: 12px 12px 0 0;
  margin-bottom: 50px;
}
.banner-placeholder {
  width: 100%;
  height: 250px;
  background: #232323;
  border-radius: 12px 12px 0 0;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #888;
  font-size: 2.2rem;
  position: relative;
}
.banner-text { pointer-events: none; user-select: none; }

.channel-info-block {
  display: flex;
  align-items: flex-start;
  padding: 16px 32px;
  margin-top: -50px;
}

.avatar-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}
.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  border: 4px solid #121212;
  object-fit: cover;
  background-color: white;
}
.avatar-initial {
  width: 100px; height: 100px;
  border-radius: 50%;
  background: #444;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2.3rem;
  font-weight: bold;
  color: #fff;
  border: 4px solid #121212;
  user-select: none;
}

.channel-meta {
  margin-left: 24px;
}
.channel-name {
  margin: 0;
  font-size: 24px;
  font-weight: bold;
}
.channel-description {
  margin: 6px 0;
  color: #ccc;
  font-size: 14px;
  max-width: 500px;
}
.subscribe-row {
  margin-top: 8px;
  display: flex;
  align-items: center;
  gap: 12px;
}
.subscribe-row button {
  background-color: #cc0000;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 18px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}
.subscribe-row button.subscribed {
  background-color: gray;
}

.channel-tabs {
  display: flex;
  gap: 24px;
  padding: 16px 32px;
  border-bottom: 1px solid #333;
}
.channel-tabs .tab {
  font-weight: 500;
  color: #aaa;
  cursor: pointer;
  padding-bottom: 8px;
}
.channel-tabs .tab.selected {
  color: white;
  border-bottom: 2px solid white;
}
.channel-videos {
  padding: 24px 32px;
}
.channel-videos h3 {
  margin: 0 0 16px;
  font-size: 20px;
  font-weight: bold;
}
.video-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 16px;
}
.loading {
  color: white;
  padding: 50px;
  text-align: center;
}
    `]
})
export class ChannelDetailPage implements OnInit {
  channelId!: string;
  channel!: ChannelWithVideosDto;
  videos: Video[] = [];
  subscriberCount = 0;
  isSubscribed = false;
  loading = true;
  isSubscribing = false;

  constructor(private api: ApiService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.channelId = this.route.snapshot.paramMap.get('id')!;
    this.loadChannelData();
  }

  loadChannelData() {
    this.loading = true;
    this.api.getChannel(this.channelId).subscribe({
      next: res => {
        this.channel = res.data;
        this.videos = res.data.videos;
        this.updateSubscriptionStatus();
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        console.error('Channel yuklanmadi:', err);
      }
    });
  }

  updateSubscriptionStatus() {
    const id = Number(this.channelId);
    // 1. Channel subscriber count
    this.api.getSubscriberCount(id).subscribe({
      next: res => {
        this.subscriberCount = res.data;
        // 2. My subscriptions
        this.api.getMySubscriptions().subscribe({
          next: (mySubs: any) => {
            this.isSubscribed = mySubs.data.some((sub: any) =>
              sub.channelDto.id === id
            );
          },
          error: () => this.isSubscribed = false
        });
      },
      error: () => this.subscriberCount = 0
    });
  }

  onSubscribe() {
    if (this.isSubscribing) return;
    this.isSubscribing = true;
    const id = Number(this.channelId);

    this.api.toggleSubscription(id).subscribe({
      next: () => {
        // Har doim yangilash uchun status va countni qaytadan chaqiramiz
        this.updateSubscriptionStatus();
      },
      error: err => {
        this.isSubscribing = false;
        console.error('Subscribe/unsubscribe xato:', err);
      },
      complete: () => {
        this.isSubscribing = false;
      }
    });
  }
}
