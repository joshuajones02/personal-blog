namespace Home.Blog.Mvc.Models;

using System;

public class PaginationModel
{
    public int CurrentPage { get; set; }
    public int TotalPages  { get; set; }
    public Func<int, string> UrlBuilder { get; set; }
}
