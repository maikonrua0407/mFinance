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
using Presentation.Process.Common;

namespace PresentationWPF.HoaDonTienKy.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopupChiTietThucThu.xaml
    /// </summary>
    public partial class ucPopupThucThu : UserControl
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

        private string _thuTuPhanBo = "";

        private string _cachThuLaiTToan = "";

        private string _maGiaoDich = "";

        int _idGDich = 0;

        private decimal _soTienTKQDNop = 0;
        private decimal _quyTTro = 0;
        #endregion

        #region KhoiTao
        public ucPopupThucThu(DANH_SACH_KHE_UOC_VONG_VAY obj, string ngayGD, string maGiaoDich, int idGDich)
        {
            InitializeComponent();
            _ngayThuTien = ngayGD;
            _maGiaoDich = maGiaoDich;
            _idGDich = idGDich;
            _objKheUocViMo = obj;
            _kheUoc = CopyObject(obj);
            GetValueParams();
            KhoiTaoCombobox();
            EvenHandlerRegister();
            LayDSSoTietKiemKhongKyHan();
            LayThongTinKeHoach();
            LoadThongTinTietKiemNopTien();
            LoadThongTinForm();
            _soTienTKQDNop = _kheUoc.KE_HOACH_TKQD;
            _quyTTro = _kheUoc.THUC_THU_QUY_TT;
        }

        private void EvenHandlerRegister()
        {
            chkThuTKQD.Unchecked += new RoutedEventHandler(chkThuTKQD_Unchecked);
            chkThuTKQD.Checked += new RoutedEventHandler(chkThuTKQD_Checked);
            chkQuyTTro.Unchecked += new RoutedEventHandler(chkQuyTTro_Unchecked);
            chkQuyTTro.Checked += new RoutedEventHandler(chkQuyTTro_Checked);
            numTienMat.ValueChanged +=new EventHandler<Telerik.Windows.RadRoutedEventArgs>(numTienMat_ValueChanged);
            numTKQD.ValueChanged +=new EventHandler<Telerik.Windows.RadRoutedEventArgs>(numTKQD_ValueChanged);
            chkTKTietKiemKhongKyHan.Click +=new RoutedEventHandler(chkTKTietKiemKhongKyHan_Click);
            chkTKKhongKyHanTienThua.Click +=new RoutedEventHandler(chkTKKhongKyHanTienThua_Click);
            chkTraTruocHan.Click +=new RoutedEventHandler(chkTraTruocHan_Click);
            chkTatToan.Checked +=new RoutedEventHandler(chkTatToan_Checked);
            chkTatToan.Unchecked +=new RoutedEventHandler(chkTatToan_Unchecked);
        }

        private void EvenHandlerUnRegister()
        {
            chkThuTKQD.Unchecked -= new RoutedEventHandler(chkThuTKQD_Unchecked);
            chkThuTKQD.Checked -= new RoutedEventHandler(chkThuTKQD_Checked);
            chkQuyTTro.Unchecked -= new RoutedEventHandler(chkQuyTTro_Unchecked);
            chkQuyTTro.Checked -= new RoutedEventHandler(chkQuyTTro_Checked);
            numTienMat.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(numTienMat_ValueChanged);
            numTKQD.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(numTKQD_ValueChanged);
            chkTKTietKiemKhongKyHan.Click -= new RoutedEventHandler(chkTKTietKiemKhongKyHan_Click);
            chkTKKhongKyHanTienThua.Click -= new RoutedEventHandler(chkTKKhongKyHanTienThua_Click);
            chkTraTruocHan.Click -= new RoutedEventHandler(chkTraTruocHan_Click);
            chkTatToan.Checked -= new RoutedEventHandler(chkTatToan_Checked);
            chkTatToan.Unchecked -= new RoutedEventHandler(chkTatToan_Unchecked);
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstDK.Add(_kheUoc.MA_KHACH_HANG);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GetValueParams()
        {
            _thuTuPhanBo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY, ClientInformation.MaDonVi);
            _cachThuLaiTToan = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TDVM_CACH_THU_LAI_KHI_TTOAN, ClientInformation.MaDonVi);
            if (_thuTuPhanBo.IsNullOrEmptyOrSpace())
                _thuTuPhanBo = "GOCVAY#LAI_VAY#LAI_QHAN#TKBB#QUY_TT";
            if (_cachThuLaiTToan.IsNullOrEmptyOrSpace())
                _cachThuLaiTToan = "TOAN_BO";
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
            EvenHandlerUnRegister();
            decimal tongThu = _soTienGocConPhaiNop + _kheUoc.KE_HOACH_GOC_VAY + _kheUoc.KE_HOACH_LAI_QUA_HAN + _kheUoc.KE_HOACH_LAI_TRONG_HAN + +Convert.ToDecimal(numTKQD.Value.GetValueOrDefault(0));
            decimal tongTienMat = Convert.ToDecimal(numTienMat.Value.GetValueOrDefault());
            if (_cachThuLaiTToan.Equals("TOAN_BO"))
                tongThu += _soTienLaiConPhaiTra;
            if (chkTKTietKiemKhongKyHan.IsChecked.GetValueOrDefault() && tongThu <= tongTienMat)
            {
                chkTKTietKiemKhongKyHan.IsChecked = false;
            }
            else if (chkTKTietKiemKhongKyHan.IsChecked.GetValueOrDefault())
            {
                numTienRutTK.Value = 0;
                grNopTienTuTKTK.IsEnabled = true;
                chkTKKhongKyHanTienThua.IsEnabled = false;
                numKhongKyHanTienThua.Value = 0;
                grTKKhongKyHanTienThua.IsReadOnly = true;
            }
            TinhToanTienTraTrongKy();
            TinhToanTienTraTruoc();
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
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
                                        dr["so_du"] = obj.SO_DU;
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
                    numKhongKyHanTienThua.Value = Convert.ToDouble(ds.Tables[0].AsEnumerable().Sum(f => f.Field<Decimal>("so_tien_nop")));
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
                        _soTienLaiConPhaiTra += Convert.ToDecimal(dr["kh_tra_lai"]);
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
            if(_kheUoc.KE_HOACH_TKQD <= _kheUoc.THUC_THU_TKQD)
                numTKQD.Value = Convert.ToDouble(_kheUoc.KE_HOACH_TKQD);
            else
                numTKQD.Value = Convert.ToDouble(_kheUoc.THUC_THU_TKQD);
            if (_kheUoc.KE_HOACH_GOC_VAY <= _kheUoc.THUC_THU_GOC_VAY)
                numGocVay.Value = Convert.ToDouble(_kheUoc.KE_HOACH_GOC_VAY);
            else
                numGocVay.Value = Convert.ToDouble(_kheUoc.THUC_THU_GOC_VAY);
            if (_kheUoc.KE_HOACH_LAI_TRONG_HAN <= _kheUoc.THUC_THU_LAI_TRONG)
                numLaiTrongHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_TRONG_HAN);
            else
                numLaiTrongHan.Value = Convert.ToDouble(_kheUoc.THUC_THU_LAI_TRONG);
            if (_kheUoc.KE_HOACH_LAI_QUA_HAN <= _kheUoc.THUC_THU_LAI_QUA_HAN)
                numLaiQuaHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_QUA_HAN);
            else
                numLaiQuaHan.Value = Convert.ToDouble(_kheUoc.THUC_THU_LAI_QUA_HAN);
            numQuyTTro.Value = Convert.ToDouble(_kheUoc.THUC_THU_QUY_TT);
            numTienMat.Value = Convert.ToDouble(_kheUoc.THUC_THU_TIEN_MAT);
            // Số tiền nộp
            if (_kheUoc.NOP_TIEN_TU_TKKKH == "KHONG")
            {
                chkTKTietKiemKhongKyHan.IsChecked = false;
            }
            else
            {
                numTienRutTK.Value = Convert.ToDouble(_kheUoc.THUC_NOP_TU_TKKKH);
                chkTKTietKiemKhongKyHan.IsChecked = true;

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
                decimal soTienNop = _kheUoc.TONG_TIEN_TRA_TRUOC;
                numSoTienNop.Value = Convert.ToDouble(soTienNop);
                decimal tongGocLaiTT = _soTienGocConPhaiNop;
                if (soTienNop > 0)
                {
                    DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        dt.Rows[i]["tt_tra_lai"] = 0;
                        dt.Rows[i]["tt_tra_goc"] = 0;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal soTienTraLai = Convert.ToDecimal(dt.Rows[i]["kh_tra_lai"]);
                        decimal soTienTraGoc = Convert.ToDecimal(dt.Rows[i]["kh_tra_goc"]);
                        foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
                        {
                            switch (loaiPhanBo)
                            {
                                case "GOCVAY":
                                    if (soTienNop <= soTienTraGoc)
                                    {
                                        dt.Rows[i]["tt_tra_goc"] = soTienNop;
                                    }
                                    else
                                    {
                                        dt.Rows[i]["tt_tra_goc"] = soTienTraGoc;

                                    }
                                    soTienNop -= Convert.ToDecimal(dt.Rows[i]["tt_tra_goc"]);
                                    break;
                                case "LAI_VAY":
                                    if (soTienNop <= soTienTraLai)
                                    {
                                        dt.Rows[i]["tt_tra_lai"] = soTienNop;
                                        break;
                                    }
                                    else
                                    {
                                        dt.Rows[i]["tt_tra_lai"] = Convert.ToDecimal(dt.Rows[i]["tt_tra_lai"]);
                                    }
                                    soTienNop -= Convert.ToDecimal(dt.Rows[i]["tt_tra_lai"]);
                                    break;
                                case "LAI_QHAN":

                                    break;
                                case "TKBB":

                                    break;
                            }
                            if (soTienNop == 0)
                                break;
                        }
                        if (soTienNop == 0)
                            break;
                    }
                    grGocLaiTruocHan.ItemsSource = dt.DefaultView;
                    grGocLaiTruocHan.Rebind();
                }
                if(_cachThuLaiTToan.Equals("TOAN_BO"))
                    tongGocLaiTT += _soTienLaiConPhaiTra;
                if (Convert.ToDecimal(numSoTienNop.Value.GetValueOrDefault()) >= tongGocLaiTT)
                {
                    chkTatToan.IsEnabled = true;
                    chkTatToan.IsChecked = true;
                    numPhiTraTruoc.IsEnabled = true;
                    numPhiTraTruoc.Value = Convert.ToDouble(_kheUoc.PHI_TRA_TRUOC);
                }
                else
                {
                    chkTatToan.IsEnabled = false;
                    chkTatToan.IsChecked = false;
                    numPhiTraTruoc.IsEnabled = false;
                    numPhiTraTruoc.Value = Convert.ToDouble(0);
                }
            }

            if (_kheUoc.PHI_TRA_TRUOC > 0)
            {
                chkTatToan.IsEnabled = true;
                chkTatToan.IsChecked = true;
                numPhiTraTruoc.IsEnabled = true;
                numPhiTraTruoc.Value = Convert.ToDouble(_kheUoc.PHI_TRA_TRUOC);
            }
            
            numTKQD.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(numTKQD_ValueChanged);

            SetEnabledControl();
        }

        void numTKQD_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            _soTienTKQDNop = Convert.ToDecimal(numTKQD.Value.GetValueOrDefault(0));
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();
            SetEnabledControl();
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            EvenHandlerRegister();
        }

        private void LoadThongTinTietKiemNopTien()
        {
            DataSet ds = new HoaDonTienKyProcess().getDSSoTKBTV(_kheUoc.MA_KHE_UOC, _ngayThuTien, _idGDich.ToString());
            DANH_SACH_SO[] arrSoTK = _objKheUocViMo.DSACH_SO_RUT_TIEN;
            if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
            {
                DANH_SACH_SO objSoTK = null;
                List<DANH_SACH_SO> lstSoTK = new List<DANH_SACH_SO>();
                foreach (DataRow dr in ds.Tables["POPUP"].Rows)
                {
                    objSoTK = new DANH_SACH_SO();
                    objSoTK.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                    objSoTK.SO_SO = dr["SO_SO_TG"].ToString();
                    objSoTK.SO_DU = Convert.ToDecimal(dr["SO_TIEN"]);
                    objSoTK.MA_KHANG = dr["MA_KHANG"].ToString();
                    objSoTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    if (!arrSoTK.IsNullOrEmpty() && !arrSoTK.FirstOrDefault(f => f.SO_SO.Equals(dr["SO_SO_TG"].ToString())).IsNullOrEmpty())
                        objSoTK.SO_TIEN_RUT_RA = arrSoTK.FirstOrDefault(f => f.SO_SO.Equals(dr["SO_SO_TG"].ToString())).SO_TIEN_RUT_RA;
                    else
                        objSoTK.SO_TIEN_RUT_RA = Convert.ToDecimal(dr["SO_TIEN_RUT"]);
                    lstSoTK.Add(objSoTK);
                }
                grNopTienTuTKTK.ItemsSource = lstSoTK;
                grNopTienTuTKTK.Rebind();
                numTienRutTK.Value = Convert.ToDouble(lstSoTK.Sum(f => f.SO_TIEN_NOP_VAO));
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
                lstRutTien = grNopTienTuTKTK.ItemsSource as List<DANH_SACH_SO>;
                _kheUoc.THUC_NOP_TU_TKKKH = lstRutTien.Sum(f => f.SO_TIEN_RUT_RA);
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
                            _kheUoc.NOP_TIEN_VAO_TKKKH = "CO";
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
            _objKheUocViMo = _kheUoc;
        }

        private bool Validation()
        {
            bool kq = true;
            double? tongSoTienNop = numTienMat.Value + numTienRutTK.Value;
            double? tongSoTienHToan = numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value
                + numKhongKyHanTienThua.Value + numSoTienNop.Value + numPhiTraTruoc.Value + numQuyTTro.Value;
            if (tongSoTienHToan!=tongSoTienNop)
            {
                LMessage.ShowMessage("M.TinDung.PopupNghiepVu.BinhKhanh.ucPopupChiTietThucThuBinhKhanh.LoiTongTienNop", LMessage.MessageBoxType.Warning);
                return false;
            }
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


        private void chkNopBangTienMat_Click(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            if (chkNopBangTienMat.IsChecked == true)
            {
                numTienMat.Value = Convert.ToDouble(numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value);
                numTienMat.IsEnabled = true;

            }
            else
            {
                numTienMat.IsEnabled = false;
                numTienMat.Value = 0;
            }
            EvenHandlerRegister();
        }

        private void PhanBoSoTienThuDuoc()
        {
            _kheUoc.THUC_THU_TIEN_MAT = Convert.ToDecimal(numTienMat.Value);
            _kheUoc.THUC_THU_LAI_QUA_HAN = Convert.ToDecimal(numLaiQuaHan.Value);
            _kheUoc.THUC_THU_LAI_TRONG = Convert.ToDecimal(numLaiTrongHan.Value);
            _kheUoc.THUC_THU_GOC_VAY = Convert.ToDecimal(numGocVay.Value);
            _kheUoc.THUC_THU_TKQD = Convert.ToDecimal(numTKQD.Value);
            _kheUoc.THUC_THU_QUY_TT = Convert.ToDecimal(numQuyTTro.Value);
            if (_kheUoc.NOP_TIEN_VAO_TKKKH == "KHONG")
            {
                _kheUoc.THUC_THU_TKQD += Convert.ToDecimal(numKhongKyHanTienThua.Value);
            }
            else
            {
                _kheUoc.THUC_THU_TKQD += Convert.ToDecimal(numKhongKyHanTienThua.Value) - _kheUoc.THUC_THU_NOP_VAO_TKKKH;
            }
            if (grGocLaiTruocHan.ItemsSource != null && (chkTraTruocHan.IsChecked.GetValueOrDefault() || chkTatToan.IsChecked.GetValueOrDefault()))
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
            EvenHandlerUnRegister();
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();
            SetEnabledControl();
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            EvenHandlerRegister();
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
        private void TinhToanTienTraTrongKy()
        {
            decimal soTienNop = Convert.ToDecimal(numTienMat.Value.GetValueOrDefault()) + Convert.ToDecimal(numTienRutTK.Value.GetValueOrDefault());
            numGocVay.Value = 0;
            numLaiTrongHan.Value = 0;
            numLaiQuaHan.Value = 0;
            numTKQD.Value = 0;
            numQuyTTro.Value = 0;
            foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
            {
                switch (loaiPhanBo)
                {
                    case "GOCVAY":
                        if (soTienNop > _kheUoc.KE_HOACH_GOC_VAY)
                        {
                            numGocVay.Value = Convert.ToDouble(_kheUoc.KE_HOACH_GOC_VAY);
                        }
                        else
                        {
                            numGocVay.Value = Convert.ToDouble(soTienNop);
                        }
                        soTienNop -= Convert.ToDecimal(numGocVay.Value.GetValueOrDefault());
                        break;
                    case "LAI_VAY":
                        if (soTienNop > _kheUoc.KE_HOACH_LAI_TRONG_HAN)
                        {
                            numLaiTrongHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_TRONG_HAN);
                        }
                        else
                        {
                            numLaiTrongHan.Value = Convert.ToDouble(soTienNop);
                        }
                        soTienNop -= Convert.ToDecimal(numLaiTrongHan.Value.GetValueOrDefault());
                        break;
                    case "LAI_QHAN":
                        if (soTienNop > _kheUoc.KE_HOACH_LAI_QUA_HAN)
                        {
                            numLaiQuaHan.Value = Convert.ToDouble(_kheUoc.KE_HOACH_LAI_QUA_HAN);
                        }
                        else
                        {
                            numLaiQuaHan.Value = Convert.ToDouble(soTienNop);
                        }
                        soTienNop -= Convert.ToDecimal(numLaiQuaHan.Value.GetValueOrDefault());
                        break;
                    case "TKBB":
                        if (!chkThuTKQD.IsChecked.GetValueOrDefault())
                        {
                            if (soTienNop > _soTienTKQDNop)
                            {
                                numTKQD.Value = Convert.ToDouble(_soTienTKQDNop);
                            }
                            else 
                            {
                                numTKQD.Value = Convert.ToDouble(soTienNop);
                            }
                            soTienNop -= Convert.ToDecimal(numTKQD.Value.GetValueOrDefault());
                        }
                        
                        break;
                    case "QUY_TT":
                        if (!chkQuyTTro.IsChecked.GetValueOrDefault())
                        {
                            if (soTienNop > _quyTTro)
                            {
                                numQuyTTro.Value = Convert.ToDouble(_quyTTro);
                            }
                            else 
                            {
                                numQuyTTro.Value = Convert.ToDouble(soTienNop);
                            }
                            soTienNop -= Convert.ToDecimal(numQuyTTro.Value.GetValueOrDefault());
                        }
                        
                        break;
                }
                if (soTienNop == 0)
                    break;
            }
        }

        private void TinhToanTienTraTruoc()
        {
            decimal soTienThua = Convert.ToDecimal((numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numKhongKyHanTienThua.Value + numQuyTTro.Value));
            decimal tongTienGocLai = _soTienGocConPhaiNop;
            if (_cachThuLaiTToan.Equals("TOAN_BO"))
            {
                tongTienGocLai += _soTienLaiConPhaiTra;
            }
            if (soTienThua >= tongTienGocLai)
            {
                TaoThongTinTatToan();
            }
            else
            {
                TaoThongTinTraTruoc(false);
            }
            TinhToanNopVaoTietKiem();
        }

        private void TinhToanNopVaoTietKiem()
        {
            decimal soTienThua = Convert.ToDecimal(numTienMat.Value + numTienRutTK.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numSoTienNop.Value + numQuyTTro.Value));
            if (!chkTKKhongKyHanTienThua.IsChecked.GetValueOrDefault())
            {
                numKhongKyHanTienThua.Value = 0;
            }
            if (grTKKhongKyHanTienThua.ItemsSource != null)
            {
                numKhongKyHanTienThua.Value = Convert.ToDouble(soTienThua);
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
            if (LObject.IsNullOrEmpty(grGocLaiTruocHan.ItemsSource))
                return;
            chkTraTruocHan.IsChecked = true;
            if (grGocLaiTruocHan.ItemsSource != null)
            {
                DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    dt.Rows[i]["tt_tra_lai"] = 0;
                    dt.Rows[i]["tt_tra_goc"] = 0;
                }
                if(1==1 && !isBack)
                {
                    decimal soTienThua = Convert.ToDecimal((numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal soTienTraLai = Convert.ToDecimal(dt.Rows[i]["kh_tra_lai"]);
                        decimal soTienTraGoc = Convert.ToDecimal(dt.Rows[i]["kh_tra_goc"]);
                        foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
                        {
                            switch (loaiPhanBo)
                            {
                                case "GOCVAY":
                                    if (soTienThua <= soTienTraGoc)
                                    {
                                        dt.Rows[i]["tt_tra_goc"] = soTienThua;
                                    }
                                    else
                                    {
                                        dt.Rows[i]["tt_tra_goc"] = soTienTraGoc;
                                        
                                    }
                                    soTienThua -= Convert.ToDecimal(dt.Rows[i]["tt_tra_goc"]);
                                    break;
                                case "LAI_VAY":
                                    if (soTienThua <= soTienTraLai)
                                    {
                                        dt.Rows[i]["tt_tra_lai"] = soTienThua;
                                    }
                                    else
                                    {
                                        dt.Rows[i]["tt_tra_lai"] = soTienTraLai;
                                    }
                                    soTienThua -= Convert.ToDecimal(dt.Rows[i]["tt_tra_lai"]);
                                    break;
                                case "LAI_QHAN":
                                    
                                    break;
                                case "TKBB":
                                  
                                    break;
                            }
                            if (soTienThua == 0)
                                break;
                        }
                        if (soTienThua == 0)
                            break;
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
                        
                    }
                    else
                    {
                        numPhiTraTruoc.Value = 0;
                    }
                }
                grGocLaiTruocHan.ItemsSource = dt.DefaultView;
                numSoTienNop.Value = Convert.ToDouble(dt.AsEnumerable().Sum(f => f.Field<Decimal>("tt_tra_lai") + f.Field<Decimal>("tt_tra_goc")));
            }
        }

        private void TaoThongTinTatToan()
        {
            if (LObject.IsNullOrEmpty(grGocLaiTruocHan.ItemsSource))
                return;
            decimal soTienThua = Convert.ToDecimal(numTienMat.Value + numTienRutTK.Value - numTKQD.Value - numGocVay.Value - numLaiTrongHan.Value - numLaiQuaHan.Value - numQuyTTro.Value);
            decimal tongGocLai = _soTienGocConPhaiNop;
            if (_cachThuLaiTToan.Equals("TOAN_BO"))
            {
                tongGocLai += _soTienLaiConPhaiTra;
            }
            if (soTienThua < tongGocLai)
            {
                TaoThongTinTraTruoc(true);
                return;
            }
            DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal soTienTraGoc = Convert.ToDecimal(dt.Rows[i]["kh_tra_goc"]);
                decimal soTienTraLai = Convert.ToDecimal(dt.Rows[i]["kh_tra_lai"]);
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
                if (_cachThuLaiTToan.Equals("TOAN_BO"))
                {
                    if (soTienThua <= soTienTraLai)
                    {
                        dt.Rows[i]["tt_tra_lai"] = soTienThua;
                        break;
                    }
                    else
                    {
                        dt.Rows[i]["tt_tra_lai"] = soTienTraLai;
                        soTienThua = soTienThua - soTienTraLai;
                    }
                }
            }
            numPhiTraTruoc.IsEnabled = true;
            grGocLaiTruocHan.ItemsSource = dt.DefaultView;
            numSoTienNop.Value = Convert.ToDouble(tongGocLai);
        }
        #endregion

        private void chkTKKhongKyHanTienThua_Click(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            if (chkTKKhongKyHanTienThua.IsChecked == true)
            {
                TinhToanNopVaoTietKiem();
                grTKKhongKyHanTienThua.IsReadOnly = true;
            }
            else
            {
                grTKKhongKyHanTienThua.IsReadOnly = false;
                numKhongKyHanTienThua.Value = 0;
                TinhToanTienTraTruoc();
            }
            EvenHandlerRegister();
        }

        private void chkTraTruocHan_Click(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            if (chkTraTruocHan.IsChecked.GetValueOrDefault())
            {
                decimal soTienThua = Convert.ToDecimal((numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value));
                decimal tongGocLai = _soTienGocConPhaiNop;
                if (_cachThuLaiTToan.Equals("TOAN_BO"))
                {
                    tongGocLai += _soTienLaiConPhaiTra;
                }
                if (soTienThua < tongGocLai)
                {
                    TaoThongTinTraTruoc(false);
                }
                else
                {
                    TaoThongTinTatToan();
                }
            }
            else
            {
                DataTable dt = ((DataView)grGocLaiTruocHan.ItemsSource).ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["tt_tra_lai"] = 0;
                    dt.Rows[i]["tt_tra_goc"] = 0;
                }
                numPhiTraTruoc.IsEnabled = false;
                chkTatToan.IsChecked = false;
                grGocLaiTruocHan.ItemsSource = dt.DefaultView;
                numSoTienNop.Value = 0;
            }
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
        }

        private void numPhiTraTruoc_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            decimal maxPhi = Convert.ToDecimal(numTienMat.Value - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value)) - _soTienGocConPhaiNop;
            if (Convert.ToDecimal(numPhiTraTruoc.Value) > maxPhi)
            {
                numPhiTraTruoc.Value = Convert.ToDouble(maxPhi);
            }
            numSoTienNop.Value = numSoTienNop.Value + numPhiTraTruoc.Value;
            TinhToanTienTraTruoc();
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
        }

        private void numKhongKyHanTienThua_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            PhanBoSoTienNopVaoTietKiemKoKyHan(Convert.ToDecimal(numKhongKyHanTienThua.Value));
            TinhToanTienTraTruoc();
            EvenHandlerRegister();
        }

        private void numSoTienNop_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            if (Convert.ToDecimal(numSoTienNop.Value) < _soTienGocConPhaiNop)
            {
                numPhiTraTruoc.Value = 0;
                chkTatToan.IsChecked = false;
                numPhiTraTruoc.IsEnabled = false;
                TaoThongTinTraTruoc(false);
            }
            else
            {
                chkTatToan.IsChecked = true;
                numPhiTraTruoc.IsEnabled = true;
                TaoThongTinTatToan();
            }
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
        }

        private void chkTatToan_Checked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            TaoThongTinTatToan();
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
        }

        private void chkTatToan_Unchecked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            numSoTienNop.Value = numSoTienNop.Value - numPhiTraTruoc.Value;
            numPhiTraTruoc.Value = 0;
            numPhiTraTruoc.IsEnabled = false;
            TaoThongTinTraTruoc(true);
            TaoThongTinTraTruoc(false);
            TinhToanNopVaoTietKiem();
            EvenHandlerRegister();
        }

        private void grNopTienTuTKTK_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            EvenHandlerUnRegister();
            decimal tongTienRutTK = 0;
            if (!LObject.IsNullOrEmpty(grNopTienTuTKTK.ItemsSource))
            {
                List<DANH_SACH_SO> lstDSachSoTK = grNopTienTuTKTK.ItemsSource as List<DANH_SACH_SO>;
                tongTienRutTK = lstDSachSoTK.Sum(f => f.SO_TIEN_RUT_RA);
            }
            _kheUoc.TONG_SO_TIEN = (decimal)numTienMat.Value.GetValueOrDefault() + tongTienRutTK;
            _kheUoc.THUC_NOP_TU_TKKKH = tongTienRutTK;
            numTienRutTK.Value = (double)tongTienRutTK;
            TinhToanTienTraTrongKy();
            TinhToanTienTraTruoc();
            SetEnabledControl();
            EvenHandlerRegister();
        }

        private void grNopTienTuTKTK_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            if (LObject.IsNullOrEmpty(e.NewValue))
            {
                e.IsValid = false;
                e.ErrorMessage = "Value not null";
            }
            else if (!e.NewValue.ToString().IsNumeric())
            {
                e.IsValid = false;
                e.ErrorMessage = "Value must is Numeric";
            }
            else if (e.NewValue.ToString().StringToDecimal() < 0)
            {
                e.IsValid = false;
                e.ErrorMessage = "Value must than 0";
            }
            else
            {
                decimal tongThu = _soTienGocConPhaiNop + _kheUoc.KE_HOACH_GOC_VAY + _kheUoc.KE_HOACH_LAI_QUA_HAN + _kheUoc.KE_HOACH_LAI_TRONG_HAN + Convert.ToDecimal(numTKQD.Value.GetValueOrDefault(0));
                if (_cachThuLaiTToan.Equals("TOAN_BO"))
                    tongThu += _soTienLaiConPhaiTra;
                decimal tongTienRutTK = Convert.ToDecimal(numTienRutTK.Value.GetValueOrDefault(0)) + Convert.ToDecimal(numTienMat.Value.GetValueOrDefault(0)) + Convert.ToDecimal(e.NewValue) - Convert.ToDecimal(e.OldValue);
                
                if (tongThu < tongTienRutTK)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value not valid";
                }
            }
        }

        private void SetEnabledControl()
        {
            decimal keHoachNop = _kheUoc.KE_HOACH_TONG - _kheUoc.KE_HOACH_TKQD + Convert.ToDecimal(numTKQD.Value.GetValueOrDefault(0));

            if (keHoachNop < _kheUoc.TONG_SO_TIEN)
            {
                chkTatToan.IsEnabled = true;
                chkTraTruocHan.IsEnabled = true;
                grGocLaiTruocHan.IsEnabled = true;
                grbXuLyTienThua.IsEnabled = true;
                chkTKKhongKyHanTienThua.IsEnabled = true;
                grTKKhongKyHanTienThua.IsEnabled = true;
            }
            else
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsEnabled = false;
                chkTraTruocHan.IsEnabled = false;
                grGocLaiTruocHan.IsEnabled = false;
                chkTKKhongKyHanTienThua.IsEnabled = false;
                grTKKhongKyHanTienThua.IsEnabled = false;
            }
        }

        void chkThuTKQD_Checked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            numTKQD.IsEnabled = false;
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();
            
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            SetEnabledControl();
            EvenHandlerRegister();
        }

        void chkThuTKQD_Unchecked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            numTKQD.IsEnabled = true;
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();
            
            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            SetEnabledControl();
            EvenHandlerRegister();
        }

        void chkQuyTTro_Checked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            numQuyTTro.IsEnabled = false;
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();

            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            SetEnabledControl();
            EvenHandlerRegister();
        }

        void chkQuyTTro_Unchecked(object sender, RoutedEventArgs e)
        {
            EvenHandlerUnRegister();
            numQuyTTro.IsEnabled = true;
            TinhToanTienTraTrongKy();
            double? soTienThua = (numTienMat.Value + numTienRutTK.Value) - (numTKQD.Value + numGocVay.Value + numLaiTrongHan.Value + numLaiQuaHan.Value + numQuyTTro.Value);
            _kheUoc.TONG_SO_TIEN = _kheUoc.THUC_NOP_TU_TKKKH + (decimal)numTienMat.Value.GetValueOrDefault();

            if (soTienThua == null || soTienThua.Value <= 0)
            {
                grbXuLyTienThua.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkTraTruocHan.IsChecked = false;
                //chkGhiTKNoiBo.IsChecked = false;
                TaoThongTinMacDinhNopTienVaoTKKKH();
                TaoThongTinMacDinhTraTruoc();
            }
            else
            {
                TinhToanTienTraTruoc();
                TinhToanNopVaoTietKiem();
            }
            SetEnabledControl();
            EvenHandlerRegister();
        }

    }
}
