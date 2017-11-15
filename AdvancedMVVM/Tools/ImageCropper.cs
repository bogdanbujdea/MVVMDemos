using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.Tools
{
    public class ImageCropper
    {
        public static async Task<ImageSource> CropFaceFromImage(SoftwareBitmap softwareBitmap, FaceRectangle faceRectangle)
        {
            return await GetCroppedBitmapAsync(softwareBitmap,
                new Point(faceRectangle.Left, faceRectangle.Top),
                new Size(faceRectangle.Width, faceRectangle.Height), 1);
        }

        private static async Task<ImageSource> GetCroppedBitmapAsync(SoftwareBitmap originalImgFile,
            Point startPoint, Size corpSize, double scale)
        {
            if (double.IsNaN(scale) || double.IsInfinity(scale))
            {
                scale = 1;
            }

            var startPointX = (uint)Math.Floor(startPoint.X * scale);
            var startPointY = (uint)Math.Floor(startPoint.Y * scale);
            var height = (uint)Math.Floor(corpSize.Height * scale);
            var width = (uint)Math.Floor(corpSize.Width * scale);
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, ms);
                encoder.SetSoftwareBitmap(originalImgFile);
                await encoder.FlushAsync();

                var arrayOriginal = new byte[ms.Size];
                await ms.ReadAsync(arrayOriginal.AsBuffer(), (uint)ms.Size, InputStreamOptions.None);

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ms);

                uint scaledWidth = (uint)Math.Floor(decoder.PixelWidth * scale);
                uint scaledHeight = (uint)Math.Floor(decoder.PixelHeight * scale);

                if (startPointX + width > scaledWidth)
                {
                    startPointX = scaledWidth - width;
                }


                if (startPointY + height > scaledHeight)
                {
                    startPointY = scaledHeight - height;
                }


                BitmapTransform transform = new BitmapTransform();
                BitmapBounds bounds = new BitmapBounds();
                bounds.X = startPointX;
                bounds.Y = startPointY;
                bounds.Height = height;
                bounds.Width = width;
                transform.Bounds = bounds;


                transform.ScaledWidth = scaledWidth;
                transform.ScaledHeight = scaledHeight;

                PixelDataProvider pix = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.ColorManageToSRgb);
                byte[] pixels = pix.DetachPixelData();


                WriteableBitmap cropBmp = new WriteableBitmap((int)width, (int)height);
                Stream pixStream = cropBmp.PixelBuffer.AsStream();
                pixStream.Write(pixels, 0, (int)(width * height * 4));


                return cropBmp;
            }
        }
    }
}
