using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Democms2.Models.Pages.Base;
using Democms2.Models.ViewModels;

using EPiServer.Globalization;

namespace Democms2.Services
{
    public class NavigationService : INavService
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public NavigationService(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public List<NavItemViewModel> GetMainNavigation(ContentReference rootLink, ContentReference currentLink)
        {
            return BuildMenu(rootLink, currentLink);
        }

        private List<NavItemViewModel> BuildMenu(ContentReference parentLink, ContentReference currentLink)
        {
            var children = _contentLoader.GetChildren<PageData>(parentLink);
            var list = new List<NavItemViewModel>();

            foreach (var page in children.OfType<PageData>())
            {
                if (page is SitePageBase sitePage && sitePage.HideFromNavigation)
                    continue;

                list.Add(new NavItemViewModel
                {
                    Title = page.Name,
                    Url = _urlResolver.GetUrl(page.ContentLink),
                    IsActive = page.ContentLink == currentLink,
                    Children = BuildMenu(page.ContentLink, currentLink)
                });
            }

            return list;
        }


        // public List<NavItemViewModel> GetBreadcrumb(ContentReference currentLink)
        // {
        //     var breadcrumb = new List<NavItemViewModel>();

        //     var ancestors = _contentLoader
        //         .GetAncestors(currentLink)
        //         .OfType<PageData>()
        //         .Reverse()
        //         .ToList();

        //     if (ancestors.Any())
        //     {
        //         ancestors = ancestors.Skip(1).ToList();
        //     }

        //     foreach (var page in ancestors)
        //     {
        //         if (page is SitePageBase sitePage && sitePage.HideFromNavigation)
        //             continue;

        //         breadcrumb.Add(new NavItemViewModel
        //         {
        //             Title = page.Name,
        //             Url = _urlResolver.GetUrl(page.ContentLink),
        //             IsActive = false
        //         });
        //     }

        //     var currentPage = _contentLoader.Get<PageData>(currentLink);
        //     if (currentPage != null)
        //     {
        //         breadcrumb.Add(new NavItemViewModel
        //         {
        //             Title = currentPage.Name,
        //             Url = _urlResolver.GetUrl(currentPage.ContentLink),
        //             IsActive = true
        //         });
        //     }

        //     return breadcrumb;
        // }

public List<NavItemViewModel> GetBreadcrumb(ContentReference currentLink)
{
    var breadcrumb = new List<NavItemViewModel>();

    var languageOptions = new LoaderOptions
    {
        LanguageLoaderOption.FallbackWithMaster(ContentLanguage.PreferredCulture)
    };

    var ancestors = _contentLoader
        .GetAncestors(currentLink)
        .OfType<PageData>()
        .Reverse()
        .ToList();

    if (ancestors.Any())
    {
        ancestors = ancestors.Skip(1).ToList();
    }

    foreach (var ancestor in ancestors)
    {
        var page = _contentLoader.Get<PageData>(ancestor.ContentLink, languageOptions);

        if (page is SitePageBase sitePage && sitePage.HideFromNavigation)
            continue;

        breadcrumb.Add(new NavItemViewModel
        {
            Title = page.Name,
            Url = _urlResolver.GetUrl(page.ContentLink),
            IsActive = false
        });
    }

    var currentPage = _contentLoader.Get<PageData>(currentLink, languageOptions);

    if (currentPage != null)
    {
        breadcrumb.Add(new NavItemViewModel
        {
            Title = currentPage.Name,
            Url = _urlResolver.GetUrl(currentPage.ContentLink),
            IsActive = true
        });
    }

    return breadcrumb;
}

    }
}