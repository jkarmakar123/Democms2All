using System.Reflection;
using Democms2.Controllers;
using Democms2.Models.Pages;
using Democms2.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

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

    [Fact]
    public void KnowledgeBasePageController_requires_content_loader()
    {
        var constructor = typeof(KnowledgeBasePageController)
            .GetConstructor(new[] { typeof(IContentLoader) });

        Assert.NotNull(constructor);
    }

    [Fact]
    public void Index_returns_articles_from_content_loader()
    {
        // Arrange
        var currentPage = new KnowledgeBasePage
        {
            PageSize = 10
        };

        var article1 = new KnowledgeArticlePage
        {
            ArticleTitle = "First Article",
            PublishedDate = new DateTime(2024, 05, 01),
            Tags = new List<string> { "tag1" },
            ArticleCategory = "Category1"
        };

        var article2 = new KnowledgeArticlePage
        {
            ArticleTitle = "Second Article",
            PublishedDate = new DateTime(2024, 06, 01),
            Tags = new List<string> { "tag2" },
            ArticleCategory = "Category2"
        };

        var contentLoaderMock = new Mock<IContentLoader>();
        contentLoaderMock
            .Setup(x => x.GetChildren<KnowledgeArticlePage>(It.IsAny<ContentReference>()))
            .Returns(new List<KnowledgeArticlePage> { article1, article2 });

        var controller = new KnowledgeBasePageController(contentLoaderMock.Object);

        // Act
        var result = controller.Index(currentPage, page: 1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<KnowledgeBasePageViewModel>(viewResult.Model);

        Assert.NotNull(model.Articles);
        Assert.Equal(2, model.Articles.Count());
        contentLoaderMock.Verify(
            x => x.GetChildren<KnowledgeArticlePage>(It.IsAny<ContentReference>()),
            Times.Once());
    }

    [Fact]
    public void Index_returns_articles_sorted_by_published_date_descending()
    {
        // Arrange
        var currentPage = new KnowledgeBasePage
        {
           
            PageSize = 10
        };

        var oldArticle = new KnowledgeArticlePage
        {
           
            ArticleTitle = "Old Article",
            PublishedDate = new DateTime(2024, 01, 01),
            Tags = new List<string>(),
            ArticleCategory = "News"
        };

        var newArticle = new KnowledgeArticlePage
        {
          
            ArticleTitle = "New Article",
            PublishedDate = new DateTime(2024, 12, 01),
            Tags = new List<string>(),
            ArticleCategory = "News"
        };

        var contentLoaderMock = new Mock<IContentLoader>();
        contentLoaderMock
            .Setup(x => x.GetChildren<KnowledgeArticlePage>(It.IsAny<ContentReference>()))
            .Returns(new List<KnowledgeArticlePage> { oldArticle, newArticle });

        var controller = new KnowledgeBasePageController(contentLoaderMock.Object);

        // Act
        var result = controller.Index(currentPage);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<KnowledgeBasePageViewModel>(viewResult.Model);

        var articles = model.Articles.ToList();
        Assert.Equal("New Article", articles[0].ArticleTitle);
        Assert.Equal("Old Article", articles[1].ArticleTitle);
    }

    [Fact]
    public void Index_applies_tag_filter_correctly()
    {
        // Arrange
        var currentPage = new KnowledgeBasePage
        {
            PageSize = 10
        };

        var articleWithTag = new KnowledgeArticlePage
        {
            ArticleTitle = "Tagged Article",
            PublishedDate = new DateTime(2024, 05, 01),
            Tags = new List<string> { "important", "news" },
            ArticleCategory = "News"
        };

        var articleWithoutTag = new KnowledgeArticlePage
        {
            ArticleTitle = "Untagged Article",
            PublishedDate = new DateTime(2024, 06, 01),
            Tags = new List<string> { "product" },
            ArticleCategory = "Product"
        };

        var contentLoaderMock = new Mock<IContentLoader>();
        contentLoaderMock
            .Setup(x => x.GetChildren<KnowledgeArticlePage>(It.IsAny<ContentReference>()))
            .Returns(new List<KnowledgeArticlePage> { articleWithTag, articleWithoutTag });

        var controller = new KnowledgeBasePageController(contentLoaderMock.Object);

        // Act
        var result = controller.Index(currentPage, page: 1, tag: "important");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<KnowledgeBasePageViewModel>(viewResult.Model);

        Assert.Single(model.Articles);
        var filteredArticles = model.Articles.ToList();
        Assert.Equal("Tagged Article", filteredArticles[0].ArticleTitle);
        Assert.Equal("important", model.SelectedTag);
    }

    [Fact]
    public void Index_applies_category_filter_correctly()
    {
        // Arrange
        var currentPage = new KnowledgeBasePage
        {
            
            PageSize = 10
        };

        var newsArticle = new KnowledgeArticlePage
        {
            
            ArticleTitle = "News Article",
            PublishedDate = new DateTime(2024, 05, 01),
            Tags = new List<string>(),
            ArticleCategory = "News"
        };

        var productArticle = new KnowledgeArticlePage
        {
            
            ArticleTitle = "Product Article",
            PublishedDate = new DateTime(2024, 06, 01),
            Tags = new List<string>(),
            ArticleCategory = "Product"
        };

        var contentLoaderMock = new Mock<IContentLoader>();
        contentLoaderMock
            .Setup(x => x.GetChildren<KnowledgeArticlePage>(It.IsAny<ContentReference>()))
            .Returns(new List<KnowledgeArticlePage> { newsArticle, productArticle });

        var controller = new KnowledgeBasePageController(contentLoaderMock.Object);

        // Act
        var result = controller.Index(currentPage, page: 1, articleCategory: "News");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<KnowledgeBasePageViewModel>(viewResult.Model);

        Assert.Single(model.Articles);
        var categoryArticles = model.Articles.ToList();
        Assert.Equal("News Article", categoryArticles[0].ArticleTitle);
        Assert.Equal("News", model.SelectedArticleCategory);
    }

    [Theory]
    [InlineData(1, 2, "Article 1")]
    [InlineData(2, 2, "Article 3")]
    [InlineData(3, 1, "Article 5")]
    public void Index_applies_pagination_correctly(
        int page,
        int expectedCount,
        string expectedFirstArticle)
    {
        // Arrange
        var currentPage = new KnowledgeBasePage
        {
            PageSize = 2
        };

        var articles = new List<KnowledgeArticlePage>
    {
        new()
        {
            ArticleTitle = "Article 1",
            PublishedDate = new DateTime(2024, 03, 01)
        },
        new()
        {
            ArticleTitle = "Article 2",
            PublishedDate = new DateTime(2024, 02, 01)
        },
        new()
        {
            ArticleTitle = "Article 3",
            PublishedDate = new DateTime(2024, 01, 01)
        },
        new()
        {
            ArticleTitle = "Article 4",
            PublishedDate = new DateTime(2023, 12, 01)
        },
        new()
        {
            ArticleTitle = "Article 5",
            PublishedDate = new DateTime(2023, 11, 01)
        }
    };

        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x =>
                x.GetChildren<KnowledgeArticlePage>(
                    It.IsAny<ContentReference>()))
            .Returns(articles);

        var controller =
            new KnowledgeBasePageController(
                contentLoaderMock.Object);

        // Act
        var result =
            controller.Index(currentPage, page);

        // Assert
        var viewResult =
            Assert.IsType<ViewResult>(result);

        var model =
            Assert.IsType<KnowledgeBasePageViewModel>(
                viewResult.Model);

        var resultArticles =
            model.Articles.ToList();

        Assert.Equal(
            expectedCount,
            resultArticles.Count);

        Assert.Equal(
            expectedFirstArticle,
            resultArticles[0].ArticleTitle);

        Assert.Equal(
            page,
            model.CurrentPageNumber);

        Assert.Equal(
            3,
            model.TotalPages);
    }

}
