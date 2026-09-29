using Democms2.Models.Pages.Base;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Democms2.Models.Pages
{
    [ContentType(
        DisplayName = "Search Results Page",
        GUID = "c1d2e3f4-5678-4a90-bcde-1234567890ab",
        Description = "Displays search results from Optimizely Find")]
    public class SearchResultsPage : SitePageBase
    {
        [Display(
            Name = "Page Title",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string PageTitle { get; set; }

        [Display(
            Name = "No Results Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string NoResultsText { get; set; }
    }
}