using Democms2.Models.ViewModels;

namespace Democms2.Services
{
public interface INavService
{
   public List<NavItemViewModel> GetMainNavigation(ContentReference rootLink, ContentReference currentLink);
   public List<NavItemViewModel> GetBreadcrumb(ContentReference currentLink);
}}