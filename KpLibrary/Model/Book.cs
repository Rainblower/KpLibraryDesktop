using System.Windows.Media;
using KpLibrary.Convertors;
using Newtonsoft.Json;
using ZXing;
using ZXing.QrCode;

namespace KpLibrary.Model
{
    public class Book
    {
        [JsonProperty("imageUrl")] public string ImageUrl { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("uid")] public string Uid { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonIgnore]
        public ImageSource QrCode
        {
            get
            {
                var writer = new QRCodeWriter();
                var qBitMatrix = writer.encode(
                    Url,
                    BarcodeFormat.QR_CODE,
                    500,
                    500
                );
                var qrWriter = new BarcodeWriter();

                var bitmap = qrWriter.Write(qBitMatrix);

                return BitmapConverter.Convert(bitmap);
            }
        }

        [JsonIgnore]
        public string GetImage
        {
            get
            {
                if (ImageUrl.Contains("https://firebasestorage.googleapis.com/v0/b/kotlinlibrary"))
                    return ImageUrl;
                return "/KpLibrary;component/Resources/Images/book.png";
            }
        }
    }
}