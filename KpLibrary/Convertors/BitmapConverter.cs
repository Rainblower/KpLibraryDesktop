using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace KpLibrary.Convertors
{
    public static class BitmapConverter
    {
        /// <summary>
        ///     Convert bitmap to ImageSource
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static BitmapImage Convert(Bitmap src)
        {
            var ms = new MemoryStream();
            src.Save(ms, ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}