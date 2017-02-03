using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.BaoCaoServiceRef;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.DungChung;
using PresentationWPF.CustomControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utilities.Common;

namespace PresentationWPF.BaoCao.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_TONG_HOP_DU_LIEU_KTOAN.xaml
    /// </summary>
    public partial class GDKT_TONG_HOP_DU_LIEU_KTOAN : UserControl
    {

        const int INT_ZERO = 0;
        const int INT_ONE = 1;
        const string LITERAL_NGAY = "1";
        const string LITERAL_THANG = "4";
        const string LITERAL_QUY = "5";
        const string LITERAL_NAM = "8";
        const int ID_REPORT = 0;
        const string CODE_REPORT = "GDKT_TONG_HOP_DU_LIEU_KTOAN";
        const string MAX_DATE_TRANSACTION = "_GET_MAX_DATE_TRANSACTION";

        #region Khai bao

        Application app = Application.Current;
        UserControl uc;

        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        BaoCaoProcess process = new BaoCaoProcess();
        bool isLoaded = false;
        public string formCase = null;
        List<HT_CNANG> lstBaoCaoTheoPhanHe = new List<HT_CNANG>();

        delegate void DieuKienBaoCao();

        HT_BAOCAO htBaoCao = null;
        List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);


        #endregion

        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        string TuNgay = string.Empty;
        string DenNgay = string.Empty;
        List<string> LstChiNhanh = null;
        List<string> LstDinhKyTongHop = null;

        #region Dang ky hot key

        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    if (key != null)
                        InputBindings.Add(key);
                }
            }
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                ShowReport();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        #endregion
        public GDKT_TONG_HOP_DU_LIEU_KTOAN()
        {
            InitializeComponent();
            LoadCombobox();
            // Lấy thông tin báo cáo và tham số
            BaoCaoProcess process = new BaoCaoProcess();
            process.LayThongTinBaoCao(ID_REPORT, CODE_REPORT, ref htBaoCao, ref lstHtBaoCaoTso);
            GetLastDate();
        }

        /// <summary>
        /// Check/Uncheck all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDinhKyAll_Checked(object sender, RoutedEventArgs e)
        {
            chkDinhKyNgay.IsChecked = chkDinhKyThang.IsChecked = chkDinhKyQuy.IsChecked = chkDinhKyNam.IsChecked = chkDinhKyAll.IsChecked;
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
        }

        /// <summary>
        /// Xử lý DropDownClosed combobox chi nhánh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            GetLastDate();
        }

        /// <summary>
        /// Validation dữ liệu input
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            // Kiểm tra đã chọn chi nhánh hay không?
            if (lstSourceChiNhanh.Where(e => e.CheckedMember == true).ToList().Count < 1)
            {
                return false;
            }

            // Kiểm tra đã chọn một kỳ tổng hợp báo cáo hay chưa?
            if (!chkDinhKyNgay.IsChecked.GetValueOrDefault()
                && !chkDinhKyThang.IsChecked.GetValueOrDefault()
                && !chkDinhKyQuy.IsChecked.GetValueOrDefault()
                && !chkDinhKyNam.IsChecked.GetValueOrDefault())
            {
                return false;
            }

            // Kiểm tra chưa input từ ngày
            if(raddtTuNgay.Value.IsNullOrEmpty())
            {
                return false;
            }

            // Kiểm tra chưa input đến ngày
            if (raddtDenNgay.Value.IsNullOrEmpty())
            {
                return false;
            }

            // Kiểm tra từ ngày lớn hơn đến ngày dữ liệu
            if (raddtTuNgay.Value.GetValueOrDefault() > raddtDenNgay.Value.GetValueOrDefault())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get doi so input form
        /// </summary>
        /// <returns></returns>
        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            foreach (string maChiNhanh in LstChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", maChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgayBaoCao", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string dinhKyTongHop in LstDinhKyTongHop)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DinhKyTongHop", dinhKyTongHop, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }

        /// <summary>
        /// Lấy thông tin input trên form
        /// </summary>
        private void GetFormData()
        {
            // Lấy mã chi nhánh
            LstChiNhanh = new List<string>();
            foreach (AutoCompleteCheckBox ChiNhanh in lstSourceChiNhanh.Where(e => e.CheckedMember == true))
            {
                LstChiNhanh.Add(ChiNhanh.ValueMember[INT_ZERO].ToString());
            }

            // Lấy từ ngày đến ngày
            DateTime tuNgay = raddtTuNgay.Value.GetValueOrDefault();
            DateTime denNgay = raddtDenNgay.Value.GetValueOrDefault();
            TuNgay = tuNgay.ToString("yyyyMMdd");
            DenNgay = denNgay.ToString("yyyyMMdd");

            // Lấy định kỳ tổng hợp
            LstDinhKyTongHop = new List<string>();
            if(chkDinhKyNgay.IsChecked.GetValueOrDefault())
            {
                LstDinhKyTongHop.Add(LITERAL_NGAY);
            }
            if (chkDinhKyThang.IsChecked.GetValueOrDefault())
            {
                LstDinhKyTongHop.Add(LITERAL_THANG);
            }
            if (chkDinhKyQuy.IsChecked.GetValueOrDefault())
            {
                LstDinhKyTongHop.Add(LITERAL_QUY);
            }
            if (chkDinhKyNam.IsChecked.GetValueOrDefault())
            {
                LstDinhKyTongHop.Add(LITERAL_NAM);
            }
        }

        /// <summary>
        /// Get ngay cuoi cung tong hop
        /// </summary>
        private void GetLastDate()
        {
            try
            {
                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                foreach (AutoCompleteCheckBox ChiNhanh in lstSourceChiNhanh.Where(f => f.CheckedMember == true))
                {
                    LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", ChiNhanh.ValueMember[INT_ZERO].ToString());
                }
                TruyVanProcess truyVanProcess = new TruyVanProcess();
                DataSet ds = truyVanProcess.TruyVanUDTT(MAX_DATE_TRANSACTION, dt);
                if (!ds.IsNullOrEmpty() && INT_ZERO < ds.Tables.Count && INT_ZERO < ds.Tables[INT_ZERO].Rows.Count)
                {
                    raddtDenNgay.Value = LDateTime.StringToDate(ds.Tables[INT_ZERO].Rows[INT_ZERO][INT_ZERO].ToString(), ApplicationConstant.defaultDateTimeFormat);                
                }
                else
                {
                    raddtDenNgay.Value = DateTime.Now;
                }
                raddtTuNgay.Value = LDateTime.GetFirstDateOfMonth(raddtDenNgay.Value.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                raddtDenNgay.Value = DateTime.Now;
                raddtTuNgay.Value = LDateTime.GetFirstDateOfMonth(raddtDenNgay.Value.GetValueOrDefault());
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #region Help
        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }
        #endregion

        #region Xu ly nghiep vu

        private void ShowReport()
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Thuc hien tong hop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbView_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Lấy dữ liệu từ form điều kiện                
                List<HT_BAOCAO_TSO> listHtBaoCaoTso = lstHtBaoCaoTso;
                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;

                listThamSoBaoCao = this.GetParameters();

                int soNgayTongHop = LDateTime.CountDayBetweenDates(
                                LDateTime.StringToDate(TuNgay, ApplicationConstant.defaultDateTimeFormat),
                                LDateTime.StringToDate(DenNgay, ApplicationConstant.defaultDateTimeFormat));
                progbarTongHop.Minimum = 0;
                progbarTongHop.Maximum = soNgayTongHop + INT_ONE;
                progbarTongHop.Value = 0;
                UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(progbarTongHop.SetValue);
                string tuNgay = TuNgay;
                double value = INT_ZERO;
                progbarPercent.Content = "0%";
                do
                {
                    processDate.Content = LDateTime.StringToDate(tuNgay, ApplicationConstant.defaultDateTimeFormat).DateToString("dd/MM/yyyy");
                    DataSet ds = new DataSet();
                    // Chuẩn bị điều kiện cho báo cáo
                    if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                    {
                        listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            if(tso.MA_TSO != "@TuNgay" && tso.MA_TSO != "@DenNgay")
                            {
                                tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            }
                            else
                            {
                                tso.GTRI_TSO = tuNgay;
                            }
                            
                            listHtBaoCaoTso.Add(tso);
                            if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                ds = thamSoBaoCao.DsThamSo;
                        }
                    }
                    ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    FileBase fileResponse = new FileBase();
                    List<FileBase> lstFileResponse = new List<FileBase>();
                    string responseMessage = null;

                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);

                    if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        value++;
                        Dispatcher.Invoke(updatePbDelegate,
                            System.Windows.Threading.DispatcherPriority.Background,
                            new object[] { ProgressBar.ValueProperty, value });
                        progbarPercent.Content = LNumber.Rounding((decimal)((progbarTongHop.Value / progbarTongHop.Maximum) * 100), 0).ToString() + "%";
                        tuNgay = LDateTime.PlusDays(LDateTime.StringToDate(tuNgay, ApplicationConstant.defaultDateTimeFormat), INT_ONE)
                                                                                    .DateToString(ApplicationConstant.defaultDateTimeFormat);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                        return;
                    }
                }
                while (progbarTongHop.Value != progbarTongHop.Maximum);
                Mouse.OverrideCursor = Cursors.Arrow;
                LMessage.ShowMessage("M.DungChung.Result.ThanhCong", LMessage.MessageBoxType.Information);
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }
        #endregion
    }
}
