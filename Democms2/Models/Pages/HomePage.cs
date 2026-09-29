using Democms2.Models.Blocks;
using Democms2.Models.Pages.Base;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;


namespace Democms2.Models.Pages
{
[ContentType(
    DisplayName = "Home Page",
    GUID = "c504c479-1789-4dea-945c-f81931adcc6b")]
public class HomePage : SitePageBase
{
    [CultureSpecific]
    [Display(
        Name = "Page Title",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string PageTitle { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Hero Area",
        GroupName = SystemTabNames.Content,
        Order = 20)]
        [AllowedTypes(typeof(HeroBlock))]
    public virtual ContentArea HeroArea { get; set; }

[CultureSpecific]
    [Display(
        Name = "/contenttypes/homepage/properties/featuredarticles/caption",
        GroupName = SystemTabNames.Content,
        Order = 30)]
         [AllowedTypes(typeof(KnowledgeArticlePage),ErrorMessage = "Only knowledge article page allowed")]
    public virtual ContentArea FeaturedArticles { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Quick Links",
        GroupName = SystemTabNames.Content,
        Order = 40)]
            [AllowedTypes(typeof(QuickLinkBlock))]
    public virtual ContentArea QuickLinksArea { get; set; }
}
}