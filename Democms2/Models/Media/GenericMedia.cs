using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Democms2.Models.Media
{
    [ContentType(DisplayName = "Generic Media",GUID ="79663130-9448-4b76-9e4f-3ebdcc48ed02")]
    [MediaDescriptor(ExtensionString = "pdf,doc,docx,xlsx,pptx")]
    public class GenericMedia : MediaData
    {
    }
}