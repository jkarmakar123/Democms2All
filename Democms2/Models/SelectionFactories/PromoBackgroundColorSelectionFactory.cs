using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Democms2.Models.SelectionFactories
{   
public class PromoBackgroundColorSelectionFactory : ISelectionFactory
{
    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {
        return new List<SelectItem>
        {
            new SelectItem { Text = "Blue", Value = "Blue" },
            new SelectItem { Text = "Green", Value = "Green" },
            new SelectItem { Text = "Orange", Value = "Orange" },
            new SelectItem { Text = "Gray", Value = "Gray" }
        };
    }
}
}