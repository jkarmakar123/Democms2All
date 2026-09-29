
using Democms2.Models.Media;
using Democms2.Models.Pages.Base;
using Democms2.SelectionFactories;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using System;
using System.ComponentModel.DataAnnotations;

namespace Democms2.Models.Pages
{
    [ContentType(
        DisplayName = "Resource Page",
        GUID = "757c80eb-c68c-4bec-9340-e5f557183462",
        Description = "Represents a single downloadable resource in the Resource Library")]
    public class ResourcePage : SitePageBase
    {
        [Required]
        [Display(Name = "Resource Title", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string ResourceTitle { get; set; }

        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Description { get; set; }

        [Display(Name = "Resource File", GroupName = SystemTabNames.Content, Order = 30)]
        [UIHint(UIHint.MediaFile)] // helps pick from Media Library
        [AllowedTypes(typeof(GenericMedia))] // restrict to certain media types
        public virtual ContentReference ResourceFile { get; set; }

        [Display(Name = "Category", GroupName = SystemTabNames.Content, Order = 40)]
       [SelectOne(SelectionFactoryType = typeof(ResourceCategorySelectionFactory))]
        public virtual string ResourceCategory { get; set; }

        [Display(Name = "Thumbnail Image", GroupName = SystemTabNames.Content, Order = 50)]
        [UIHint(UIHint.Image)]
        [AllowedTypes(typeof(ImageFile))]
        public virtual ContentReference ThumbnailImage { get; set; }

        [Display(Name = "Published Date", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual DateTime PublishedDate { get; set; }
    }

    // Strongly recommended instead of string SelectOne
    // public enum ResourceCategory
    // {
    //     Guide,
    //     Template,
    //     Whitepaper,
    //     Checklist
    // }
}