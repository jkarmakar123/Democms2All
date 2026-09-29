using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class KnowledgeArticlePageControllerTests
{
    [Fact]
    public void KnowledgeArticlePageController_inherits_page_controller()
    {
        var type = typeof(KnowledgeArticlePageController);

        Assert.True(typeof(PageController<KnowledgeArticlePage>).IsAssignableFrom(type));
    }

    [Fact]
    public void KnowledgeArticlePageController_has_index_action()
    {
        var method = typeof(KnowledgeArticlePageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
