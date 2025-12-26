export interface VideoDto {
  id: number;
  title: string;
  description?: string;
  thumbnailUrl?: string;
  videoUrl: string;
  duration: string;
  viewCount: number;
  uploadedAt: string;
  channelId: number;
  channelName: string;
  channelAvatar?: string;
}
////////////////////////////////////////
export interface VideoListItemDto {
    id: number;
  title: string;
  description?: string | null;

  videoUrl: string;
  thumbnailUrl?: string | null;
  duration: string; // TimeSpan → ISO duration or "hh:mm:ss" format
  uploadedAt: string; // DateTime → ISO string

  channelId: number;
  channel?: ChannelDto | null;

  playlistId?: number | null;
  playlistName?: string | null;

  likeCount: number;
  dislikeCount: number;
  viewCount: number;

  tags: TagDto[];
}

export interface ChannelDto {
  id: number;
  name: string;
  description?: string;

  ownerId: number;
  ownerUsername: string;

  videoCount: number;
  subscriberCount: number;
  playlistCount: number;
  bannerUrl?: string;
  avatarUrl?: string;
}

export interface TagDto {
  id: number;
  name: string;
  videoCount: number;
}
//////////////////////////
export interface VideoUploadDto {
  title: string;
  description?: string;
  thumbnail?: File;
  tagIds?: number[];
}

export interface VideoUpdateDto {
  title: string;
  description?: string;
  thumbnailUrl?: string;
  playlistId?: number;
  tagIds?: number[];
}