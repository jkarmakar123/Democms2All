using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;


namespace Democms2.Models.Pages.Base
{   

public abstract class SitePageBase : PageData
{
    [Display(GroupName = SystemTabNames.Settings)]
    public virtual string MetaTitle { get; set; }

    [Display(GroupName = SystemTabNames.Settings)]
    public virtual string MetaDescription { get; set; }

    [Display(GroupName = SystemTabNames.Settings)]
    public virtual Url CanonicalUrl { get; set; }

    [Display(GroupName = SystemTabNames.Settings)]
    public virtual bool HideFromNavigation { get; set; }
}
}