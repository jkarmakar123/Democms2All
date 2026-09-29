using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class KnowledgeBasePageControllerTests
{
    [Fact]
    public void KnowledgeBasePageController_inherits_page_controller()
    {
        var type = typeof(KnowledgeBasePageController);

        Assert.True(typeof(PageController<KnowledgeBasePage>).IsAssignableFrom(type));
    }

    [Fact]
    public void KnowledgeBasePageController_has_index_action()
    {
        var method = typeof(KnowledgeBasePageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
