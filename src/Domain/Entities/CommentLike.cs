﻿namespace Domain.Entities;

public class CommentLike
{
    public long Id { get; set; }
    public long CommentId { get; set; }
    public Comment Comment { get; set; }

    public long UserId { get; set; }
}
