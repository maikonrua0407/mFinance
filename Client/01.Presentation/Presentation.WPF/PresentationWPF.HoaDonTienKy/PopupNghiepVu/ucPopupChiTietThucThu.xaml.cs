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
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Presentation.Process;
using System.Reflection;

namespace PresentationWPF.HoaDonTienKy.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopupChiTietThucThu.xaml
    /// </summary>
    public partial class ucPopupChiTietThucThu : UserControl
    {
        #region KhaiBao
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public event EventHandler OnSavingCompleted;

        private DANH_SACH_KHE_UOC_VONG_VAY _objKheUocViMo = new DANH_SACH_KHE_UOC_VONG_VAY();
        public DANH_SACH_KHE_UOC_VONG_VAY objKheUocViMo
        {
            get { return _objKheUocViMo; }
            set { _objKheUocViMo = value; }
        }

        private DANH_SACH_KHE_UOC_VONG_VAY _kheUoc = new DANH_SACH_KHE_UOC_VONG_VAY();

        List<AutoCompleteEntry> lstSourceTaiKhoanKhongKyHan = new List<AutoCompleteEntry>();

        private string _ngayThuTien = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;

        private decimal _soTienGocConPhaiNop = 0;
        private decimal _soTienLaiConPhaiTra = 0;

        private bool _tinhPhiTraTruoc = false;

        #endregion

        #region KhoiTao
        public ucPopupChiTietThucThu(DANH_SACH_KHE_UOC_VONG_VAY obj, string ngayGD)
        {
            InitializeComponent();
            _ngayThuTien = ngayGD;
            _objKheUocViMo = obj;
            _kheUoc = CopyObject(obj);
            KhoiTaoCombobox();
            LayDSSoTietKiemKhongKyHan();
            LayThongTinKeHoach();
            LoadThongTinForm();
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstDK.Add(_kheUoc.MA_KHACH_HANG);
                auto.GenAutoComboBox(ref lstSourceTaiKhoanKhongKyHan, ref cmbTaiKhoanTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TK_KHONG_KY_HAN.getValue(), lstDK);
            }
            catch (System.Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                //LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                if (lstSourceTaiKhoanKhongKyHan == null || lstSourceTaiKhoanKhongKyHan.Count == 0)
                {
                    chkTKTietKiemKhongKyHan.IsEnabled = false;
                }
            }
        }
        #endregion

        #region Dang ky hot key, shortcut key

        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion

        #region Xu ly giao dien
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

        /// <summary>
        /// Sự kiện load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void chkTKTietKiemKhongKyHan_Click(object sender, RoutedEventArgs e)
        {
            if (chkTKTietKiemKhongKyHan.IsChecked == true)
            {
                cmbTaiKhoanTK.IsEnabled = true;
                numSoTienRutTuTK.IsEnabled = true;
                numTienMat.Value = 0;
                numTienMat.IsEnabled = false;
            }
            else
            {
                cmbTaiKhoanTK.IsEnabled = false;
                numSoTienRutTuTK.IsEnabled = false;
                numTienMat.IsEnabled = true;
            }
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void LayDSSoTietKiemKhongKyHan()
        {
            Presentation.Process.TinDungProcess bus = new Presentation.Process.TinDungProcess();
            try
            {
                DataSet ds = bus.getDanhSachSoTKKhongKH(_kheUoc.MA_KHACH_HANG);
                bool isEnable = false;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["so_so_tg"].ToString().Equals(_kheUoc.SO_SO_TG))
                        {
                            dr["so_tien_nop"] = Math.Max(0, _kheUoc.THUC_THU_TKQD - _kheUoc.KE_HOACH_TKQD);
                            _kheUoc.THUC_THU_TKQD = _kheUoc.KE_HOACH_TKQD;
                        }
                        else
                        {
                            if (_kheUoc.THUC_THU_NOP_VAO_TKKKH > 0)
                            {
                                if (isEnable == false)
                                {
                                    grbXuLyTienThua.IsEnabled = true;
                                    chkTKKhongKyHanTienThua.IsChecked = true;
                                    numKhongKyHanTienThua.Value = Convert.ToDouble(_kheUoc.THUC_THU_NOP_VAO_TKKKH + _kheUoc.THUC_THU_TKQD - _kheUoc.KE_HOACH_TKQD);
                                    isEnable = true;
                                }

                                if (_kheUoc.DSACH_SO_NOP_TIEN != null && _kheUoc.DSACH_SO_NOP_TIEN.Length > 0)
                                {
                                    List<DANH_SACH_SO> lst = _kheUoc.DSACH_SO_NOP_TIEN.ToList();
                                    DANH_SACH_SO obj = lst.FirstOrDefault(e => e.SO_SO.Equals(dr["so_so_tg"].ToString()));
                                    if (obj != null)
                                    {
                                        dr["so_tien_nop"] = obj.SO_TIEN_NOP_VAO;
                                        _kheUoc.THUC_THU_NOP_VAO_TKKKH -= obj.SO_TIEN_NOP_VAO;
                                    }
                                }
                                else
                                {
                                    dr["so_tien_nop"] = _kheUoc.THUC_THU_NOP_VAO_TKKKH;
                                    _kheUoc.THUC_THU_NOP_VAO_TKKKH = 0;
                                }
                            }
                        }
                    }
                    grTKKhongKyHanTienThua.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    chkTKKhongKyHanTienThua.IsChecked = false;
                    chkTKKhongKyHanTienThua.Tag = "False";
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                bus = null;
            }
        }

        private void LayThongTinKeHoach()
        {
            TinDungProcess process = new TinDungProcess();
            try
            {
                DataSet ds = process.getThongTinKeHoach(_kheUoc.MA_KHE_UOC, _ngayThuTien, Presentation.Process.Common.ClientInformation.MaDonVi);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    grGocLaiTruocHan.ItemsSource = ds.Tables[0].DefaultView;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        _soTienGocConPhaiNop += Convert.ToDecimal(dr["kh_tra_goc"]);
                    }
                }
                else
                {
                    chkTraTruocHan.Tag = "False";
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void LoadThongTinForm()
        {
            numTKQD.Value = Convert.ToDouble(_kheUoc.KE_HOACH_TKQD);
            numGocVay.Value = Convert.ToDouble(_kheUoc.KE_HOACH_GOC_VAY);
            numLaiTrongHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_TRONG_HAN);
            numLaiQuaHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_QUA_HAN);

            // Số tiền nộp
            if (_kheUoc.NOP_TIEN_TU_TKKKH == "KHONG")
            {
                numTienMat.Value = Convert.ToDouble(_kheUoc.THUC_THU_TIEN_MAT);
                chkTKTietKiemKhongKyHan.IsChecked = false;
            }
            else
            {
                grbXuLyTienThua.IsEnabled = true;
                chkTKTietKiemKhongKyHan.IsChecked = true;
                numSoTienRutTuTK.Value = Convert.ToDouble(_kheUoc.THUC_THU_TKKKH);
                cmbTaiKhoanTK.SelectedIndex = lstSourceTaiKhoanKhongKyHan.IndexOf(lstSourceTaiKhoanKhongKyHan.FirstOrDefault(f => f.KeywordStrings.First().Equals(_kheUoc.DSACH_SO_RUT_TIEN[0].SO_SO)));
            }

            // Ghi nhận tiền thừa vào tài khoản tiết kiệm không kỳ hạn
            if (_kheUoc.NOP_TIEN_VAO_TKKKH == "KHONG")
            {
                chkTKKhongKyHanTienThua.IsChecked = false;
            }
            else
            {
                grbXuLyTienThua.IsEnabled = true;
                chkTKKhongKyHanTienThua.IsChecked = true;
            }

            // Ghi nhận tiền thừa vào tài khoản nội bộ
            if (_kheUoc.GHI_NHAN_VAO_TKNB == "KHONG")
            {
                //chkGhiTKNoiBo.IsChecked = false;
            }
            else
            {
                grbXuLyTienThua.IsEnabled = true;
                //chkGhiTKNoiBo.IsChecked = true;
            }

            // Ghi nhận tiền thừa vào trả trước
            if (_kheUoc.TRA_GOC_LAI_TRUOC_HAN == "KHONG")
            {
                chkTraTruocHan.IsChecked = false;
            }
            else
            {
                grbXuLyTienThua.IsEnabled = true;
                chkTraTruocHan.IsChecked = true;
                numSoTienNop.Value = Convert.ToDouble(_kheUoc.TONG_TIEN_TRA_TRUOC);
                if (_kheUoc.THUC_THU_LAI_TRONG + _kheUoc.THUC_THU_LAI_QUA_HAN != _kheUoc.KE_HOACH_LAI_QUA_HAN + _kheUoc.KE_HOACH_LAI_TRONG_HAN)
                {
                    TaoThongTinTraTruoc(false);
                }
                else
                {
                    chkTatToan.IsEnabled = true;
                    chkTatToan.IsChecked = true;
                    numPhiTraTruoc.IsEnabled = true;
                    numPhiTraTruoc.Value = Convert.ToDouble(_kheUoc.PHI_TRA_TRUOC);
                }
            }

            if (_kheUoc.PHI_TRA_TRUOC > 0)
            {
                chkTatToan.IsEnabled = true;
                chkTatToan.IsChecked = true;
                numPhiTraTruoc.IsEnabled = true;
                numPhiTraTruoc.Value = Convert.ToDouble(_kheUoc.PHI_TRA_TRUOC);
            }
        }

        #endregion  

        #region Xu ly nghiep vu

        private void onSave()
        {
            try
            {
                if (Validation())
                {
                    GetDataOnForm(ref _kheUoc);
                    _objKheUocViMo = _kheUoc;
                    onClose();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void GetDataOnForm(ref DANH_SACH_KHE_UOC_VONG_VAY obj)
        {
            List<Presentation.Process.TinDungServiceRef.DANH_SACH_SO> lstRutTien = new List<Presentation.Process.TinDungServiceRef.DANH_SACH_SO>();
            List<Presentation.Process.TinDungServiceRef.DANH_SACH_SO> lstNopTien = new List<Presentation.Process.TinDungServiceRef.DANH_SACH_SO>();

            if (_kheUoc.DSACH_SO_NOP_TIEN != null)
            {
                _kheUoc.DSACH_SO_NOP_TIEN = null;
            }

            if (_kheUoc.DSACH_SO_RUT_TIEN != null)
            {
                _kheUoc.DSACH_SO_RUT_TIEN = null;
            }

            if (chkNopBangTienMat.IsChecked == true)
            {
                _kheUoc.THUC_THU_TIEN_MAT = Convert.ToDecimal(numTienMat.Value);
            }

            if (chkTKTietKiemKhongKyHan.IsChecked == true)
            {
                _kheUoc.NOP_TIEN_TU_TKKKH = "CO";
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry au = auto.getEntryByDisplayName(lstSourceTaiKhoanKhongKyHan,ref cmbTaiKhoanTK);
                Presentation.Process.TinDungServiceRef.DANH_SACH_SO objRutTK = new Presentation.Process.TinDungServiceRef.DANH_SACH_SO();
                objRutTK.SO_SO = au.DisplayName;
                objRutTK.SO_TAI_KHOAN = cmbTaiKhoanTK.Tag.ToString();
                objRutTK.SO_DU = Convert.ToDecimal(numSoDuTaiKhoanTK.Value);
                objRutTK.SO_TIEN_RUT_RA = Convert.ToDecimal(numSoTienRutTuTK.Value);
                lstRutTien.Add(objRutTK);
            }
            else
            {
                _kheUoc.NOP_TIEN_TU_TKKKH = "KHONG";
            }

            if (chkTKKhongKyHanTienThua.IsChecked == false)
            {
                _kheUoc.NOP_TIEN_VAO_TKKKH = "KHONG";
            }
            else
            {
                _kheUoc.NOP_TIEN_VAO_TKKKH = "CO";
                DataTable dt = ((DataView)grTKKhongKyHanTienThua.ItemsSource).Table;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["so_so_tg"].ToString().Equals(obj.SO_SO_TG))
                    {
                        Presentation.Process.TinDungServiceRef.DANH_SACH_SO objNopTK = new Presentation.Process.TinDungServiceRef.DANH_SACH_SO();
                        if (!dr["so_so_tg"].ToString().Equals(_kheUoc.SO_SO_TG))
                        {
                            objNopTK.SO_SO = dr["so_so_tg"].ToString();
                            objNopTK.SO_TAI_KHOAN = dr["so_tai_khoan"].ToString();
                            objNopTK.SO_DU = Convert.ToDecimal(dr["so_tien"]);
                            objNopTK.SO_TIEN_NOP_VAO = Convert.ToDecimal(dr["so_tien_nop"]);
                            lstNopTien.Add(objNopTK);
                            obj.THUC_THU_NOP_VAO_TKKKH += Convert.ToDecimal(dr["so_tien_nop"]);
                        }
                    }
                    else
                    {
                        obj.THUC_THU_TKQD += Convert.ToDecimal(dr["so_tien_nop"]);
                    }
                }
            }

            //if (chkGhiTKNoiBo.IsChecked == false)
            //{
                _kheUoc.GHI_NHAN_VAO_TKNB = "KHONG";
            //}
            //else
            //{
            //    _kheUoc.GHI_NHAN_VAO_TKNB = "CO";
            //}

            if (chkTraTruocHan.IsChecked == false)
            {
                _kheUoc.TRA_GOC_LAI_TRUOC_HAN = "KHONG";
            }
            else
            {
                _kheUoc.TRA_GOC_LAI_TRUOC_HAN = "CO";
                _kheUoc.TONG_TIEN_TRA_TRUOC = Convert.ToDecimal(numSoTienNop.Value);
                _kheUoc.TONG_TIEN_TRA_TRUOC = Convert.ToDecimal(numSoTienNop.Value);
            }

            if (lstNopTien != null && lstNopTien.Count > 0)
            {
                obj.DSACH_SO_NOP_TIEN = lstNopTien.ToArray();
            }

            if (lstRutTien != null && lstRutTien.Count > 0)
            {
                obj.DSACH_SO_RUT_TIEN = lstRutTien.ToArray();
            }

            _kheUoc.PHI_TRA_TRUOC = Convert.ToDecimal(numPhiTraTruoc.Value);

            PhanBoSoTienThuDuoc();
        }

        private bool Validation()
        {
            bool kq = true;
            if (chkTKKhongKyHanTienThua.IsChecked == true)
            {
                decimal tongTienNop = 0;
                DataTable dt = ((DataView)grTKKhongKyHanTienThua.ItemsSource).Table;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tongTienNop += Convert.ToDecimal(dr["so_tien_nop"]);
                    }
                }
                if (tongTienNop != Convert.ToDecimal(numKhongKyHanTienThua.Value))
                {
                    LMessage.ShowMessage("M.TinDung.PopupNghiepVu.BinhKhanh.ucPopupChiTietThucThuBinhKhanh.LoiTongTienNopTK", LMessage.MessageBoxType.Warning);
                    kq = false;
                }
            }
            else if (chkTraTruocHan.IsChecked == true)
            {
                if (chkTatToan.IsChecked == false)
                {
                    decimal tongLaiPhaiNop = 0;
                    if (grGocLaiTruocHan.ItemsSource != null)
                    {
                        DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).Table;
                        if (dt != null)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                tongLaiPhaiNop += Convert.ToDecimal(dr["kh_tra_lai"]);
                            }
                        }
                    }
                    if (_soTienGocConPhaiNop + tongLaiPhaiNop < Convert.ToDecimal(numSoTienNop.Value))
                    {
                        LMessage.ShowMessage("M.TinDung.PopupNghiepVu.BinhKhanh.ucPopupChiTietThucThuBinhKhanh.LoiTongTienTraTruoc", LMessage.MessageBoxType.Warning);
                        kq = false;
                        return kq;
                    }
                }
                else
                {

                    if (_soTienGocConPhaiNop + Convert.ToDecimal(numPhiTraTruoc.Value) != Convert.ToDecimal(numSoTienNop.Value))
                    {
                        LMessage.ShowMessage("M.TinDung.PopupNghiepVu.BinhKhanh.ucPopupChiTietThucThuBinhKhanh.LoiTongTienTraCacKySauVaPhi", LMessage.MessageBoxType.Warning);
                        numPhiTraTruoc.Focus();
                        return false;
                    }
                }
            }
            return kq;
        }

        #endregion

        private void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void cmbTaiKhoanTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox auto = new AutoComboBox();
            try
            {
                AutoCompleteEntry au = auto.getEntryByDisplayName(lstSourceTaiKhoanKhongKyHan, ref cmbTaiKhoanTK);
                if (au != null)
                {
                    string[] a = au.KeywordStrings[0].Split('#');
                    numSoDuTaiKhoanTK.Value = Convert.ToDouble(a[0]);
                    cmbTaiKhoanTK.Tag = a[1];
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void chkNopBangTienMat_Click(object sender, RoutedEventArgs e)
        {
            if (chkNopBangTienMat.IsChecked == true)
            {
                numTienMat.Value = Convert.ToDouble(numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value);
                numTienMat.IsEnabled = true;
                cmbTaiKhoanTK.IsEnabled = false;
                numSoTienRutTuTK.IsEnabled = false;
                numSoTienRutTuTK.Value = 0;
            }
            else
            {
                numTienMat.IsEnabled = false;
                numTienMat.Value = 0;
            }
        }

        private void PhanBoSoTienThuDuoc()
        {
            decimal tongtienNop = Convert.ToDecimal(numTienMat.Value) + Convert.ToDecimal(numSoTienRutTuTK.Value);
            _kheUoc.THUC_THU_TONG = tongtienNop;
            _kheUoc.TONG_SO_TIEN = tongtienNop;
            _kheUoc.THUC_THU_TKKKH = Convert.ToDecimal(numSoTienRutTuTK.Value);
            _kheUoc.THUC_THU_TIEN_MAT = Convert.ToDecimal(numTienMat.Value);
            _kheUoc.THUC_THU_LAI_QUA_HAN = 0;
            _kheUoc.THUC_THU_LAI_TRONG = 0;
            _kheUoc.THUC_THU_GOC_VAY = 0;

            if (_kheUoc.NOP_TIEN_VAO_TKKKH == "KHONG")
            {
                _kheUoc.THUC_THU_TKQD = _kheUoc.KE_HOACH_TKQD;
                _kheUoc.THUC_THU_NOP_VAO_TKKKH = 0;
            }


            _kheUoc.THUC_THU_TKQD = Math.Min(_kheUoc.THUC_THU_TKQD, tongtienNop);
            tongtienNop -= _kheUoc.THUC_THU_TKQD;
            if (tongtienNop > 0)
            {
                _kheUoc.THUC_THU_LAI_QUA_HAN = Math.Min(_kheUoc.KE_HOACH_LAI_QUA_HAN, tongtienNop);
                tongtienNop -= _kheUoc.THUC_THU_LAI_QUA_HAN;
                if (tongtienNop > 0)
                {
                    _kheUoc.THUC_THU_LAI_TRONG = Math.Min(_kheUoc.KE_HOACH_LAI_TRONG_HAN, tongtienNop);
                    tongtienNop -= _kheUoc.THUC_THU_LAI_TRONG;
                    if (tongtienNop > 0)
                    {
                        _kheUoc.THUC_THU_GOC_VAY = Math.Min(_kheUoc.KE_HOACH_GOC_VAY, tongtienNop);
                    }
                }
                else
                {
                    _kheUoc.THUC_THU_LAI_QUA_HAN = 0;
                    _kheUoc.THUC_THU_LAI_TRONG = 0;
                    _kheUoc.THUC_THU_GOC_VAY = 0;
                }
            }

            if (grGocLaiTruocHan.ItemsSource != null)
            {
                DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
                foreach (DataRow dr in dt.Rows)
                {
                    decimal soTienLai = Convert.ToDecimal(dr["tt_tra_lai"]);
                    decimal soTienGoc = Convert.ToDecimal(dr["tt_tra_goc"]);
                    if (soTienLai > 0)
                    {
                        _kheUoc.THUC_THU_LAI_TRONG += soTienLai;
                    }

                    if (soTienGoc > 0)
                    {
                        _kheUoc.THUC_THU_GOC_VAY += soTienGoc;
                    }
                }
            }
        }

        private void TaoThongTinMacDinhNopTienVaoTKKKH()
        {
            numKhongKyHanTienThua.Value = 0;
            //numKhongKyHanTienThua_ValueChanged(null, null);
        }

        private void TaoThongTinMacDinhTraTruoc()
        {
            numSoTienNop.Value = 0;
            numPhiTraTruoc.Value = 0;
            //numSoTienNop_ValueChanged(null, null);
        }

        private void numTienMat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            double? soTienThua = numTienMat.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value);
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                if (grbXuLyTienThua.IsEnabled == false)
                {
                    grbXuLyTienThua.IsEnabled = true;
                    if (chkTKKhongKyHanTienThua.Tag != "False")
                    {
                        chkTKKhongKyHanTienThua.IsChecked = true;
                        numKhongKyHanTienThua.IsEnabled = true;
                        numKhongKyHanTienThua.Value = soTienThua;
                        TinhToanNopVaoTietKiem();
                    }
                    else
                    {
                        chkTKKhongKyHanTienThua.IsEnabled = false;
                    }

                    if (chkTraTruocHan.Tag == "False")
                    {
                        chkTraTruocHan.IsEnabled = false;
                    }
                }
                else
                {
                    if (chkTraTruocHan.IsChecked == true)
                    {
                        numSoTienNop.Value = Convert.ToDouble(Math.Min(Convert.ToDecimal(soTienThua),_soTienGocConPhaiNop));
                        if (Convert.ToDecimal(numPhiTraTruoc.Value) == 0)
                        {
                            numPhiTraTruoc.Value = 0;
                        }
                        numKhongKyHanTienThua.Value = Math.Max(0, Convert.ToDouble(soTienThua - numSoTienNop.Value - numPhiTraTruoc.Value));
                        numSoTienNop.IsEnabled = true;
                        //numSoTienNop_ValueChanged(null, null);
                        //numKhongKyHanTienThua_ValueChanged(null, null);
                        TinhToanTienTraTruoc();
                        TinhToanNopVaoTietKiem();
                    }
                    else if (chkTKKhongKyHanTienThua.IsChecked == true)
                    {
                        numKhongKyHanTienThua.Value = soTienThua;
                        //numKhongKyHanTienThua_ValueChanged(null, null);
                        TinhToanNopVaoTietKiem();
                    }
                    
                }
            }
        }

        private void numSoTienRutTuTK_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (numSoTienRutTuTK.Value > numSoDuTaiKhoanTK.Value)
            {
                numSoTienRutTuTK.Value = numSoDuTaiKhoanTK.Value;
            }
            double? soTienThua = numSoTienRutTuTK.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value);
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                if (grbXuLyTienThua.IsEnabled == false)
                {
                    grbXuLyTienThua.IsEnabled = true;
                    chkTKKhongKyHanTienThua.IsChecked = true;
                    numKhongKyHanTienThua.Value = soTienThua;
                    //numKhongKyHanTienThua_ValueChanged(null, null);
                }
                else
                {
                    if (chkTKKhongKyHanTienThua.IsChecked == true)
                    {
                        numKhongKyHanTienThua.Value = soTienThua;
                        //numKhongKyHanTienThua_ValueChanged(null, null);
                    }
                    else if (chkTraTruocHan.IsChecked == true)
                    {
                        numSoTienNop.Value = soTienThua;
                        numPhiTraTruoc.Value = 0;
                        //numSoTienNop_ValueChanged(null, null);
                    }
                }
            }
        }
   
        private DANH_SACH_KHE_UOC_VONG_VAY CopyObject(DANH_SACH_KHE_UOC_VONG_VAY obj)
        {
            DANH_SACH_KHE_UOC_VONG_VAY objCopy = new DANH_SACH_KHE_UOC_VONG_VAY();
            foreach (PropertyInfo prty in objCopy.GetType().GetProperties())
            {
                if (obj.GetType().GetProperties().Contains(prty))
                {
                    prty.SetValue(objCopy, prty.GetValue(obj, null), null);
                }
            }
            return objCopy;
        }

        #region Tinh toan so tien
        private void TinhToanTienTraTruoc()
        {
            decimal soTienThua = Convert.ToDecimal(numTienMat.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value));
            numSoTienNop.Value = Convert.ToDouble(soTienThua) + Convert.ToDouble(numPhiTraTruoc.Value);
            if (soTienThua >= _soTienGocConPhaiNop)
            {
                soTienThua = _soTienGocConPhaiNop;
                numSoTienNop.Value = Convert.ToDouble(soTienThua) + Convert.ToDouble(numPhiTraTruoc.Value);
                chkTatToan.IsEnabled = true;
                chkTatToan.IsChecked = true;
            }
            else
            {
                chkTatToan.IsEnabled = false;
                chkTatToan.IsChecked = false;
            }

            if (chkTatToan.IsChecked == false)
            {
                if (numSoTienNop.Value != null && numSoTienNop.Value > 0)
                {
                    chkTraTruocHan.IsChecked = true;
                    TaoThongTinTraTruoc(false);
                }
                else
                {
                    chkTraTruocHan.IsChecked = false;
                    TaoThongTinTraTruoc(true);
                }
            }
        }

        private void TinhToanNopVaoTietKiem()
        {
            decimal soTienThua = Convert.ToDecimal(numTienMat.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value));
            if (grTKKhongKyHanTienThua.ItemsSource != null)
            {
                numKhongKyHanTienThua.Value = Convert.ToDouble(soTienThua) - numSoTienNop.Value;
                if (numKhongKyHanTienThua.Value != null && numKhongKyHanTienThua.Value > 0)
                {
                    chkTKKhongKyHanTienThua.IsChecked = true;
                }
                PhanBoSoTienNopVaoTietKiemKoKyHan(Convert.ToDecimal(numKhongKyHanTienThua.Value));
            }
        }

        private void PhanBoSoTienNopVaoTietKiemKoKyHan(decimal sotien)
        {
            if (grTKKhongKyHanTienThua.ItemsSource != null)
            {
                DataTable dt = ((DataView)grTKKhongKyHanTienThua.ItemsSource).Table;
                foreach (DataRow dr in dt.Rows)
                {
                    if (sotien > 0)
                    {
                        dr["so_tien_nop"] = sotien;
                        sotien = 0;
                    }
                    else
                    {
                        dr["so_tien_nop"] = sotien;
                    }
                }
            }
        }

        private void TaoThongTinTraTruoc(bool isBack)
        {
            if (grGocLaiTruocHan.ItemsSource != null)
            {
                DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
                if (isBack)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        dt.Rows[i]["tt_tra_lai"] = 0;
                        dt.Rows[i]["tt_tra_goc"] = 0;
                    }
                }
                else
                {
                    decimal soTienThua = Convert.ToDecimal(numSoTienNop.Value);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal soTienTraLai = Convert.ToDecimal(dt.Rows[i]["kh_tra_lai"]);
                        decimal soTienTraGoc = Convert.ToDecimal(dt.Rows[i]["kh_tra_goc"]);

                        if (soTienThua <= soTienTraLai)
                        {
                            dt.Rows[i]["tt_tra_lai"] = soTienThua;
                            break;
                        }
                        else
                        {
                            dt.Rows[i]["tt_tra_lai"] = soTienTraLai;
                            soTienThua = soTienThua - soTienTraLai;

                            if (soTienThua <= soTienTraGoc)
                            {
                                dt.Rows[i]["tt_tra_goc"] = soTienThua;
                                break;
                            }
                            else
                            {
                                dt.Rows[i]["tt_tra_goc"] = soTienTraGoc;
                                soTienThua = soTienThua - soTienTraGoc;
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(dt.Rows[i]["tt_tra_lai"]) != Convert.ToDecimal(dt.Rows[i]["kh_tra_lai"]))
                        {
                            _tinhPhiTraTruoc = false;
                            break;
                        }
                        else
                        {
                            _tinhPhiTraTruoc = true;
                        }
                    }

                    if (_tinhPhiTraTruoc == true)
                    {
                        //numPhiTraTruoc.Value = Convert.ToDouble(_soTienGocConPhaiNop) * numMucPhi.Value;
                        if (soTienThua > 0)
                        {
                            _kheUoc.THUC_THU_NOP_VAO_TKKKH = soTienThua;
                        }
                    }
                    else
                    {
                        numPhiTraTruoc.Value = 0;
                    }
                }
                grGocLaiTruocHan.ItemsSource = dt.DefaultView;
            }
        }

        private void TaoThongTinTatToan()
        {
            numPhiTraTruoc.IsEnabled = true;
            decimal soTienThua = Convert.ToDecimal(numSoTienNop.Value);
            TaoThongTinTraTruoc(true);
            DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal soTienTraGoc = Convert.ToDecimal(dt.Rows[i]["kh_tra_goc"]);
                if (soTienThua <= soTienTraGoc)
                {
                    dt.Rows[i]["tt_tra_goc"] = soTienThua;
                    break;
                }
                else
                {
                    dt.Rows[i]["tt_tra_goc"] = soTienTraGoc;
                    soTienThua = soTienThua - soTienTraGoc;
                }
            }
            grGocLaiTruocHan.ItemsSource = dt.DefaultView;
        }
        #endregion

        private void chkTKKhongKyHanTienThua_Click(object sender, RoutedEventArgs e)
        {
            if (chkTKKhongKyHanTienThua.IsChecked == true)
            {
                TinhToanNopVaoTietKiem();
            }
            else
            {
                numKhongKyHanTienThua.Value = 0;
                TinhToanTienTraTruoc();
            }
        }

        private void chkTraTruocHan_Click(object sender, RoutedEventArgs e)
        {
            if (chkTraTruocHan.IsChecked == true)
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            else
            {
                numPhiTraTruoc.Value = 0;
                chkTatToan.IsChecked = false;
                chkTatToan.IsEnabled = false;
                numSoTienNop.Value = 0;
                TinhToanNopVaoTietKiem();
            }
        }

        private void numPhiTraTruoc_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            decimal maxPhi = Convert.ToDecimal(numTienMat.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value)) - _soTienGocConPhaiNop;
            if (Convert.ToDecimal(numPhiTraTruoc.Value) > maxPhi)
            {
                numPhiTraTruoc.Value = Convert.ToDouble(maxPhi);
            }
            numSoTienNop.Value = numSoTienNop.Value + numPhiTraTruoc.Value;
            TinhToanTienTraTruoc();
            TinhToanNopVaoTietKiem();
        }

        private void numKhongKyHanTienThua_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            PhanBoSoTienNopVaoTietKiemKoKyHan(Convert.ToDecimal(numKhongKyHanTienThua.Value));
        }

        private void numSoTienNop_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (Convert.ToDecimal(numSoTienNop.Value) < _soTienGocConPhaiNop)
            {
                numPhiTraTruoc.Value = 0;
                chkTatToan.IsChecked = false;
                numPhiTraTruoc.IsEnabled = false;
            }
            else
            {
                chkTatToan.IsChecked = true;
                numPhiTraTruoc.IsEnabled = true;
            }
        }

        private void chkTatToan_Checked(object sender, RoutedEventArgs e)
        {
            numPhiTraTruoc.IsEnabled = true;
            TaoThongTinTatToan();
        }

        private void chkTatToan_Unchecked(object sender, RoutedEventArgs e)
        {
            numSoTienNop.Value = numSoTienNop.Value - numPhiTraTruoc.Value;
            numPhiTraTruoc.Value = 0;
            numPhiTraTruoc.IsEnabled = false;
            TaoThongTinTraTruoc(true);
            TaoThongTinTraTruoc(false);
            TinhToanNopVaoTietKiem();
        }

        private void chkTKKhongKyHanTienThua_Checked(object sender, RoutedEventArgs e)
        {
            if (LObject.IsNullOrEmpty(grTKKhongKyHanTienThua.ItemsSource))
            {
                chkTKKhongKyHanTienThua.IsChecked = false;
                cmbTaiKhoanTK.IsEnabled = false;
                numSoTienRutTuTK.IsEnabled = false;
                numTienMat.IsEnabled = true;
                numKhongKyHanTienThua.IsEnabled = false;
            }
        }

    }

    public class CalTienNopTK : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double total = 0;
            total = Math.Max(0, System.Convert.ToDouble(values[0]) + System.Convert.ToDouble(values[1]) - System.Convert.ToDouble(values[2]) - (System.Convert.ToDouble(values[3]) + System.Convert.ToDouble(values[4]) + System.Convert.ToDouble(values[5]) + System.Convert.ToDouble(values[6])));
            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class CalTraTruoc : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double total = 0;
            total = Math.Max(0, System.Convert.ToDouble(values[0]) + System.Convert.ToDouble(values[1]) - System.Convert.ToDouble(values[2]) - (System.Convert.ToDouble(values[3]) + System.Convert.ToDouble(values[4]) + System.Convert.ToDouble(values[5]) + System.Convert.ToDouble(values[6])));
            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
