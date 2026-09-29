using System.ComponentModel.DataAnnotations;
using Democms2.Models.SelectionFactories;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;

namespace Democms2.Models.Blocks
{
    [ContentType(
        DisplayName = "Promo Block",
        GUID = "a26135cd-5fea-4021-b1c3-ec78be774958",
        Description = "Highlighted promotional or announcement block that can be placed in content areas.")]
    public class PromoBlock : BlockData
    {
        [CultureSpecific]
        [Required]
        [Display(
            Name = "Title",
            Description = "Bold title displayed at the top of the promotional block.",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Body",
            Description = "Rich text content for the promotional message.",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString Body { get; set; }

       [SelectOne(SelectionFactoryType = typeof(PromoBackgroundColorSelectionFactory))]
        [Display(
            Name = "Background Color",
            Description = "Select the background color for the promo block.",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual string BackgroundColor { get; set; }
        
        [CultureSpecific]
        [Display(
            Name = "CTA Text",
            Description = "Optional call-to-action button label.",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual string CTAText { get; set; }

        [Display(
            Name = "CTA Link",
            Description = "Optional destination URL for the call-to-action button.",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual Url CTALink { get; set; }
    }
}