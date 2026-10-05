using System.Reflection;
using Democms2.Services;
using EPiServer.Core;
using Xunit;
using Moq;
using EPiServer;
using EPiServer.Web.Routing;
using Democms2.Models.Pages.Base;

namespace Democms2.Tests.services;

public class NavigationServiceTests
{
    [Fact]
    public void NavigationService_is_public_and_exposes_expected_api()
    {
        var type = typeof(NavigationService);

        Assert.True(type.IsClass);
        Assert.True(type.IsPublic);

        Assert.NotNull(type.GetMethod(
            "GetMainNavigation",
            new[] { typeof(ContentReference), typeof(ContentReference) }));

        Assert.NotNull(type.GetMethod(
            "GetBreadcrumb",
            new[] { typeof(ContentReference) }));
    }

    [Fact]
    public void NavigationService_implements_nav_service_contract()
    {
        var type = typeof(NavigationService);

        Assert.True(typeof(INavService).IsAssignableFrom(type));
    }


    // ---------------------------------------------------------
    // Main Navigation
    // ---------------------------------------------------------

    [Fact]
    public void GetMainNavigation_returns_navigation_items()
    {
        // Arrange
        var rootLink = new ContentReference(1);
        var aboutLink = new ContentReference(2);
        var contactLink = new ContentReference(3);

        var aboutPage = CreatePage(
            aboutLink,
            "About");

        var contactPage = CreatePage(
            contactLink,
            "Contact");

        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(rootLink))
            .Returns(new[]
            {
                aboutPage.Object,
                contactPage.Object
            });

        // BuildMenu recursively asks for children,
        // so return empty collections for leaf pages.
        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(aboutLink))
            .Returns(Array.Empty<PageData>());

        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(contactLink))
            .Returns(Array.Empty<PageData>());

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(aboutLink))
            .Returns("/about/");

        urlResolverMock
            .Setup(x => x.GetUrl(contactLink))
            .Returns("/contact/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetMainNavigation(
            rootLink,
            aboutLink);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("About", result[0].Title);
        Assert.Equal("/about/", result[0].Url);
        Assert.True(result[0].IsActive);

        Assert.Equal("Contact", result[1].Title);
        Assert.Equal("/contact/", result[1].Url);
        Assert.False(result[1].IsActive);
    }


    [Fact]
    public void GetMainNavigation_builds_child_navigation_recursively()
    {
        // Arrange
        var rootLink = new ContentReference(1);
        var parentLink = new ContentReference(2);
        var childLink = new ContentReference(3);

        var parentPage = CreatePage(
            parentLink,
            "Services");

        var childPage = CreatePage(
            childLink,
            "Consulting");

        var contentLoaderMock = new Mock<IContentLoader>();

        // Root
        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(rootLink))
            .Returns(new[]
            {
                parentPage.Object
            });

        // Parent contains child
        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(parentLink))
            .Returns(new[]
            {
                childPage.Object
            });

        // Child has no more children
        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(childLink))
            .Returns(Array.Empty<PageData>());

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(parentLink))
            .Returns("/services/");

        urlResolverMock
            .Setup(x => x.GetUrl(childLink))
            .Returns("/services/consulting/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetMainNavigation(
            rootLink,
            childLink);

        // Assert
        Assert.Single(result);

        var parent = result[0];

        Assert.Equal("Services", parent.Title);
        Assert.False(parent.IsActive);

        Assert.Single(parent.Children);

        var child = parent.Children[0];

        Assert.Equal("Consulting", child.Title);
        Assert.Equal("/services/consulting/", child.Url);

        Assert.True(child.IsActive);
    }


    [Fact]
    public void GetMainNavigation_excludes_pages_hidden_from_navigation()
    {
        // Arrange
        var rootLink = new ContentReference(1);

        var hiddenLink = new ContentReference(2);
        var visibleLink = new ContentReference(3);

        var hiddenPage = new Mock<SitePageBase>();

        hiddenPage
            .SetupGet(x => x.ContentLink)
            .Returns(hiddenLink);

        hiddenPage
            .SetupGet(x => x.Name)
            .Returns("Hidden");

        hiddenPage
            .SetupGet(x => x.HideFromNavigation)
            .Returns(true);


        var visiblePage = new Mock<SitePageBase>();

        visiblePage
            .SetupGet(x => x.ContentLink)
            .Returns(visibleLink);

        visiblePage
            .SetupGet(x => x.Name)
            .Returns("Visible");

        visiblePage
            .SetupGet(x => x.HideFromNavigation)
            .Returns(false);


        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(rootLink))
            .Returns(new PageData[]
            {
                hiddenPage.Object,
                visiblePage.Object
            });

        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(visibleLink))
            .Returns(Array.Empty<PageData>());

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(visibleLink))
            .Returns("/visible/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetMainNavigation(
            rootLink,
            visibleLink);

        // Assert
        Assert.Single(result);

        Assert.Equal("Visible", result[0].Title);

        // Hidden page should never have its URL generated.
        urlResolverMock.Verify(
            x => x.GetUrl(hiddenLink),
            Times.Never());
    }


    [Fact]
    public void GetMainNavigation_returns_empty_list_when_no_children_exist()
    {
        // Arrange
        var rootLink = new ContentReference(1);

        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x => x.GetChildren<PageData>(rootLink))
            .Returns(Array.Empty<PageData>());

        var urlResolverMock = new Mock<UrlResolver>();

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetMainNavigation(
            rootLink,
            new ContentReference(100));

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }


    // ---------------------------------------------------------
    // Breadcrumb
    // ---------------------------------------------------------

    [Fact]
    public void GetBreadcrumb_returns_ancestors_and_current_page()
    {
        // Arrange
        var rootLink = new ContentReference(1);
        var sectionLink = new ContentReference(2);
        var currentLink = new ContentReference(3);

        var rootPage = CreatePage(
            rootLink,
            "Home");

        var sectionPage = CreatePage(
            sectionLink,
            "Knowledge");

        var currentPage = CreatePage(
            currentLink,
            "Article");

        var contentLoaderMock = new Mock<IContentLoader>();

        /*
         * GetAncestors result:
         *
         * current
         *    |
         * Knowledge
         *    |
         * Home
         *
         * Your service reverses the collection
         * and then Skip(1) removes Home.
         */
        contentLoaderMock
            .Setup(x => x.GetAncestors(currentLink))
            .Returns(new IContent[]
            {
                sectionPage.Object,
                rootPage.Object
            });

        contentLoaderMock
            .Setup(x => x.Get<PageData>(
                sectionLink,
                It.IsAny<LoaderOptions>()))
            .Returns(sectionPage.Object);

        contentLoaderMock
            .Setup(x => x.Get<PageData>(
                currentLink,
                It.IsAny<LoaderOptions>()))
            .Returns(currentPage.Object);

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(sectionLink))
            .Returns("/knowledge/");

        urlResolverMock
            .Setup(x => x.GetUrl(currentLink))
            .Returns("/knowledge/article/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetBreadcrumb(currentLink);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("Knowledge", result[0].Title);
        Assert.Equal("/knowledge/", result[0].Url);
        Assert.False(result[0].IsActive);

        Assert.Equal("Article", result[1].Title);
        Assert.Equal("/knowledge/article/", result[1].Url);
        Assert.True(result[1].IsActive);
    }


    [Fact]
    public void GetBreadcrumb_returns_only_current_page_when_no_ancestors_exist()
    {
        // Arrange
        var currentLink = new ContentReference(10);

        var currentPage = CreatePage(
            currentLink,
            "Current Page");

        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x => x.GetAncestors(currentLink))
            .Returns(Array.Empty<IContent>());

        contentLoaderMock
            .Setup(x => x.Get<PageData>(
                currentLink,
                It.IsAny<LoaderOptions>()))
            .Returns(currentPage.Object);

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(currentLink))
            .Returns("/current-page/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetBreadcrumb(currentLink);

        // Assert
        Assert.Single(result);

        Assert.Equal(
            "Current Page",
            result[0].Title);

        Assert.Equal(
            "/current-page/",
            result[0].Url);

        Assert.True(result[0].IsActive);
    }


    [Fact]
    public void GetBreadcrumb_excludes_hidden_ancestor()
    {
        // Arrange
        var rootLink = new ContentReference(1);
        var hiddenLink = new ContentReference(2);
        var currentLink = new ContentReference(3);

        var rootPage = CreatePage(
            rootLink,
            "Home");

        var hiddenPage = new Mock<SitePageBase>();

        hiddenPage
            .SetupGet(x => x.ContentLink)
            .Returns(hiddenLink);

        hiddenPage
            .SetupGet(x => x.Name)
            .Returns("Hidden Section");

        hiddenPage
            .SetupGet(x => x.HideFromNavigation)
            .Returns(true);

        var currentPage = CreatePage(
            currentLink,
            "Article");

        var contentLoaderMock = new Mock<IContentLoader>();

        contentLoaderMock
            .Setup(x => x.GetAncestors(currentLink))
            .Returns(new IContent[]
            {
                hiddenPage.Object,
                rootPage.Object
            });

        contentLoaderMock
            .Setup(x => x.Get<PageData>(
                hiddenLink,
                It.IsAny<LoaderOptions>()))
            .Returns(hiddenPage.Object);

        contentLoaderMock
            .Setup(x => x.Get<PageData>(
                currentLink,
                It.IsAny<LoaderOptions>()))
            .Returns(currentPage.Object);

        var urlResolverMock = new Mock<UrlResolver>();

        urlResolverMock
            .Setup(x => x.GetUrl(currentLink))
            .Returns("/article/");

        var service = new NavigationService(
            contentLoaderMock.Object,
            urlResolverMock.Object);

        // Act
        var result = service.GetBreadcrumb(currentLink);

        // Assert
        Assert.Single(result);

        Assert.Equal(
            "Article",
            result[0].Title);

        Assert.True(result[0].IsActive);

        // Hidden ancestor shouldn't get a URL.
        urlResolverMock.Verify(
            x => x.GetUrl(hiddenLink),
            Times.Never());
    }


    // ---------------------------------------------------------
    // Helper
    // ---------------------------------------------------------

    private static Mock<PageData> CreatePage(
        ContentReference contentLink,
        string name)
    {
        var page = new Mock<PageData>();

        page
            .SetupGet(x => x.ContentLink)
            .Returns(contentLink);

        page
            .SetupGet(x => x.Name)
            .Returns(name);

        return page;
    }
}
