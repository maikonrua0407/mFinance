using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.TinDung;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TinDungServiceRef;

namespace PresentationWPF.TinDung.XoaNo
{
    /// <summary>
    /// Interaction logic for ucXuLyNoKheUoc01.xaml
    /// </summary>
    public partial class ucXuLyNoKheUoc01 : UserControl
    {
        #region KhaiBao
        public static RoutedCommand SelectCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();        
        List<AutoCompleteEntry> lstThoiHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHinhThucXuLy = new List<AutoCompleteEntry>();
        List<int> _lstKUocID;
        List<DataRow> lstPopup = new List<DataRow>();
        KH_KHANG_HSO _objKHang;
        private DataSet _dsResources;
        public DataSet dsResource
        {
            get { return _dsResources; }
            set { _dsResources = value; }
        }

        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }

        private decimal _decTongTaiSan = 0;
        private decimal _decDuPhongCuThe = 0;
        private decimal _decDuPhongChung = 0;
        public decimal decDuPhongChung
        {
            get { return _decDuPhongChung; }
            set { _decDuPhongChung = value; }
        }        
        private List<DANH_SACH_KHE_UOC_XU_LY_NO> _listKUocXLN;
        public List<DANH_SACH_KHE_UOC_XU_LY_NO> listKUocXLN
        {
            get { return _listKUocXLN; }
            set { _listKUocXLN = value; }
        }
        private TD_KUOCVM _objKuoc;
        public delegate void LayDSXoaNo(List<DANH_SACH_KHE_UOC_XU_LY_NO> lstDSKUocXLN);
        public LayDSXoaNo GetListXoaNo;

        private DANH_SACH_KHE_UOC_XU_LY_NO _objKUocXLN;
        public DANH_SACH_KHE_UOC_XU_LY_NO objKUocXLN
        {
            get { return _objKUocXLN; }
            set { _objKUocXLN = value; }
        }

        private string _TrangThai;
        public string TrangThai
        {
            get { return _TrangThai; }
            set { _TrangThai = value; }    
        }
        List<DANH_SACH_TSDB> _listTSDB;
        #endregion

        #region KhoiTao
        public ucXuLyNoKheUoc01()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            BindHotkey();            
        }

        private void KhoiTaoCombobox()
        {
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();

            //Khoi tao combobox nhom no hien tai
            lstDieuKien.Clear();
            lstNhomNo.Clear();            
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NHOM_NO.getValue());
            auto.GenAutoComboBox(ref lstNhomNo, ref cmbNhomNo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            //Khoi tao combobox Hinh thuc xu ly      
            lstDieuKien.Clear();
            lstHinhThucXuLy.Clear();
            auto.GenAutoComboBox(ref lstHinhThucXuLy,ref cmbHinhThucXuLyNo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_HINHTHUC_XULYNO.getValue());            

            //Khoi tao combobox thoi han vay
            lstDieuKien.Clear();
            lstThoiHanVay.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.KY_HAN_DVI_TINH.getValue());
            auto.GenAutoComboBox(ref lstThoiHanVay, ref cmbThoiHanVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
        }

        #endregion

        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Binding HotKey
        /// </summary>
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals("Select"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SelectCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
                    }

                    InputBindings.Add(key);
                }
            }
        }
        private void SelectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SelectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onClosePopup();
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
            if (strTinhNang.Equals("Select"))
            {
                GetDSKUocXLN();
                XoaDuLieu();
                CustomControl.CommonFunction.CloseUserControl(this);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals("Select"))
            {
                onClosePopup();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        #endregion

        #region XuLyGiaoDien

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_objKUocXLN != null) LoadData();
        }

        private void HienThiDSKheUoc()
        {
            Cursor = Cursors.Wait;
            try
            {
                string sHinhThucXuLy = lstHinhThucXuLy.ElementAt(cmbHinhThucXuLyNo.SelectedIndex).KeywordStrings[0];
                List<string> lstDieuKien = new List<string>();
                string sidKheUoc = "0";
                int idKUoc = 0;
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(sHinhThucXuLy);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                PopupProcess process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, false);                
                popup.LayGiaTriListID = new ucPopupKheUocViMo.LayListID(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopupKU.Count > 0)
                {
                    idKUoc = lstPopupKU[0];
                    LayThongTinKheUoc(idKUoc, txtSoKheUoc.Text.Trim(), "GetByID");
                }
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void LayThongTinKheUoc(int idKheUoc, string sMaKheUoc,string sLoai)
        {
            TinDungProcess tdProcess = new TinDungProcess();            
            TDVM_KHE_UOC objKuoc = new TDVM_KHE_UOC();
            objKuoc.KUOC_VM = new TD_KUOCVM();
            objKuoc.KUOC_VM.ID = idKheUoc;
            objKuoc.KUOC_VM.MA_KUOCVM = sMaKheUoc;
            int iRet = 0;
            if (sLoai.Equals("GetByID")) iRet = tdProcess.GetKUocById(ref objKuoc);
            else iRet=tdProcess.GetKUocByMaKuoc(ref objKuoc);
            decimal decDuNoLai = 0;
            _objKHang = new KH_KHANG_HSO();
            if(iRet!=0)
            {
                if (objKuoc.KUOC_VM != null)
                {                    
                    decDuNoLai = objKuoc.KUOC_VM.LAI_PHAI_THU - objKuoc.KUOC_VM.LAI_DA_THU;
                    _objKuoc = objKuoc.KUOC_VM;
                    if (_objKuoc.MA_DVI_TAO != ClientInformation.MaDonViGiaoDich)
                    {
                        _objKuoc = null;
                        XoaDuLieu();
                        return;
                    }
                    HienThiDSTaiSan(_objKuoc.ID.ToString());
                    _decDuPhongCuThe = objKuoc.KUOC_VM.SO_TIEN_TLDP;
                    txtSoKheUoc.Text = objKuoc.KUOC_VM.MA_KUOCVM;
                    teldtNgayVay.Value = LDateTime.StringToDate(objKuoc.KUOC_VM.NGAY_GIAI_NGAN, "yyyyMMdd");
                    txtThoiHanVay.Text = objKuoc.KUOC_VM.TGIAN_VAY.ToString();
                    cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(e => e.KeywordStrings[0].Equals(objKuoc.KUOC_VM.TGIAN_VAY_DVI_TINH)));
                    txtLaiSuat.Value = Convert.ToDouble(objKuoc.KUOC_VM.LAI_SUAT);
                    teldtNgayDaoHan.Value = LDateTime.StringToDate(objKuoc.KUOC_VM.NGAY_DAO_HAN, "yyyyMMdd");
                    txtSoTienVay.Value = Convert.ToDouble(objKuoc.KUOC_VM.SO_TIEN_GIAI_NGAN);
                    cmbNhomNo.SelectedIndex = lstNhomNo.IndexOf(lstNhomNo.FirstOrDefault(e => e.KeywordStrings[0].Equals(objKuoc.KUOC_VM.NHOM_NO_HIEN_TAI)));
                    txtDuNoGoc.Value = Convert.ToDouble(objKuoc.KUOC_VM.SO_DU);
                    txtDuNoLai.Value = Convert.ToDouble(decDuNoLai);

                    txtXuKyDuNoGoc.Value = Convert.ToDouble(objKuoc.KUOC_VM.SO_DU);
                    txtXuKyDuNoLai.Value = Convert.ToDouble(decDuNoLai);
                    iRet = tdProcess.GetKHangByKuoc(ref _objKHang, objKuoc);    
                }
                else
                {
                    txtSoKheUoc.Text = "";
                    teldtNgayVay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtThoiHanVay.Text = "0";
                    cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(e => e.KeywordStrings[0].Equals(BusinessConstant.KY_HAN_DVI_TINH.THANG.layGiaTri())));
                    txtLaiSuat.Value = 0;
                    teldtNgayDaoHan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtSoTienVay.Value = 0;
                    cmbNhomNo.SelectedIndex = lstNhomNo.IndexOf(lstNhomNo.FirstOrDefault(e => e.KeywordStrings[0].Equals(BusinessConstant.NHOM_NO.NHOM1.layGiaTri())));
                    txtDuNoGoc.Value = 0;
                    txtDuNoLai.Value = 0;

                    txtXuKyDuNoGoc.Value = 0;
                    txtXuKyDuNoLai.Value = 0;
                }    
            }
            TToanTrichLapDuPhongChiPhi();      
        }        

        private void XoaDuLieu()
        {
            _decDuPhongCuThe = 0;
            _decTongTaiSan = 0;
            _dsResources = null;
            _objKHang = null;
            _objKuoc = null;
            _objKUocXLN = null;
            _listTSDB = null;            
            grdTaiSanDamBao.ItemsSource = null;
            chkTDNBGoc.IsChecked = false;
            chkTDNBLai.IsChecked = false;
            txtChiPhi.Value = 0;
            txtDuNoGoc.Value = 0;
            txtDuNoLai.Value = 0;
            txtDuPhongChung.Value = 0;
            txtDuPhongCuThe.Value = 0;
            txtLaiSuat.Value = 0;
            txtSoKheUoc.Text = "";
            txtSoTienVay.Value = 0;
            txtThoiHanVay.Value = 0;
            txtXuKyDuNoGoc.Value = 0;
            txtXuKyDuNoLai.Value = 0;
        }

        private void TToanTrichLapDuPhongChiPhi()
        {
            decimal decXuLyGoc = Convert.ToDecimal(txtXuKyDuNoGoc.Value);
            decimal decXyLyLai = Convert.ToDecimal(txtXuKyDuNoLai.Value);
            decimal decTongXuLy = decXuLyGoc + decXyLyLai;
            decimal decDPCuThe = 0;
            decimal decDPChung = 0;

            if (_decDuPhongCuThe <= (decXuLyGoc - _decTongTaiSan)) txtDuPhongCuThe.Value = Convert.ToDouble(_decDuPhongCuThe);
            else if (_decDuPhongCuThe > (decXuLyGoc - _decTongTaiSan)) txtDuPhongCuThe.Value = Convert.ToDouble(decXuLyGoc - _decTongTaiSan);
            decDPCuThe = Convert.ToDecimal(txtDuPhongCuThe.Value);

            if ((decXuLyGoc - _decTongTaiSan - decDPCuThe) > 0 && (decXuLyGoc - _decTongTaiSan - decDPCuThe) < _decDuPhongChung) txtDuPhongChung.Value = LNumber.ToDouble(decXuLyGoc - _decTongTaiSan - decDPCuThe);
            else if ((decXuLyGoc - _decTongTaiSan - decDPCuThe) > 0 && (decXuLyGoc - _decTongTaiSan - decDPCuThe) > _decDuPhongChung) txtDuPhongChung.Value = Convert.ToDouble(_decDuPhongChung);
            else txtDuPhongChung.Value = 0;
            decDPChung = Convert.ToDecimal(txtDuPhongChung.Value);

            if ((decXuLyGoc - _decTongTaiSan - decDPCuThe - decDPChung) > 0) txtChiPhi.Value = Convert.ToDouble(decXuLyGoc - _decTongTaiSan - decDPCuThe - decDPChung);
            else txtChiPhi.Value = 0;
        }

        private void btnSoKheUoc_Click(object sender, RoutedEventArgs e)
        {
            HienThiDSKheUoc();
        }

        private void txtSoKheUoc_LostFocus(object sender, RoutedEventArgs e)
        {
            LayThongTinKheUoc(0, txtSoKheUoc.Text.Trim(), "");
        }

        private void txtXuKyDuNoGoc_LostFocus(object sender, RoutedEventArgs e)
        {
            double dobDuNoGoc = Convert.ToDouble(txtDuNoGoc.Value);
            double dobXoaNoGoc = Convert.ToDouble(txtXuKyDuNoGoc.Value);
            if (dobXoaNoGoc > dobDuNoGoc || dobDuNoGoc < 0)
            {
                txtXuKyDuNoGoc.Value = dobDuNoGoc;
            }
            TToanTrichLapDuPhongChiPhi();
        }

        private void txtXuKyDuNoLai_LostFocus(object sender, RoutedEventArgs e)
        {
            double dobDuNoLai = Convert.ToDouble(txtDuNoLai.Value);
            double dobXuLyLai = Convert.ToDouble(txtXuKyDuNoLai.Value);
            if (dobXuLyLai > dobDuNoLai || dobXuLyLai < 0)
            {
                txtXuKyDuNoLai.Value = dobDuNoLai;
            }
            TToanTrichLapDuPhongChiPhi();
        }        

        public void GetDSKUocXLN()
        {
            if (_listKUocXLN == null)
            {
                _listKUocXLN = new List<DANH_SACH_KHE_UOC_XU_LY_NO>();
            }
            if (_objKuoc == null || _objKHang == null) return;
            bool bExist = false;
            string sHinhThucXuLy = lstHinhThucXuLy.ElementAt(cmbHinhThucXuLyNo.SelectedIndex).KeywordStrings[0];
            DANH_SACH_KHE_UOC_XU_LY_NO objDSKuocXLN = new DANH_SACH_KHE_UOC_XU_LY_NO();
            objDSKuocXLN.LOAI_XU_LY_NO = sHinhThucXuLy;
            objDSKuocXLN.DU_NO_GOC = Convert.ToDecimal(txtDuNoGoc.Value);
            objDSKuocXLN.DU_NO_LAI = Convert.ToDecimal(txtDuNoLai.Value);
            objDSKuocXLN.GOC_DUOC_XU_LY = Convert.ToDecimal(txtXuKyDuNoGoc.Value);
            objDSKuocXLN.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
            objDSKuocXLN.ID_KHACH_HANG = _objKHang.ID;
            objDSKuocXLN.ID_KHE_UOC = _objKuoc.ID;
            objDSKuocXLN.LAI_DUOC_XU_LY = Convert.ToDecimal(txtXuKyDuNoLai.Value);
            objDSKuocXLN.MA_DON_VI = ClientInformation.MaDonViGiaoDich;
            objDSKuocXLN.MA_KHACH_HANG = _objKHang.MA_KHANG;
            objDSKuocXLN.MA_KHE_UOC = _objKuoc.MA_KUOCVM;
            objDSKuocXLN.MA_SAN_PHAM = _objKuoc.MA_SAN_PHAM;
            objDSKuocXLN.NGAY_VAY = _objKuoc.NGAY_GIAI_NGAN;
            objDSKuocXLN.SO_TIEN_VAY = _objKuoc.SO_TIEN_GIAI_NGAN;
            objDSKuocXLN.TEN_DON_VI = ClientInformation.TenDonViGiaoDich;
            objDSKuocXLN.TEN_KHACH_HANG = _objKHang.TEN_KHANG;
            objDSKuocXLN.THOI_HAN_VAY = _objKuoc.TGIAN_VAY.ToString() + _objKuoc.TGIAN_VAY_DVI_TINH;
            objDSKuocXLN.DU_PHONG_CHUNG = Convert.ToDecimal(txtDuPhongChung.Value);
            objDSKuocXLN.DU_PHONG_CU_THE = Convert.ToDecimal(txtDuPhongCuThe.Value);
            objDSKuocXLN.CHI_PHI = Convert.ToDecimal(txtChiPhi.Value);
            objDSKuocXLN.lstDSTaiSanDB = _listTSDB.ToArray();            
            objDSKuocXLN.GTRI_TAI_SAN = _decTongTaiSan;
            objDSKuocXLN.DU_PHONG_CU_THE_TRUOC_XLY = _objKuoc.SO_TIEN_TLDP;
            if (chkTDNBGoc.IsChecked == true) objDSKuocXLN.XUAT_GOC_NGOAI_BANG = BusinessConstant.CoKhong.CO.layGiaTri();
            else objDSKuocXLN.XUAT_GOC_NGOAI_BANG = BusinessConstant.CoKhong.KHONG.layGiaTri();
            if (chkTDNBLai.IsChecked == true) objDSKuocXLN.XUAT_LAI_NGOAI_BANG = BusinessConstant.CoKhong.CO.layGiaTri();
            else objDSKuocXLN.XUAT_LAI_NGOAI_BANG = BusinessConstant.CoKhong.KHONG.layGiaTri();

            if (_listKUocXLN.Count > 0)
            {                
                //if (!(_listKUocXLN.Contains(objDSKuocXLN))) _listKUocXLN.Add(objDSKuocXLN);
                for (int i = 0; i < _listKUocXLN.Count; i++)
                {
                    if (_listKUocXLN[i].ID_KHE_UOC == objDSKuocXLN.ID_KHE_UOC)
                    {
                        if (_TrangThai != "EDIT")
                        {
                            bExist = true;
                            break;
                        }
                        else
                        {
                            _listKUocXLN[i] = objDSKuocXLN;
                            bExist = true;
                            break;
                        }
                    }
                }
                if (!bExist) _listKUocXLN.Add(objDSKuocXLN);
            }
            else
            {
                _listKUocXLN.Add(objDSKuocXLN);
            }
            if (GetListXoaNo != null)
            {
                GetListXoaNo(_listKUocXLN);
            }
        }

        private void LoadData()
        {
            try
            {
                LayThongTinKheUoc(_objKUocXLN.ID_KHE_UOC, _objKUocXLN.MA_KHE_UOC, "GetByID");
                txtChiPhi.Value = Convert.ToDouble(_objKUocXLN.CHI_PHI);
                txtDuPhongCuThe.Value = Convert.ToDouble(_objKUocXLN.DU_PHONG_CHUNG);
                txtDuPhongChung.Value = Convert.ToDouble(_objKUocXLN.DU_PHONG_CU_THE);
                txtXuKyDuNoGoc.Value = Convert.ToDouble(_objKUocXLN.GOC_DUOC_XU_LY);
                txtXuKyDuNoLai.Value = Convert.ToDouble(_objKUocXLN.LAI_DUOC_XU_LY);
                if (_objKUocXLN.XUAT_GOC_NGOAI_BANG.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) chkTDNBGoc.IsChecked = true;
                if (_objKUocXLN.XUAT_LAI_NGOAI_BANG.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) chkTDNBLai.IsChecked = true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void HienThiDSTaiSan(string idKuoc)
        {
            try
            {
                DataSet dsTaiSan = new DataSet();
                TinDungProcess tdProcess = new TinDungProcess();
                dsTaiSan = tdProcess.LayTaiSanTheoIDKuoc(idKuoc);
                _listTSDB = new List<DANH_SACH_TSDB>();
                DANH_SACH_TSDB objTSDB = new DANH_SACH_TSDB();
                if (dsTaiSan != null && dsTaiSan.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsTaiSan.Tables[0].Rows)
                    {
                        objTSDB = new DANH_SACH_TSDB();
                        objTSDB.GTRI_CON_DBAO = Convert.ToDecimal(dr["GTRI_CON_DBAO"]);
                        objTSDB.GTRI_DAM_BAO = Convert.ToDecimal(dr["GTRI_DAM_BAO"]);
                        objTSDB.GTRI_DBAO_DTUONG = Convert.ToDecimal(dr["GTRI_DBAO_DTUONG"]);
                        objTSDB.GTRI_DINH_GIA = Convert.ToDecimal(dr["GTRI_DINH_GIA"]);
                        objTSDB.GTRI_TSDB_XLN = Convert.ToDecimal(dr["GIA_TRI_TS_DB"]);
                        objTSDB.GTRI_TY_LE = Convert.ToDecimal(dr["GTRI_TY_LE"]);
                        objTSDB.ID_TSDB = Convert.ToInt32(dr["ID_TSDB"]);
                        objTSDB.MA_HDTC = dr["MA_HDTC"].ToString();
                        objTSDB.MA_LOAI_TSDB = dr["MA_LOAI_TSDB"].ToString();
                        objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                        objTSDB.TEN_TAI_SAN = dr["TEN_TSDB"].ToString();
                        objTSDB.MA_THAM_CHIEU = dr["MA_TCHIEU"].ToString();
                        objTSDB.MA_KHE_UOC = _objKuoc.MA_KUOCVM;
                        _listTSDB.Add(objTSDB);
                    }
                }
                grdTaiSanDamBao.ItemsSource = _listTSDB;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grdTaiSanDamBao_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (grdTaiSanDamBao.Items.Count == 0) return;            
            _listTSDB = (List<DANH_SACH_TSDB>)grdTaiSanDamBao.ItemsSource;
            decimal decGiaTri = 0;
            for (int i = 0; i < _listTSDB.Count; i++)
            {
                decGiaTri = _listTSDB[i].GTRI_TSDB_XLN;
                _decTongTaiSan = _decTongTaiSan + decGiaTri;
            }
            TToanTrichLapDuPhongChiPhi();
        }

        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void cmbHinhThucXuLyNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XoaDuLieu();
        }

        #endregion

        #region XuLyNghiepVu
        void onClosePopup()
        {
            CommonFunction.CloseUserControl(this);
        }
        #endregion

    }
}
