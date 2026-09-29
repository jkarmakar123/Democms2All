using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class HomePageControllerTests
{
    [Fact]
    public void HomePageController_inherits_page_controller()
    {
        var type = typeof(HomePageController);

        Assert.True(typeof(PageController<HomePage>).IsAssignableFrom(type));
    }

    [Fact]
    public void HomePageController_has_index_action()
    {
        var method = typeof(HomePageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
