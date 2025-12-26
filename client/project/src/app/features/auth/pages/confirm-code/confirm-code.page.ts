import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-confirm-code',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="auth-container">
      <div class="auth-card">
        <div class="auth-header">
          <h1>Verify Your Email</h1>
          <p>Enter the verification code sent to {{email}}</p>
        </div>

        <form [formGroup]="codeForm" (ngSubmit)="onSubmit()" class="auth-form">
          <div class="form-group">
            <label for="code">Verification Code</label>
            <input
              id="code"
              type="text"
              formControlName="code"
              class="form-input code-input"
              [class.error]="codeForm.get('code')?.invalid && codeForm.get('code')?.touched"
              placeholder="Enter 6-digit code"
              maxlength="6"
            >
            @if (codeForm.get('code')?.invalid && codeForm.get('code')?.touched) {
              <span class="error-message">Please enter a valid 6-digit code</span>
            }
          </div>

          @if (errorMessage()) {
            <div class="error-alert">
              {{errorMessage()}}
            </div>
          }

          @if (successMessage()) {
            <div class="success-alert">
              {{successMessage()}}
            </div>
          }

          <button
            type="submit"
            class="submit-btn"
            [disabled]="codeForm.invalid || loading()"
          >
            @if (loading()) {
              <span class="loading-spinner"></span>
              Verifying...
            } @else {
              Verify Code
            }
          </button>

          <button
            type="button"
            class="resend-btn"
            (click)="resendCode()"
            [disabled]="resendLoading() || resendCooldown() > 0"
          >
            @if (resendLoading()) {
              <span class="loading-spinner"></span>
              Sending...
            } @else if (resendCooldown() > 0) {
              Resend in {{resendCooldown()}}s
            } @else {
              Resend Code
            }
          </button>
        </form>

        <div class="auth-footer">
          <p><a routerLink="/auth/login">Back to Sign In</a></p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .auth-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: var(--bg-primary);
      padding: 20px;
    }

    .auth-card {
      background: var(--bg-secondary);
      border-radius: 16px;
      padding: 40px;
      width: 100%;
      max-width: 400px;
      box-shadow: var(--shadow-lg);
    }

    .auth-header {
      text-align: center;
      margin-bottom: 32px;
    }

    .auth-header h1 {
      font-size: 28px;
      font-weight: 700;
      color: var(--text-primary);
      margin: 0 0 8px 0;
    }

    .auth-header p {
      color: var(--text-secondary);
      margin: 0;
      word-break: break-all;
    }

    .auth-form {
      display: flex;
      flex-direction: column;
      gap: 20px;
    }

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .form-group label {
      font-weight: 500;
      color: var(--text-primary);
      font-size: 14px;
    }

    .form-input {
      padding: 12px 16px;
      border: 2px solid var(--border-color);
      border-radius: 8px;
      background: var(--bg-primary);
      color: var(--text-primary);
      font-size: 16px;
      transition: border-color 0.2s;
    }

    .code-input {
      text-align: center;
      font-size: 24px;
      font-weight: 600;
      letter-spacing: 4px;
    }

    .form-input:focus {
      outline: none;
      border-color: var(--accent-color);
    }

    .form-input.error {
      border-color: #dc3545;
    }

    .error-message {
      color: #dc3545;
      font-size: 12px;
    }

    .error-alert {
      background: #dc3545;
      color: white;
      padding: 12px;
      border-radius: 8px;
      font-size: 14px;
      text-align: center;
    }

    .success-alert {
      background: #28a745;
      color: white;
      padding: 12px;
      border-radius: 8px;
      font-size: 14px;
      text-align: center;
    }

    .submit-btn {
      background: var(--accent-color);
      color: white;
      border: none;
      padding: 14px;
      border-radius: 8px;
      font-size: 16px;
      font-weight: 600;
      cursor: pointer;
      transition: background-color 0.2s;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
    }

    .submit-btn:hover:not(:disabled) {
      background: var(--accent-hover);
    }

    .submit-btn:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }

    .resend-btn {
      background: transparent;
      color: var(--accent-color);
      border: 2px solid var(--accent-color);
      padding: 12px;
      border-radius: 8px;
      font-size: 14px;
      font-weight: 500;
      cursor: pointer;
      transition: all 0.2s;
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
    }

    .resend-btn:hover:not(:disabled) {
      background: var(--accent-color);
      color: white;
    }

    .resend-btn:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }

    .loading-spinner {
      width: 16px;
      height: 16px;
      border: 2px solid transparent;
      border-top: 2px solid currentColor;
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }

    .auth-footer {
      text-align: center;
      margin-top: 24px;
      padding-top: 24px;
      border-top: 1px solid var(--border-color);
    }

    .auth-footer p {
      color: var(--text-secondary);
      margin: 0;
    }

    .auth-footer a {
      color: var(--accent-color);
      text-decoration: none;
      font-weight: 500;
    }

    .auth-footer a:hover {
      text-decoration: underline;
    }

    @keyframes spin {
      to {
        transform: rotate(360deg);
      }
    }

    @media (max-width: 480px) {
      .auth-card {
        padding: 24px;
      }
    }
  `]
})
export class ConfirmCodePage implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  
  loading = signal(false);
  resendLoading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  resendCooldown = signal(0);
  email = '';

  codeForm: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]]
  });

  ngOnInit(): void {
    this.email = this.route.snapshot.queryParams['email'] || '';
    if (!this.email) {
      this.router.navigate(['/auth/register']);
    }
  }

  onSubmit(): void {
    if (this.codeForm.valid) {
      this.loading.set(true);
      this.errorMessage.set('');

      const code = this.codeForm.value.code;

      this.authService.confirmCode(code, this.email).subscribe({
        next: () => {
          this.loading.set(false);
          this.successMessage.set('Email verified successfully! Redirecting to login...');
          
          setTimeout(() => {
            this.router.navigate(['/auth/login']);
          }, 2000);
        },
        error: (error) => {
          this.loading.set(false);
          this.errorMessage.set(error.error?.message || 'Invalid verification code. Please try again.');
        }
      });
    }
  }

  resendCode(): void {
    if (this.resendCooldown() > 0) return;

    this.resendLoading.set(true);
    this.errorMessage.set('');

    this.authService.sendCode(this.email).subscribe({
      next: () => {
        this.resendLoading.set(false);
        this.successMessage.set('New verification code sent to your email.');
        
        // Start cooldown
        this.resendCooldown.set(60);
        const interval = setInterval(() => {
          const current = this.resendCooldown();
          if (current <= 1) {
            clearInterval(interval);
            this.resendCooldown.set(0);
          } else {
            this.resendCooldown.set(current - 1);
          }
        }, 1000);
      },
      error: (error) => {
        this.resendLoading.set(false);
        this.errorMessage.set(error.error?.message || 'Failed to resend code. Please try again.');
      }
    });
  }
}