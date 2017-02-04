using System;
using System.Collections.Generic;
using System.Linq;
using Presentation.WebClient.Business;
using Presentation.Process;
using Utilities.Common;
using Presentation.WebClient.Business.CustomControl;
using System.Windows.Input;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using DataTable = System.Data.DataTable;

namespace Presentation.WebClient.Modules.GDKT.PhanLoai
{
    public partial class ucPhanLoaiCT : ControlBase
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        // Source combobox
        List<AutoCompleteEntry> lstSourceLoaiTK = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKHNB = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThuNhap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhChatTK = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHieu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTChatGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTChatBTru = new List<AutoCompleteEntry>();
        List<TTHAI_LY_DO> lstLyDo = new List<TTHAI_LY_DO>();
        public void LayDuLieuLyDo(List<TTHAI_LY_DO> lst)
        {
            lstLyDo = lst;
        }

        public event EventHandler OnSavingCompleted;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idPhanLoai = -1;
        private int _idKyHieuPLoai = -1;

        private bool? updateTNCP = null;
        #endregion

        #region Khoi tao
        public ucPhanLoaiCT()
        {
            //txtNgayHieuLuc.Text = ClientInformation.NgayLamViecHienTai ?? "";
            //lblTenPLTKCapTren.Text = "";
            InitCombobox();
            //txtMaPLTKCapTren.Focus();
            ResetForm();
        }

        public ucPhanLoaiCT(int id, string tthai, DatabaseConstant.Action action) : this()
        {
            _idPhanLoai = id;
            tthaiNvu = tthai;
            SetFormData();
            BeforeModifyFromList(action);
        }

        //private void KhoiTaoChung()
        //{
        //    InitializeComponent();
        //    HeThong hethong = new HeThong();
        //    hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiCT.xaml", ref Toolbar, ref mnuMain);
        //    foreach (var item in mnuMain.Items)
        //    {
        //        if (item is MenuItem)
        //            ((MenuItem)item).Click += btnShortcutKey_Click;
        //    }
        //    BindShortkey();
        //    cmbLoaiTK.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiTK_SelectionChanged);
        //    cmbLoaiKHNBo.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKHNBo_SelectionChanged);
        //    cmbTinhChatTK.SelectionChanged += new SelectionChangedEventHandler(cmbTinhChatTK_SelectionChanged);
        //    raddtTuNgayApDung.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
        //    lblTenPLTKCapTren.Content = "";
        //    txtMaPLTKCapTren.KeyDown += new KeyEventHandler(txtMaPLTKCapTren_KeyDown);
        //    InitCombobox();
        //    txtMaPLTKCapTren.Focus();
        //}



        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            try
            {
                //Loại tài khoản
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_PVI.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiTK, ref cmbLoaiTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, "NOI_BANG");

                //Loại khách hàng/ nội bộ
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_DTUONG.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiKHNB, ref cmbLoaiKHNBo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, "NOI_BO");

                //Loại khách hàng/ nội bộ
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.CO_KHONG.getValue());
                auto.GenAutoComboBox(ref lstSourceTChatBTru, ref cmbTinhChatTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, "CO");

                //Loại thu nhập/ chi phí
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_MDSD.getValue());
                auto.GenAutoComboBox(ref lstSourceThuNhap, ref cmbLoaiTNCP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, "KHAC");

                //Tính chất tài khoản
                auto.GenAutoComboBox(ref lstSourceTinhChatTK, ref cmbTinhChatTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue(), null);

                List<string> lstChon = new List<string>();
                lstChon.Add("NO");
                lstChon.Add("CO");
                //Tính chất gốc tài khoản
                //auto.GenAutoComboBox(ref lstSourceTChatGoc, ref cmbTinhChatGocTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue(), null, "NO", lstChon);

                //Kỳ hiệu
                auto.GenAutoComboBox(ref lstSourceKyHieu, ref cmbKyHieu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KY_HIEU.getValue(), null);
                cmbKyHieu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly Giao dien

        protected void Page_Load(object sender, EventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Trợ giúp
        /// </summary>
        //private void onHelp()
        //{
        //    CustomControl.CommonFunction.ShowHelp(this);
        //}

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idPhanLoai);

            bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void cmbLoaiTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auLoaiTK = au.GetEntryByDisplayName(lstSourceLoaiTK, ref cmbLoaiTK);
                if (auLoaiTK != null)
                {
                    if (auLoaiTK.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoan.NOI_BANG.layGiaTri())
                    {
                        cmbLoaiKHNBo.Enabled = true;
                        cmbLoaiKHNBo.SelectedIndex = 0;
                        lblRedLoaiKHNBo.Visible = true;
                        cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())));
                    }
                    else
                    {
                        cmbLoaiKHNBo.Enabled = false;
                        cmbLoaiKHNBo.SelectedIndex = -1;
                        lblRedLoaiKHNBo.Visible = false;

                        cmbLoaiTNCP.Enabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ////CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
        }

        private void cmbLoaiKHNBo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auLoaiKHNBo = au.GetEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                if (auLoaiKHNBo != null)
                {
                    if (auLoaiKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.KHACH_HANG.layGiaTri())
                    {
                        cmbLoaiTNCP.Enabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visible = false;
                    }
                    else
                    {
                        if (updateTNCP == null)
                        {
                            cmbLoaiTNCP.Enabled = true;
                            cmbLoaiTNCP.SelectedIndex = 0;
                            lblRedLoaiTNChiPhi.Visible = true;
                        }
                        else
                        {
                            if (cmbLoaiTNCP.SelectedItem.Value == null)
                            {
                                cmbLoaiTNCP.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbLoaiTNCP.SelectedIndex = Convert.ToInt32(cmbLoaiTNCP.SelectedIndex);
                            }

                            if (updateTNCP == true)
                            {
                                cmbLoaiTNCP.Enabled = true;
                                lblRedLoaiTNChiPhi.Visible = true;
                            }
                            else
                            {
                                cmbLoaiTNCP.Enabled = false;
                                lblRedLoaiTNChiPhi.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ////CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinChung.Enabled = enable;
        }


        void cmbTinhChatTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceTinhChatTK.ElementAt(cmbTinhChatTK.SelectedIndex);
            if (au.KeywordStrings.FirstOrDefault().Equals("LT"))
            {
                //stpTinhChatGocTK.Visible = true;
                //cmbTinhChatGocTK.Visible = true;

                //stpTinhChatBTru.Visible = true;
                cmbTinhChatTK.Visible = true;
            }
            else
            {
                //stpTinhChatGocTK.Visibility = System.Windows.Visibility.Collapsed;
                //cmbTinhChatGocTK.Visibility = System.Windows.Visibility.Collapsed;

                //stpTinhChatBTru.Visibility = System.Windows.Visibility.Collapsed;
                //cmbTinhChatBTru.Visibility = System.Windows.Visibility.Collapsed;
                //stpTinhChatGocTK.Visible = false;
                //cmbTinhChatTK.Visible = false;

                //stpTinhChatBTru.Visible = false;
                cmbTinhChatTK.Visible = false;
                cmbTinhChatTK.SelectedIndex = lstSourceTChatGoc.IndexOf(lstSourceTChatGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(au.KeywordStrings.FirstOrDefault())));
            }
        }

        void txtMaPLTKCapTren_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaPLTKCapTren_Click(sender, null);
        }
        #endregion

        #region Xu ly nghiep vu
        private void BeforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
        }

        private void BeforeAddNew()
        {
            //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            lblTrangThai.Text = "";
            txtNgayNhap.Text = null;
            txtNgayCNhat.Text = null;
            lblTenPLTKCapTren.Text = "";
        }

        private void BeforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            //CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
        }

        private void BeforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idPhanLoai);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetFormData();
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        private void BeforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    OnDelete();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void BeforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    //ucLyDo lydo = new ucLyDo(lstLyDo);
                    //lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    //Window win = new Window();
                    ////win.Title = "Danh sách mã phân loại tài khoản";
                    //win.Content = lydo;
                    //win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.Duyet");
                    //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.ShowDialog();
                    // Gọi tới hàm duyệt dữ liệu
                    OnApprove();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void BeforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    //ucLyDo lydo = new ucLyDo(lstLyDo);
                    //lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    //Window win = new Window();
                    ////win.Title = "Danh sách mã phân loại tài khoản";
                    //win.Content = lydo;
                    //win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.ThoaiDuyet");
                    //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.ShowDialog();
                    // Gọi tới hàm thoái duyệt dữ liệu
                    OnCancel();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void BeforeRefuse()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    //ucLyDo lydo = new ucLyDo(lstLyDo);
                    //lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    //Window win = new Window();
                    ////win.Title = "Danh sách mã phân loại tài khoản";
                    //win.Content = lydo;
                    //win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.TuChoi");
                    //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.ShowDialog();
                    // Gọi tới hàm xóa dữ liệu
                    OnRefuse();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void OnSave()
        {
            string trangThai = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                    Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                    // Dữ liệu truyền vào và dữ liệu trả về
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idPhanLoai == -1)
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THEM, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                        AfterAddNew(ret, objPLoai, objKHPLoai, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.SUA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                        AfterModify(ret, objPLoai, listResponseDetail);
                    }
                }
                catch (Exception ex)
                {
                    //this.Cursor = Cursors.Arrow;
                    //CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void OnHold()
        {
            string trangThai = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idPhanLoai == -1)
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THEM, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                    AfterAddNew(ret, objPLoai, objKHPLoai, listResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.SUA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                    AfterModify(ret, objPLoai, listResponseDetail);
                }
            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void OnDelete()
        {
            string trangThai = "";
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.XOA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                AfterDelete(ret, listResponseDetail);
            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void OnApprove()
        {
            string trangThai = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                AfterApprove(ret, objPLoai, listResponseDetail);
            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void OnCancel()
        {
            string trangThai = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.THOAI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THOAI_DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                AfterCancel(ret, objPLoai, listResponseDetail);

            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void OnRefuse()
        {
            string trangThai = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.TU_CHOI_DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                AfterRefuse(ret, objPLoai, listResponseDetail);

            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void AfterAddNew(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai, Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);
                lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(objPLoai.TTHAI_NVU);
                txtNguoiLap.Text = objPLoai.NGUOI_NHAP;
                txtNgayNhap.Text = LDateTime.StringToDate(objPLoai.NGAY_NHAP, "yyyyMMdd").ToString();
                txtTrangThai.Text = lblTrangThai.Text.ToString();
                tthaiNvu = objPLoai.TTHAI_NVU;
                _idPhanLoai = objPLoai.ID;
                _idKyHieuPLoai = objKHPLoai.ID;

                if (cbThemnhieulan.Checked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    //CommonFuntion.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                }
                else
                {
                    OnClose();
                }
            }
            else
            {
                //CommonFuntion.ThongBaoKetQua(listResponseDetail);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterModify(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                txtNgayCNhat.Text = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd").ToString();
            }
            else
            {
                //CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterDelete(ApplicationConstant.ResponseStatus kq, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                //CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idPhanLoai);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            OnClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterApprove(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                txtNgayCNhat.Text = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd").ToString();
            }
            else
            {
                //CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterCancel(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                txtNgayCNhat.Text = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd").ToString();
            }
            else
            {
                //CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void AfterRefuse(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                txtNgayCNhat.Text = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd").ToString();
            }
            else
            {
                //CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auTinhChat = au.GetEntryByDisplayName(lstSourceTinhChatTK, ref cmbTinhChatTK);
                AutoCompleteEntry auKyHieu = au.GetEntryByDisplayName(lstSourceKyHieu, ref cmbKyHieu);

                if (LString.IsNullOrEmptyOrSpace(txtMaPLTK.Text))
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiMaPhanLoaiTrong", LMessage.MessageBoxType.Warning);
                    txtMaPLTK.Focus();
                    return false;
                }
                else if (auKyHieu == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNhomTaiKhoanTrong", LMessage.MessageBoxType.Warning);
                    cmbKyHieu.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtTenPLTK.Text))
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTenPhanLoaiTKTrong", LMessage.MessageBoxType.Warning);
                    txtTenPLTK.Focus();
                    return false;
                }
                else if (txtNgayHieuLuc.Text == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNgayApDungTrong", LMessage.MessageBoxType.Warning);
                    txtNgayHieuLuc.Focus();
                    return false;
                }
                else if (auTinhChat == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTinhChatTaiKhoanTrong", LMessage.MessageBoxType.Warning);
                    cmbTinhChatTK.Focus();
                    return false;
                }
                else if (_idPhanLoai == -1 && Convert.ToDateTime(txtNgayHieuLuc.Text).ToString("yyyyMMdd").CompareTo(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai) < 0)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNgayApDungNhoHonNgayHienTai", LMessage.MessageBoxType.Warning);
                    txtNgayHieuLuc.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
            return true;
        }

        private void SetFormData()
        {
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                DataTable dt = process.getThongTinMaPhanLoaiTheoID(_idPhanLoai);
                if (dt != null && dt.Rows.Count > 0)
                {

                    lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                    //Thông tin tài khoản
                    txtMaPLTKCapTren.Text = dt.Rows[0]["MA_PLOAI_CHA"].ToString();
                    txtMaPLTKCapTren_LostFocus(null, null);
                    txtMaPLTK.Text = dt.Rows[0]["MA_PLOAI"].ToString();
                    txtTenPLTK.Text = dt.Rows[0]["TEN_PLOAI"].ToString();
                    if (dt.Rows[0]["ID_KHPL"] != DBNull.Value)
                    {
                        _idKyHieuPLoai = Convert.ToInt32(dt.Rows[0]["ID_KHPL"]);
                    }
                    else
                    {
                        _idKyHieuPLoai = -1;
                    }
                    cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_KY_HIEU"].ToString())));
                    txtNgayHieuLuc.Text = LDateTime.StringToDate(dt.Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd").ToString();
                    cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_NHOM_PLOAI"].ToString())));
                    //cmbTinhChatGocTK.SelectedIndex = lstSourceTChatGoc.IndexOf(lstSourceTChatGoc.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_TCHAT_GOC"].ToString())));
                    string tinhChatBTru = BusinessConstant.CoKhong.CO.layGiaTri();
                    if (dt.Rows[0]["MA_TCHAT_LTINH"].ToString().Equals("KOBT"))
                        tinhChatBTru = BusinessConstant.CoKhong.CO.layGiaTri();
                    if (dt.Rows[0]["MA_TCHAT_LTINH"] != DBNull.Value && dt.Rows[0]["MA_TCHAT_LTINH"].ToString().IsNullOrEmptyOrSpace())
                        cmbTinhChatTK.SelectedIndex = lstSourceTChatBTru.IndexOf(lstSourceTChatGoc.FirstOrDefault(e => e.KeywordStrings.First().Equals(tinhChatBTru)));
                    if (dt.Rows[0]["MA_TCHAT_CNO"].ToString().Equals("CO"))
                    {
                        chkTheoDoiCongNo.Checked = true;
                    }
                    else
                    {
                        chkTheoDoiCongNo.Checked = false;
                    }

                    if (dt.Rows[0]["MA_NHOM_PLOAI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_NHOM_PLOAI"].ToString()))
                    {
                        cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_NHOM_PLOAI"].ToString())));
                    }

                    //Thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    txtNgayNhap.Text = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd").ToString();
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                        txtNgayCNhat.Text = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd").ToString();
                    else
                        txtNgayCNhat.Text = null;
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();

                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void GetFormData(ref Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai, ref Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai, string tthai)
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLoaiTK = au.GetEntryByDisplayName(lstSourceLoaiTK, ref cmbLoaiTK);
            AutoCompleteEntry auLoaiKHNB = au.GetEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
            AutoCompleteEntry auThuNhap = au.GetEntryByDisplayName(lstSourceThuNhap, ref cmbLoaiTNCP);
            AutoCompleteEntry auTinhChat = au.GetEntryByDisplayName(lstSourceTinhChatTK, ref cmbTinhChatTK);
            AutoCompleteEntry auKyHieu = au.GetEntryByDisplayName(lstSourceKyHieu, ref cmbKyHieu);
            AutoCompleteEntry auTinhChatGoc = au.GetEntryByDisplayName(lstSourceTChatGoc, ref cmbTinhChatTK);
            //AutoCompleteEntry auTinhChatBTru = au.GetEntryByDisplayName(lstSourceTChatBTru, ref cmbTinhChatBTru);

            #region KT_PLOAI
            if (_idPhanLoai != -1)
            {
                objPLoai.ID = _idPhanLoai;
            }

            if (!LString.IsNullOrEmptyOrSpace(txtMaPLTKCapTren.Text))
            {
                objPLoai.ID_PLOAI_CHA = Convert.ToInt32(txtMaPLTKCapTren.Text);
                objPLoai.MA_PLOAI_CHA = txtMaPLTKCapTren.Text;
            }
            else
            {
                objPLoai.MA_PLOAI_CHA = "";
            }
            objPLoai.MA_PLOAI = txtMaPLTK.Text;
            objPLoai.ID_NHOM_PLOAI = Convert.ToInt32(auTinhChat.KeywordStrings[1]);
            objPLoai.MA_NHOM_PLOAI = auTinhChat.KeywordStrings[0];
            objPLoai.TEN_PLOAI = txtTenPLTK.Text;
            objPLoai.NGAY_ADUNG = LDateTime.DateToString(Convert.ToDateTime(txtNgayHieuLuc.Text), "yyyyMMdd");
            objPLoai.MA_NOI_NGOAI = auLoaiTK.KeywordStrings[0];
            objPLoai.MA_TCHAT_GOC = auTinhChatGoc.KeywordStrings[0];
            objPLoai.MA_TCHAT_LTINH = "";
            if (auTinhChat.KeywordStrings[0].Equals("LT"))
            {
                if (cmbTinhChatTK.SelectedItem.Value.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    objPLoai.MA_TCHAT_LTINH = "COBT";
                else
                    objPLoai.MA_TCHAT_LTINH = "KOBT";
            }
            if (auLoaiKHNB != null)
            {
                objPLoai.MA_KHANG_NBO = auLoaiKHNB.KeywordStrings[0];
            }
            if (auThuNhap != null)
            {
                objPLoai.MA_TNHAP_CPHI = auThuNhap.KeywordStrings[0];
            }
            if (chkTheoDoiCongNo.Checked == true)
            {
                objPLoai.MA_TCHAT_CNO = "CO";
            }
            else
            {
                objPLoai.MA_TCHAT_CNO = "KHONG";
            }

            objPLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            objPLoai.TTHAI_NVU = tthai;
            objPLoai.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
            objPLoai.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
            if (_idPhanLoai == -1)
            {
                objPLoai.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                objPLoai.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            else
            {
                objPLoai.NGAY_NHAP = Convert.ToDateTime(txtNgayNhap.Text).ToString("yyyyMMdd");
                objPLoai.NGUOI_NHAP = txtNguoiLap.Text;
                objPLoai.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                objPLoai.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            #endregion

            #region KT_KY_HIEU_PLOAI
            if (_idKyHieuPLoai != -1)
                objKHPLoai.ID = _idKyHieuPLoai;
            objKHPLoai.ID_KY_HIEU = Convert.ToInt32(auKyHieu.KeywordStrings[1]);
            objKHPLoai.ID_PLOAI = objPLoai.ID;
            objKHPLoai.MA_KY_HIEU = auKyHieu.KeywordStrings[0];
            objKHPLoai.MA_PLOAI = objPLoai.MA_PLOAI;
            objKHPLoai.TTHAI_BGHI = objPLoai.TTHAI_BGHI;
            objKHPLoai.TTHAI_NVU = objPLoai.TTHAI_NVU;
            objKHPLoai.MA_DVI_QLY = objPLoai.MA_DVI_QLY;
            objKHPLoai.MA_DVI_TAO = objPLoai.MA_DVI_TAO;
            objKHPLoai.NGAY_NHAP = objPLoai.NGAY_NHAP;
            objKHPLoai.NGUOI_NHAP = objPLoai.NGUOI_NHAP;
            objKHPLoai.NGAY_CNHAT = objPLoai.NGAY_CNHAT;
            objKHPLoai.NGUOI_CNHAT = objPLoai.NGUOI_CNHAT;
            #endregion
        }

        private void ResetForm()
        {
            //txtMaPLTKCapTren.Text = "";
            //txtMaPLTKCapTren.Text = "";
            //lblTenPLTKCapTren.Text = "";
            //txtMaPLTK.Text = "";
            //txtTenPLTK.Text = "";

            //chkTheoDoiCongNo.Enabled = false;
            //chkTheoDoiCongNo.Checked = true;

            //txtNgayHieuLuc.Text = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd").ToString();

            //_idPhanLoai = -1;
            //_idKyHieuPLoai = -1;
            //lblTrangThai.Text = "";
            //tthaiNvu = "";
            //txtTrangThai.Text = "";
            //txtNguoiLap.Text = "";
            //txtNguoiCapNhat.Text = "";
            //txtNgayNhap.Text = "";
            //txtNgayCNhat.Text = null;
            ////CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            //SetEnabledAllControls(true);
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaPLTKCapTren.Focus();
        }

        private bool KiemTraTonTaiTKChiTiet(string maPhanLoai)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            DataSet ds = ketoanProcess.GetTaiKhoanByMaPLoai(maPhanLoai, ClientInformation.MaDonVi);
            if (ds != null && ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }

        private void btnMaPLTKCapTren_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_KT_PLOAI.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                //ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                //popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                //Window win = new Window();
                ////win.Title = "Danh sách mã phân loại tài khoản";
                //win.Content = popup;
                //win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    if (KiemTraTonTaiTKChiTiet(dr[2].ToString()) == false)
                    {
                        LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTonTaiTKCT", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    txtMaPLTKCapTren.Text = dr[1].ToString();
                    txtMaPLTKCapTren.Text = dr[2].ToString();
                    lblTenPLTKCapTren.Text = dr[3].ToString();
                    //Tao ma phan loai tai khoan
                    txtMaPLTK.Text = ketoanProcess.getMaPhanLoaiGoiY(txtMaPLTKCapTren.Text);
                    txtTenPLTK.Text = dr[3].ToString();
                    if (dr[5] != null && !LString.IsNullOrEmptyOrSpace(dr[5].ToString()))
                    {
                        cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[5].ToString())));
                        cmbLoaiTK.Enabled = false;
                    }

                    if (dr[6] != null && !LString.IsNullOrEmptyOrSpace(dr[6].ToString()))
                    {
                        cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[6].ToString())));
                        //cmbLoaiKHNBo.Enabled = false;
                    }

                    AutoComboBox auto = new AutoComboBox();
                    AutoCompleteEntry auKHNBo = auto.GetEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                    if (auKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())
                    {
                        if (dr[7] != null && !LString.IsNullOrEmptyOrSpace(dr[7].ToString()))
                        {
                            cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[7].ToString())));
                            cmbLoaiTNCP.SelectedIndex = cmbLoaiTNCP.SelectedIndex;
                            cmbLoaiTNCP.Enabled = false;
                            updateTNCP = false;
                        }
                    }
                    else
                    {
                        cmbLoaiTNCP.Enabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visible = false;
                        updateTNCP = false;
                        cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[7].ToString())));
                    }

                    if (!dr[8].ToString().IsNullOrEmptyOrSpace())
                    {
                        cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[8].ToString())));
                    }
                    else
                    {
                        cmbKyHieu.SelectedIndex = -1;
                    }

                    if (!dr[10].ToString().IsNullOrEmptyOrSpace())
                    {
                        cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[10].ToString())));
                    }
                    else
                    {
                        cmbTinhChatTK.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                //CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }

        private void txtMaPLTKCapTren_LostFocus(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                if (!LString.IsNullOrEmptyOrSpace(txtMaPLTKCapTren.Text))
                {
                    DataTable dt = ketoanProcess.getThongTinMaPhanLoaiTheoMa(txtMaPLTKCapTren.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (KiemTraTonTaiTKChiTiet(dt.Rows[0]["MA_PLOAI"].ToString()) == false)
                        {
                            LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTonTaiTKCT", LMessage.MessageBoxType.Warning);
                            return;
                        }
                        txtMaPLTKCapTren.Text = dt.AsEnumerable().FirstOrDefault(f => f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<int>("ID").ToString();
                        txtMaPLTKCapTren.Text = dt.AsEnumerable().FirstOrDefault(f => f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<string>("MA_PLOAI").ToString();
                        lblTenPLTKCapTren.Text = dt.AsEnumerable().FirstOrDefault(f => f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<string>("TEN_PLOAI").ToString();
                        //Kiem tra
                        txtMaPLTK.Text = ketoanProcess.getMaPhanLoaiGoiY(txtMaPLTKCapTren.Text);
                        txtTenPLTK.Text = dt.Rows[0]["TEN_PLOAI"].ToString();
                        if (!dt.Rows[0]["MA_KY_HIEU"].ToString().IsNullOrEmptyOrSpace())
                        {
                            cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_KY_HIEU"].ToString())));
                        }
                        else
                        {
                            cmbKyHieu.SelectedIndex = -1;
                        }
                        if (dt.Rows[0]["MA_NOI_NGOAI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_NOI_NGOAI"].ToString()))
                        {
                            cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_NOI_NGOAI"].ToString())));
                            cmbLoaiTK.Enabled = false;
                        }

                        if (dt.Rows[0]["MA_KHANG_NBO"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_KHANG_NBO"].ToString()))
                        {
                            cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_KHANG_NBO"].ToString())));
                            //cmbLoaiKHNBo.Enabled = false;
                        }

                        AutoComboBox auto = new AutoComboBox();
                        AutoCompleteEntry auKHNBo = auto.GetEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                        if (auKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())
                        {
                            if (dt.Rows[0]["MA_TNHAP_CPHI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_TNHAP_CPHI"].ToString()))
                            {
                                cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_TNHAP_CPHI"].ToString())));
                                cmbLoaiTNCP.SelectedIndex = cmbLoaiTNCP.SelectedIndex;
                                cmbLoaiTNCP.Enabled = false;
                                updateTNCP = false;
                            }
                        }
                        else
                        {
                            cmbLoaiTNCP.Enabled = false;
                            cmbLoaiTNCP.SelectedIndex = -1;
                            lblRedLoaiTNChiPhi.Visible = false;
                            updateTNCP = false;
                            cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_TNHAP_CPHI"].ToString())));
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("Không tồn tại mã phân loại tài khoản này", LMessage.MessageBoxType.Warning);
                        txtMaPLTKCapTren.Text = "";
                        txtMaPLTKCapTren.Text = "";
                        lblTenPLTKCapTren.Text = "";
                        txtMaPLTK.Text = "";
                        updateTNCP = true;
                    }
                }
                else
                {
                    txtMaPLTKCapTren.Text = "";
                    txtMaPLTKCapTren.Text = "";
                    lblTenPLTKCapTren.Text = "";
                    txtMaPLTK.Text = "";

                    cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals("NOI_BANG")));
                    cmbLoaiTK.Enabled = true;


                    cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals("NOI_BO")));
                    cmbLoaiKHNBo.Enabled = true;


                    cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals("KHAC")));
                    //cmbLoaiTNCP.SelectedIndex = null;
                    cmbLoaiTNCP.Enabled = true;
                    updateTNCP = true;
                }
            }
            catch (Exception ex)
            {
                ////CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }
    }
}