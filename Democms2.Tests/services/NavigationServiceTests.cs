using System.Reflection;
using Democms2.Services;
using EPiServer.Core;
using Xunit;

namespace Democms2.Tests.services;

public class NavigationServiceTests
{
    [Fact]
    public void NavigationService_is_public_and_exposes_expected_api()
    {
        var type = typeof(NavigationService);

        Assert.True(type.IsClass);
        Assert.True(type.IsPublic);

        Assert.NotNull(type.GetMethod(
            "GetMainNavigation",
            new[] { typeof(ContentReference), typeof(ContentReference) }));

        Assert.NotNull(type.GetMethod(
            "GetBreadcrumb",
            new[] { typeof(ContentReference) }));
    }

    [Fact]
    public void NavigationService_implements_nav_service_contract()
    {
        var type = typeof(NavigationService);

        Assert.True(typeof(INavService).IsAssignableFrom(type));
    }
}
