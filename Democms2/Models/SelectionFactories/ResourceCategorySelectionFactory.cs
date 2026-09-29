using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Democms2.SelectionFactories
{
    public class ResourceCategorySelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Guide", Value = "Guide" },
                new SelectItem { Text = "Template", Value = "Template" },
                new SelectItem { Text = "Whitepaper", Value = "Whitepaper" },
                new SelectItem { Text = "Checklist", Value = "Checklist" }
            };
        }
    }
}