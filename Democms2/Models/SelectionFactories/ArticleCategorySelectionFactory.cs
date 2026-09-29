using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Democms2.Models.SelectionFactories
{
    public class ArticleCategorySelectionFactory : ISelectionFactory
    {
        public static readonly IReadOnlyList<string> Categories = new List<string>
        {
            "Technical",
            "Business",
            "Operations",
            "General"
        };

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return Categories.Select(category => new SelectItem
            {
                Text = category,
                Value = category
            });
        }
    }
}
