import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../../core/services/api.service';
import { LikeDislikeStats, Video } from '../../../../core/models/types';
import { VideoListItemDto } from '../../../../core/models/video.model';
import { VideoService } from '../../../../core/services/video.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-video-watch',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="video-container" role="main">
      <div class="video-player-section" *ngIf="video; else loading">
        <div class="video-player" aria-label="Video player">
          <video width="100%" height="100%" controls [attr.src]="video.videoUrl" poster="{{video.thumbnailUrl}}" tabindex="0"></video>
        </div>
        <div class="video-info">
          <h1 class="video-title" tabindex="0">{{ video.title }}</h1>
          <div class="video-meta">
            <div class="video-channel">
              <div class="thumbnail">
                @if (video.thumbnailUrl) {
                  <img [src]="video.channel?.avatarUrl" [alt]="video.channel?.name?.charAt(0)" class="avatar-image">
                } @else {
                  <div class="thumbnail-placeholder">
                    <span>{{video?.channel?.name?.charAt(0)}}</span>
                  </div>
                }
              </div>
              <p class="channel-name" [routerLink]="['/channel', video?.channel?.id]">{{ video?.channel?.name }}</p>
              <p class="view-count" aria-label="Views">{{ video?.channel?.subscriberCount || 0 }} subscribers</p>
            
            <button class="action-btn subscribe-btn" 
                [ngClass]="{'active': video?.isSubscribed}" 
                (click)="onSubscribe()" 
                aria-label="Subscribe">
                {{ video?.isSubscribed ? 'Subscribed' : 'Subscribe' }}
              </button>
            </div>
            <div class="video-actions" role="group" aria-label="Video actions">
              <button class="action-btn like-btn" (click)="likeDislike(true)" [ngClass]="{'active': userLiked}" [attr.aria-pressed]="userLiked" aria-label="Like">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" aria-hidden="true">
                  <path d="M7 22H4C3.46957 22 2.96086 21.7893 2.58579 21.4142C2.21071 21.0391 2 20.5304 2 20V13C2 12.4696 2.21071 11.9609 2.58579 11.5858C2.96086 11.2107 3.46957 11 4 11H7M14 9V5C14 4.20435 13.6839 3.44129 13.1213 2.87868C12.5587 2.31607 11.7956 2 11 2L7 11V22H18.28C18.7623 22.0055 19.2304 21.8364 19.5979 21.524C19.9654 21.2116 20.2077 20.7769 20.28 20.3L21.66 11.3C21.7035 11.0134 21.6842 10.7207 21.6033 10.4423C21.5225 10.1638 21.3821 9.90629 21.1919 9.68751C21.0016 9.46873 20.7661 9.29393 20.5016 9.17522C20.2371 9.0565 19.9499 8.99672 19.66 9H14Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <span class="visually-hidden">Like</span>
                {{ stats?.likeCount || 0 }}
              </button>
              <button class="action-btn dislike-btn" (click)="likeDislike(false)" [ngClass]="{'active': userDisliked}" [attr.aria-pressed]="userDisliked" aria-label="Dislike">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" aria-hidden="true">
                  <path d="M17 2H20C20.5304 2 21.0391 2.21071 21.4142 2.58579C21.7893 2.96086 22 3.46957 22 4V11C22 11.5304 21.7893 12.0391 21.4142 12.4142C21.0391 12.7893 20.5304 13 20 13H17M10 15V19C10 19.7956 10.3161 20.5587 10.8787 21.1213C11.4413 21.6839 12.2044 22 13 22L17 13V2H5.72C5.23767 1.99448 4.76962 2.16359 4.40206 2.47599C4.03451 2.78839 3.79227 3.22309 3.72 3.7L2.34 12.7C2.29649 12.9866 2.31579 13.2793 2.39667 13.5577C2.47755 13.8362 2.61792 14.0937 2.80813 14.3125C2.99833 14.5313 3.23389 14.7061 3.49843 14.8248C3.76297 14.9435 4.05014 15.0033 4.34 15H10Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <span class="visually-hidden">Dislike</span>
                {{ stats?.dislikeCount || 0 }}
              </button>
            </div>
          </div>
        </div>
        <div class="video-description">
          <div class="description-content">
            <p class="views">{{ video?.viewCount | number }} views</p>
            <p>{{ video.description }}</p>
            <p class="tags">{{ video?.tags?.join(', ') }}</p>
          </div>
        </div>
        <div class="comments-section">
          <h3 class="comments-title">Comments ({{ comments?.length || 0 }})</h3>
          <div class="comment-form">
            <div class="comment-input-container">
              <textarea [(ngModel)]="newCommentText" name="comment" placeholder="Add a public comment..." class="comment-input" (focus)="showCommentActions = true" required></textarea>
              <div class="comment-actions" [ngClass]="{'show': showCommentActions}">
                <button type="button" class="cancel-btn" (click)="cancelComment()">Cancel</button>
                <button type="submit" class="submit-btn" (click)="submitComment()" [disabled]="!newCommentText.trim()">Comment</button>
              </div>
            </div>
          </div>
          <div class="comments-list" *ngIf="comments?.length; else noComments">
            <div class="comment" *ngFor="let c of comments">
              <div class="comment-avatar">
                <div class="avatar-circle">{{ c.authorName?.charAt(0)?.toUpperCase() }}</div>
              </div>
              <div class="comment-content">
                <div class="comment-header">
                  <span>
                  <span class="comment-author">{{ c.username }}</span>
                  <span class="comment-time">{{ c.createdAt | date:'short' }}</span>
                  </span>
                  <button class="delete-btn"  (click)="onDeleteComment(c.id)">
                  <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="15" height="15" viewBox="0,0,256,256">
<g fill="#ffffff" fill-rule="nonzero" stroke="none" stroke-width="1" stroke-linecap="butt" stroke-linejoin="miter" stroke-miterlimit="10" stroke-dasharray="" stroke-dashoffset="0" font-family="none" font-weight="none" font-size="none" text-anchor="none" style="mix-blend-mode: normal"><g transform="scale(10.66667,10.66667)"><path d="M10,2l-1,1h-6v2h18v-2h-6l-1,-1zM4.36523,7l1.70313,15h11.86328l1.70313,-15z"></path></g></g>
</svg>
                  </button>
                </div>

                <div class="comment-text">{{ c.text }}</div>
                <div class="comment-actions-small">
                  <button class="comment-action-btn">
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
                      <path d="M7 22H4C3.46957 22 2.96086 21.7893 2.58579 21.4142C2.21071 21.0391 2 20.5304 2 20V13C2 12.4696 2.21071 11.9609 2.58579 11.5858C2.96086 11.2107 3.46957 11 4 11H7M14 9V5C14 4.20435 13.6839 3.44129 13.1213 2.87868C12.5587 2.31607 11.7956 2 11 2L7 11V22H18.28C18.7623 22.0055 19.2304 21.8364 19.5979 21.524C19.9654 21.2116 20.2077 20.7769 20.28 20.3L21.66 11.3C21.7035 11.0134 21.6842 10.7207 21.6033 10.4423C21.5225 10.1638 21.3821 9.90629 21.1919 9.68751C21.0016 9.46873 20.7661 9.29393 20.5016 9.17522C20.2371 9.0565 19.9499 8.99672 19.66 9H14Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>
                  </button>
                  <button class="comment-action-btn">
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none">
                      <path d="M17 2H20C20.5304 2 21.0391 2.21071 21.4142 2.58579C21.7893 2.96086 22 3.46957 22 4V11C22 11.5304 21.7893 12.0391 21.4142 12.4142C21.0391 12.7893 20.5304 13 20 13H17M10 15V19C10 19.7956 10.3161 20.5587 10.8787 21.1213C11.4413 21.6839 12.2044 22 13 22L17 13V2H5.72C5.23767 1.99448 4.76962 2.16359 4.40206 2.47599C4.03451 2.78839 3.79227 3.22309 3.72 3.7L2.34 12.7C2.29649 12.9866 2.31579 13.2793 2.39667 13.5577C2.47755 13.8362 2.61792 14.0937 2.80813 14.3125C2.99833 14.5313 3.23389 14.7061 3.49843 14.8248C3.76297 14.9435 4.05014 15.0033 4.34 15H10Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    </svg>
                  </button>
                  <button class="reply-btn">Reply</button>
                </div>
              </div>
            </div>
          </div>
          <ng-template #noComments>
            <div class="no-comments">
              <p>No comments yet. Be the first to comment!</p>
            </div>
          </ng-template>
        </div>
      </div>
      <div class="sidebar">
        <div class="related-videos">
          <div class="related-video" *ngFor="let v of videos()">
            <a [routerLink]="['/watch', v.id]" class="video-link">
              <div class="video-thumbnail" [routerLink]="['/watch', v.id]">
                @if (v.thumbnailUrl) {
                  <img [src]="v.thumbnailUrl" [alt]="v.title" class="thumbnail-image">
                } @else {
                  <div class="thumbnail-placeholder">
                    <span>{{v.title.charAt(0).toUpperCase()}}</span>
                  </div>
                }
                <div class="video-duration">{{ formatDuration(v?.duration || '') }}</div>
              </div>
              <div class="video-details">
                <h4 class="video-title-small">{{ v.title }}</h4>
                <p class="video-channel" [routerLink]="['/channel', v?.channel?.id]">{{ v?.channel?.name }}</p>
                <p class="video-views">{{ v.viewCount | number }} views | {{ v?.uploadedAt | date:'short' }}</p>
              </div>
            </a>
          </div>
        </div>
      </div>
    </div>
    <ng-template #loading>
      <div class="loading-container">
        <div class="loading-spinner"></div>
        <p>Loading video...</p>
      </div>
    </ng-template>
  `,
  styles: [`
    * {
      box-sizing: border-box;
      margin: 0;
      padding: 0;
    }

    .video-container {
      display: flex;
      max-width: 95%;
      margin: 0 auto;
      padding: 24px;
      gap: 24px;
      font-family: 'Roboto', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
      min-height: 100vh;
    }

    .video-player-section {
      flex: 1;
      min-width: 0;
    }

    .video-player {
      background: #000000;
      border-radius: 12px;
      overflow: hidden;
      margin-bottom: 16px;
      aspect-ratio: 16/9;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      transition: transform 0.3s ease;
    }

.delete-btn {
  border: none;
  cursor: pointer;
  color: #888;
  padding: 4px;
  transition: color 0.2s;
}
.delete-btn:hover {
  color: red;
}

    .video-player:hover {
      transform: scale(1.01);
    }

    .video-player video {
      width: 100%;
      height: 100%;
      object-fit: contain;
    }

    .video-info {
      margin-bottom: 20px;
    }

.video-channel {
  display: flex;
  align-items: center;
  gap: 12px;
}

.thumbnail {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  overflow: hidden;
  background-color: #ccc;
  display: flex;
  align-items: center;
  justify-content: center;
}

.thumbnail-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
}

.thumbnail-placeholder {
  width: 100%;
  height: 100%;
  background-color: #aaa;
  color: #fff;
  font-weight: 600;
  font-size: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  text-transform: uppercase;
}

.channel-name {
  font-size: 14px;
  font-weight: 500;
  color: #ffffffff;
}

.view-count {
  font-size: 12px;
  color: #606060;
  margin-left: auto;
}

    .video-title {
      font-size: 20px;
      font-weight: 500;
      line-height: 28px;
      margin: 0 0 12px 0;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }

    .video-meta {
      display: flex;
      justify-content: space-between;
      align-items: center;
      flex-wrap: wrap;
      gap: 12px;
    }

    .video-stats-left {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .view-count {
      color: #606060; /* YouTube's secondary text color */
      font-size: 14px;
      font-weight: 400;
    }

    .video-actions {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .action-btn {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 8px 16px;
      border: none;
      border-radius: 18px;
      background: #F2F2F2; /* YouTube's button background */
      color: #0F0F0F;
      font-size: 14px;
      font-weight: 500;
      cursor: pointer;
      transition: all 0.2s ease;
      box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    }
    .tags {
      font-size: 14px;
      color: primary;
      margin-top: 8px;
  }
    .action-btn:hover {
      background: #E0E0E0; /* Slightly darker for hover */
      transform: translateY(-1px);
    }

    .action-btn.active {
      background: #CC0000; /* YouTube red */
      color: #FFFFFF;
    }

    .action-btn.active svg path {
      stroke: #FFFFFF;
    }

    .action-btn svg {
      flex-shrink: 0;
      transition: transform 0.2s ease;
    }

    .action-btn:hover svg {
      transform: scale(1.1);
    }

    .video-description {
      background: #272727;
      border-radius: 7px;
      padding: 16px;
      margin-bottom: 24px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      transition: box-shadow 0.2s ease;
    }

    .video-description:hover {
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .description-content {
      color: #0F0F0F;
      font-size: 14px;
      line-height: 22px;
      white-space: pre-wrap;
    }

    .comments-section {
      margin-top: 32px;
    }

    .comments-title {
      font-size: 18px;
      font-weight: 400;
      margin: 0 0 20px 0;
    }
      .comment-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

    .comment-form {
      margin-bottom: 32px;
    }

    .comment-input-container {
      position: relative;
    }

    .comment-input {
      width: 100%;
      border: none;
      border-bottom: 1px solid #CCCCCC;
      font-size: 14px;
      background: transparent;
      resize: none;
      outline: none;
      color: #cccccc;
    }

    .comment-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      margin-top: 12px;
      opacity: 0;
      transform: translateY(-10px);
      transition: all 0.3s ease;
      pointer-events: none;
    }

    .comment-actions.show {
      opacity: 1;
      transform: translateY(0);
      pointer-events: all;
    }

    .cancel-btn, .submit-btn {
      padding: 8px 20px;
      border: none;
      border-radius: 18px;
      font-size: 14px;
      font-weight: 500;
      cursor: pointer;
      transition: all 0.3s ease;
      box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    }

    .cancel-btn {
      background: transparent;
      color: #cccccc;
    }

    .submit-btn {
      background: #065FD4; /* YouTube blue */
      color: #FFFFFF;
    }

    .submit-btn:hover:not(:disabled) {
      background: #003087; /* Darker YouTube blue */
      transform: translateY(-1px);
    }

    .submit-btn:disabled {
      background: #333333ff;
      cursor: not-allowed;
      box-shadow: none;
    }

    .comments-list {
      display: flex;
      flex-direction: column;
      gap: 20px;
    }

    .comment {
      display: flex;
      gap: 16px;
      padding: 12px;
      border-radius: 8px;
      transition: background-color 0.2s ease;
    }

    .comment-avatar {
      flex-shrink: 0;
    }

    .avatar-circle {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: linear-gradient(135deg, #CC0000, #065FD4); /* YouTube red to blue gradient */
      color: #FFFFFF;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 600;
      font-size: 16px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
      transition: transform 0.2s ease;
    }

    .comment:hover .avatar-circle {
      transform: scale(1.05);
    }

    .comment-content {
      flex: 1;
      min-width: 0;
    }

    .comment-header {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 6px;
    }

    .comment-author {
      font-weight: 500;
      font-size: 14px;
      color: #cccccc;
      margin-right: 8px;
    }

    .comment-time {
      font-size: 12px;
      color: #606060;
    }

    .comment-text {
      font-size: 14px;
      line-height: 22px;
      margin-bottom: 8px;
    }

    .comment-actions-small {
      display: flex;
      align-items: center;
      gap: 12px;
    }

    .comment-action-btn {
      display: flex;
      align-items: center;
      background: none;
      border: none;
      padding: 8px;
      border-radius: 50%;
      cursor: pointer;
      color: #606060;
      transition: all 0.2s ease;
    }

    .comment-action-btn:hover {
      background: #E5E5E5;
      transform: scale(1.1);
    }

    .reply-btn {
      background: none;
      border: none;
      color: #606060;
      font-size: 13px;
      font-weight: 500;
      padding: 6px 12px;
      border-radius: 16px;
      cursor: pointer;
      transition: all 0.2s ease;
    }

    .reply-btn:hover {
      background: #F2F2F2;
      color: #0F0F0F;
    }

    .no-comments {
      padding: 32px;
      text-align: center;
      color: #606060;
      font-size: 14px;
        border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    }

    .sidebar {
      width: 402px;
      flex-shrink: 0;
    }

    .related-videos {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .related-video {
      border-radius: 12px;
      transition: all 0.3s ease;
    }

    .video-link {
      display: flex;
      gap: 12px;
      padding: 8px;
      text-decoration: none;
      color: inherit;
      border-radius: 12px;
    }

    .video-thumbnail {
      position: relative;
      width: 168px;
      height: 94px;
      flex-shrink: 0;
      border-radius: 8px;
      overflow: hidden;
      transition: transform 0.3s ease;
    }

    .video-thumbnail img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      border-radius: 8px;
    }

    .video-duration {
      position: absolute;
      bottom: 6px;
      right: 6px;
      background: rgba(0, 0, 0, 0.85);
      color: #FFFFFF;
      padding: 3px 6px;
      border-radius: 4px;
      font-size: 12px;
      font-weight: 500;
      box-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
    }

    .video-details {
      flex: 1;
      min-width: 0;
    }

    .video-title-small {
      font-size: 14px;
      font-weight: 500;
      line-height: 20px;
      margin: 0 0 6px 0;
      color: #cccccc;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
      transition: color 0.2s ease;
    }

    .video-channel {
      font-size: 12px;
      color: #cccccc;
      margin: 0 0 4px 0;
    }

    .video-views {
      font-size: 12px;
      color: #cccccc;
      margin: 0;
    }

    .loading-container {
      width: 100%;
      height: 100%;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 80px;
      color: #606060;
      border-radius: 12px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    }

    .loading-spinner {
      width: 32px;
      height: 32px;
      border: 3px solid #F1F1F1;
      border-top: 3px solid #CC0000; /* YouTube red */
      border-radius: 50%;
      animation: spin 0.8s linear infinite;
      margin-bottom: 16px;
    }

    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }

    @media (max-width: 1312px) {
      .video-container {
        flex-direction: column;
        padding: 16px;
      }
      
      .sidebar {
        width: 100%;
      }
      
      .related-videos {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
        gap: 16px;
      }
      
      .video-player {
        border-radius: 0;
      }
    }

    @media (max-width: 768px) {
      .video-container {
        padding: 12px;
      }
      
      .video-meta {
        flex-direction: column;
        align-items: flex-start;
        gap: 16px;
      }
      
      .video-actions {
        width: 100%;
        justify-content: flex-start;
        flex-wrap: wrap;
        gap: 12px;
      }
      
      .action-btn {
        font-size: 13px;
        padding: 6px 14px;
      }
      
      .video-title {
        font-size: 16px;
        line-height: 24px;
      }
      
      .comments-title {
        font-size: 16px;
      }
      
      .comment {
        padding: 8px;
      }
      
      .avatar-circle {
        width: 32px;
        height: 32px;
        font-size: 14px;
      }
      
      .video-thumbnail {
        width: 140px;
        height: 78px;
      }
    }

    @media (max-width: 480px) {
      .video-title {
        font-size: 14px;
        line-height: 20px;
      }
      
      .action-btn {
        font-size: 12px;
        padding: 6px 12px;
      }
      
      .comment-input {
        font-size: 13px;
      }
      
      .cancel-btn, .submit-btn {
        font-size: 13px;
        padding: 6px 16px;
      }
      
      .related-videos {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class VideoWatchPage implements OnInit {
  videoId: string = '';
  video?: Video;
  stats?: LikeDislikeStats;
  comments: any[] = [];
  allVideos: Video[] = [];
  newCommentText: string = '';
  showCommentActions: boolean = false;
  userLiked: boolean = false;
  userDisliked: boolean = false;

  videos = signal<VideoListItemDto[]>([]);
  loading = signal(true);

  constructor(
    private route: ActivatedRoute,
    private api: ApiService,
    private videoService: VideoService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.videoId = params.get('id') || '';
      if (this.videoId) {
        this.loadVideo();
        this.loadStats();
        this.loadComments();
        this.loadVideos();
        this.loadUserReaction();
      }
    });
  }

  loadVideo() {
    this.api.getVideo(this.videoId).subscribe({
      next: res => {
        this.video = res.data;
        this.api.recordView(this.videoId).subscribe();

        const channelId = this.video?.channel?.id;
        if (channelId) {
          this.loadSubscriptionStatus(channelId);
        }
      },
      error: err => console.error('Video yuklanmadi:', err)
    });
  }



  loadStats() {
    this.api.getLikeDislikeStats(this.videoId).subscribe({
      next: (res: any) => {
        this.stats = res.data;
        this.loadUserReaction();
      },
      error: err => console.error('Statistikani olishda xatolik:', err)
    });
  }
  
likeDislike(isLike: boolean): void {
  this.api.toggleLikeDislike(this.videoId, isLike).subscribe({
    next: () => {
      if (isLike) {
        this.userLiked = !this.userLiked;
        if (this.userLiked) this.userDisliked = false;
      } else {
        this.userDisliked = !this.userDisliked;
        if (this.userDisliked) this.userLiked = false;
      }
      this.loadStats(); // countlarni yangilaydi
    },
    error: err => console.error('Like/Dislike xato:', err)
  });
}


loadUserReaction(): void {
  this.api.getUserReaction(+this.videoId).subscribe({
    next: (res) => {
      const reaction = res.data;
      if (reaction === true) {
        this.userLiked = true;
        this.userDisliked = false;
      } else if (reaction === false) {
        this.userLiked = false;
        this.userDisliked = true;
      } else {
        this.userLiked = false;
        this.userDisliked = false;
      }
    },
    error: err => {
      console.error('Reaksiya olishda xatolik:', err);
      this.userLiked = false;
      this.userDisliked = false;
    }
  });
}

  loadComments() {
    this.api.getComments(this.videoId).subscribe({
      next: res => this.comments = res.data,
      error: err => console.error('Commentlarni olishda xatolik:', err)
    });
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

  submitComment() {
    if (!this.newCommentText.trim()) return;

    this.api.createComment(this.newCommentText, this.videoId).subscribe({
      next: () => {
        this.newCommentText = '';
        this.showCommentActions = false;
        this.loadComments();
      },
      error: err => console.error('Comment yuborishda xatolik:', err)
    });
  }

  cancelComment() {
    this.newCommentText = '';
    this.showCommentActions = false;
  }

  private loadSubscriptionStatus(channelId: number): void {
    this.api.getMySubscriptions().subscribe({
      next: (res: any) => {
        if (this.video && this.video.channel) {
          const subscribed = res.data.some((sub: any) => sub.channelDto.id === channelId);
          this.video.isSubscribed = subscribed;
          this.api.getSubscriberCount(channelId).subscribe({
            next: res => this.video!.channel!.subscriberCount = res.data,
            error: err => console.error('Subscribe count olishda xatolik:', err)
          });
        }
      },
      error: () => { }
    });
  }

  onSubscribe() {
    if (!this.video?.channel) return;
    const channelId = this.video.channel.id;

    this.api.toggleSubscription(channelId).subscribe({
      next: () => {
        // Toggle'dan keyin subscription status va count yangilanadi
        this.loadSubscriptionStatus(channelId);
      },
      error: err => {
        console.error('Subscribe/unsubscribe xato:', err);
      } 
    });
  }

onDeleteComment(commentId: number) {
  Swal.fire({
    title: 'Delete comment?',
    text: 'Are you sure you want to delete this comment?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Yes, delete it',
    cancelButtonText: 'Cancel',
  }).then((result) => {
    if (result.isConfirmed) {
      this.api.deleteComment(commentId).subscribe({
        next: () => {
          this.loadComments();

          Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'success',
            title: 'Comment has been deleted',
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
          console.error('Comment o‘chirishda xatolik:', err);

          Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'error',
            title: 'Error deleting comment',
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

formatDuration(duration: string): string {
    // Convert duration string to readable format
    const [hours, minutes, seconds] = duration.split(':');
    const h = parseInt(hours);
    const m = parseInt(minutes);
    const s = parseInt(seconds);
    
    if (h > 0) {
      return `${h}:${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
    }
    return `${m}:${s.toString().padStart(2, '0')}`;
  }
}
