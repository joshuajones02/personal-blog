namespace Home.Blog.Mvc.Models.ViewModels;

using System;
using System.Collections.Generic;
using Piranha.Extend.Fields;

/// <summary>
/// Presentation-only DTO for the page hero partial. Deliberately NOT a
/// Piranha content type: ContentTypeBuilder only scans attributed types,
/// so this plain class is ignored by the CMS.
/// </summary>
public class HeroViewModel
{
    /// <summary>
    /// The page heading (rendered as the single h1).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional excerpt. Raw HTML from the CMS.
    /// </summary>
    public string? ExcerptHtml { get; set; }

    /// <summary>
    /// Optional primary image.
    /// </summary>
    public ImageField? Image { get; set; }

    /// <summary>
    /// Optional eyebrow label above the title (e.g. category).
    /// </summary>
    public string? EyebrowText { get; set; }

    /// <summary>
    /// Optional url for the eyebrow label.
    /// </summary>
    public string? EyebrowUrl { get; set; }

    /// <summary>
    /// Optional publish date shown in the meta row.
    /// </summary>
    public DateTime? Published { get; set; }

    /// <summary>
    /// Comment count linked to #comments when ShowCommentCount is set.
    /// </summary>
    public int CommentCount { get; set; }

    /// <summary>
    /// Whether the comment count should be rendered.
    /// </summary>
    public bool ShowCommentCount { get; set; }

    /// <summary>
    /// Optional tag links rendered below the excerpt.
    /// </summary>
    public IList<HeroLink> Tags { get; set; } = new List<HeroLink>();
}

/// <summary>
/// A simple title/url pair used by the hero partial.
/// </summary>
public class HeroLink
{
    public HeroLink(string title, string url)
    {
        Title = title;
        Url = url;
    }

    public string Title { get; }

    public string Url { get; }
}
