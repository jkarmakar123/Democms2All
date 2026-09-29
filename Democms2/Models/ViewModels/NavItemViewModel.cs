
namespace Democms2.Models.ViewModels
{   
public class NavItemViewModel
{
    public string Title { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public List<NavItemViewModel> Children { get; set; } = new();
}
}