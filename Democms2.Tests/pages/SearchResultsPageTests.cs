using System.Reflection;
using Democms2.Models.Pages;
using EPiServer.DataAnnotations;
using Xunit;

namespace Democms2.Tests.pages;

public class SearchResultsPageTests
{
    [Fact]
    public void SearchResultsPage_has_content_type_attribute()
    {
        var type = typeof(SearchResultsPage);
        var attribute = type.GetCustomAttribute<ContentTypeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal("Search Results Page", attribute!.DisplayName);
    }
}
