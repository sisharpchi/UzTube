export interface UserDto {
  id: number;
  fullName: string;
  email: string;
  profileImageUrl?: string;
  createdAt: string;
  roleId: number;
  roleName: string;
}

export interface UserRegisterDto {
  fullName: string;
  email: string;
  password: string;
}

export interface UserLoginDto {
  email: string;
  password: string;
}

export interface UserLoginResponseDto {
  accessToken: string;
  refreshToken?: string;
  tokenType: string;
  expires: number;
}

export interface UserUpdateDto {
  fullName: string;
  profileImageUrl?: string;
}

export interface UserWithChannelDto {
  id: number;
  fullName: string;
  email: string;
  profileImageUrl?: string;
  channel: ChannelDto;
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
}