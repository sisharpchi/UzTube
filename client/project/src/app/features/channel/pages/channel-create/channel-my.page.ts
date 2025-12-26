import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../../core/services/api.service';
import { ChannelWithVideosDto } from '../../../../core/models/types';
import { VideoCardComponent } from '../../../video/components/video-card/video-card.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LucideAngularModule, Upload } from 'lucide-angular';
import { UploadVideoPage } from '../../../video/pages/upload-video/upload-video.page';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-channel-my',
  standalone: true,
  imports: [
    CommonModule,
    VideoCardComponent,
    FormsModule,
    RouterModule,
    LucideAngularModule,
    UploadVideoPage
  ],
  template: `
    <div *ngIf="loading" class="loading">Loading...</div>

    <ng-container *ngIf="!loading && channel; else createForm">
      <div class="channel-container">

        <!-- Banner section -->
        <div class="banner-wrapper">
          <ng-container *ngIf="channel?.bannerUrl; else bannerPlaceholder">
            <img class="banner" [src]="channel.bannerUrl" alt="Banner" />
          </ng-container>
          <!-- Banner Upload Icon -->
          <button class="banner-upload-btn" (click)="bannerInput.click()" title="Upload Banner">
            <lucide-angular [img]="UploadIcon" size="22"></lucide-angular>
          </button>
          <input type="file" #bannerInput style="display:none" accept="image/*" (change)="onBannerSelected($event)" />
        </div>
        <!-- Banner placeholder if null -->
        <ng-template #bannerPlaceholder>
          <div class="banner banner-placeholder">
            <span class="banner-text">Your banner</span>
          </div>
        </ng-template>

        <!-- Channel Info section -->
        <div class="channel-info">
          <div class="channel-info-block">
            <div class="avatar-wrapper">
              <ng-container *ngIf="channel?.avatarUrl; else avatarInitial">
                <img class="avatar" [src]="channel.avatarUrl" alt="Avatar" />
              </ng-container>
              <!-- Avatar placeholder if null -->
              <ng-template #avatarInitial>
                <div class="avatar avatar-initial">{{ channel?.name?.[0] | uppercase }}</div>
              </ng-template>
              <!-- Avatar Upload Icon -->
              <button class="avatar-upload-btn" (click)="avatarInput.click()" title="Upload Avatar">
                <lucide-angular [img]="UploadIcon" size="18"></lucide-angular>
              </button>
              <input type="file" #avatarInput style="display:none" accept="image/*" (change)="onAvatarSelected($event)" />
            </div>
            <div class="channel-meta">
              <h2 class="channel-name">{{ channel.name }}</h2>
              <p class="channel-description">{{ channel.description }}</p>
              <div class="subscribe-row">
                <span class="subscriber-count">{{ subscriberCount }} subscribers</span>
              </div>
            </div>
          </div>
          <a (click)="uploadMode = true" class="icon-btn upload-btn">
            <lucide-angular [img]="UploadIcon" size="20"></lucide-angular>
          </a>
        </div>

        <!-- Tabs -->
        <div class="channel-tabs">
          <span class="tab" [class.selected]="!uploadMode" (click)="uploadMode = false">Videos</span>
          <span class="tab" [class.selected]="uploadMode" (click)="uploadMode = true">Upload</span>
          <span class="tab">Playlists</span>
          <span class="tab">About</span>
        </div>

        <!-- Tab Content -->
        <div class="channel-content">
          <div *ngIf="!uploadMode && channel.videos && channel.videos.length > 0" class="channel-videos">
            <h3>Videos</h3>
            <div class="video-grid">
              <app-video-card
  *ngFor="let video of channel.videos"
  [video]="video"
  [showOwnerActions]="true"
  (delete)="onDeleteVideo(video.id)">
</app-video-card>
            </div>
          </div>
          <div *ngIf="uploadMode" class="upload-section">
            <app-upload-video (onClose)="uploadMode = false" />
          </div>
        </div>
      </div>
    </ng-container>

    <!-- Channel yaratish formi -->
<ng-template #createForm>
  <div class="create-channel">
    <h2>Create Your Channel</h2>
    <input [(ngModel)]="name" placeholder="Channel name" />
    <textarea [(ngModel)]="description" placeholder="Channel description"></textarea>
    <button (click)="createChannel()">Create</button>
  </div>
</ng-template>

  `,
  styles: [`

.create-channel {
  max-width: 700px;
  margin: 0 auto;
  padding: 2rem;
  border-radius: 12px;
  background-color: #0f0f0f;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.create-channel h2 {
  font-size: 1.5rem;
  text-align: center;
  margin-bottom: 1.5rem;
  color: #fff;
}

.create-channel input,
.create-channel textarea {
  background-color: #0f0f0f;
  width: 100%;
  padding: 0.75rem 1rem;
  margin-bottom: 1rem;
  border-bottom: 1px solid #fff;
  color: #fff;
  font-size: 1rem;
  transition: border 0.3s ease;
}

.create-channel input:focus,
.create-channel textarea:focus {
  outline: none;
  border-color: #007bff;
}

.create-channel textarea {
  resize: none;
  min-height: 100px;
}
.create-channel {
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: center; /* Add this line to center children horizontally */
}

.create-channel button {
  width: 50%;
  padding: 0.75rem;
  font-size: 1rem;
  color: white;
  margin-top: 1rem;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

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
.banner-upload-btn {
  position: absolute;
  top: 18px;
  right: 18px;
  background: rgba(0,0,0,0.5);
  border: none;
  border-radius: 50%;
  padding: 8px;
  color: white;
  cursor: pointer;
  transition: background .2s;
  z-index: 10;
}
.banner-upload-btn:hover { background: #cc0000; }

.channel-info {
  display: flex;
  justify-content: space-between;
}

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
.avatar-upload-btn {
  position: absolute;
  bottom: 8px;
  right: -8px;
  background: rgba(0,0,0,0.6);
  border: none;
  border-radius: 50%;
  padding: 4px;
  color: white;
  cursor: pointer;
  z-index: 5;
}
.avatar-upload-btn:hover { background: #cc0000; }
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
  transition: color 0.2s;
}
.channel-tabs .tab:hover {
  color: #ddd;
}
.channel-tabs .tab.selected {
  color: white;
  border-bottom: 2px solid white;
}
.channel-content {
  min-height: 400px;
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
.upload-section {
  padding: 24px 32px;
}
.loading {
  color: white;
  padding: 50px;
  text-align: center;
}
.create-channel {
  padding: 40px;
  color: white;
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.create-channel input, .create-channel textarea {
  padding: 8px;
}
.create-channel button {
  padding: 10px;
  background: #cc0000a9;
  color: white;
  border: none;
  border-radius: 6px;
}
.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 8px;
  transition: background-color 0.2s;
  text-decoration: none;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 10px 15px;
  margin: 16px 32px;
}
.upload-btn {
  border-radius: 20%;
  background: var(--hover-bg, #212121);
  color: white;
}
.upload-btn:hover {
  background: var(--accent-hover, #333);
}
.swal2-popup.small-toast {
  padding: 0.5rem 1rem !important;
  font-size: 0.85rem !important;
  min-height: 50px !important;
  min-width: 200px !important;
}
  `]
})
export class ChannelMyPage implements OnInit {
  loading = true;
  channel: ChannelWithVideosDto | null = null;
  subscriberCount = 0;
  uploadMode = false;

  UploadIcon = Upload;

  name = '';
  description = '';
  defaultBanner = 'https://i.pinimg.com/1200x/80/7a/1a/807a1a2700bd768a03f50c29d29289f8.jpg';

  @ViewChild('bannerInput') bannerInput!: HTMLInputElement;
  @ViewChild('avatarInput') avatarInput!: HTMLInputElement;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadMyChannel();
  }

  loadSubscriberCount(): void {
    this.api.getMyChannelSubscribersCount().subscribe({
      next: (res) => this.subscriberCount = res.data ?? 0,
      error: () => this.subscriberCount = 0,
      complete: () => this.loading = false
    });
  }

  createChannel(): void {
    this.name = this.name.trim();
    this.description = this.description.trim();
    if (!this.name) {
      alert('Please enter a channel name');
      return;
    }
    this.api.createChannel(this.name, this.description).subscribe({
      next: (res) => {
        this.channel = res.data;
        this.loadSubscriberCount();
        this.name = '';
        this.description = '';
      },
      error: (err) => {
        console.error('Error creating channel:', err);
        if (err.status === 409 || err.status === 400) {
          alert(err.error?.message || 'You already have a channel.');
        } else {
          alert('Unexpected error while creating channel.');
        }
      }
    });
  }

  onUploadComplete(): void {
    this.uploadMode = false;
    this.loadMyChannel();
  }
  loadMyChannel(): void {
    this.loading = true;
    this.api.getMyChannel().subscribe({
      next: (res) => {
        this.channel = res.data;
        this.loading = false;
        if (this.channel) {
          this.loadSubscriberCount();
        }
      },
      error: () => {
        this.channel = null;
        this.loading = false;
      }
    });
  }

  onBannerSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;
    this.api.uploadBanner(file).subscribe({
      next: () => this.loadMyChannel(),
      error: () => alert("Banner yuklanmadi")
    });
    (event.target as HTMLInputElement).value = ''; // allows re-upload
  }
  onAvatarSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;
    this.api.uploadAvatar(file).subscribe({
      next: () => this.loadMyChannel(),
      error: () => alert("Avatar yuklanmadi")
    });
    (event.target as HTMLInputElement).value = '';
  }

  onDeleteVideo(videoId: number) {
    Swal.fire({
      title: 'Delete video?',
      text: 'Are you sure you want to delete this video?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it',
      cancelButtonText: 'Cancel',
    }).then((result) => {
      if (result.isConfirmed) {
        this.api.deleteVideo(videoId.toString()).subscribe({
          next: () => {
            this.channel!.videos = this.channel!.videos.filter(v => v.id !== videoId);

            Swal.fire({
              toast: true,
              position: 'top-end',
              icon: 'success',
              title: 'Video has been deleted',
              showConfirmButton: false,
              timer: 2000,
              timerProgressBar: true,
              width: '300px',
              customClass: {
                popup: 'small-toast',
              }
            });
          },
          error: (err) => {
            console.error('Error deleting video', err);

            Swal.fire({
              toast: true,
              position: 'top-end',
              icon: 'error',
              title: 'Error deleting video',
              showConfirmButton: false,
              timer: 1500,
              timerProgressBar: true,
              width: '300px',
              customClass: {
                popup: 'small-toast',
              }
            });
          }
        });
      }
    });
  }
}
