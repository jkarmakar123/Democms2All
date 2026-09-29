using Democms2.Models.Pages;
using Xunit;

namespace Democms2.Tests.pages;

public class ResourceLibraryPageTests
{
    [Fact]
    public void ResourceLibraryPage_inherits_article_listing_page()
    {
        var type = typeof(Democms2.Models.Pages.ResourceLibraryPage);

        Assert.True(typeof(Democms2.Models.Pages.ArticleListingPage).IsAssignableFrom(type));
    }
}
