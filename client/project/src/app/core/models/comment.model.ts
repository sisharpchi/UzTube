export interface CommentDto {
  id: number;
  text: string;
  createdAt: string;
  videoId: number;
  userId: number;
  username: string;
  parentCommentId?: number;
  likeCount: number;
  isLikedByCurrentUser: boolean;
  replies: CommentDto[];
}

export interface CommentCreateDto {
  text: string;
  videoId: number;
  parentCommentId?: number;
}