using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace Democms2.Models.Blocks
{

    [ContentType(DisplayName = "Quick Link Block", GUID = "73fe5109-a380-421a-8925-982b1f025167")]
    public class QuickLinkBlock : BlockData
    {
        [Required(ErrorMessage = "Link title is required.")]
        [CultureSpecific]
        [Display(
        Name = "Link Title",
        Description = "The text that will be displayed for the link.",
        Order = 10)]
        public virtual string LinkTitle { get; set; }

        [CultureSpecific]
        [Display(
        Name = "Link Url",
        Description = "The URL that the link will point to.",
        Order = 20)]
        public virtual Url LinkUrl { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Icon Class",
            Description = "The CSS class for the icon associated with the link.",
            Order = 30)]

        public virtual string IconClass { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Description",
            Description = "A brief description of the link.",
            Order = 40)]
        public virtual string Description { get; set; }
    }
}