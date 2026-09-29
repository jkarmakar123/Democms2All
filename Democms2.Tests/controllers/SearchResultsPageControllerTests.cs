using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Xunit;

namespace Democms2.Tests.controllers;

public class SearchResultsPageControllerTests
{
    [Fact]
    public void SearchResultsPageController_inherits_page_controller()
    {
        var type = typeof(SearchResultsPageController);

        Assert.True(typeof(PageController<SearchResultsPage>).IsAssignableFrom(type));
    }

    [Fact]
    public void SearchResultsPageController_has_index_action()
    {
        var method = typeof(SearchResultsPageController)
            .GetMethod("Index", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(method);
    }
}
