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
using Telerik.Windows.Controls;
using Utilities.Common;
using PresentationWPF.CustomControl;
using PresentationWPF.HoTro.DungChung;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.HoTro.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_DU_CHI_DS_XOA.xaml
    /// </summary>
    public partial class HDVO_DU_CHI_DS_XOA : UserControl
    {
        #region Khai bao
        public string dataQueryGet { get; set; }
        #endregion

        public HDVO_DU_CHI_DS_XOA()
        {
            InitializeComponent();
        }

        private void GetFormData()
        {
        }

        private void txtMaGiaoDich_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LayDuLieu();
            }
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            LayDuLieu();
        }

        private bool Validation()
        {
            if (txtMaGiaoDich.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblMaGiaoDich.Content.ToString());
                txtMaGiaoDich.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            raddtNgayGiaoDich.Value = null;
            txtDienGiai.Text = "";
            grDuChiDS.ItemsSource = null;
        }

        public DataTable GetParameters()
        {
            if (!Validation())
            {
                return null;
            }

            GetFormData();

            DataTable dt = null;
            LDatatable.MakeParameterListTable(ref dt);
            LDatatable.AddParameterList(ref dt, "MA_NSD", ClientInformation.TenDangNhap);
            LDatatable.AddParameterList(ref dt, "MA_DVI_QLY", ClientInformation.MaDonVi);
            LDatatable.AddParameterList(ref dt, "MA_DVI_TAO", ClientInformation.MaDonViGiaoDich);
            LDatatable.AddParameterList(ref dt, "MA_GDICH", txtMaGiaoDich.Text);
            LDatatable.AddParameterList(ref dt, "NGAY_GDICH", Convert.ToDateTime(raddtNgayGiaoDich.Value).ToString("yyyyMMdd"));

            return dt;
        }

        private void LayDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (txtMaGiaoDich.Text.IsNullOrEmptyOrSpace())
                    return;

                DataTable dt = null;
                LDatatable.MakeParameterListTable(ref dt);
                LDatatable.AddParameterList(ref dt, "MA_NSD", ClientInformation.TenDangNhap);
                LDatatable.AddParameterList(ref dt, "MA_DVI_QLY", ClientInformation.MaDonVi);
                LDatatable.AddParameterList(ref dt, "MA_DVI_TAO", ClientInformation.MaDonViGiaoDich);
                LDatatable.AddParameterList(ref dt, "MA_GDICH", txtMaGiaoDich.Text);

                DataSet ds = new HoTroProcess().LayDuLieu(dataQueryGet, dt);

                if (KiemTraDuLieuTraVe(ds) == 0)
                {
                    ClearForm();
                    return;
                }

                if (ds != null && ds.Tables.Count > 2)
                {

                    raddtNgayGiaoDich.Value = LDateTime.StringToDate(ds.Tables[1].Rows[0]["NGAY_GDICH"].ToString(), "yyyyMMdd");
                    txtDienGiai.Text = ds.Tables[1].Rows[0]["DIEN_GIAI"].ToString();

                    grDuChiDS.ItemsSource = ds.Tables[2].DefaultView;
                }
                else
                {
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private int KiemTraDuLieuTraVe(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string ketQua = ds.Tables[0].Rows[0][0].ToString();
                if (ketQua == "0")
                {
                    if (ds.Tables.Count > 1)
                    {
                        CommonFunction.ThongBaoKetQua(ds.Tables[1]);
                    }
                    else
                    {
                        LMessage.ShowMessage("Xử lý không thành công", LMessage.MessageBoxType.Warning);
                    }
                    return 0;
                }
            }
            return 1;
        }

    }
}