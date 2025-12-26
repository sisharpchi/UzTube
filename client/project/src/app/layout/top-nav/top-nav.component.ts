import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LucideAngularModule, Search, Upload, Bell, Menu, Sun, Moon, User } from 'lucide-angular';
import { ThemeService } from '../../core/services/theme.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-top-nav',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, LucideAngularModule],
  template: `
    <nav class="top-nav">
      <div class="nav-left">
        <button class="menu-btn" type="button">
          <lucide-angular [img]="MenuIcon" size="24"></lucide-angular>
        </button>
        <a routerLink="/" class="logo">
          <span class="logo-text">UzTube</span>
        </a>
      </div>

      <div class="nav-center">
        <div class="search-container">
          <input 
            type="text" 
            placeholder="Search" 
            class="search-input"
            [(ngModel)]="searchQuery"
            (keyup.enter)="onSearch()"
          >
          <button class="search-btn" type="button" (click)="onSearch()">
            <lucide-angular [img]="SearchIcon" size="20"></lucide-angular>
          </button>
        </div>
      </div>

      <div class="nav-right">
        <button class="icon-btn theme-btn" type="button" (click)="toggleTheme()">
          <lucide-angular [img]="themeService.theme() === 'light' ? MoonIcon : SunIcon" size="20"></lucide-angular>
        </button>
        
        @if (authService.isAuthenticated()) {
          <a routerLink="/notifications" class="icon-btn">
            <lucide-angular [img]="BellIcon" size="20"></lucide-angular>
          </a>
          
          <div class="profile-dropdown">
            <button class="profile-btn" type="button">
              <lucide-angular [img]="UserIcon" size="20"></lucide-angular>
            </button>
            <div class="dropdown-menu">
              <a routerLink="/profile" class="dropdown-item">Profile</a>
              <a routerLink="/channel/my" class="dropdown-item">My Channel</a>
              <button class="dropdown-item" type="button" (click)="logout()">Sign Out</button>
            </div>
          </div>
        } @else {
          <a routerLink="/auth/login" class="login-btn">Sign In</a>
        }
      </div>
    </nav>
  `,
  styles: [`
    .top-nav {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0 16px;
      height: 64px;
      position: sticky;
      top: 0;
      z-index: 100;
    }

    .nav-left {
      display: flex;
      align-items: center;
      gap: 16px;
    }

    .menu-btn {
      background: none;
      border: none;
      color: var(--text-primary);
      cursor: pointer;
      padding: 8px;
      border-radius: 8px;
      transition: background-color 0.2s;
    }

    .menu-btn:hover {
      background: var(--hover-bg);
    }

    .logo {
      text-decoration: none;
      color: var(--text-primary);
    }

    .logo-text {
      font-size: 20px;
      font-weight: bold;
      color: var(--accent-color);
    }

    .nav-center {
      flex: 1;
      max-width: 600px;
      margin: 0 40px;
    }

    .search-container {
      display: flex;
      align-items: center;
      position: relative;
    }

    .search-input {
      flex: 1;
      padding: 10px 16px;
      border: 1px solid var(--border-color);
      border-radius: 24px 0 0 24px;
      background: var(--bg-primary);
      color: var(--text-primary);
      font-size: 16px;
      outline: none;
    }

    .search-input:focus {
      border-color: var(--accent-color);
    }

    .search-btn {
      padding: 12px 20px;
      border: 1px solid var(--border-color);
      border-left: none;
      border-radius: 0 24px 24px 0;
      background: var(--bg-tertiary);
      color: var(--text-secondary);
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .search-btn:hover {
      background: var(--hover-bg);
    }

    .nav-right {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .icon-btn {
      background: none;
      border: none;
      color: var(--text-primary);
      cursor: pointer;
      padding: 8px;
      border-radius: 8px;
      transition: background-color 0.2s;
      text-decoration: none;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .icon-btn:hover {
      background: var(--hover-bg);
    }

    .theme-btn {
      background: var(--bg-tertiary);
    }

    .upload-btn {
      background: var(--accent-color);
      color: white;
    }

    .upload-btn:hover {
      background: var(--accent-hover);
    }

    .login-btn {
      background: var(--accent-color);
      color: white;
      padding: 8px 16px;
      border-radius: 20px;
      text-decoration: none;
      font-weight: 500;
      transition: background-color 0.2s;
    }

    .login-btn:hover {
      background: var(--accent-hover);
    }

    .profile-dropdown {
      position: relative;
    }

    .profile-btn {
      background: var(--bg-tertiary);
      border: none;
      color: var(--text-primary);
      cursor: pointer;
      padding: 8px;
      border-radius: 50%;
      transition: background-color 0.2s;
    }

    .profile-btn:hover {
      background: var(--hover-bg);
    }

    .dropdown-menu {
      position: absolute;
      top: 100%;
      right: 0;
      background: var(--bg-secondary);
      border: 1px solid var(--border-color);
      border-radius: 8px;
      box-shadow: var(--shadow-lg);
      padding: 8px 0;
      min-width: 200px;
      z-index: 1000;
      opacity: 0;
      visibility: hidden;
      transform: translateY(-8px);
      transition: all 0.2s;
    }

    .profile-dropdown:hover .dropdown-menu {
      opacity: 1;
      visibility: visible;
      transform: translateY(0);
    }

    .dropdown-item {
      display: block;
      width: 100%;
      padding: 12px 16px;
      color: var(--text-primary);
      text-decoration: none;
      border: none;
      background: none;
      text-align: left;
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .dropdown-item:hover {
      background: var(--hover-bg);
    }

    @media (max-width: 768px) {
      .nav-center {
        display: none;
      }
      
      .nav-right {
        gap: 4px;
      }
    }
  `]
})
export class TopNavComponent {
  themeService = inject(ThemeService);
  authService = inject(AuthService);
  
  SearchIcon = Search;
  UploadIcon = Upload;
  BellIcon = Bell;
  MenuIcon = Menu;
  SunIcon = Sun;
  MoonIcon = Moon;
  UserIcon = User;
  
  searchQuery = '';

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  onSearch(): void {
    if (this.searchQuery.trim()) {
      // Implement search functionality
      console.log('Searching for:', this.searchQuery);
    }
  }

  logout(): void {
    this.authService.logout();
  }
}