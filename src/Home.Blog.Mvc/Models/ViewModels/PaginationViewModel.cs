namespace Home.Blog.Mvc.Models.ViewModels;

/// <summary>
/// Presentation-only DTO for the pagination partial.
/// </summary>
public class PaginationViewModel
{
    /// <summary>
    /// The archive url pagination links are appended to
    /// (e.g. "/blog" or "/blog/category/travel").
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// The current page number (1-based).
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages { get; set; }
}
