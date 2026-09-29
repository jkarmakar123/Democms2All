using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Democms2.Models.Pages;

namespace Democms2.Controllers
{
    public class KnowledgeArticlePageController : PageController<KnowledgeArticlePage>
    {
        public IActionResult Index(KnowledgeArticlePage currentPage)
        {
            return View(currentPage);
        }
    }
}