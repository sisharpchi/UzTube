  import { Injectable } from '@angular/core';
  import { HttpClient, HttpParams } from '@angular/common/http';
  import { Observable, BehaviorSubject } from 'rxjs';
  import { 
    User, 
    AuthResponse, 
    Channel, 
    Video, 
    Comment, 
    LikeDislikeStats,
    Subscription,
    ApiResponse,
    WeatherForecast,
    ChannelWithVideosDto,
    UserChangePasswordDto
  } from '../models/types';

  @Injectable({
    providedIn: 'root'
  })
  export class ApiService {
    private readonly baseUrl = 'https://localhost:7299'; // Replace with actual API URL
    private loadingSubject = new BehaviorSubject<boolean>(false);
    public loading$ = this.loadingSubject.asObservable();

    constructor(private http: HttpClient) {}

    private setLoading(loading: boolean) {
      this.loadingSubject.next(loading);
    }

    // Authentication
    signUp(email: string, password: string, firstName: string, lastName: string): Observable<ApiResponse<User>> {
      this.setLoading(true);
      return this.http.post<ApiResponse<User>>(`${this.baseUrl}/api/auth/sign-up`, {
        email, password, firstName, lastName
      });
    }

    sendCode(email: string): Observable<ApiResponse<any>> {
      this.setLoading(true);
      return this.http.get<ApiResponse<any>>(`${this.baseUrl}/api/auth/send-code`, {
        params: { email }
      });
    }

    confirmCode(email: string, code: string): Observable<ApiResponse<any>> {
      this.setLoading(true);
      return this.http.post<ApiResponse<any>>(`${this.baseUrl}/api/auth/confirm-code`, null, {
        params: { email, code }
      });
    }

    login(email: string, password: string): Observable<ApiResponse<AuthResponse>> {
      this.setLoading(true);
      return this.http.post<ApiResponse<AuthResponse>>(`${this.baseUrl}/api/auth/login`, {
        email, password
      });
    }

    logout(refreshToken: string): Observable<any> {
      return this.http.delete(`${this.baseUrl}/api/auth/log-out`, {
        params: { refreshToken }
      });
    }

    refreshToken(refreshToken: string): Observable<ApiResponse<AuthResponse>> {
      return this.http.post<ApiResponse<AuthResponse>>(`${this.baseUrl}/api/auth/refresh-token`, {
        refreshToken
      });
    }

    changePassword(dto: UserChangePasswordDto): Observable<ApiResponse<any>> {
      return this.http.put<ApiResponse<any>>(
        `${this.baseUrl}/api/auth/change-password`,
        dto
      );
    }

    getProfile(): Observable<ApiResponse<User>> {
      return this.http.get<ApiResponse<User>>(`${this.baseUrl}/api/auth/profile`);
    }
 
    editProfile(fullName: string): Observable<ApiResponse<any>> {
      const params = new HttpParams().set('fullName', fullName);
      return this.http.put<ApiResponse<any>>(`${this.baseUrl}/api/auth/edit-profile`, null, { params });
    }

    // Channel
    createChannel(name: string, description: string): Observable<ApiResponse<ChannelWithVideosDto>> {
      this.setLoading(true);
      return this.http.post<ApiResponse<ChannelWithVideosDto>>(`${this.baseUrl}/api/channel/create`, {
        name, description
      });
    }

    getMyChannel(): Observable<ApiResponse<ChannelWithVideosDto>> {
      return this.http.get<ApiResponse<ChannelWithVideosDto>>(`${this.baseUrl}/api/channel/my`);
    }

    getMyChannelVideos(): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/channel/my-videos`);
    }

  getMyChannelSubscribersCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(`${this.baseUrl}/api/channel/subscribers-count`);
  }

    updateChannel(channelData: Partial<Channel>): Observable<ApiResponse<Channel>> {
      this.setLoading(true);
      return this.http.put<ApiResponse<Channel>>(`${this.baseUrl}/api/channel/update`, channelData);
    }

    searchChannels(query: string): Observable<ApiResponse<Channel[]>> {
      return this.http.get<ApiResponse<Channel[]>>(`${this.baseUrl}/api/channel/search/${query}`);
    }

    getChannel(channelId: string): Observable<ApiResponse<ChannelWithVideosDto>> {
      return this.http.get<ApiResponse<ChannelWithVideosDto>>(`${this.baseUrl}/api/channel/${channelId}`);
    }

    getChannelVideos(channelId: string): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/by-channel/${channelId}`);
    }

    uploadAvatar(file: File): Observable<ApiResponse<boolean>> {
  const formData = new FormData();
  formData.append('file', file);
  return this.http.post<ApiResponse<boolean>>(
    `${this.baseUrl}/api/channel/upload-avatar`,
    formData
  );
}

uploadBanner(file: File): Observable<ApiResponse<boolean>> {
  const formData = new FormData();
  formData.append('file', file);
  return this.http.post<ApiResponse<boolean>>(
    `${this.baseUrl}/api/channel/upload-banner`,
    formData
  );
}

    // Video
    getAllVideos(): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/all`);
    }

    createVideo(videoData: FormData): Observable<ApiResponse<Video>> {
      this.setLoading(true);
      return this.http.post<ApiResponse<Video>>(`${this.baseUrl}/api/video/create`, videoData);
    }

    getVideo(videoId: string): Observable<ApiResponse<Video>> {
      return this.http.get<ApiResponse<Video>>(`${this.baseUrl}/api/video/${videoId}`);
    }

    recordView(videoId: string): Observable<any> {
      return this.http.post(`${this.baseUrl}/api/video/${videoId}/view`, {});
    }

    getMyVideos(): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/my`);
    }

    updateVideo(videoId: string, videoData: Partial<Video>): Observable<ApiResponse<Video>> {
      this.setLoading(true);
      return this.http.put<ApiResponse<Video>>(`${this.baseUrl}/api/video/${videoId}`, videoData);
    }

    deleteVideo(videoId: string): Observable<any> {
      return this.http.delete(`${this.baseUrl}/api/video/delete/${videoId}`);
    }

    getPlaylistVideos(playlistId: string): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/playlist/${playlistId}`);
    }

    getTrendingVideos(): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/trending`);
    }

    getVideosByTag(tagId: string): Observable<ApiResponse<Video[]>> {
      return this.http.get<ApiResponse<Video[]>>(`${this.baseUrl}/api/video/tag/${tagId}`);
    }

    // Like/Dislike
    toggleLikeDislike(videoId: string, isLike: boolean): Observable<any> {
      return this.http.post(`${this.baseUrl}/api/likedislike/toggle`, {
        videoId,
        isLike // bu endi `true` yoki `false` sifatida yuboriladi
      });
    }

    getLikeDislikeStats(videoId: string): Observable<ApiResponse<LikeDislikeStats>> {
      return this.http.get<ApiResponse<LikeDislikeStats>>(`${this.baseUrl}/api/likedislike/stats/${videoId}`);
    }

    getUserReaction(videoId: number): Observable<ApiResponse<boolean | null>> {
      return this.http.get<ApiResponse<boolean | null>>(
        `${this.baseUrl}/api/likedislike/reactions/${videoId}`
      );
    }

    getChannelLikesCount(): Observable<ApiResponse<number>> {
      return this.http.get<ApiResponse<number>>(`${this.baseUrl}/api/likedislike/likes/count`);
    }

    getChannelDislikesCount(): Observable<ApiResponse<number>> {
      return this.http.get<ApiResponse<number>>(`${this.baseUrl}/api/likedislike/dislikes/count`);
    }


    // Comments
    getComments(videoId: string): Observable<ApiResponse<Comment[]>> {
      return this.http.get<ApiResponse<Comment[]>>(`${this.baseUrl}/api/comment/all/${videoId}`);
    }

    getCommentReplies(commentId: string): Observable<ApiResponse<Comment[]>> {
      return this.http.get<ApiResponse<Comment[]>>(`${this.baseUrl}/api/comment/replies/${commentId}`);
    }

    createComment(text: string, videoId: string, parentCommentId?: number): Observable<ApiResponse<Comment>> {
      return this.http.post<ApiResponse<Comment>>(`${this.baseUrl}/api/comment/create`, {
        text, videoId, parentCommentId
      });
    }

    getChannelCommentCount(): Observable<ApiResponse<number>> {
      return this.http.get<ApiResponse<number>>(
        `${this.baseUrl}/api/comment/count`
      );
    }

    deleteComment(commentId: number): Observable<any> {
      return this.http.delete(`${this.baseUrl}/api/comment/delete/${commentId}`);
    }

    // Subscriptions
    toggleSubscription(channelId: number): Observable<any> {
      return this.http.post(`${this.baseUrl}/api/subscriptions/toggle`, {
        channelId
      });
    }

    isSubscribed(channelId: number): Observable<ApiResponse<{ isSubscribed: boolean }>> {
      return this.http.get<ApiResponse<{ isSubscribed: boolean }>>(`${this.baseUrl}/api/subscriptions/is-subscribed/${channelId}`);
    }

    getMySubscriptions(): Observable<ApiResponse<Subscription[]>> {
      return this.http.get<ApiResponse<Subscription[]>>(`${this.baseUrl}/api/subscriptions/my-subscriptions`);
    }

  getSubscriberCount(channelId: number): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(`${this.baseUrl}/api/subscriptions/subscribers/${channelId}`);
  }

    // Weather Test
    getWeatherForecast(): Observable<WeatherForecast[]> {
      return this.http.get<WeatherForecast[]>(`${this.baseUrl}/WeatherForecast`);
    }
  }