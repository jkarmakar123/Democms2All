using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Democms2.Models.Pages;

namespace Democms2.Controllers
{
    public class ResourcePageController : PageController<ResourcePage>
    {
        public IActionResult Index(ResourcePage currentPage)
        {
            return View(currentPage);
        }
    }
}