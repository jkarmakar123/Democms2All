

using EPiServer;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Democms2.Models.Pages;
using Democms2.Models.SelectionFactories;
using Democms2.Models.ViewModels;


namespace Democms2.Controllers
{   
public class KnowledgeBasePageController
    : PageController<KnowledgeBasePage>
{
    private readonly IContentLoader _contentLoader;

    public KnowledgeBasePageController(
        IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public IActionResult Index(
        ArticleListingPage currentPage,
        int page = 1,
        string? tag = null,
        string? articleCategory = null)
    {
        var selectedTag = string.IsNullOrWhiteSpace(tag) ? null : tag.Trim();
        var selectedArticleCategory = string.IsNullOrWhiteSpace(articleCategory) ? null : articleCategory.Trim();

        var allArticles = _contentLoader
            .GetChildren<KnowledgeArticlePage>(
                currentPage.ContentLink)
            .OrderByDescending(x => x.PublishedDate)
            .ToList();

        var availableTags = allArticles
            .Where(x => x.Tags != null)
            .SelectMany(x => x.Tags)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        var availableArticleCategories = ArticleCategorySelectionFactory.Categories;

        var articles = allArticles.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(selectedTag))
        {
            articles = articles.Where(x =>
                x.Tags != null &&
                x.Tags.Any(articleTag =>
                    string.Equals(articleTag?.Trim(), selectedTag, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(selectedArticleCategory))
        {
            articles = articles.Where(x =>
                string.Equals(x.ArticleCategory?.Trim(), selectedArticleCategory, StringComparison.OrdinalIgnoreCase));
        }

        var filteredArticles = articles.ToList();
        var pageSize = currentPage.PageSize > 0 ? currentPage.PageSize : 9;
        var totalPages = (int)Math.Ceiling(filteredArticles.Count / (double)pageSize);
        var currentPageNumber = Math.Max(1, page);

        if (totalPages > 0 && currentPageNumber > totalPages)
        {
            currentPageNumber = totalPages;
        }

        var pagedArticles = filteredArticles
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();



        var model = new KnowledgeBasePageViewModel
        {
            CurrentPage = currentPage,
            Articles = pagedArticles,
            CurrentPageNumber = currentPageNumber,
            TotalPages = totalPages,
            AvailableTags = availableTags,
            AvailableArticleCategories = availableArticleCategories,
            SelectedTag = selectedTag,
            SelectedArticleCategory = selectedArticleCategory
        };

        return View(model);
    }
}
}
