using Microsoft.UI.Xaml.Media.Imaging;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ChromiumBasedAppLauncherGUI.Helpers;

public class IconHelper
{
    public static BitmapImage ExtractAssociatedIconBitmapImage(string path)
    {
        MemoryStream stream = new();
        Icon.ExtractAssociatedIcon(path)!.ToBitmap().Save(stream, ImageFormat.Png);
        stream.Position = 0;
        BitmapImage bitmapImage = new();
        bitmapImage.SetSource(stream.AsRandomAccessStream());
        return bitmapImage;
    }
}
