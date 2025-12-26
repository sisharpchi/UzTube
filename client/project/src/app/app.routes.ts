import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/video/pages/home/home.page').then(m => m.HomePage)
      },
      {
        path: 'watch/:id',
        loadComponent: () => import('./features/video/pages/video-watch/video-watch.page').then(m => m.VideoWatchPage)
      },
      {
        path: 'upload',
        loadComponent: () => import('./features/video/pages/upload-video/upload-video.page').then(m => m.UploadVideoPage),
        canActivate: [authGuard]
      },
      {
        path: 'channel/my',
        loadComponent: () => import('./features/channel/pages/channel-create/channel-my.page').then(m => m.ChannelMyPage),
        canActivate: [authGuard]
      },
      {
        path: 'channel/:id',
        loadComponent: () => import('./features/channel/pages/channel-detail/channel-detail.page').then(m => m.ChannelDetailPage)
      },
      {
        path: 'profile',
        loadComponent: () => import('./features/user/pages/profile/profile.page').then(m => m.ProfilePage),
        canActivate: [authGuard]
      },
      {
        path: 'subscriptions',
        loadComponent: () => import('./features/subscription/pages/subscriptions/subscriptions.page').then(m => m.SubscriptionsPage),
        canActivate: [authGuard]
      },
      {
        path: 'notifications',
        loadComponent: () => import('./features/notification/pages/notifications/notifications.page').then(m => m.NotificationsPage),
        canActivate: [authGuard]
      }
    ]
  },
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        loadComponent: () => import('./features/auth/pages/login/login.page').then(m => m.LoginPage)
      },
      {
        path: 'register',
        loadComponent: () => import('./features/auth/pages/register/register.page').then(m => m.RegisterPage)
      },
      {
        path: 'confirm-code',
        loadComponent: () => import('./features/auth/pages/confirm-code/confirm-code.page').then(m => m.ConfirmCodePage)
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];