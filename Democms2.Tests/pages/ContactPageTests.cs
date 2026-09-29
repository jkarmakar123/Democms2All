using System.Reflection;
using Democms2.Models.Pages;
using EPiServer.DataAnnotations;
using Xunit;

namespace Democms2.Tests.pages;

public class ContactPageTests
{
    [Fact]
    public void ContactPage_has_content_type_attribute()
    {
        var type = typeof(ContactPage);
        var attribute = type.GetCustomAttribute<ContentTypeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal("Contact Page", attribute!.DisplayName);
    }
}
