import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../../core/services/api.service';
import { User, UserChangePasswordDto } from '../../../../core/models/types';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="min-screen dark:bg-gray-900 py-8 px-4">
      <div class="max-w-8xl mx-auto">
        <!-- Header 
        <div class="mb-8">
          <h1 class="text-3xl font-bold dark:text-white mb-2">Profile Settings</h1>
          <p class="dark:text-gray-400">Manage your account information and preferences</p>
        </div>-->

        @if (loading()) {
          <div class="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-8">
            <div class="flex items-center justify-center">
              <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-red-500"></div>
              <span class="ml-3 text-gray-600 dark:text-gray-400">Loading your profile...</span>
            </div>
          </div>
        } @else if (user()) {
          <div class="rounded-2xl shadow-sm dark:border-gray-700 overflow-hidden">
            <!-- Profile Header -->
            <div class="bg-gradient-to-r from-red-500 to-red-600 px-8 py-12 relative">
              <div class="absolute inset-0 bg-black opacity-10"></div>
              <div class="relative flex flex-col sm:flex-row items-center sm:items-start gap-6">
                <div class="relative group">
                  <div class="w-24 h-24 sm:w-28 sm:h-28 bg-white bg-opacity-20 backdrop-blur-sm rounded-full flex items-center justify-center text-white text-2xl sm:text-3xl font-bold shadow-lg ring-4 ring-white ring-opacity-30 transition-transform duration-300 group-hover:scale-105">
                    {{ user()!.fullName.charAt(0) }}
                  </div>
                  <div class="absolute -bottom-1 -right-1 w-8 h-8 bg-green-500 rounded-full border-4 border-white shadow-sm"></div>
                </div>
                <div class="text-center sm:text-left text-white">
                  <h2 class="text-2xl sm:text-3xl font-bold mb-2">{{ user()!.fullName }}</h2>
                  <p class="text-red-100 text-lg">{{ user()!.email }}</p>
                  <div class="mt-3 inline-flex items-center px-3 py-1 rounded-full text-sm bg-white bg-opacity-20 backdrop-blur-sm">
                    <div class="w-2 h-2 bg-green-400 rounded-full mr-2"></div>
                    Active Member
                  </div>
                </div>
              </div>
            </div>

            <!-- Profile Form -->
            <div class="p-8">
              <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
                <!-- Personal Information -->
                <div class="space-y-6">
                  <div>
                    <h3 class="text-lg font-semibold dark:text-white mb-4 flex items-center">
                      <div class="w-2 h-2 bg-red-500 rounded-full mr-3"></div>
                      Personal Information
                    </h3>
                    
                    <div class="space-y-4">
                      <div class="group">
                        <label class="block text-sm font-medium dark:text-gray-300 mb-2">
                          Full Name
                        </label>
                        <div class="relative">
                          <input 
                            type="text" 
                            [(ngModel)]="editedUser.fullName"
                            [disabled]="!isEditing()"
                            class="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-500 dark:placeholder-gray-400 focus:ring-2 focus:ring-red-500 focus:border-transparent transition-all duration-200 disabled:bg-gray-100 dark:disabled:bg-gray-800 disabled:text-gray-500 dark:disabled:text-gray-400"
                            [class.ring-2]="isEditing()"
                            [class.ring-red-100]="isEditing()"
                          />
                          @if (isEditing()) {
                            <div class="absolute inset-y-0 right-0 flex items-center pr-3">
                              <div class="w-2 h-2 bg-red-500 rounded-full animate-pulse"></div>
                            </div>
                          }
                        </div>
                      </div>

                      <div class="group">
                        <label class="block text-sm font-medium dark:text-gray-300 mb-2">
                          Email Address
                        </label>
                        <div class="relative">
                          <input 
                            type="email" 
                            [value]="user()!.email"
                            disabled
                            class="w-full px-4 py-3 border border-gray-200 dark:border-gray-700 rounded-xl bg-gray-50 dark:bg-gray-800 text-gray-500 dark:text-gray-400 cursor-not-allowed"
                          />
                          <div class="absolute inset-y-0 right-0 flex items-center pr-3">
                            <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path>
                            </svg>
                          </div>
                        </div>
                        <p class="mt-2 text-xs text-gray-500 dark:text-gray-400 flex items-center">
                          <svg class="w-3 h-3 mr-1" fill="currentColor" viewBox="0 0 20 20">
                            <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"></path>
                          </svg>
                          Email cannot be changed for security reasons
                        </p>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Account Stats -->
                <div class="space-y-6">
                  <div>
                    <h3 class="text-lg font-semibold dark:text-white mb-4 flex items-center">
                      <div class="w-2 h-2 bg-blue-500 rounded-full mr-3"></div>
                      Account Overview
                    </h3>
                    
                    <div class="grid grid-cols-2 gap-4">
                      <div class="bg-gradient-to-br from-blue-50 to-blue-100 dark:from-blue-900/20 dark:to-blue-800/20 p-4 rounded-xl border border-blue-200 dark:border-blue-800">
                        <div class="text-2xl font-bold text-blue-600 dark:text-blue-400">
                          {{ likeCount() }}
                        </div>
                        <div class="text-sm text-blue-700 dark:text-blue-300">Likes Given</div>
                      </div>
                      <div class="bg-gradient-to-br from-green-50 to-green-100 dark:from-green-900/20 dark:to-green-800/20 p-4 rounded-xl border border-green-200 dark:border-green-800">
                        <div class="text-2xl font-bold text-green-600 dark:text-green-400">
                          {{ subscriberCount() }}
                        </div>
                        <div class="text-sm text-green-700 dark:text-green-300">Subscriptions</div>
                      </div>
                      <div class="bg-gradient-to-br from-purple-50 to-purple-100 dark:from-purple-900/20 dark:to-purple-800/20 p-4 rounded-xl border border-purple-200 dark:border-purple-800">
                        <div class="text-2xl font-bold text-purple-600 dark:text-purple-400">
                          {{ dislikeCount() }}
                        </div>
                        <div class="text-sm text-purple-700 dark:text-purple-300">Dislikes Given</div>
                      </div>
                      <div class="bg-gradient-to-br from-orange-50 to-orange-100 dark:from-orange-900/20 dark:to-orange-800/20 p-4 rounded-xl border border-orange-200 dark:border-orange-800">
                        <div class="text-2xl font-bold text-orange-600 dark:text-orange-400">
                          {{ commentCount() }}
                        </div>
                        <div class="text-sm text-orange-700 dark:text-orange-300">Comments</div>
                      </div>
                    </div>
                  </div>

                  <div class="bg-gray-50 dark:bg-gray-700/50 rounded-xl p-6 border border-gray-200 dark:border-gray-600">
                    <h4 class="font-medium text-gray-900 dark:text-white mb-3">Quick Actions</h4>
                    <div class="space-y-3">
                      <button
                        (click)="openChangePasswordModal()"
                        class="w-full text-left px-4 py-3 rounded-lg bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors duration-200 group"
                      >
                        <div class="flex items-center">
                          <div
                            class="w-8 h-8 bg-blue-100 dark:bg-blue-900/30 rounded-lg flex items-center justify-center mr-3 group-hover:bg-blue-200 dark:group-hover:bg-blue-800/50 transition-colors duration-200"
                          >
                            <svg class="w-4 h-4 text-blue-600 dark:text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                              <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                stroke-width="2"
                                d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"
                              ></path>
                            </svg>
                          </div>
                          <div>
                            <div class="font-medium text-gray-900 dark:text-white">Change Password</div>
                            <div class="text-sm text-gray-500 dark:text-gray-400">Update your account security</div>
                          </div>
                        </div>
                      </button>
                      <button class="w-full text-left px-4 py-3 rounded-lg bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors duration-200 group">
                        <div class="flex items-center">
                          <div class="w-8 h-8 bg-green-100 dark:bg-green-900/30 rounded-lg flex items-center justify-center mr-3 group-hover:bg-green-200 dark:group-hover:bg-green-800/50 transition-colors duration-200">
                            <svg class="w-4 h-4 text-green-600 dark:text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                            </svg>
                          </div>
                          <div>
                            <div class="font-medium text-gray-900 dark:text-white">Privacy Settings</div>
                            <div class="text-sm text-gray-500 dark:text-gray-400">Manage your privacy preferences</div>
                          </div>
                        </div>
                      </button>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Action Buttons -->
              <div class="mt-8 pt-6 border-t border-gray-200 dark:border-gray-700">
                <div class="flex flex-col sm:flex-row gap-3 sm:justify-end">
                  @if (!isEditing()) {
                    <button 
                      class="inline-flex items-center justify-center px-6 py-3 bg-red-600 hover:bg-red-700 text-white font-medium rounded-xl shadow-sm hover:shadow-md transition-all duration-200 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
                      (click)="startEditing()"
                    >
                      <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
                      </svg>
                      Edit Profile
                    </button>
                  } @else {
                    <button 
                      class="inline-flex items-center justify-center px-6 py-3 bg-gray-200 hover:bg-gray-300 dark:bg-gray-700 dark:hover:bg-gray-600 text-gray-700 dark:text-gray-300 font-medium rounded-xl shadow-sm hover:shadow-md transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800"
                      (click)="cancelEditing()"
                    >
                      <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                      </svg>
                      Cancel
                    </button>
                    <button 
                      class="inline-flex items-center justify-center px-6 py-3 bg-green-600 hover:bg-green-700 disabled:bg-green-400 text-white font-medium rounded-xl shadow-sm hover:shadow-md transition-all duration-200 transform hover:scale-105 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 dark:focus:ring-offset-gray-800 disabled:cursor-not-allowed disabled:transform-none"
                      (click)="saveProfile()" 
                      [disabled]="saving()"
                    >
                      @if (saving()) {
                        <div class="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                        Saving...
                      } @else {
                        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                        </svg>
                        Save Changes
                      }
                    </button>
                  }
                </div>
              </div>

              <!-- Messages -->
              @if (errorMessage()) {
                <div class="mt-6 p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-xl">
                  <div class="flex items-center">
                    <svg class="w-5 h-5 text-red-500 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                    </svg>
                    <span class="text-red-700 dark:text-red-300 font-medium">{{ errorMessage() }}</span>
                  </div>
                </div>
              }

              @if (successMessage()) {
                <div class="mt-6 p-4 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 rounded-xl">
                  <div class="flex items-center">
                    <svg class="w-5 h-5 text-green-500 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                    </svg>
                    <span class="text-green-700 dark:text-green-300 font-medium">{{ successMessage() }}</span>
                  </div>
                </div>
              }
            </div>
          </div>
        } @else {
          <div class="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-8">
            <div class="text-center">
              <svg class="w-16 h-16 text-gray-400 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
              <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Failed to load profile</h3>
              <p class="text-gray-500 dark:text-gray-400">Please try refreshing the page or contact support if the problem persists.</p>
            </div>
          </div>
        }
      </div>
    </div>
    <!-- Change Password Modal -->
<div *ngIf="showPasswordModal()" class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
  <div class="bg-gray-900 dark:bg-gray-800 rounded-xl p-6 w-full max-w-md shadow-lg relative">
    <h2 class="text-xl font-bold mb-4 dark:text-white">Change Password</h2>

    <div class="space-y-4">
      <div>
        <label class="block text-sm font-medium dark:text-gray-300">Old Password</label>
        <input
          [(ngModel)]="passwordDto.oldPassword"
          type="password"
          class="text-gray-900 w-full px-4 py-2 mt-1 border rounded-md dark:bg-gray-700 dark:text-white dark:border-gray-600"
        placeholder="Enter old password" />
      </div>
      <div>
        <label class="block text-sm font-medium dark:text-gray-300">New Password</label>
        <input
          [(ngModel)]="passwordDto.newPassword"
          type="password"
          class="text-gray-900 w-full px-4 py-2 mt-1 border rounded-md dark:bg-gray-700 dark:text-white dark:border-gray-600"
        placeholder="Enter new password"
      />
      </div>

      <div *ngIf="passwordError()" class="text-red-600 dark:text-red-400 text-sm">
        {{ passwordError() }}
      </div>
      <div *ngIf="passwordSuccess()" class="text-green-600 dark:text-green-400 text-sm">
        {{ passwordSuccess() }}
      </div>
    </div>

    <div class="mt-6 flex justify-end space-x-3">
      <button
        (click)="closeChangePasswordModal()"
        class="px-4 py-2 bg-gray-200 hover:bg-gray-300 dark:bg-gray-700 dark:hover:bg-gray-600 text-gray-800 dark:text-white rounded-md"
      >
        Cancel
      </button>
      <button
        (click)="submitChangePassword()"
        [disabled]="passwordSaving()"
        class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md disabled:opacity-50 flex items-center"
      >
        <span *ngIf="passwordSaving()" class="animate-spin h-4 w-4 border-b-2 border-white rounded-full mr-2"></span>
        <span *ngIf="!passwordSaving()">Change</span>
        <span *ngIf="passwordSaving()">Saving...</span>
      </button>
    </div>
  </div>
</div>

  `,
  styles: [`
    /* Custom animations and additional styles */
    @keyframes fadeIn {
      from { opacity: 0; transform: translateY(10px); }
      to { opacity: 1; transform: translateY(0); }
    }
    
    .fade-in {
      animation: fadeIn 0.3s ease-out;
    }
    
    /* Smooth focus transitions */
    input:focus {
      transform: translateY(-1px);
    }
    
    /* Custom scrollbar for better UX */
    ::-webkit-scrollbar {
      width: 6px;
    }
    
    ::-webkit-scrollbar-track {
      background: transparent;
    }
    
    ::-webkit-scrollbar-thumb {
      background: rgba(156, 163, 175, 0.5);
      border-radius: 3px;
    }
    
    ::-webkit-scrollbar-thumb:hover {
      background: rgba(156, 163, 175, 0.7);
    }

    input {
      border: none !important;
      border-bottom: 2px solid #e5e7eb !important;
    }
  `]
})
export class ProfilePage implements OnInit {
  user = signal<User | null>(null);
  loading = signal(true);
  isEditing = signal(false);
  saving = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  subscriberCount = signal<number>(0);
  commentCount = signal<number>(0);
  likeCount = signal<number>(0);
  dislikeCount = signal<number>(0);

  editedUser: Partial<User> = {};
  showPasswordModal = signal(false);
  passwordDto: UserChangePasswordDto = { oldPassword: '', newPassword: '' };
  passwordSaving = signal(false);
  passwordError = signal('');
  passwordSuccess = signal('');

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadProfile();
    this.loadSubscriberCount();
    this.loadCommentCount();
    this.loadLikeCount();
    this.loadDislikeCount();
  }

  loadProfile(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.api.getProfile().subscribe({
      next: (res) => {
        this.user.set(res.data);
        this.resetEditedUser();
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.errorMessage.set('Failed to load profile');
        this.loading.set(false);
      }
    });
  }

  loadSubscriberCount(): void {
    this.api.getMyChannelSubscribersCount().subscribe({
      next: (res) => this.subscriberCount.set(res.data),
      error: (err) => console.error('Failed to load subscriber count:', err)
    });
  }

  loadCommentCount(): void {
    this.api.getChannelCommentCount().subscribe({
      next: (res) => this.commentCount.set(res.data),
      error: (err) => console.error('Failed to load comments:', err)
    });
  }

  loadLikeCount(): void {
    this.api.getChannelLikesCount().subscribe({
      next: (res) => this.likeCount.set(res.data),
      error: (err) => console.error('Failed to load likes:', err)
    });
  }

  loadDislikeCount(): void {
    this.api.getChannelDislikesCount().subscribe({
      next: (res) => this.dislikeCount.set(res.data),
      error: (err) => console.error('Failed to load dislikes:', err)
    });
  }

  startEditing(): void {
    this.isEditing.set(true);
    this.resetEditedUser();
    this.clearMessages();
  }

  cancelEditing(): void {
    this.isEditing.set(false);
    this.resetEditedUser();
    this.clearMessages();
  }

  saveProfile(): void {
    const fullName = this.editedUser.fullName?.trim();
    if (!fullName) {
      this.errorMessage.set('Full name is required');
      return;
    }

    this.saving.set(true);
    this.clearMessages();

    this.api.editProfile(fullName).subscribe({
      next: () => {
        this.user.update(current => ({ ...current!, fullName }));
        this.isEditing.set(false);
        this.saving.set(false);
        this.successMessage.set('Profile updated successfully!');
        setTimeout(() => this.successMessage.set(''), 3000);
      },
      error: (err) => {
        console.error('Failed to update profile:', err);
        this.errorMessage.set('Failed to update profile');
        this.saving.set(false);
      }
    });
  }

  openChangePasswordModal(): void {
    this.showPasswordModal.set(true);
    this.passwordDto = { oldPassword: '', newPassword: '' };
    this.passwordError.set('');
    this.passwordSuccess.set('');
  }

  closeChangePasswordModal(): void {
    this.showPasswordModal.set(false);
  }

  submitChangePassword(): void {
    if (!this.passwordDto.oldPassword || !this.passwordDto.newPassword) {
      this.passwordError.set('All fields are required');
      return;
    }

    this.passwordSaving.set(true);
    this.passwordError.set('');
    this.passwordSuccess.set('');

    this.api.changePassword(this.passwordDto).subscribe({
      next: () => {
        this.passwordSaving.set(false);
        this.passwordSuccess.set('Password updated successfully');
        setTimeout(() => this.closeChangePasswordModal(), 1500);
      },
      error: (err) => {
        this.passwordSaving.set(false);
        this.passwordError.set(err?.error?.message || 'Failed to change password');
      }
    });
  }

  private resetEditedUser(): void {
    const currentUser = this.user();
    if (currentUser) {
      this.editedUser = { fullName: currentUser.fullName };
    }
  }

  private clearMessages(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
  }
}
