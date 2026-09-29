using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Democms2.Models.Media
{
    [ContentType(
        DisplayName = "Image File",
        GUID = "0a54ec3d-3b93-4f21-9e36-4e53618769d4",
        Description = "Used for image uploads in the media library.")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,png,gif,bmp,webp,ico")]
    public class ImageFile : ImageData
    {
    }
}
