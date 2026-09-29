using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace Democms2.Models.Blocks
{
    [ContentType(DisplayName = "Hero Block", GUID = "a4c04dc3-3685-41a3-aaa2-35493fd931d1")]
    public class HeroBlock : BlockData
    {
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        public virtual string SubText { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        public virtual string CTAButtonText { get; set; }

        [CultureSpecific]
        public virtual Url CTAButtonLink { get; set; }
    }
}