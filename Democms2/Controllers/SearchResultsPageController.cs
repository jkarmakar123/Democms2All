// using Democms2.Models.Pages;
// using Democms2.Models.ViewModels;



// using EPiServer.Web.Mvc;
// using Microsoft.AspNetCore.Mvc;
// using EPiServer.Web.Routing;
// using Optimizely.Graph.Cms.Query;
// using Optimizely.Graph.Cms.Query.Abstractions;



// namespace Democms2.Controllers
// {
//     public class SearchResultsPageController
//         : PageController<SearchResultsPage>
//     {

//         private readonly IGraphContentClient _graphClient;

//         private readonly IUrlResolver _urlResolver;

//         public SearchResultsPageController(IGraphContentClient graphClient, IUrlResolver urlResolver)
//         {
//             _graphClient = graphClient;
//             _urlResolver = urlResolver;
//         }
//         public async Task<IActionResult> Index(SearchResultsPage currentPage, string q)
//         {
//             var model = new SearchResultsViewModel
//             {
//                 CurrentPage = currentPage,
//                 Query = q
//             };

//             if (string.IsNullOrWhiteSpace(q))
//             {
//                 model.Results = Enumerable.Empty<SearchResultItem>();
//                 return View(model);
//             }


//             // Optimizely Graph equivalent of a boosted search query.
//             // var results = await _graphClient
//             //     .QueryContent<IContent>()
//             //     .SearchFor(q)
//             //     .UsingFullText(null, 5)
//             //     .IncludeTotal()
//             //     .GetAsContentAsync();

//             var results = await _graphClient.QueryContent<IContent>()
//     .SearchFor(q)
//     .IncludeTotal()
//     .GetAsContentAsync();


//             var items = results
//                 .Select(content =>
//                 {
//                     var title = content switch
//                     {
//                         KnowledgeArticlePage article when !string.IsNullOrWhiteSpace(article.ArticleTitle) => article.ArticleTitle,
//                         ResourcePage resource when !string.IsNullOrWhiteSpace(resource.ResourceTitle) => resource.ResourceTitle,
//                         _ => content.Name
//                     };

//                     var summary = content switch
//                     {
//                         KnowledgeArticlePage article => article.Summary ?? string.Empty,
//                         ResourcePage resource => resource.Description ?? string.Empty,
//                         _ => string.Empty
//                     };

//                     var publishedDate = content switch
//                     {
//                         KnowledgeArticlePage article when article.PublishedDate != default => article.PublishedDate,
//                         ResourcePage resource when resource.PublishedDate != default => resource.PublishedDate,
//                         _ => DateTime.Now//content
//                     };

//                     return new SearchResultItem
//                     {
//                         Title = title,
//                         Summary = summary,
//                         PublishedDate = publishedDate,
//                         Url = _urlResolver.GetUrl(content.ContentLink),
//                         ContentType = content is KnowledgeArticlePage ? nameof(KnowledgeArticlePage)
//                                       : content is ResourcePage ? nameof(ResourcePage)
//                                       : content.GetType().Name
//                     };
//                 });

//             model.TotalResults = results.Total ?? 0;

//             model.Results = items;
//             return View(model);
//         }
//     }
// }

using Democms2.Models.Pages;
using Democms2.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Democms2.Controllers
{
    public class SearchResultsPageController : PageController<SearchResultsPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IUrlResolver _urlResolver;

        public SearchResultsPageController(
            IContentLoader contentLoader,
            IUrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public IActionResult Index(SearchResultsPage currentPage, string q)
        {
            var model = new SearchResultsViewModel
            {
                CurrentPage = currentPage,
                Query = q,
                Results = Enumerable.Empty<SearchResultItem>()
            };

            if (string.IsNullOrWhiteSpace(q))
            {
                return View(model);
            }

            var results = _contentLoader
                .GetDescendents(ContentReference.StartPage)
                .Select(link =>
                {
                    _contentLoader.TryGet<PageData>(link, out var page);
                    return page;
                })
                .Where(page => page != null)
                .Where(page => MatchesSearch(page!, q))
                .Select(page => new { page, score = CalculateRelevanceScore(page!, q) })
                .OrderByDescending(x => x.score)
                .ThenByDescending(x => x.page.Created)
                .Select(x => CreateSearchResult(x.page!))
                .OrderByDescending(result => result.PublishedDate)
                .ToList();

            model.Results = results;
            model.TotalResults = results.Count;

            return View(model);
        }


         /// <summary>
        /// Calculates relevance score for boosted search
        /// Higher scores = higher relevance
        /// </summary>
        private static double CalculateRelevanceScore(PageData page, string query)
        {
            double score = 0;
            var queryLower = query.ToLowerInvariant();
 
            // BOOST 1: Content Type Boost (5x for Article/Resource pages)
            if (page is KnowledgeArticlePage or ResourcePage)
            {
                score += 5;
            }
 
            // BOOST 2: Title Match (3x weight)
            var title = GetTitle(page).ToLowerInvariant();
            if (title.Contains(queryLower))
            {
                score += 3;
                // Extra boost if query is at the start of title
                if (title.StartsWith(queryLower))
                {
                    score += 2;
                }
            }
 
            // BOOST 3: Summary/Description Match (2x weight)
            var summary = GetSummary(page).ToLowerInvariant();
            if (summary.Contains(queryLower))
            {
                score += 2;
            }
 
            // BOOST 4: Field-Specific Matches
            switch (page)
            {
                case KnowledgeArticlePage article:
                    // Boost for matching article category
                    if (!string.IsNullOrEmpty(article.ArticleCategory?.ToLowerInvariant()) && 
                        article.ArticleCategory.ToLowerInvariant().Contains(queryLower))
                    {
                        score += 1.5;
                    }
                    // Boost for matching tags
                    if (article.Tags != null && 
                        article.Tags.Any(tag => tag.ToLowerInvariant().Contains(queryLower)))
                    {
                        score += 1.5;
                    }
                    break;
 
                case ResourcePage resource:
                    // Boost for matching resource category
                    if (!string.IsNullOrEmpty(resource.ResourceCategory?.ToLowerInvariant()) && 
                        resource.ResourceCategory.ToLowerInvariant().Contains(queryLower))
                    {
                        score += 1.5;
                    }
                    break;
            }
 
            // BOOST 5: Word Frequency Bonus
            var titleWords = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var summaryWords = summary.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var matchCount = titleWords.Count(w => w.Contains(queryLower)) + 
                           summaryWords.Count(w => w.Contains(queryLower));
            score += matchCount * 0.5;
 
            return score;
        }

        private static bool MatchesSearch(PageData page, string query)
        {
            var searchText = page switch
            {
                KnowledgeArticlePage article => string.Join(" ",
                    article.Name,
                    article.ArticleTitle,
                    article.Summary,
                    article.Body?.ToString(),
                    article.Author,
                    article.ArticleCategory,
                    article.Tags != null ? string.Join(" ", article.Tags) : string.Empty),

                ResourcePage resource => string.Join(" ",
                    resource.Name,
                    resource.ResourceTitle,
                    resource.Description,
                    resource.ResourceCategory),

                SearchResultsPage searchPage => string.Join(" ",
                    searchPage.Name,
                    searchPage.PageTitle,
                    searchPage.NoResultsText),

                HomePage homePage => string.Join(" ",
                    homePage.Name,
                    homePage.PageTitle),

                ArticleListingPage listingPage => string.Join(" ",
                    listingPage.Name,
                    listingPage.PageTitle,
                    listingPage.IntroText),

                _ => page.Name
            };

            return searchText.Contains(query, StringComparison.OrdinalIgnoreCase);
        }

        private SearchResultItem CreateSearchResult(PageData page)
        {
            return new SearchResultItem
            {
                Title = GetTitle(page),
                Summary = GetSummary(page),
                PublishedDate = GetPublishedDate(page),
                Url = _urlResolver.GetUrl(page.ContentLink),
                ContentType = page.GetOriginalType().Name
            };
        }

        private static string GetTitle(PageData page)
        {
            return page switch
            {
                KnowledgeArticlePage article when !string.IsNullOrWhiteSpace(article.ArticleTitle) => article.ArticleTitle,
                ResourcePage resource when !string.IsNullOrWhiteSpace(resource.ResourceTitle) => resource.ResourceTitle,
                SearchResultsPage searchPage when !string.IsNullOrWhiteSpace(searchPage.PageTitle) => searchPage.PageTitle,
                HomePage homePage when !string.IsNullOrWhiteSpace(homePage.PageTitle) => homePage.PageTitle,
                ArticleListingPage listingPage when !string.IsNullOrWhiteSpace(listingPage.PageTitle) => listingPage.PageTitle,
                _ => page.Name
            };
        }

        private static string GetSummary(PageData page)
        {
            return page switch
            {
                KnowledgeArticlePage article => article.Summary ?? string.Empty,
                ResourcePage resource => resource.Description ?? string.Empty,
                SearchResultsPage searchPage => searchPage.NoResultsText ?? string.Empty,
                ArticleListingPage listingPage => listingPage.IntroText ?? string.Empty,
                _ => string.Empty
            };
        }

        private static DateTime GetPublishedDate(PageData page)
        {
            return page switch
            {
                KnowledgeArticlePage article when article.PublishedDate != default => article.PublishedDate,
                ResourcePage resource when resource.PublishedDate != default => resource.PublishedDate,
                _ => page.Created
            };

//    return page.StopPublish??page.Created;
     // return page.Changed != default ? page.Changed : page.Created;

        }
    }
}
