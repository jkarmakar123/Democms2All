using System.ComponentModel.DataAnnotations;
using Democms2.Models.Pages.Base;
using EPiServer.DataAnnotations;

namespace Democms2.Models.Pages
{
    [ContentType(
        DisplayName = "Article Listing Page",
        GUID = "2e9469fd-aea4-4ccd-8080-a98ca259ef97",
        Description = "Lists all child knowledge articles.")]
    public class ArticleListingPage : SitePageBase
    {
        [CultureSpecific]
        [Display(
            Name = "Page Title",
            Description = "Title shown at the top of the listing page.",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string PageTitle { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Introduction Text",
            Description = "Intro paragraph displayed above the article listing.",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string IntroText { get; set; }

        [Range(1, 100)]
        [Display(
            Name = "Page Size",
            Description = "Number of articles displayed per page.",
            GroupName = SystemTabNames.Settings,
            Order = 30)]
        public virtual int PageSize { get; set; }

         public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            PageSize = 9;
        }
    }
}