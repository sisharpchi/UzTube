import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { TopNavComponent } from '../top-nav/top-nav.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent, TopNavComponent],
  template: `
    <div class="app-layout">
      <app-top-nav></app-top-nav>
      <div class="main-content">
        <app-sidebar></app-sidebar>
        <main class="content-area">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `,
  styles: [`
    .app-layout {
      display: flex;
      flex-direction: column;
      height: 100vh;
      background: var(--bg-primary);
    }

    .main-content {
      display: flex;
      flex: 1;
      overflow: hidden;
    }

    .content-area {
      flex: 1;
      overflow-y: auto;
      padding: 24px;
      background: var(--bg-primary);
    }

    @media (max-width: 768px) {
      .content-area {
        padding: 16px;
      }
    }
  `]
})
export class MainLayoutComponent {}