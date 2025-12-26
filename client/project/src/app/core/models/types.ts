
export interface User {
  id: number;
  fullName: string;
  email: string;
  profileImageUrl?: string;
  createdAt: string;

  roleId: number;
  roleName: string;
}

export interface UserChangePasswordDto {
  oldPassword: string;
  newPassword: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

export interface Channel {
  id: number;
  name: string;
  description?: string;

  ownerId: number;
  ownerUsername: string; // yoki alohida OwnerDto bo'lishi mumkin

  videoCount: number;
  subscriberCount: number;
  playlistCount: number;
  avatarUrl?: string;
  bannerUrl?: string;
}

export interface ChannelWithVideosDto {
  id: number;
  name: string;
  description?: string;
  avatarUrl?: string;
  bannerUrl?: string;
  avatarCloudPublicId?: string;
  bannerCloudPublicId?: string;
  videos: Video[];
}

export interface Video {
  id: number;
  title: string;
  description?: string;

  videoUrl: string;
  thumbnailUrl?: string;
  duration: string;
  uploadedAt: string;

  channelId: number;
  channel?: Channel;

  playlistId?: number;
  playlistName?: string;

  likeCount: number;
  dislikeCount: number;
  viewCount: number;
  isSubscribed: boolean;
  tags: TagDto[];
}

export interface TagDto {
  id: number;
  name: string;
  videoCount: number;
}
export interface Comment {
  id: number;
  text: string;
  createdAt: string;

  videoId: number;
  userId: number;
  username: string;

  parentCommentId?: number;

  likeCount: number;
  isLikedByCurrentUser: boolean;

  replies: Comment[];
}
export interface LikeDislikeStats {
  videoId: number;
  likeCount: number;
  dislikeCount: number;
  userReaction?: 'like' | 'dislike';
}

export interface Subscription {
  id: string;
  channelDto?: Channel;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface VideoUploadData {
  title: string;
  description: string;
  thumbnail: File;
  video: File;
  tags: string[];
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}