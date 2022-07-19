using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;

namespace HHITtoolsTray.USBQRCode
{
    public class USBQRCodeHelp
    {
        public static BitmapImage GenerateBitmapImage(string text)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;

            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 120;
            options.Height = 120;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;
            writer.Options = options;

            BitmapImage bitmapimage = new BitmapImage();

            using (Bitmap bitmap = writer.Write(text))
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();              
            }

            return bitmapimage;
        }
    }
}
