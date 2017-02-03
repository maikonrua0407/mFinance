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
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using System.Data;

namespace PresentationWPF.HoaDonTienKy.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopupThuGocLaiCT.xaml
    /// </summary>
    public partial class ucPopupThuGocLaiCT : UserControl
    {
        #region Khai bao bien
        DANH_SACH_KHE_UOC_VONG_VAY obj = null;
        List<DANH_SACH_SO> lstThongTinRutTK = null;
        List<DANH_SACH_SO> lstThongTinNopTK = null;
        List<THONG_TIN_THU_NO> lstThongTinThuNoTTruoc = null;
        List<THONG_TIN_THU_NO> lstThongTinThuNo = null;
        BIEU_PHI_DTO objBieuPhi = null;
        List<BIEU_PHI_CTIET_DTO> lstBieuPhi = null;
        string _thuTuPhanBo;
        string _ngayKHoach = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        
        public delegate void LayDuLieu(DANH_SACH_KHE_UOC_VONG_VAY _objDelegate);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;

        public decimal GocTrongHan{get; set;}
        public decimal GocQuaHan { get; set; }
        public decimal LaiTrongHan { get; set; }
        public decimal LaiQuaHan { get; set; }
        public decimal DuThuTrongHan { get; set; }
        public decimal NumLaiPhat { get; set; }
        public decimal SoTienMat { get; set; }
        public decimal SoTienNopCATK { get; set; }
        public decimal SoTienNopThua { get; set; }
        public decimal SoTienGocLaiTruoc { get; set; }
        public decimal SoTienPhi { get; set; }
        public bool CoThuTienMat { get; set; }
        public bool CoThuTuCA { get; set; }
        public bool CoNopVaoCA { get; set; }
        public bool CoTraTruoc { get; set; }
        public bool CoTatToan { get; set; }
        public bool CoThuLai { get; set; }

        #endregion

        #region Khoi tao
        public ucPopupThuGocLaiCT()
        {
            InitializeComponent();
            btnPhiTraTruoc.Click += new RoutedEventHandler(btnPhiTraTruoc_Click);
            chkThuLai.IsChecked = false;
        }

        public ucPopupThuGocLaiCT(DANH_SACH_KHE_UOC_VONG_VAY _obj, string ngayKHoach)
            : this()
        {
            obj = new DANH_SACH_KHE_UOC_VONG_VAY();
            obj =_obj;
            _ngayKHoach = ngayKHoach;
            GetValueParams();
            SetInfomationForm();
            InitEventHanler();
        }

        private void GetValueParams()
        {
            _thuTuPhanBo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY, ClientInformation.MaDonVi);
            if (_thuTuPhanBo.IsNullOrEmptyOrSpace())
                _thuTuPhanBo = "GOCVAY#LAI_VAY#LAI_QHAN#TKBB#QUY_TT";
        }
        #endregion

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {

            // Truongnx
            string strTinhNang = "";
            if (sender is RibbonButton)
            {
                RibbonButton tlb = (RibbonButton)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            else if (sender is RibbonMenuItem)
            {
                RibbonMenuItem tlb = (RibbonMenuItem)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                if (Validition())
                {
                    SaveForm();
                    CustomControl.CommonFunction.CloseUserControl(this);
                }
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {

            }
            else if (strTinhNang.Equals("Print"))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                obj = null;
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

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

        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        private void InitEventHanler()
        {
            radNumSoTienMat.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienMat_ValueChanged);
            radNumSoNopCA.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoNopCA_ValueChanged);
            radNumSoTienThuaTKKKH.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienThuaTKKKH_ValueChanged);
            radNumSoTienGocLaiTruoc.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienGocLaiTruoc_ValueChanged);
            radNumSoTienPhiTruoc.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienPhiTruoc_ValueChanged);
            radNumLaiPhat.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumLaiPhat_ValueChanged);
            grdThuTuCATK.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grdThuTuCATK_CellEditEnded);
            grdThuTuCATK.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(grdThuTuCATK_CellValidating);
            chkNopTienMat.Checked += new RoutedEventHandler(chkNopTienMat_Checked);
            chkNopTienMat.Unchecked += new RoutedEventHandler(chkNopTienMat_Unchecked);
            chkNopTuCATK.Checked += new RoutedEventHandler(chkNopTuCATK_Checked);
            chkNopTuCATK.Unchecked += new RoutedEventHandler(chkNopTuCATK_Unchecked);
            chkNopThuaVaoTKKKH.Checked += new RoutedEventHandler(chkNopThuaVaoTKKKH_Checked);
            chkNopThuaVaoTKKKH.Unchecked += new RoutedEventHandler(chkNopThuaVaoTKKKH_Unchecked);
            chkTraTruoc.Checked += new RoutedEventHandler(chkTraTruoc_Checked);
            chkTraTruoc.Unchecked += new RoutedEventHandler(chkTraTruoc_Unchecked);
            chkTatToan.Checked += new RoutedEventHandler(chkTatToan_Checked);
            chkTatToan.Unchecked += new RoutedEventHandler(chkTatToan_Unchecked);

        }

        void btnPhiTraTruoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("'HD01','HD02','HD03'");
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_BIEU_PHI_LOAI_GDICH", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup.FirstOrDefault();
                    obj.BIEU_PHI = new BIEU_PHI_DTO();
                    obj.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                    obj.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID"]);
                    obj.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                    obj.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                    obj.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                    obj.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                    if (dr["NGAY_HHAN"] != DBNull.Value)
                        obj.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                    obj.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                    obj.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                    obj.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                    DataSet ds = new PhiProcess().GetPhiByID(obj.BIEU_PHI.ID_BPHI);
                    DataTable dt = ds.Tables[1];
                    lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                    foreach (DataRow dtr in dt.Rows)
                    {
                        BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                        objBieuPhiCT.ID_BPHI = obj.BIEU_PHI.ID_BPHI;
                        objBieuPhiCT.LOAI_BPHI = dtr["LOAI_BPHI"].ToString();
                        objBieuPhiCT.MA_BPHI = dtr["MA_BPHI"].ToString();
                        if (dtr["SO_TIEN"] != DBNull.Value)
                            objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(dtr["SO_TIEN"]);
                        if (dtr["SO_TIEN_PHI"] != DBNull.Value)
                            objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(dtr["SO_TIEN_PHI"]);
                        if (dtr["STIEN_PHI_TDA"] != DBNull.Value)
                            objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(dtr["STIEN_PHI_TDA"]);
                        if (dtr["STIEN_PHI_TTHIEU"] != DBNull.Value)
                            objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(dtr["STIEN_PHI_TTHIEU"]);
                        if (dtr["TY_LE_PHI"] != DBNull.Value)
                            objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(dtr["TY_LE_PHI"]);
                        if (dtr["TY_LE_VAT"] != DBNull.Value)
                            objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(dtr["TY_LE_VAT"]);
                        lstBieuPhi.Add(objBieuPhiCT); 
                    }
                    obj.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                    TinhPhiTraTruoc();
                    TinhToanTienThuaNopTKBB();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void ClearEventHanler()
        {
            radNumSoTienMat.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienMat_ValueChanged);
            radNumSoNopCA.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoNopCA_ValueChanged);
            radNumSoTienThuaTKKKH.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienThuaTKKKH_ValueChanged);
            radNumSoTienGocLaiTruoc.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienGocLaiTruoc_ValueChanged);
            radNumSoTienPhiTruoc.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienPhiTruoc_ValueChanged);
            radNumLaiPhat.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumLaiPhat_ValueChanged);
            grdThuTuCATK.CellEditEnded -= new EventHandler<GridViewCellEditEndedEventArgs>(grdThuTuCATK_CellEditEnded);

            chkNopTienMat.Checked -= new RoutedEventHandler(chkNopTienMat_Checked);
            chkNopTienMat.Unchecked -= new RoutedEventHandler(chkNopTienMat_Unchecked);
            chkNopTuCATK.Checked -= new RoutedEventHandler(chkNopTuCATK_Checked);
            chkNopTuCATK.Unchecked -= new RoutedEventHandler(chkNopTuCATK_Unchecked);
            chkNopThuaVaoTKKKH.Checked -= new RoutedEventHandler(chkNopThuaVaoTKKKH_Checked);
            chkNopThuaVaoTKKKH.Unchecked -= new RoutedEventHandler(chkNopThuaVaoTKKKH_Unchecked);
            chkTraTruoc.Checked -= new RoutedEventHandler(chkTraTruoc_Checked);
            chkTraTruoc.Unchecked -= new RoutedEventHandler(chkTraTruoc_Unchecked);
            chkTatToan.Checked -= new RoutedEventHandler(chkTatToan_Checked);
            chkTatToan.Unchecked -= new RoutedEventHandler(chkTatToan_Unchecked);
        }

        void chkTatToan_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            SoTienPhi = 0;
            radNumSoTienPhiTruoc.Value = 0;
            radNumSoTienPhiTruoc.IsEnabled = false;
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void chkTatToan_Checked(object sender, RoutedEventArgs e)
        {
            //grdGocLaiTraTruoc.IsReadOnly = false;
            ClearEventHanler();
            TinhToanTraGocLai();
            lstThongTinThuNoTTruoc.ForEach(f => f.GOC_TT = f.GOC_KH);
            if (chkThuLai.IsChecked.GetValueOrDefault())
                lstThongTinThuNoTTruoc.ForEach(f => f.LAI_TT = f.LAI_KH);
            else
                lstThongTinThuNoTTruoc.ForEach(f => f.LAI_TT = 0);
            SoTienGocLaiTruoc = lstThongTinThuNoTTruoc.Sum(f => f.GOC_TT + f.LAI_TT);
            radNumSoTienGocLaiTruoc.Value = (double)SoTienGocLaiTruoc;
            radNumSoTienPhiTruoc.IsEnabled = true;
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienNopThua - NumLaiPhat - SoTienGocLaiTruoc;
            if (chkTatToan.IsChecked.Value)
            {
                TinhPhiTraTruoc();
                obj.PHI_TRA_TRUOC = Math.Min(obj.PHI_TRA_TRUOC, TongTienGD);
                radNumSoTienPhiTruoc.Value = (double)obj.PHI_TRA_TRUOC;
                SoTienPhi = obj.PHI_TRA_TRUOC;
            }
            else
            {
                SoTienPhi = 0;
                obj.PHI_TRA_TRUOC = 0;
                radNumSoTienPhiTruoc.Value = 0;
            }
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void chkTraTruoc_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            lstThongTinThuNoTTruoc.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            radNumSoTienGocLaiTruoc.Value = 0;
            SoTienGocLaiTruoc = 0;
            TinhToanTraGocLai();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void chkTraTruoc_Checked(object sender, RoutedEventArgs e)
        {
            //grdGocLaiTraTruoc.IsReadOnly = false;
            radNumSoTienGocLaiTruoc.IsEnabled = true;
        }

        void chkNopThuaVaoTKKKH_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            lstThongTinNopTK.ForEach(f => f.SO_TIEN_NOP_VAO = 0);
            grdNopThuaVaoTKKKH.IsReadOnly = true;
            radNumSoTienThuaTKKKH.IsEnabled = false;
            radNumSoTienThuaTKKKH.Value = 0;
            SoTienNopThua = 0;
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            grdNopThuaVaoTKKKH.ItemsSource = lstThongTinNopTK;
            InitEventHanler();
        }

        void chkNopThuaVaoTKKKH_Checked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            radNumSoTienThuaTKKKH.IsEnabled = true;
            grdNopThuaVaoTKKKH.IsReadOnly = false;
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void chkNopTuCATK_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            lstThongTinRutTK.ForEach(f => f.SO_TIEN_RUT_RA = 0);
            grdThuTuCATK.Columns["SO_TIEN_GUI_RUT"].IsReadOnly = true;
            SoTienNopCATK = 0;
            radNumSoNopCA.Value = 0;
            TinhToanTraGocLai();
            TinhToanTienThuaNopTKBB();
            TinhToanGocLaiTraTruoc();
            InitEventHanler();
        }

        void chkNopTuCATK_Checked(object sender, RoutedEventArgs e)
        {
            grdThuTuCATK.Columns["SO_TIEN_GUI_RUT"].IsReadOnly = false;
        }

        void chkNopTienMat_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearEventHanler();
            radNumSoTienMat.Value = 0;
            radNumSoTienMat.IsEnabled = false;
            SoTienMat = 0;
            TinhToanTraGocLai();
            TinhToanTienThuaNopTKBB();
            TinhToanGocLaiTraTruoc();
            InitEventHanler();
        }

        void chkNopTienMat_Checked(object sender, RoutedEventArgs e)
        {
            radNumSoTienMat.IsEnabled = true;
        }

        void radNumSoTienPhiTruoc_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            if (!radNumSoTienPhiTruoc.Value.ToString().IsNumeric())
                SoTienPhi = 0;
            else
                SoTienPhi = (decimal)radNumSoTienPhiTruoc.Value.GetValueOrDefault(0);
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void radNumSoTienGocLaiTruoc_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            SoTienGocLaiTruoc = (decimal)radNumSoTienGocLaiTruoc.Value.GetValueOrDefault();
            SoTienNopThua = 0;
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void radNumSoTienThuaTKKKH_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            SoTienNopCATK = (decimal)radNumSoNopCA.Value.GetValueOrDefault();
            SoTienGocLaiTruoc = 0;
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void radNumSoNopCA_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            SoTienNopCATK = (decimal)radNumSoNopCA.Value.GetValueOrDefault();
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void radNumSoTienMat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            if (!radNumSoTienMat.Value.ToString().IsNumeric())
                SoTienMat = 0;
            else
                SoTienMat = (decimal)radNumSoTienMat.Value.GetValueOrDefault(0);
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void grdThuTuCATK_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            ClearEventHanler();
            SoTienNopCATK = lstThongTinRutTK.Sum(f => f.SO_TIEN_RUT_RA);
            radNumSoNopCA.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienNopCATK"));
            TinhToanTraGocLai();
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void radNumLaiPhat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanler();
            NumLaiPhat = Convert.ToDecimal(radNumLaiPhat.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienPhi;
            if (TongTienGD < NumLaiPhat)
            {
                NumLaiPhat = TongTienGD;
            }
            obj.LAI_PHAT = NumLaiPhat;
            TinhToanGocLaiTraTruoc();
            TinhToanTienThuaNopTKBB();
            InitEventHanler();
        }

        void grdThuTuCATK_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            DANH_SACH_SO objTTTK = e.Cell.ParentRow.Item as DANH_SACH_SO;
            if (e.NewValue.IsNullOrEmpty())
            {
                e.IsValid = false;
                e.ErrorMessage = "";
                return;
            }
            if (!e.NewValue.ToString().IsNumeric())
            {
                e.IsValid = false;
                e.ErrorMessage = "";
                return;
            }
            if (objTTTK.SO_DU < Convert.ToDecimal(e.NewValue))
            {
                e.IsValid = false;
                e.ErrorMessage = "";
                return;
            }
        }

        #endregion

        #region Xu ly nghiep vu
        private void SetInfomationForm()
        {
            if (!obj.DSACH_SO_NOP_TIEN.IsNullOrEmpty())
            {
                lstThongTinNopTK = obj.DSACH_SO_NOP_TIEN.ToList();
            }
            if (!obj.DSACH_SO_RUT_TIEN.IsNullOrEmpty())
            {
                lstThongTinRutTK = obj.DSACH_SO_RUT_TIEN.ToList();
            }
            if (!obj.DSACH_THONG_TIN_THU_NO.IsNullOrEmpty())
            {
                lstThongTinThuNo = obj.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(_ngayKHoach) <= 0).ToList();
                lstThongTinThuNoTTruoc = obj.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(_ngayKHoach) > 0).ToList();
            }
            objBieuPhi = obj.BIEU_PHI;
            if (!objBieuPhi.IsNullOrEmpty())
                lstBieuPhi = objBieuPhi.DSACH_BPHI_CT.ToList();


            DuThuTrongHan = obj.SO_TIEN_DU_THU + obj.SO_TIEN_DU_THU_QH;
            NumLaiPhat = obj.LAI_PHAT;
            SoTienMat = obj.THUC_THU_TIEN_MAT;
           
            if (!lstThongTinRutTK.IsNullOrEmpty())
                SoTienNopCATK = lstThongTinRutTK.Sum(f => f.SO_TIEN_RUT_RA);
            if (!lstThongTinNopTK.IsNullOrEmpty())
                SoTienNopThua = lstThongTinNopTK.Sum(f => f.SO_TIEN_NOP_VAO);
            if (!lstThongTinThuNoTTruoc.IsNullOrEmpty())
                SoTienGocLaiTruoc = lstThongTinThuNoTTruoc.Sum(f => f.GOC_TT + f.LAI_TT);
            CoThuTienMat = (SoTienMat > 0 ? true : false);
            CoThuTuCA = (SoTienNopCATK > 0 ? true : false);
            CoNopVaoCA = (SoTienNopThua > 0 ? true : false);
            CoTatToan = (obj.TAT_TOAN.Equals(BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false);
            CoTraTruoc = (obj.TRA_GOC_LAI_TRUOC_HAN.Equals(BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false);
            if (!lstThongTinThuNoTTruoc.IsNullOrEmpty())
                CoThuLai = (CoTatToan.Equals(true) && lstThongTinThuNoTTruoc.Sum(f => f.LAI_TT) > 0 ? true : false);
            chkTatToan.IsEnabled = CoTatToan;
            chkNopThuaVaoTKKKH.IsChecked = CoNopVaoCA;
            grdNopThuaVaoTKKKH.IsReadOnly = !chkNopThuaVaoTKKKH.IsChecked.GetValueOrDefault();
            chkNopTuCATK.IsChecked = CoThuTuCA;
            grdThuTuCATK.Columns["SO_TIEN_GUI_RUT"].IsReadOnly = !chkNopTuCATK.IsChecked.GetValueOrDefault();
            lblTenPhiTraTruoc.Content = obj.BIEU_PHI.TEN_BPHI;
            SoTienPhi = obj.PHI_TRA_TRUOC;
            GocTrongHan = lstThongTinThuNo.Where(f=>f.NHOM_NO.Equals("part0")).Sum(f=>f.GOC_TT);
            GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            BindingData();
        }

        private Binding CreateValueBinding(object dataItem, string path)
        {
            Binding valueBinding = new Binding();
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            valueBinding.Source = dataItem;
            valueBinding.Path = new PropertyPath(path);
            valueBinding.Mode = BindingMode.TwoWay;
            return valueBinding;
        }

        private void BindingData()
        {
            radNumGocTrongHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "GocTrongHan"));
            radNumLaiTrongHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "LaiTrongHan"));
            radNumDuThu.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "DuThuTrongHan"));
            radNumGocQuaHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "GocQuaHan"));
            radNumLaiQuaHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "LaiQuaHan"));
            radNumLaiPhat.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "NumLaiPhat"));
            radNumSoTienMat.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienMat"));
            radNumSoNopCA.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienNopCATK"));
            radNumSoTienThuaTKKKH.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienNopThua"));
            radNumSoTienGocLaiTruoc.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienGocLaiTruoc"));
            radNumSoTienPhiTruoc.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "SoTienPhi"));
            chkNopTienMat.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoThuTienMat"));
            chkNopTuCATK.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoThuTuCA"));
            chkNopThuaVaoTKKKH.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoNopVaoCA"));
            chkTraTruoc.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoTraTruoc"));
            chkTatToan.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoTatToan"));
            chkThuLai.SetBinding(CheckBox.IsCheckedProperty, CreateValueBinding(this, "CoThuLai"));
            grMain.DataContext = obj;
            grdThuTuCATK.ItemsSource = lstThongTinRutTK;
            grdNopThuaVaoTKKKH.ItemsSource = lstThongTinNopTK;
            grdGocLaiTraTruoc.ItemsSource = lstThongTinThuNoTTruoc;
        }

        private void TinhToanTraGocLai()
        {
            SoTienMat = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
            SoTienNopCATK = Convert.ToDecimal(radNumSoNopCA.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK;
            lstThongTinThuNo.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            NumLaiPhat = 0;
            for (int i = 0; i < lstThongTinThuNo.Count; i++)
            {
                NumLaiPhat += lstThongTinThuNo[i].LAI_PHAT_KH;
                decimal soTienTraLai = lstThongTinThuNo[i].LAI_KH;
                decimal soTienTraGoc = lstThongTinThuNo[i].GOC_KH;
                foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
                {
                    switch (loaiPhanBo)
                    {
                        case "GOCVAY":
                            if (TongTienGD <= soTienTraGoc)
                            {
                                lstThongTinThuNo[i].GOC_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNo[i].GOC_TT = soTienTraGoc;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNo[i].GOC_TT);
                            break;
                        case "LAI_VAY":
                            if (TongTienGD <= soTienTraLai)
                            {
                                lstThongTinThuNo[i].LAI_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNo[i].LAI_TT = soTienTraLai;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNo[i].LAI_TT);
                            break;
                        case "LAI_QHAN":

                            break;
                        case "TKBB":

                            break;
                    }
                    if (TongTienGD == 0)
                        break;
                }
                if (TongTienGD == 0)
                    break;
            }
            GocTrongHan = lstThongTinThuNo.Where(f=>f.NHOM_NO.Equals("part0")).Sum(f=>f.GOC_TT);
            GocQuaHan = lstThongTinThuNo.Where(f=>!f.NHOM_NO.Equals("part0")).Sum(f=>f.GOC_TT);
            LaiTrongHan = lstThongTinThuNo.Where(f=>f.NHOM_NO.Equals("part0")).Sum(f=>f.LAI_TT);
            LaiQuaHan = lstThongTinThuNo.Where(f=>!f.NHOM_NO.Equals("part0")).Sum(f=>f.LAI_TT);
            radNumGocTrongHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "GocTrongHan"));
            radNumLaiTrongHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "LaiTrongHan"));
            radNumGocQuaHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "GocQuaHan"));
            radNumLaiQuaHan.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(this, "LaiQuaHan"));
            if (TongTienGD > 0)
            {
                NumLaiPhat = Math.Min(NumLaiPhat, TongTienGD);
                TongTienGD -= NumLaiPhat;
                radNumLaiPhat.Value = (double)NumLaiPhat;
                radNumLaiPhat.IsEnabled = true;
            }
            else
            {
                NumLaiPhat = 0;
                radNumLaiPhat.Value = (double)NumLaiPhat;
                radNumLaiPhat.IsEnabled = false;
            }
            obj.LAI_PHAT = NumLaiPhat;
        }

        private void TinhToanTienThuaNopTKBB()
        {
            SoTienMat = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
            SoTienNopCATK = Convert.ToDecimal(radNumSoNopCA.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienGocLaiTruoc - SoTienPhi - NumLaiPhat;
            if (TongTienGD > 0)
            {
                chkNopThuaVaoTKKKH.IsChecked = true;
                if (!lstThongTinNopTK.IsNullOrEmpty() && lstThongTinNopTK.Count > 0)
                    lstThongTinNopTK[0].SO_TIEN_NOP_VAO = TongTienGD;
                else
                    TongTienGD = 0;
                radNumSoTienThuaTKKKH.Value = (double)TongTienGD;
                grdNopThuaVaoTKKKH.ItemsSource = lstThongTinNopTK;
                SoTienNopThua = TongTienGD;
            }
            else
            {
                chkNopThuaVaoTKKKH.IsChecked = false;
                TongTienGD = 0;
                lstThongTinNopTK.ForEach(f => f.SO_TIEN_NOP_VAO = 0);
                radNumSoTienThuaTKKKH.Value = 0;
                grdNopThuaVaoTKKKH.ItemsSource = lstThongTinNopTK;
                SoTienNopThua = TongTienGD;
            }
            
        }

        private void TinhToanGocLaiTraTruoc()
        {
            SoTienMat = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
            SoTienNopCATK = Convert.ToDecimal(radNumSoNopCA.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienNopThua - SoTienPhi - NumLaiPhat;

            if (lstThongTinThuNoTTruoc.IsNullOrEmpty() || lstThongTinThuNoTTruoc.Count == 0)
            {
                chkTraTruoc.IsChecked = false;
                radNumSoTienGocLaiTruoc.Value = 0;
                radNumSoTienGocLaiTruoc.IsEnabled = false;
                return;
            }
            lstThongTinThuNoTTruoc.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            if (TongTienGD > 0)
                chkTraTruoc.IsChecked = true;
            else
            {
                chkTraTruoc.IsChecked = false;
                radNumSoTienGocLaiTruoc.Value = 0;
                radNumSoTienGocLaiTruoc.IsEnabled = false;
                chkTatToan.IsChecked = false;
                chkThuLai.IsChecked = false;
                chkTatToan.IsEnabled = false;
                chkThuLai.IsEnabled = false;
                return;
            }
            decimal TongTienTT = 0;
            TongTienTT = lstThongTinThuNoTTruoc.Sum(f => f.GOC_KH);
            if (TongTienTT > TongTienGD)
            {
                chkTatToan.IsChecked = false;
                chkThuLai.IsChecked = false;
                chkTatToan.IsEnabled = false;
                chkThuLai.IsEnabled = false;
            }
            else
            {
                chkTatToan.IsEnabled = true;
                chkThuLai.IsEnabled = true;
            }
            
            for (int i = 0; i < lstThongTinThuNoTTruoc.Count; i++)
            {
                decimal soTienTraLai = lstThongTinThuNoTTruoc[i].LAI_KH;
                if (chkThuLai.IsChecked.GetValueOrDefault())
                    soTienTraLai = 0;
                decimal soTienTraGoc = lstThongTinThuNoTTruoc[i].GOC_KH;
                foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
                {
                    switch (loaiPhanBo)
                    {
                        case "GOCVAY":
                            if (TongTienGD <= soTienTraGoc)
                            {
                                lstThongTinThuNoTTruoc[i].GOC_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNoTTruoc[i].GOC_TT = soTienTraGoc;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNoTTruoc[i].GOC_TT);
                            break;
                        case "LAI_VAY":
                            if (TongTienGD <= soTienTraLai)
                            {
                                lstThongTinThuNoTTruoc[i].LAI_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNoTTruoc[i].LAI_TT = soTienTraLai;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNoTTruoc[i].LAI_TT);
                            break;
                        case "LAI_QHAN":

                            break;
                        case "TKBB":

                            break;
                    }
                    if (TongTienGD == 0)
                        break;
                }
                if (TongTienGD == 0)
                    break;
            }
            SoTienGocLaiTruoc = lstThongTinThuNoTTruoc.Sum(f => f.GOC_TT + f.LAI_TT);
            radNumSoTienGocLaiTruoc.Value = (double)SoTienGocLaiTruoc;
            grdGocLaiTraTruoc.ItemsSource = lstThongTinThuNoTTruoc;
            if (chkTatToan.IsChecked.Value)
            {
                TinhPhiTraTruoc();
                obj.PHI_TRA_TRUOC = Math.Min(obj.PHI_TRA_TRUOC, TongTienGD);
                radNumSoTienPhiTruoc.Value = (double)obj.PHI_TRA_TRUOC;
            }
            else
            {
                obj.PHI_TRA_TRUOC = 0;
                radNumSoTienPhiTruoc.Value = 0;
            }
        }

        private void SaveForm()
        {
            
            GocTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            GocTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            GocQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            obj.THUC_THU_GOC_VAY = GocTrongHan + GocQuaHan;
            obj.THUC_THU_LAI_TRONG = LaiTrongHan + LaiQuaHan;
            obj.THUC_THU_LAI_QUA_HAN = LaiQuaHan;
            obj.THUC_THU_TIEN_MAT = SoTienMat;
            obj.TONG_SO_TIEN = SoTienMat + SoTienNopCATK;
            obj.THUC_THU_TONG = SoTienMat + SoTienNopCATK;
            obj.PHI_TRA_TRUOC = SoTienPhi;
            if (CoTraTruoc)
                obj.TRA_GOC_LAI_TRUOC_HAN = BusinessConstant.CoKhong.CO.layGiaTri();
            else
                obj.TRA_GOC_LAI_TRUOC_HAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
            if (CoTatToan)
                obj.TAT_TOAN = BusinessConstant.CoKhong.CO.layGiaTri();
            else
                obj.TAT_TOAN = BusinessConstant.CoKhong.KHONG.layGiaTri();

            List<DANH_SACH_SO> lstThongTinTK = new List<DANH_SACH_SO>();
            List<THONG_TIN_THU_NO> lstThongTinKH = new List<THONG_TIN_THU_NO>();
            lstThongTinKH.AddRange(lstThongTinThuNo);
            lstThongTinKH.AddRange(lstThongTinThuNoTTruoc);
            obj.DSACH_SO_NOP_TIEN = lstThongTinNopTK.ToArray();
            obj.DSACH_SO_RUT_TIEN = lstThongTinRutTK.ToArray();
            obj.DSACH_THONG_TIN_THU_NO = lstThongTinKH.ToArray();
            if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV != "T01").IsNullOrEmpty())
                obj.THUC_THU_NOP_VAO_TKKKH = lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV != "T01").Sum(f => f.SO_TIEN_NOP_VAO);
            if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV == "T01").IsNullOrEmpty())
                obj.THUC_THU_TKQD = lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV == "T01").Sum(f => f.SO_TIEN_NOP_VAO);
            if (!lstThongTinRutTK.IsNullOrEmpty())
                obj.THUC_NOP_TU_TKKKH = lstThongTinRutTK.Sum(f => f.SO_TIEN_RUT_RA);
            DANH_SACH_KHE_UOC_VONG_VAY _obj = new DANH_SACH_KHE_UOC_VONG_VAY();
            _obj = obj;
            DuLieuTraVe(_obj);
        }

        private bool Validition()
        {
            bool bresult = true;
            if (SoTienNopCATK < 0)
            {
                LMessage.ShowMessage("M.PresentationWPF.HoaDonTienKy.ucPopupThuGocLaiCT.SoTienRutKhongHopLe", LMessage.MessageBoxType.Warning);
                grdThuTuCATK.Focus();
                return false;
            }
            if (SoTienMat < 0)
            {
                LMessage.ShowMessage("M.PresentationWPF.HoaDonTienKy.ucPopupThuGocLaiCT.SoTienKhongHopLe", LMessage.MessageBoxType.Warning);
                radNumSoTienMat.Focus();
                return false;
            }
            if (SoTienNopThua < 0)
            {
                LMessage.ShowMessage("M.PresentationWPF.HoaDonTienKy.ucPopupThuGocLaiCT.SoTienNopThemKhongHopLe", LMessage.MessageBoxType.Warning);
                radNumSoTienThuaTKKKH.Focus();
                return false;
            }
            decimal soTienNopTK = lstThongTinNopTK.Sum(f => f.SO_TIEN_NOP_VAO);
            if (SoTienNopThua != soTienNopTK)
            {
                LMessage.ShowMessage("M.PresentationWPF.HoaDonTienKy.ucPopupThuGocLaiCT.SoTienNopThemKhongHopLe", LMessage.MessageBoxType.Warning);
                radNumSoTienThuaTKKKH.Focus();
                return false;
            }
            decimal GocKH = (decimal)(radNumGocQuaHan.Value.GetValueOrDefault() + radNumGocTrongHan.Value.GetValueOrDefault());
            decimal LaiKH = (decimal)(radNumLaiQuaHan.Value.GetValueOrDefault() + radNumLaiTrongHan.Value.GetValueOrDefault());
            if (SoTienMat + SoTienNopCATK != SoTienGocLaiTruoc + SoTienNopThua + SoTienPhi + NumLaiPhat + GocKH + LaiKH)
            {
                LMessage.ShowMessage("M.PresentationWPF.HoaDonTienKy.ucPopupThuGocLaiCT.TongTienKhongHopLe", LMessage.MessageBoxType.Warning);
                radNumSoTienMat.Focus();
                return false;
            }
            return bresult;
        }

        private void TinhPhiTraTruoc()
        {
            try
            {
                decimal soTienPhi = 0;
                if (chkTatToan.IsChecked.GetValueOrDefault())
                {
                    string ngayDaoHan = obj.DSACH_THONG_TIN_THU_NO.LastOrDefault(f => f.GOC_KH > 0).NGAY_KH;
                    decimal soDu = obj.DU_NO;
                    int soNgayTraTruoc = LDateTime.StringToDate(ngayDaoHan, ApplicationConstant.defaultDateTimeFormat).CountDayBetweenDates(LDateTime.StringToDate(obj.NGAY_GD, ApplicationConstant.defaultDateTimeFormat));
                    decimal tyLe = 0;
                    decimal soTien = 0;
                    decimal soTienTThieu = 0;
                    decimal soTienTDa = 0;
                    lstBieuPhi = obj.BIEU_PHI.DSACH_BPHI_CT.ToList();
                    soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
                    soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;

                    if (obj.BIEU_PHI.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
                    {
                        if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                        {
                            tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                        }
                        else if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                        {
                            soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                        }
                        else if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                        {
                            tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                            soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                        }
                    }
                    else
                    {
                    }
                    if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                    {
                        soTienPhi = obj.DU_NO * soNgayTraTruoc * (tyLe / 360 / 100);
                    }
                    else if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                    {
                        soTienPhi = soTien;
                    }
                    else if (obj.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                    {

                    }
                    if (soTienPhi < soTienTThieu)
                        soTienPhi = soTienTThieu;
                    if (soTienPhi > soTienTDa)
                        soTienPhi = soTienTDa;
                }
                else
                    soTienPhi = 0;
                soTienPhi = soTienPhi.Rounding(0);
                obj.PHI_TRA_TRUOC = soTienPhi;
                obj.MA_PHI_TRA_TRUOC = obj.BIEU_PHI.MA_BPHI;
                lblTenPhiTraTruoc.Content = obj.BIEU_PHI.TEN_BPHI;
                radNumSoTienPhiTruoc.Value = (double)soTienPhi;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
