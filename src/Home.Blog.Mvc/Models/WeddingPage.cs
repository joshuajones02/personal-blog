using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace Home.Blog.Mvc.Models;

[PageType(Title = "Wedding page")]
[ContentTypeRoute(Title = "Default", Route = "/weddingpage")]
public class WeddingPage : Page<WeddingPage>
{
    /// <summary>
    /// Gets/sets the wedding details region.
    /// </summary>
    [Region(Title = "Wedding Details", Display = RegionDisplayMode.Setting)]
    public WeddingDetailsRegion WeddingDetails { get; set; }
}

public class WeddingDetailsRegion
{
    /// <summary>
    /// Gets/sets the wedding date.
    /// </summary>
    [Field(Title = "Wedding Date")]
    public DateField Date { get; set; }

    /// <summary>
    /// Gets/sets the venue name.
    /// </summary>
    [Field(Title = "Venue Name")]
    public StringField VenueName { get; set; }

    /// <summary>
    /// Gets/sets the venue address.
    /// </summary>
    [Field(Title = "Venue Address")]
    public StringField VenueAddress { get; set; }

    /// <summary>
    /// Gets/sets a personal message from the couple.
    /// </summary>
    [Field(Title = "Couple Message")]
    public HtmlField CoupleMessage { get; set; }
}
