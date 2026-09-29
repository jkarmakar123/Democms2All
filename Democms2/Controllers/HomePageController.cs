using Democms2.Models.Pages;
using Democms2.Models.ViewModels;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EPiServer.Core;
using EPiServer;

namespace Democms2.Controllers
{
    public class HomePageController : PageController<HomePage>
    {
private readonly IContentLoader _contentLoader;
 public HomePageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IActionResult Index(HomePage currentPage)
        {
            var featured =
        currentPage.FeaturedArticles?
            .Items
            .Select(x => _contentLoader.Get<KnowledgeArticlePage>(x.ContentLink))
           .Where(x => x != null && x.IsFeatured)
            .ToList();

    var viewModel = new HomePageViewModel
    {
        CurrentPage = currentPage,
        FeaturedArticlesPages = featured ?? []
    };

    return View(viewModel);
           
        }
    }
}