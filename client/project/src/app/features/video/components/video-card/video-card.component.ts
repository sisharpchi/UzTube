import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { VideoListItemDto } from '../../../../core/models/video.model';

@Component({
  selector: 'app-video-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="video-card">
      <!-- Thumbnail -->
      <a [routerLink]="['/watch', video.id]" class="thumbnail-link">
        <div class="thumbnail">
          @if (video.thumbnailUrl) {
            <img [src]="video.thumbnailUrl" [alt]="video.title" class="thumbnail-image">
          } @else {
            <div class="thumbnail-placeholder">
              <span>{{video.title.charAt(0).toUpperCase()}}</span>
            </div>
          }
          <div class="duration">{{formatDuration(video.duration) }}</div>
        </div>
      </a>

      <!-- Info -->
      <div class="video-info">
        <div class="channel-avatar">
          <div class="avatar-placeholder">
            <a [routerLink]="['/channel', video.channel?.id]">
              <img [src]="video.channel?.avatarUrl" [alt]="video.channel?.name?.charAt(0)" class="avatar-image">
            </a>
          </div>
        </div>

        <div class="video-details">
          <h3 class="video-title">
            <a [routerLink]="['/watch', video.id]">{{video.title}}</a>
          </h3>

          <div class="video-meta">
            <p class="channel-name">{{video.channel?.name}}</p>
            <div class="meta-stats">
              <span>{{formatViewCount(video.viewCount)}} views</span>
              <span class="separator">•</span>
              <span>{{formatUploadDate(video.uploadedAt)}}</span>
            </div>
          </div>
        </div>

        <!-- Menu Button -->
        <div class="menu-wrapper" (click)="toggleMenu($event)">
          <button class="menu-button">⋮</button>
          <div *ngIf="menuOpen" class="menu-dropdown">
            <button *ngIf="showOwnerActions" (click)="onDelete($event)">Delete</button>
            <ng-container *ngIf="!showOwnerActions">
              <button (click)="onReport($event)">Report</button>
              <button (click)="onSave($event)">Save</button>
              <button (click)="onCopyLink($event)">Copy Link</button>
            </ng-container>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .video-card {
      position: relative;
      background: var(--bg-secondary);
      border-radius: 12px;
      overflow: hidden;
      transition: transform 0.2s, box-shadow 0.2s;
      cursor: pointer;
    }
    .video-card:hover {
      transform: translateY(-4px);
      box-shadow: var(--shadow-lg);
    }

    .thumbnail-link {
      display: block;
      text-decoration: none;
    }

    .thumbnail {
      position: relative;
      width: 100%;
      height: 180px;
      background: var(--bg-tertiary);
      overflow: hidden;
    }

    .thumbnail-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .thumbnail-placeholder {
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      background: linear-gradient(135deg, var(--accent-color), #ff6b6b);
      color: white;
      font-size: 48px;
      font-weight: bold;
    }

    .duration {
      position: absolute;
      bottom: 8px;
      right: 8px;
      background: rgba(0, 0, 0, 0.8);
      color: white;
      padding: 2px 6px;
      border-radius: 4px;
      font-size: 12px;
    }

    .video-info {
      padding: 16px;
      display: flex;
      gap: 12px;
    }

    .channel-avatar {
      flex-shrink: 0;
    }

    .avatar-placeholder {
      width: 36px;
      height: 36px;
      border-radius: 50%;
      background: var(--accent-color);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 600;
      font-size: 14px;
    }

    .video-details {
      flex: 1;
      min-width: 0;
    }

    .video-title {
      font-size: 16px;
      font-weight: 600;
    }

    .video-title a {
      color: var(--text-primary);
      text-decoration: none;
    }

    .video-meta {
      font-size: 14px;
      color: var(--text-secondary);
    }

    .meta-stats {
      display: flex;
      gap: 4px;
    }

    .separator {
      font-weight: bold;
    }

    .menu-wrapper {
      position: absolute;
      top: 12px;
      right: 12px;
      background: rgba(0, 0, 0, 0.5);
      border-radius: 50%;
      width: 32px;
      height: 32px;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      transition: background 0.2s;
    }

    .menu-button {
      background: transparent;
      border: none;
      color: white;
      font-size: 20px;
      cursor: pointer;
    }

    .menu-dropdown {
      position: absolute;
      top: 28px;
      right: 0;
      background: #222;
      border: 1px solid #444;
      border-radius: 6px;
      padding: 6px 0;
      z-index: 100;
      display: flex;
      flex-direction: column;
    }

    .menu-dropdown button {
      background: none;
      border: none;
      color: white;
      padding: 8px 16px;
      text-align: left;
      cursor: pointer;
    }

    .menu-dropdown button:hover {
      background: #333;
    }
    .avatar-image {
      width: 36px;
      height: 36px;
      border-radius: 50%;
      object-fit: cover;
      background: #222;
      display: block;
    }
  `]
})
export class VideoCardComponent {
  @Input({ required: true }) video!: VideoListItemDto;
  @Input() showOwnerActions = false;
  @Output() delete = new EventEmitter<number>();

  menuOpen = false;

  toggleMenu(event: MouseEvent) {
    event.stopPropagation();
    this.menuOpen = !this.menuOpen;
  }

  onDelete(event: MouseEvent) {
    event.stopPropagation();
    this.delete.emit(this.video.id);
  }

  onReport(event: MouseEvent) {
    event.stopPropagation();
    alert('Reported!');
  }

  onSave(event: MouseEvent) {
    event.stopPropagation();
    alert('Saved!');
  }

  onCopyLink(event: MouseEvent) {
    event.stopPropagation();
    const url = `${location.origin}/watch/${this.video.id}`;
    navigator.clipboard.writeText(url);
    alert('Link copied!');
  }

  formatDuration(duration: string): string {
    const [hours, minutes, seconds] = duration.split(':');
    const h = parseInt(hours);
    const m = parseInt(minutes);
    const s = parseInt(seconds);
    return h > 0 ? `${h}:${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}` : `${m}:${s.toString().padStart(2, '0')}`;
  }

  formatViewCount(views: number): string {
    if (views >= 1000000) return Math.floor(views / 1000000) + 'M';
    if (views >= 1000) return Math.floor(views / 1000) + 'K';
    return views.toString();
  }

  formatUploadDate(date: string): string {
    const uploadDate = new Date(date);
    const now = new Date();
    const diff = Math.floor((now.getTime() - uploadDate.getTime()) / (1000 * 60 * 60 * 24));
    if (diff === 0) return 'Today';
    if (diff === 1) return '1 day ago';
    if (diff < 7) return `${diff} days ago`;
    if (diff < 30) return `${Math.floor(diff / 7)} weeks ago`;
    if (diff < 365) return `${Math.floor(diff / 30)} months ago`;
    return `${Math.floor(diff / 365)} years ago`;
  }
}
