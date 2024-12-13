using MediatR;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PrinterAnaliz.Application.Extensions
{
    public static class UploadFileExtension
    {
        public async static void UploadImage(IFormFile img, string FilePath, string FileName, int Width = 0, int Height = 0, bool isFixed = false, int x = 0, int y = 0) {

            using (var memoryStream = new MemoryStream())
            {
                await img.CopyToAsync(memoryStream);
                using (var imgFromStream = Image.FromStream(memoryStream))
                { 
                    UploadImage(imgFromStream, FilePath, FileName, Width, Height, isFixed, x, y);
                }
            }

        }
        public static void UploadImage(Image img, string FilePath, string FileName, int Width = 0, int Height = 0, bool isFixed = false, int x = 0, int y = 0)
        {
            if (isFixed)
            {
                using (Image thumbnail = UploadFileExtension.Inscribe(img, Width))
                {
                    UploadFileExtension.SaveToJpeg(thumbnail, FilePath + FileName);
                }
            }
            else
            {
                var Yukseklik = (img.Height * Width) / img.Width;

                if (Height > 0)
                    Yukseklik = Height;

                if (x > 0 || y > 0)
                {
                    using (Image thumbnail = UploadFileExtension.Inscribe(img, Width, Yukseklik, x, y))
                    {
                        UploadFileExtension.SaveToJpeg(thumbnail, FilePath + FileName);
                    }
                }
                else
                {
                    using (Image thumbnail = UploadFileExtension.Inscribe(img, Width, Yukseklik))
                    {
                        UploadFileExtension.SaveToJpeg(thumbnail, FilePath + FileName);
                    }
                }

            }
        }
        public static Image Inscribe(Image image, int size)
        {

            return Inscribe(image, size, size);

        }

        public static Image Inscribe(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                double factor = 1.0 * width / image.Width;
                if (image.Height * factor < height)
                    factor = 1.0 * height / image.Height;
                Size size = new Size((int)(width / factor), (int)(height / factor));
                Point sourceLocation = new Point((image.Width - size.Width) / 2, (image.Height - size.Height) / 2);

                SmoothGraphics(graphics);
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(sourceLocation, size), GraphicsUnit.Pixel);
            }
            return result;
        }
        public static Image Inscribe(Image image, int width, int height, int x, int y)
        {

            Bitmap result = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                SmoothGraphics(graphics);
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }
            return result;
        }

        static void SmoothGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;//anti;
            g.InterpolationMode = InterpolationMode.High;//HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        public static void SaveToJpeg(Image image, Stream output)
        {
            image.Save(output, ImageFormat.Jpeg);
        }

        public static void SaveToJpeg(Image image, string fileName)
        {
            image.Save(fileName, ImageFormat.Jpeg);
        }
    }
}
