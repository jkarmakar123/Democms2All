using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class ResourcePageControllerTests
{
    [Fact]
    public void ResourcePageController_inherits_page_controller()
    {
        var type = typeof(ResourcePageController);

        Assert.True(typeof(PageController<ResourcePage>).IsAssignableFrom(type));
    }

    [Fact]
    public void ResourcePageController_has_index_action()
    {
        var method = typeof(ResourcePageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
