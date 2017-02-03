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
using System.Windows.Shapes;
using PresentationWPF.CustomControl;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.Common;
using System.Threading;
using System.Globalization;
using Presentation.Process;
using Utilities.Common;

namespace PresentationWPF.ZAMainApp
{
    /// <summary>
    /// Interaction logic for InitSession.xaml
    /// </summary>
    public partial class InitSession : Window
    {
        public List<DM_DON_VI> ListPhongGD;

        public InitSession()
        {
            InitializeComponent();
            CommonFunction.setIcon(this);
        }

        public InitSession(List<DM_DON_VI> listPhongGD) : this()
        {
            this.ListPhongGD = listPhongGD;
            LoadCombobox();
            ResetForm();
        }

        private void LoadCombobox()
        {
            if (ListPhongGD != null && ListPhongGD.Count > 0)
            {
                cmbPhongGD.Items.Clear();
                foreach (DM_DON_VI item in ListPhongGD)
                {
                    cmbPhongGD.Items.Add(new AutoCompleteEntry(item.TEN_GDICH, item.MA_DVI, item.MA_HACH_TOAN, item.ID.ToString(), item.MA_DVI_CHA, item.MA_TINHTP));
                }
                cmbPhongGD.SelectedIndex = 0;
            }
        }

        private void ResetForm()
        {
            if (ListPhongGD != null && ListPhongGD.Count > 0)
            {
                lblInform.Visibility = Visibility.Visible;
                lblWarning.Visibility = Visibility.Collapsed;
                //grdMain.RowDefinitions[2].Height = new GridLength(0);
            }
            else
            {
                lblInform.Visibility = Visibility.Collapsed;
                lblWarning.Visibility = Visibility.Visible;
                //grdMain.RowDefinitions[2].Height = new GridLength(25);
            }
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            AutoCompleteEntry ace = (AutoCompleteEntry)cmbPhongGD.SelectedItem;
            if (ace != null)
            {
                string id = ace.KeywordStrings[2];
                string ma = ace.KeywordStrings[0];
                string ten = ace.DisplayName;
                string hachtoan = ace.KeywordStrings[1];
                string maDonViCha = ace.KeywordStrings[3];
                string maTinhThanhPho = ace.KeywordStrings[4];
                string soLuongBGhi = "";

                ClientInformation.TenDonViGiaoDich = ten;
                ClientInformation.MaDonViGiaoDich = ma;
                ClientInformation.IdDonViGiaoDich = Int32.Parse(id);
                ClientInformation.PhuongPhapHachToan = hachtoan;
                ClientInformation.TinhTPDonViGiaoDich = maTinhThanhPho;

                // Nếu đơn vị vận hành khác đơn vị quản lý
                // 1. Lấy thông tin chi nhánh của phòng giao dịch được chọn
                // 2. Lấy thông tin ngày làm việc phòng giao dịch được chọn
                if (maDonViCha != null && !maDonViCha.Equals(ClientInformation.MaDonVi))
                {
                    DanhMucProcess process = new DanhMucProcess();
                    ApplicationConstant.ResponseStatus ret = new ApplicationConstant.ResponseStatus();
                    Presentation.Process.DanhMucServiceRef.DM_DON_VI dmDonVi = new Presentation.Process.DanhMucServiceRef.DM_DON_VI();
                    Presentation.Process.DanhMucServiceRef.HT_NGAY_LVIEC ngayLamViec = new Presentation.Process.DanhMucServiceRef.HT_NGAY_LVIEC();
                    string responseMessage = "";

                    ret = process.GetDonViChaByMaDonVi(ma, ref dmDonVi, ref ngayLamViec, ref responseMessage);
                    if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        ClientInformation.IdDonVi = dmDonVi.ID;
                        ClientInformation.MaDonVi = dmDonVi.MA_DVI;
                        ClientInformation.TenDonVi = dmDonVi.TEN_GDICH;

                        ClientInformation.NgayLamViecTruoc = ngayLamViec.NGAY_TRUOC;
                        ClientInformation.NgayLamViecHienTai = ngayLamViec.NGAY_LVIEC;
                        ClientInformation.NgayLamViecSau = ngayLamViec.NGAY_TTHEO;
                    }
                    process = null;
                }
                soLuongBGhi = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SOLUONG_BANGHI, ClientInformation.MaDonVi);
                if (soLuongBGhi.IsNullOrEmptyOrSpace())
                    soLuongBGhi = "10";
                ClientInformation.SoLuongBanGhi = soLuongBGhi.StringToInt32();
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Cursor = Cursors.Arrow;
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.Release();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Cursor = Cursors.Arrow;
            Close();
        }

    
    }
}
