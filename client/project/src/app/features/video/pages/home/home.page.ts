import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoService } from '../../../../core/services/video.service';
import { VideoListItemDto } from '../../../../core/models/video.model';
import { VideoCardComponent } from '../../components/video-card/video-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, VideoCardComponent],
  template: `
    <div class="home-page">
      <div class="page-header">
        <h1>Home</h1>
      </div>

      @if (loading()) {
        <div class="loading">
          <div class="loading-grid">
            @for (item of loadingItems; track $index) {
              <div class="loading-card">
                <div class="loading-thumbnail"></div>
                <div class="loading-content">
                  <div class="loading-title"></div>
                  <div class="loading-channel"></div>
                  <div class="loading-meta"></div>
                </div>
              </div>
            }
          </div>
        </div>
      } @else {
        <div class="video-grid">
          @for (video of videos(); track video.id) {
            <app-video-card [video]="video"></app-video-card>
          }
        </div>
      }

      @if (!loading() && videos().length === 0) {
        <div class="empty-state">
          <h2>No videos found</h2>
          <p>Try checking back later for new content.</p>
        </div>
      }
    </div>
  `,
  styles: [`
    .home-page {
      max-width: 95%;
      margin: 0 auto;
    }

    .page-header {
      margin-bottom: 24px;
    }

    .page-header h1 {
      font-size: 24px;
      font-weight: 600;
      color: var(--text-primary);
      margin: 0;
    }

    .video-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 24px;
    }

    .loading-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 24px;
    }

    .loading-card {
      background: var(--bg-secondary);
      border-radius: 12px;
      overflow: hidden;
      animation: pulse 2s ease-in-out infinite;
    }

    .loading-thumbnail {
      width: 100%;
      height: 180px;
      background: var(--bg-tertiary);
    }

    .loading-content {
      padding: 16px;
    }

    .loading-title {
      height: 20px;
      background: var(--bg-tertiary);
      border-radius: 4px;
      margin-bottom: 8px;
    }

    .loading-channel {
      height: 16px;
      background: var(--bg-tertiary);
      border-radius: 4px;
      margin-bottom: 8px;
      width: 60%;
    }

    .loading-meta {
      height: 14px;
      background: var(--bg-tertiary);
      border-radius: 4px;
      width: 40%;
    }

    .empty-state {
      text-align: center;
      padding: 60px 20px;
      color: var(--text-secondary);
    }

    .empty-state h2 {
      font-size: 20px;
      margin-bottom: 8px;
      color: var(--text-primary);
    }

    @keyframes pulse {
      0%, 100% {
        opacity: 1;
      }
      50% {
        opacity: 0.5;
      }
    }

    @media (max-width: 768px) {
      .video-grid,
      .loading-grid {
        grid-template-columns: 1fr;
        gap: 16px;
      }
    }

    @media (max-width: 480px) {
      .video-grid,
      .loading-grid {
        grid-template-columns: 1fr;
        gap: 12px;
      }
    }
  `]
})
export class HomePage implements OnInit {
  private videoService = inject(VideoService);
  
  videos = signal<VideoListItemDto[]>([]);
  loading = signal(true);
  loadingItems = Array(12).fill(0);

  ngOnInit(): void {
    this.loadVideos();
  }

  private loadVideos(): void {
    this.loading.set(true);
    
    this.videoService.getAllVideos().subscribe({
      next: (videos) => {
        this.videos.set(videos);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading videos:', error);
        this.loading.set(false);
      }
    });
  }
}