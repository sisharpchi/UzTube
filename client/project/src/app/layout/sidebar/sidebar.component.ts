import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LucideAngularModule, Home, Play, Users, Library, TrendingUp, History, Clock, ThumbsUp } from 'lucide-angular';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, LucideAngularModule],
  template: `
    <aside class="sidebar">
      <nav class="nav-section">
        <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact: true}" class="nav-item">
          <lucide-angular [img]="HomeIcon" size="20"></lucide-angular>
          <span>Home</span>
        </a>
        
        <a routerLink="/trending" routerLinkActive="active" class="nav-item">
          <lucide-angular [img]="TrendingIcon" size="20"></lucide-angular>
          <span>Trending</span>
        </a>

        <a routerLink="/shorts" routerLinkActive="active" class="nav-item">
          <lucide-angular [img]="PlayIcon" size="20"></lucide-angular>
          <span>Shorts</span>
        </a>
      </nav>

      @if (authService.isAuthenticated()) {
        <div class="divider"></div>
        
        <nav class="nav-section">
          <a routerLink="/subscriptions" routerLinkActive="active" class="nav-item">
            <lucide-angular [img]="UsersIcon" size="20"></lucide-angular>
            <span>Subscriptions</span>
          </a>
        </nav>

        <div class="divider"></div>
        
        <nav class="nav-section">
          <h3 class="section-title">Library</h3>
          
          <a routerLink="/library" routerLinkActive="active" class="nav-item">
            <lucide-angular [img]="LibraryIcon" size="20"></lucide-angular>
            <span>Library</span>
          </a>
          
          <a routerLink="/history" routerLinkActive="active" class="nav-item">
            <lucide-angular [img]="HistoryIcon" size="20"></lucide-angular>
            <span>History</span>
          </a>
          
          <a routerLink="/watch-later" routerLinkActive="active" class="nav-item">
            <lucide-angular [img]="ClockIcon" size="20"></lucide-angular>
            <span>Watch Later</span>
          </a>
          
          <a routerLink="/liked" routerLinkActive="active" class="nav-item">
            <lucide-angular [img]="ThumbsUpIcon" size="20"></lucide-angular>
            <span>Liked Videos</span>
          </a>
        </nav>
      }
    </aside>
  `,
  styles: [`
    .sidebar {
      width: 240px;
      padding: 12px 0;
      overflow-y: auto;
      flex-shrink: 0;
      height: 100vh;
    }

    .nav-section {
      padding: 0 12px;
      margin-bottom: 12px;
    }

    .section-title {
      font-size: 14px;
      font-weight: 500;
      color: var(--text-secondary);
      margin: 0 0 8px 12px;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .nav-item {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px 16px;
      color: var(--text-primary);
      text-decoration: none;
      border-radius: 8px;
      transition: background-color 0.2s;
      font-size: 14px;
      font-weight: 400;
    }

    .nav-item:hover {
      background: var(--hover-bg);
    }

    .nav-item.active {
      background: var(--active-bg);
      color: var(--accent-color);
      font-weight: 500;
    }

    .divider {
      height: 1px;
      background: var(--border-color);
      margin: 12px 0;
    }

    @media (max-width: 1024px) {
      .sidebar {
        width: 72px;
      }

      .nav-item span {
        display: none;
      }

      .section-title {
        display: none;
      }

      .nav-item {
        justify-content: center;
        padding: 16px 8px;
      }
    }

    @media (max-width: 768px) {
      .sidebar {
        display: none;
      }
    }
  `]
})
export class SidebarComponent {
  authService = inject(AuthService);
  
  HomeIcon = Home;
  PlayIcon = Play;
  UsersIcon = Users;
  LibraryIcon = Library;
  TrendingIcon = TrendingUp;
  HistoryIcon = History;
  ClockIcon = Clock;
  ThumbsUpIcon = ThumbsUp;
}