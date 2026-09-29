using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Democms2.Models.SelectionFactories;
using Democms2.Models.Pages.Base;
using EPiServer.Shell.ObjectEditing;

namespace Democms2.Models.Pages
{
    [ContentType(
        DisplayName = "Knowledge Article Page",
        GUID = "e4e33610-2805-4e12-801e-dd2c48825ee2",
        Description = "Knowledge base article page.")]
    public class KnowledgeArticlePage : SitePageBase
    {
        [CultureSpecific]
        [Required]
        [Display(
            Name = "Article Title",
            Description = "Title displayed as H1 on the article page.",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string ArticleTitle { get; set; }

        [CultureSpecific]
        [StringLength(200)]
        [Display(
            Name = "Summary",
            Description = "Short description shown on article cards.",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string Summary { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Body",
            Description = "Full article content.",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString Body { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Author",
            Description = "Name of the article author.",
            GroupName = "ArticleInformation",
            Order = 40)]
        public virtual string Author { get; set; }

            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(
            Name = "Published Date",
            Description = "Date displayed to visitors and used for sorting.",
            GroupName = "ArticleInformation",
            Order = 50)]
        public virtual DateTime PublishedDate { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Tags",
            Description = "Tags used for filtering articles.",
            GroupName = "ArticleInformation",
            Order = 60)]
        public virtual IList<string> Tags { get; set; } = new List<string>();

        [SelectOne(SelectionFactoryType = typeof(ArticleCategorySelectionFactory))]
        [Display(
            Name = "Category",
            Description = "Select article category.",
            GroupName = "ArticleInformation",
            Order = 70)]
        public virtual string ArticleCategory { get; set; }

        [UIHint(UIHint.Image)]
        [Display(
            Name = "Thumbnail Image",
            Description = "Image shown on article listing cards.",
            GroupName = "Media",
            Order = 80)]
        public virtual ContentReference ThumbnailImage { get; set; }

        [Display(
            Name = "Featured Article",
            Description = "Show article on Home Page featured section.",
            GroupName = "ArticleInformation",
            Order = 90)]
        public virtual bool IsFeatured { get; set; }

        [Range(1, 999)]
        [Display(
            Name = "Read Time (Minutes)",
            Description = "Estimated reading time.",
            GroupName = "ArticleInformation",
            Order = 100)]
        public virtual int ReadTimeMinutes { get; set; }
    }
}
