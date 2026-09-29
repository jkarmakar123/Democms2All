using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class ResourceLibraryPageControllerTests
{
    [Fact]
    public void ResourceLibraryPageController_inherits_page_controller()
    {
        var type = typeof(ResourceLibraryPageController);

        Assert.True(typeof(PageController<ResourceLibraryPage>).IsAssignableFrom(type));
    }

    [Fact]
    public void ResourceLibraryPageController_has_index_action()
    {
        var method = typeof(ResourceLibraryPageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
