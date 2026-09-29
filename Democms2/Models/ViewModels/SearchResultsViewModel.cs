using System.Collections.Generic;
using Democms2.Models.Pages;

namespace Democms2.Models.ViewModels
{
    public class SearchResultsViewModel
    {
        public SearchResultsPage CurrentPage { get; set; }

        public string Query { get; set; }

        public int TotalResults { get; set; }

        public IEnumerable<SearchResultItem> Results { get; set; }
    }

    public class SearchResultItem
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string ContentType { get; set; }

        public string Url { get; set; }

        public DateTime PublishedDate { get; set; }
    }
}