

using EPiServer;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Democms2.Models.Pages;
using Democms2.Models.ViewModels;


namespace Democms2.Controllers
{   
public class ResourceLibraryPageController
    : PageController<ResourceLibraryPage>
{
    private readonly IContentLoader _contentLoader;

    public ResourceLibraryPageController(
        IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public IActionResult Index(
        ArticleListingPage currentPage,
        int page = 1)
    {
        var resources = _contentLoader
            .GetChildren<ResourcePage>(
                currentPage.ContentLink)
            .OrderByDescending(x => x.PublishedDate)
            .ToList();

        var pageSize = currentPage.PageSize;

        var pagedResources = resources
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();



        var model = new ResourceLibraryPageViewModel
        {
            CurrentPage = currentPage,
            Resources = resources,
            CurrentPageNumber = page,
            TotalPages = (int)Math.Ceiling(
                resources.Count / (double)pageSize)
        };

        return View(model);
    }
}
}