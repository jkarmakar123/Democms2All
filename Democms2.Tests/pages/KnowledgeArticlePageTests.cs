using System.Reflection;
using Democms2.Models.Pages;
using EPiServer.DataAnnotations;
using Xunit;

namespace Democms2.Tests.pages;

public class KnowledgeArticlePageTests
{
    [Fact]
    public void KnowledgeArticlePage_has_content_type_attribute()
    {
        var type = typeof(KnowledgeArticlePage);
        var attribute = type.GetCustomAttribute<ContentTypeAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal("Knowledge Article Page", attribute!.DisplayName);
    }
}
