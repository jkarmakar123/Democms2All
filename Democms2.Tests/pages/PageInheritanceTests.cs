using Democms2.Models.Pages;
using Democms2.Models.Pages.Base;
using Xunit;

namespace Democms2.Tests.pages;

public class PageInheritanceTests
{
    [Fact]
    public void All_page_models_inherit_from_site_page_base()
    {
        var pageTypes = new[]
        {
            typeof(HomePage),
            typeof(ContactPage),
            typeof(KnowledgeArticlePage),
            typeof(KnowledgeBasePage),
            typeof(ResourcePage),
            typeof(ResourceLibraryPage),
            typeof(SearchResultsPage)
        };

        foreach (var pageType in pageTypes)
        {
           Assert.True(pageType.IsAssignableTo(typeof(SitePageBase)));
        }
    }
}
