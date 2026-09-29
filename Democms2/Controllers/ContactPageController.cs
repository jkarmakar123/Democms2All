
using Democms2.Models.Pages;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;


namespace Democms2.Controllers
{
   public class ContactPageController : PageController<ContactPage>
{
    public IActionResult Index(ContactPage currentPage)
    {
        return View(currentPage);
    }
}
}