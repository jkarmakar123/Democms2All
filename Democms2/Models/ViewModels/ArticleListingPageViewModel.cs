using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Democms2.Models.Media;
using Democms2.Models.Pages;
using EPiServer.Web;

namespace Democms2.Models.ViewModels
{
    public class ArticleListingPageViewModel
    {
        public ArticleListingPage CurrentPage { get; set; }

        // public IEnumerable<ListingItemViewModel> Articles { get; set; }

        public int CurrentPageNumber { get; set; }

        public int TotalPages { get; set; }
    }

    public class KnowledgeBasePageViewModel : ArticleListingPageViewModel
    {
        public IEnumerable<KnowledgeArticlePage> Articles { get; set; } = Enumerable.Empty<KnowledgeArticlePage>();

        public IEnumerable<string> AvailableTags { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> AvailableArticleCategories { get; set; } = Enumerable.Empty<string>();

        public string? SelectedTag { get; set; }

        public string? SelectedArticleCategory { get; set; }
    }

    public class ResourceLibraryPageViewModel : ArticleListingPageViewModel
    {
        public IEnumerable<ResourcePage> Resources { get; set; }
    }




}
