namespace Home.Blog.Mvc.Models;

using System;

public class PostMetaModel
{
    public string CategoryTitle { get; set; }
    public string CategoryUrl   { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int CommentCount { get; set; }
    public string CommentsAnchorUrl { get; set; }
    public bool ShowCategory { get; set; } = true;
}
