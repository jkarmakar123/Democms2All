using Democms2.Models.Pages;
using Xunit;

namespace Democms2.Tests.pages;

public class KnowledgeBasePageTests
{
    [Fact]
    public void KnowledgeBasePage_inherits_article_listing_page()
    {
        var type = typeof(KnowledgeBasePage);

        Assert.True(typeof(ArticleListingPage).IsAssignableFrom(type));
    }
}
