using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.Common;
using Presentation.Process;
using PresentationWPF.CustomControl;

namespace PresentationWPF.ZATestApp.Communication
{
    /// <summary>
    /// Interaction logic for ucDataTransfer.xaml
    /// </summary>
    public partial class ucDataTransfer : UserControl
    {
        public ucDataTransfer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnImage1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            try
            {
                dlg.FileName = "Document";
                dlg.DefaultExt = ".jpg";
                dlg.Filter = "Image (.jpg)|*.jpg";

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Lưu đường dẫn
                    txtLogo1.Text = dlg.SafeFileName;
                    txtLogo1.Tag = dlg.FileName;
                    // Hiển thị logo
                    LoadImage1();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                dlg = null;
            }
        }

        private void rbtnImage2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            try
            {
                dlg.FileName = "Document";
                dlg.DefaultExt = ".jpg";
                dlg.Filter = "Image (.jpg)|*.jpg";

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Lưu đường dẫn
                    txtLogo2.Text = dlg.SafeFileName;
                    txtLogo2.Tag = dlg.FileName;
                    // Hiển thị logo
                    LoadImage2();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                dlg = null;
            }
        }

        private void rbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy dữ liệu ảnh 1
                string img1Path = txtLogo1.Tag.ToString();
                byte[] img1 = LImage.GetByteArrayFromImage(img1Path);

                // Lấy dữ liệu ảnh 2
                string img2Path = txtLogo2.Tag.ToString();
                byte[] img2 = LImage.GetByteArrayFromImage(img2Path);

                ZATestAppProcess process = new ZATestAppProcess();
                process.processDataTransfer(img1, img2);
            }
            catch (Exception ex)
            {
                //this.Close();
                //Application.Current.Shutdown();
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                else
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.GetType() == typeof(CustomException))
                            new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                        else
                            new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
                    }
                    else
                        new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
                }
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xử lý load image theo đường dẫn truyền vào
        /// </summary>
        private void LoadImage1()
        {
            // Tạo image source
            BitmapImage myBitmapImage = new BitmapImage();

            // Set image vào image box
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(txtLogo1.Tag.ToString());
            myBitmapImage.DecodePixelWidth = (int)brdLogo1.ActualWidth;
            myBitmapImage.DecodePixelHeight = (int)brdLogo1.ActualHeight;
            myBitmapImage.EndInit();
            imgLogo1.Source = myBitmapImage;
        }

        /// <summary>
        /// Xử lý load image theo đường dẫn truyền vào
        /// </summary>
        private void LoadImage2()
        {
            // Tạo image source
            BitmapImage myBitmapImage = new BitmapImage();

            // Set image vào image box
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(txtLogo2.Tag.ToString());
            myBitmapImage.DecodePixelWidth = (int)brdLogo2.ActualWidth;
            myBitmapImage.DecodePixelHeight = (int)brdLogo2.ActualHeight;
            myBitmapImage.EndInit();
            imgLogo2.Source = myBitmapImage;
        }

        /// <summary>
        /// Xử lý sự kiện double click vào image để chọn ảnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgLogo_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                try
                {
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image (.jpg)|*.jpg";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        // Lưu đường dẫn
                        txtLogo1.Text = dlg.SafeFileName;
                        txtLogo1.Tag = dlg.FileName;
                        // Hiển thị logo
                        LoadImage1();
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        /// <summary>
        /// Xử lý sự kiện double click vào image để chọn ảnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgLogo_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                try
                {
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image (.jpg)|*.jpg";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        // Lưu đường dẫn
                        txtLogo2.Text = dlg.SafeFileName;
                        txtLogo2.Tag = dlg.FileName;
                        // Hiển thị logo
                        LoadImage2();
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }
    }
}
