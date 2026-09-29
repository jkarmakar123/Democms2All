using System.Reflection;
using Democms2.Models.Pages;
using EPiServer.DataAnnotations;
using Xunit;

namespace Democms2.Tests.pages;

public class HomePageTests
{
    [Fact]
    public void HomePage_has_content_type_attribute()
    {
        var type = typeof(HomePage);
        var attribute = type.GetCustomAttribute<ContentTypeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal("Home Page", attribute!.DisplayName);
    }
}
