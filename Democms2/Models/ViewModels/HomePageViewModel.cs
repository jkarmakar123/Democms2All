
using Democms2.Models.Pages;

namespace Democms2.Models.ViewModels
{
public class HomePageViewModel
{
    public HomePage CurrentPage { get; set; }
    public List<KnowledgeArticlePage> FeaturedArticlesPages { get; set; }
}
}