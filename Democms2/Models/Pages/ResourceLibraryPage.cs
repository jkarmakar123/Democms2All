using Democms2.Models.Pages;
using EPiServer.DataAnnotations;

namespace Democms2.Models.Pages
{
    [ContentType(
        DisplayName = "ResourceLibrary Page",
        GUID = "a8ab59d1-c0cd-4ab5-8c9c-619a5865a648")]
    public class ResourceLibraryPage : ArticleListingPage
    {
    }
}