using OneTWebApp.Client.Models;

namespace OneTWebApp.Client.Utils;

public static class ImageUtils {
    public static string GetIconAppPath(AppType type) => type switch {
        AppType.Docs => "Images/icon-docs.png",
        AppType.Meet => "Images/icon-visio.png",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}