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
using Presentation.Process.KeToanServiceRef;
using System.Data;
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;

namespace PresentationWPF.KeToan.KiemSoat
{
    /// <summary>
    /// Interaction logic for ucLyDo.xaml
    /// </summary>
    public partial class frmLyDo : Window
    {
        #region Khai bao

        private DataTable _dtGiaoDich = new DataTable();
        public DataTable dtGiaoDich
        {
            get { return _dtGiaoDich; }
            set{_dtGiaoDich = value;}
        }

        private DatabaseConstant.Action _action = DatabaseConstant.Action.XOA;
        public DatabaseConstant.Action action
        {
            get { return _action; }
            set { _action = value; }
        }

        public delegate void AfterProcess(DatabaseConstant.Action action, ApplicationConstant.ResponseStatus responseStatus, List<ClientResponseDetail> lstResponseDetail);
        public AfterProcess GetData;

        public delegate void AfterProcessException(DatabaseConstant.Action action);
        public AfterProcessException ProcessUnlockData;

        private string KT_KS_LYDO = "CHUNG"; // Tham số hệ thống

        private bool isProcess = false;
        #endregion

        #region Khoi tao
        public frmLyDo()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
            if (KT_KS_LYDO == "CHUNG")
            {
                grGiaoDichDS.Columns["LY_DO"].IsVisible = false;
            }
        }

        #endregion

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            isProcess = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grGiaoDichDS.ItemsSource = _dtGiaoDich.DefaultView;
        }

        private List<GDICH_KSOAT> GetFormData()
        {
            List<GDICH_KSOAT> lstGdich = new List<GDICH_KSOAT>();
            foreach (DataRow dr in _dtGiaoDich.Rows)
            {
                if (Convert.ToBoolean(dr["CHON"]) == true)
                {
                    
                    GDICH_KSOAT obj = new GDICH_KSOAT();
                    obj.DIEN_GIAI = dr["DIEN_GIAI"].ToString();
                    obj.ID_GDICH = Convert.ToInt32(dr["ID"]);
                    obj.LY_DO = dr["LY_DO"].ToString();
                    obj.MA_DVI = dr["MA_DVI"].ToString();
                    obj.MA_GDICH = dr["SO_GDICH"].ToString();
                    obj.MA_LOAI_GDICH = DatabaseConstant.layLoaiGiaoDich(dr["MA_LOAI_GDICH"].ToString());
                    obj.MA_PHAN_HE = DatabaseConstant.getModule(dr["MA_PHAN_HE"].ToString());
                    obj.NGAY_GDICH = dr["NGAY_GIAO_DICH"].ToString();
                    obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                    obj.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                    obj.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                    obj.MA_CNANG = dr["MA_CNANG"].ToString();
                    //if (_action != DatabaseConstant.Action.XOA)
                    //{
                    //    string tthai_nvu = CommonFunction.LayTrangThaiBanGhi(_action, BusinessConstant.layTrangThaiNghiepVu(dr["TTHAI_NVU"].ToString()));
                    //    obj.TTHAI_NVU = tthai_nvu;
                    //}
                    //else
                    //{
                        obj.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                    //}

                    

                    lstGdich.Add(obj);
                }
            }
            return lstGdich;
        }

        private void txtLyDoChung_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!LString.IsNullOrEmptyOrSpace(txtLyDoChung.Text))
            {
                foreach (DataRow dr in _dtGiaoDich.Rows)
                {
                    if (KT_KS_LYDO == "RIENG")
                    {
                        if (dr["LY_DO"] == null || LString.IsNullOrEmptyOrSpace(dr["LY_DO"].ToString()))
                        {
                            dr["LY_DO"] = txtLyDoChung.Text;
                        }
                    }
                    else
                    {
                        dr["LY_DO"] = txtLyDoChung.Text;
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (isProcess == true || _action == DatabaseConstant.Action.XOA)
            {
                Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus status = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                try
                {
                    status = process.KiemSoatGiaoDich(GetFormData(), _action, ref lstResponseDetail);
                    GetData(_action, status, lstResponseDetail);
                }
                catch (Exception ex)
                {
                    LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                    ProcessUnlockData(_action);
                    process = null;
                }
            }
        }

        private void grGiaoDichDS_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!LString.IsNullOrEmptyOrSpace(txtLyDoChung.Text))
            {
                foreach (DataRow dr in _dtGiaoDich.Rows)
                {
                    if (dr["LY_DO"] == null || LString.IsNullOrEmptyOrSpace(dr["LY_DO"].ToString()))
                    {
                        dr["LY_DO"] = txtLyDoChung.Text;
                    }
                }
            }
        }
    }
}
