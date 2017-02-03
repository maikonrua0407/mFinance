using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace Utilities.Common
{
    /// <summary>
    /// Lớp xử lý dữ liệu ảnh
    /// </summary>
    public static class LImage
    {


        /// <summary>
        /// Hàm chuyển dữ liệu file ảnh sang dữ liệu binary
        /// </summary>
        /// <param name="imagePath">File ảnh với đường dẫn tuyệt đối</param>
        /// <returns>Dữ liệu binary của ảnh</returns>
        public static Stream GetStreamDataFromImage(string imagePath)
        {
            Stream streamImage = null;
            try
            {
                streamImage = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return streamImage;
            }
            catch(Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return streamImage;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu file ảnh sang dữ liệu byte[]
        /// </summary>
        /// <param name="imagePath">File ảnh với đường dẫn tuyệt đối</param>
        /// <returns>Dữ liệu binary của ảnh</returns>
        public static byte[] GetByteArrayFromImage(string imagePath)
        {
            byte[] byteArray = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                Stream streamImage = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                streamImage.CopyTo(ms);
                byteArray = ms.ToArray();
                return byteArray;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return byteArray;
        }

        /// <summary>
        /// Chuyển ảnh từ kiểu Image sang Stream theo format của ảnh
        /// </summary>
        /// <param name="img">Image cần chuyển sang stream</param>
        /// <returns></returns>
        public static Stream ConvertImageToStream(Image img)
        {
            Stream streamImage = null;
            try
            {
                img.Save(streamImage, img.RawFormat);
                return streamImage;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return streamImage;
        }

        /// <summary>
        /// Chuyển ảnh từ kiểu Image sang byte[] theo format của ảnh
        /// </summary>
        /// <param name="img">Image cần chuyển sang stream</param>
        /// <returns></returns>
        public static byte[] ConvertImageToByteArray(Image img)
        {
            byte[] byteArray = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                img.Save(ms, img.RawFormat);
                byteArray = ms.ToArray();
                return byteArray;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return byteArray;
        }

        /// <summary>
        /// Chuyển từ Stream sang byte[]
        /// </summary>
        public static byte[] ConvertStreamToByteArray(Stream stream)
        {
            byte[] byteArray = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                byteArray = ms.ToArray();
                return byteArray;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return byteArray;
        }

        /// <summary>
        /// Chuyển từ byte[] sang Stream
        /// </summary>
        public static Stream ConvertByteArrayToStream(byte[] byteArray)
        {
            Stream stream = null;
            try
            {
                stream = new MemoryStream(byteArray);
                return stream;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return stream;
        }

        /// <summary>
        /// Convert byte array to BitmapImage
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public static BitmapImage LoadImageFromByteArray(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh
        /// </summary>
        /// <param name="streamImage">Dữ liệu binary của ảnh</param>
        /// <param name="imagePath">Đường dẫn tuyệt đối lưu trữ ảnh</param>
        /// <param name="format">Định dạng</param>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromStreamData(Stream streamImage, string imagePath, ImageFormat format)
        {
            bool result = true;
            try
            {
                Image img = System.Drawing.Image.FromStream(streamImage);
                img.Save(imagePath, format); 
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh
        /// </summary>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromByteArray(byte[] byteArray, string imagePath, ImageFormat format)
        {
            bool result = true;
            try
            {
                MemoryStream ms = new MemoryStream(byteArray);
                Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, format);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh
        /// </summary>
        /// <param name="streamImage">Dữ liệu binary của ảnh</param>
        /// <param name="imagePath">Đường dẫn tuyệt đối lưu trữ ảnh</param>
        /// <param name="format">Định dạng</param>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromStreamData(Stream streamImage, string imagePath)
        {
            bool result = true;
            try
            {
                Image img = System.Drawing.Image.FromStream(streamImage);
                img.Save(imagePath, img.RawFormat);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu byte[] sang một file ảnh
        /// </summary>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromByteArray(byte[] byteArray, string imagePath)
        {
            bool result = true;
            try
            {
                MemoryStream ms = new MemoryStream(byteArray);
                Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, img.RawFormat);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh theo một size nào đó
        /// </summary>
        /// <param name="streamImage">Dữ liệu binary của ảnh</param>
        /// <param name="imagePath">Đường dẫn tuyệt đối lưu trữ ảnh</param>
        /// <param name="format">Định dạng</param>
        /// <param name="imageSize">Size của image khi lưu</param>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromStreamData(Stream streamImage, string imagePath, ImageFormat format, Size imageSize)
        {
            bool result = true;
            try
            {
                Image img = System.Drawing.Image.FromStream(streamImage);
                img = resizeImage(img, imageSize);
                img.Save(imagePath, format);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh theo một size nào đó
        /// </summary>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromByteArray(byte[] byteArray, string imagePath, ImageFormat format, Size imageSize)
        {
            bool result = true;
            try
            {
                MemoryStream ms = new MemoryStream(byteArray);
                Image img = System.Drawing.Image.FromStream(ms);
                img = resizeImage(img, imageSize);
                img.Save(imagePath, format);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh theo một size nào đó
        /// </summary>
        /// <param name="streamImage">Dữ liệu binary của ảnh</param>
        /// <param name="imagePath">Đường dẫn tuyệt đối lưu trữ ảnh</param>
        /// <param name="imageSize">Size của image khi lưu</param>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromStreamData(Stream streamImage, string imagePath, Size imageSize)
        {
            bool result = true;
            try
            {
                Image img = System.Drawing.Image.FromStream(streamImage);
                img = resizeImage(img, imageSize);
                img.Save(imagePath, img.RawFormat);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm chuyển dữ liệu binary sang một file ảnh theo một size nào đó
        /// </summary>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool WriteImageFromByteArray(byte[] byteArray, string imagePath, Size imageSize)
        {
            bool result = true;
            try
            {
                MemoryStream ms = new MemoryStream(byteArray);
                Image img = System.Drawing.Image.FromStream(ms);
                img = resizeImage(img, imageSize);
                img.Save(imagePath, img.RawFormat);
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return result;
        }

        /// <summary>
        /// Hàm thay đổi kích thước ảnh với dữ liệu dạng stream
        /// </summary>
        /// <param name="input">Stream cần resize</param>
        /// <param name="width">Độ rộng mới</param>
        /// <param name="height">Độ cao mới</param>
        public static Stream Resize(Stream input, int width, int height)
        {
            Stream streamOutput = null;
            try
            {
                Image img = System.Drawing.Image.FromStream(input);
                Size newSize = new Size(width,height);
                img = resizeImage(img, newSize);
                streamOutput = ConvertImageToStream(img);
                return streamOutput;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return streamOutput;
        }


        /// <summary>
        /// Hàm thay đổi kích thước ảnh với dữ liệu dạng byte[]
        /// </summary>
        /// <param name="input">byte[] cần resize</param>
        /// <param name="width">Độ rộng mới</param>
        /// <param name="height">Độ cao mới</param>
        public static byte[] Resize(byte[] input, int width, int height)
        {
            byte[] byteArray = null;
            try
            {
                Stream streamOutput = ConvertByteArrayToStream(input);
                Image img = System.Drawing.Image.FromStream(streamOutput);
                Size newSize = new Size(width, height);
                img = resizeImage(img, newSize);
                byteArray = ConvertImageToByteArray(img);
                return byteArray;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return byteArray;
        }


        /// <summary>
        /// Hàm thay đổi kích thước ảnh với dữ liệu ảnh là file đường dẫn tuyệt đối
        /// Ghi lại ảnh kích thước mới với một tên ảnh khác
        /// </summary>
        /// <param name="inputFile">Tên ảnh cần resize</param>
        /// <param name="outputFile">Tên ảnh với resize mới</param>
        /// <param name="width">Độ rộng mới</param>
        /// <param name="height">Độ cao mới</param>
        /// <returns>True >> thành công, False >> Không thành công</returns>
        public static bool Resize(string inputFile, string outputFile, int width, int height)
        {
            bool result = false;
            try
            {
                Image img = System.Drawing.Image.FromFile(inputFile);
                Size reSize = new Size(width, height);
                Image imgResize = resizeImage(img,reSize);
                imgResize.Save(outputFile);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Resize exception: " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Resize image
        /// </summary>
        /// <param name="imgToResize">Image cần resize</param>
        /// <param name="size">Size mới cho image</param>
        /// <returns>Image sau khi resize</returns>
        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.Default;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
