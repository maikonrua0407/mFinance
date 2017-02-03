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

namespace PresentationWPF.KhachHang.QuanLyHinhAnh
{
    /// <summary>
    /// Interaction logic for SlideImage.xaml
    /// </summary>
    public partial class SlideImage : UserControl
    {
        public List<DuLieuHinhAnh> lstDuLieuHinhAnh
        {
            get;
            set;
        }

        private int _currentIndex = -1;

        public SlideImage()
        {
            InitializeComponent();
            SetEnableControl();
        }

        public void SetImage()
        {
            try
            {
                if (lstDuLieuHinhAnh != null && lstDuLieuHinhAnh.Count > 0 && _currentIndex <= lstDuLieuHinhAnh.Count - 1)
                {
                    if (_currentIndex < 0)
                    {
                        _currentIndex = 0;
                    }
                    imgKhachHang.Source = LImage.LoadImageFromByteArray(lstDuLieuHinhAnh[_currentIndex].Data);
                    SetEnableControl();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex--;
            SetImage();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            SetImage();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.Width = 1024;
            window.Height = 768;

            Grid mainGrd = new Grid();
            Image img = new Image();
            img.Source = LImage.LoadImageFromByteArray(lstDuLieuHinhAnh[_currentIndex].Data);
            mainGrd.Children.Add(img);

            window.Content = mainGrd;
            window.ShowDialog();
        }

        private void SetEnableControl()
        {
            if (lstDuLieuHinhAnh == null || lstDuLieuHinhAnh.Count == 0)
            {
                lblImageName.Content = "";
                btnBack.IsEnabled = false;
                btnNext.IsEnabled = false;
                btnShow.IsEnabled = false;
            }
            else
            {
                string imageName = lstDuLieuHinhAnh[_currentIndex].MaHinhAnh;
                string imageDate = LDateTime.StringToDate(lstDuLieuHinhAnh[_currentIndex].NgayHieuLuc, ApplicationConstant.defaultDateTimeFormat).ToString("dd/MM/yyyy");
                string content = imageName + " [" + imageDate + "]";

                if (_currentIndex == -1)
                {
                    btnBack.IsEnabled = false;
                    btnNext.IsEnabled = false;
                    btnShow.IsEnabled = false;
                }
                else if (_currentIndex == 0)
                {
                    lblImageName.Content = content;
                    btnBack.IsEnabled = false;
                    btnNext.IsEnabled = true;
                    btnShow.IsEnabled = true;
                }
                else if (_currentIndex == lstDuLieuHinhAnh.Count - 1)
                {
                    lblImageName.Content = content;
                    btnBack.IsEnabled = true;
                    btnNext.IsEnabled = false;
                    btnShow.IsEnabled = true;
                }
                else
                {
                    lblImageName.Content = content;
                    btnBack.IsEnabled = true;
                    btnNext.IsEnabled = true;
                    btnShow.IsEnabled = true;
                }
            }
        }
    }
}
