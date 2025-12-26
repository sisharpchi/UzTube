import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VideoDto, VideoListItemDto, VideoUploadDto } from '../models/video.model';

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  private readonly API_URL = 'https://localhost:7299/api';

  constructor(private http: HttpClient) {}

  getAllVideos(): Observable<VideoListItemDto[]> {
    return this.http.get<VideoListItemDto[]>(`${this.API_URL}/video/all`);
  }

  getVideoById(id: number): Observable<VideoDto> {
    return this.http.get<VideoDto>(`${this.API_URL}/video/${id}`);
  }

  getTrendingVideos(count: number = 20): Observable<VideoListItemDto[]> {
    return this.http.get<VideoListItemDto[]>(`${this.API_URL}/video/trending?count=${count}`);
  }

  uploadVideo(videoData: VideoUploadDto, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('videoUploadDto', JSON.stringify(videoData));
    
    return this.http.post(`${this.API_URL}/video`, formData);
  }

  incrementView(videoId: number): Observable<any> {
    return this.http.post(`${this.API_URL}/video/${videoId}/view`, {});
  }

  getChannelVideos(channelId: number): Observable<VideoListItemDto[]> {
    return this.http.get<VideoListItemDto[]>(`${this.API_URL}/video/channel/${channelId}`);
  }
}