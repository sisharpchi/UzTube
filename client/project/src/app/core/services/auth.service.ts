import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { UserDto, UserLoginDto, UserRegisterDto, UserLoginResponseDto } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = 'https://localhost:7299/api';
  private readonly TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  
  currentUser = signal<UserDto | null>(null);
  isAuthenticated = signal<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.checkAuthStatus();
  }

  login(credentials: UserLoginDto): Observable<UserLoginResponseDto> {
    return this.http.post<UserLoginResponseDto>(`${this.API_URL}/auth/login`, credentials)
      .pipe(
        tap(response => {
          this.setTokens(response.accessToken, response.refreshToken);
          this.isAuthenticated.set(true);
          this.router.navigate(['/']);
        })
      );
  }

  register(userData: UserRegisterDto): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/sign-up`, userData);
  }

  sendCode(email: string): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/send-code?email=${email}`, {});
  }

  confirmCode(code: string, email: string): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/confirm-code?code=${code}&email=${email}`, {});
  }

  logout(): void {
    const refreshToken = localStorage.getItem(this.REFRESH_TOKEN_KEY);
    if (refreshToken) {
      this.http.delete(`${this.API_URL}/auth/log-out?refreshToken=${refreshToken}`).subscribe();
    }
    
    this.clearTokens();
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  private setTokens(accessToken: string, refreshToken?: string): void {
    localStorage.setItem(this.TOKEN_KEY, accessToken);
    if (refreshToken) {
      localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    }
  }

  private clearTokens(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  private checkAuthStatus(): void {
    const token = this.getToken();
    this.isAuthenticated.set(!!token);
  }
}