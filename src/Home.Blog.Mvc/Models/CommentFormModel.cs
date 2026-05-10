namespace Home.Blog.Mvc.Models;

using System;
using System.Collections.Generic;

public class CommentFormModel
{
    public Guid PostId { get; set; }
    public string ActionUrl { get; set; }

    /// <summary>
    /// Repopulated values after a failed submission.
    /// </summary>
    public string Author { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }
    public string Body { get; set; }

    /// <summary>
    /// Field-level error messages keyed by form field name
    /// (CommentAuthor, CommentEmail, CommentUrl, CommentBody).
    /// </summary>
    public IDictionary<string, string> FieldErrors { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Top-level / form-wide error messages (e.g. spam rejection).
    /// </summary>
    public IList<string> FormErrors { get; set; } = new List<string>();

    /// <summary>
    /// Set after a successful comment is posted, so we can show a confirmation.
    /// </summary>
    public bool JustSubmitted { get; set; }

    public bool HasErrors => FieldErrors.Count > 0 || FormErrors.Count > 0;
}
