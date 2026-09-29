using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class ContactPageControllerTests
{
    [Fact]
    public void ContactPageController_inherits_page_controller()
    {
        var type = typeof(ContactPageController);

        Assert.True(typeof(PageController<ContactPage>).IsAssignableFrom(type));
    }

    [Fact]
    public void ContactPageController_has_index_action()
    {
        var method = typeof(ContactPageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
