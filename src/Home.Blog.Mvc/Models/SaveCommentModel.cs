namespace Home.Blog.Mvc.Models;

using System;

public class SaveCommentModel
{
    public Guid Id { get; set; }
    public string CommentAuthor { get; set; } = string.Empty;
    public string CommentEmail { get; set; } = string.Empty;
    public string CommentUrl { get; set; } = string.Empty;
    public string CommentBody { get; set; } = string.Empty;
}
