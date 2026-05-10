namespace Home.Blog.Mvc.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class SaveCommentModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Please enter your name.")]
    [StringLength(128, ErrorMessage = "Name must be 128 characters or fewer.")]
    public string CommentAuthor { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(128, ErrorMessage = "Email must be 128 characters or fewer.")]
    public string CommentEmail { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL (including https://).")]
    [StringLength(256, ErrorMessage = "Website must be 256 characters or fewer.")]
    public string CommentUrl { get; set; }

    [Required(ErrorMessage = "Please enter a comment.")]
    [StringLength(4000, MinimumLength = 2, ErrorMessage = "Comment must be between 2 and 4000 characters.")]
    public string CommentBody { get; set; }
}
