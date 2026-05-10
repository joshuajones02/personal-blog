namespace Home.Blog.Mvc.Models;

using System.Collections.Generic;
using Piranha.Models;

/// <summary>
/// View-model for the shared <c>_Hero</c> partial.
/// Replaces the previous anonymous-object approach so dynamic
/// dispatch works correctly across compiled Razor view boundaries.
/// </summary>
public class HeroModel
{
    public string Title { get; set; }

    /// <summary>Optional eyebrow rendered as a link (e.g. category).</summary>
    public HeroEyebrow Eyebrow { get; set; }

    /// <summary>Optional plain-text eyebrow when there's no link target.</summary>
    public string EyebrowText { get; set; }

    /// <summary>Optional intro paragraph; rendered as raw HTML.</summary>
    public string Lead { get; set; }

    /// <summary>Optional resized hero image URL.</summary>
    public string ImageUrl { get; set; }

    /// <summary>"page" or "article". Defaults to "page".</summary>
    public string Variant { get; set; } = "page";

    /// <summary>Name of an optional partial to render inside the hero.</summary>
    public string MetaPartial { get; set; }

    /// <summary>Model passed to <see cref="MetaPartial"/>.</summary>
    public object MetaModel { get; set; }

    /// <summary>Optional tag list rendered as chips.</summary>
    public IEnumerable<Taxonomy> Tags { get; set; }

    /// <summary>Base URL used to build tag-chip links.</summary>
    public string ArchiveUrl { get; set; }
}

public class HeroEyebrow
{
    public string Label { get; set; }
    public string Href { get; set; }
}
