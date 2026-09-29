using System.Reflection;
using Democms2.Models.Pages;
using Xunit;

namespace Democms2.Tests.pages;

public class ResourcePageTests
{
    [Fact]
    public void ResourcePage_has_required_resource_title_property()
    {
        var property = typeof(ResourcePage).GetProperty(nameof(ResourcePage.ResourceTitle));

        Assert.NotNull(property);
        Assert.True(property!.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), inherit: true).Any());
    }
}
