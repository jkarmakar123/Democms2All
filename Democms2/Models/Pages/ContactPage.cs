 using System.ComponentModel.DataAnnotations;
using Democms2.Models.Pages.Base;



namespace Democms2.Models.Pages
{
   
[ContentType(
    DisplayName = "Contact Page",
    GUID = "b61c33ce-a595-4091-9b47-55f268284ab3")]
public class ContactPage : SitePageBase
{
    [CultureSpecific]
    [Display(
        Name = "Page Title",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string PageTitle { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Intro Text",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual XhtmlString IntroText { get; set; }

     [CultureSpecific]
     [Display(
        Name = "Main Content Area",
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual ContentArea  MainContentArea { get; set; }
}
}