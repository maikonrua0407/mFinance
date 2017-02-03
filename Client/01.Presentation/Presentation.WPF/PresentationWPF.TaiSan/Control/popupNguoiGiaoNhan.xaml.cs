using System;
using System.Collections.Generic;
using System.Data;
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
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.QuanTriHeThongServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TaiSanServiceRef;

namespace PresentationWPF.TaiSan.Control
{
    /// <summary>
    /// Interaction logic for popupNguoiGiaoNhan.xaml
    /// </summary>
    public partial class popupNguoiGiaoNhan : UserControl
    {
        #region Khai bao
        // khai báo 1 hàm delegate
        public delegate void LayDuLieu(List<TS_BAN_GIAO_CT_GIAO_NHAN> lst);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;

        List<AutoCompleteEntry> lstSourceNguoiBanGiao = new List<AutoCompleteEntry>();

        private DataTable _dtSource = null;
        public DataTable dtSource
        {
            get { return _dtSource; }
            set { _dtSource = value; }
        }

        private int idTuSinh = 0;
        public int idSinhMa
        {
            set { idTuSinh = value; }
        }
        private bool isUpdate = false;
        string maChucVu = "";
        string tenChucVu = "";
        #endregion

        #region Khoi tao
        public popupNguoiGiaoNhan()
        {
            InitializeComponent();
            LoadDuLieuCombobox();
            KhoiTaoDataSource();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getThongTinKhac("VKH_NGUOI_DDIEN");
            if (ds != null && ds.Tables.Count > 0)
            {
                _dtSource = ds.Tables[0];
            }
        }

        /// <summary>
        /// Load du lieu vao combobox
        /// </summary>
        private void LoadDuLieuCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            //Load combobox cán bộ quản lý - Tab thông tin chung - Group thông tin khác
            lstDieuKien.Clear();
            lstDieuKien.Add("%");
            lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CHINH_THUC.layGiaTri());
            lstDieuKien.Add(ClientInformation.MaDonVi);
            auto.GenAutoComboBox(ref lstSourceNguoiBanGiao, ref cmbNSD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUOI_BAN_GIAO.getValue(), lstDieuKien);
            cmbNSD.SelectedIndex = lstSourceNguoiBanGiao.IndexOf(lstSourceNguoiBanGiao.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
            ////Load combobox NSD
            //List<HT_NSD> lst = process.layNSD().Where(e=>e.MA_DVI_QLY.Equals(ClientInformation.MaDonViQuanLy)).ToList();
            //lstSourceNSD = new List<AutoCompleteEntry>();
            //foreach(HT_NSD item in lst)
            //{
            //    lstSourceNSD.Add(new AutoCompleteEntry(item.TEN_DAY_DU, item.MA_DANG_NHAP, item.ID.ToString()));
            //}
            //auto.GenAutoComboBox(ref lstSourceNSD, ref cmbNSD, "");

        }
        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Dong form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void cmbNSD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auNSD = au.getEntryByDisplayName(lstSourceNguoiBanGiao, ref cmbNSD);
            maChucVu = auNSD.KeywordStrings[1].SplitByDelimiter("#")[0];
            tenChucVu = auNSD.KeywordStrings[1].SplitByDelimiter("#")[1];
            lbChucVu.Content = tenChucVu;
        }

        private void rbBenGiao_Checked(object sender, RoutedEventArgs e)
        {
            txtDaiDienKhac.IsEnabled = false;
        }

        private void rbBenNhan_Checked(object sender, RoutedEventArgs e)
        {
            txtDaiDienKhac.IsEnabled = false;
        }

        private void rbKhac_Checked(object sender, RoutedEventArgs e)
        {
            txtDaiDienKhac.IsEnabled = true;
        }

        /// <summary>
        /// Su kien nhan key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(e, this);
            CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Đưa các control của form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            isUpdate = false;
            txtDaiDienKhac.Text = "";
            rbBenGiao.IsChecked = false;
            rbBenNhan.IsChecked = false;
            rbKhac.IsChecked = false;
            cmbNSD.Focus();
        }

        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Load du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_dtSource != null)
            {
                grbChiTiet.IsEnabled = true;
            }
            else
            {
                grbChiTiet.IsEnabled = false;
            }
            cmbNSD.SelectionChanged += cmbNSD_SelectionChanged;
        }

        /// <summary>
        /// Kiem tra du lieu truoc khi luu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auNSD = au.getEntryByDisplayName(lstSourceNguoiBanGiao, ref cmbNSD);
            if (auNSD == null)
            {
                CommonFunction.ThongBaoTrong("Chưa chọn người sử dụng");
                cmbNSD.Focus();
                return false;
            }
            else if (rbBenGiao.IsChecked == false && rbBenNhan.IsChecked == false && rbKhac.IsChecked == false)
            {
                CommonFunction.ThongBaoTrong("Chưa chọn đại diện");
                return false;
            }
            else if (rbKhac.IsChecked == true && txtDaiDienKhac.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong("Chưa nhập thông tin đại diện khác");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Luu du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    AutoComboBox au = new AutoComboBox();
                    AutoCompleteEntry auNSD = au.getEntryByDisplayName(lstSourceNguoiBanGiao, ref cmbNSD);

                    if (DuLieuTraVe != null)
                    {
                        List<TS_BAN_GIAO_CT_GIAO_NHAN> list = new List<TS_BAN_GIAO_CT_GIAO_NHAN>();
                        TS_BAN_GIAO_CT_GIAO_NHAN nbg = new TS_BAN_GIAO_CT_GIAO_NHAN();
                        nbg.MA_HSO = auNSD.KeywordStrings.First();
                        nbg.TEN_HSO = auNSD.DisplayName;
                        if (rbBenGiao.IsChecked == true)
                        {
                            nbg.DAI_DIEN = "Bên giao";
                        }
                        else if (rbBenNhan.IsChecked == true)
                        {
                            nbg.DAI_DIEN = "Bên nhận";
                        }
                        else if (rbKhac.IsChecked == true)
                        {
                            nbg.DAI_DIEN = txtDaiDienKhac.Text;
                        }
                        nbg.MA_CHUC_VU = maChucVu;
                        nbg.TEN_CHUC_VU = tenChucVu;
                        list.Add(nbg);
                        DuLieuTraVe(list);
                    }
                    CustomControl.CommonFunction.CloseUserControl(this);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
