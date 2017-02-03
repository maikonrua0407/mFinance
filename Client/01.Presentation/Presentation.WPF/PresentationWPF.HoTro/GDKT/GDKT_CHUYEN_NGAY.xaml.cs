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
using Utilities.Common;
using PresentationWPF.CustomControl;
using PresentationWPF.HoTro.DungChung;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.HoTro.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_CHUYEN_NGAY.xaml
    /// </summary>
    public partial class GDKT_CHUYEN_NGAY : UserControl
    {
        #region Khai bao
        public string dataQueryGet { get; set; }
        #endregion

        public GDKT_CHUYEN_NGAY()
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
            if (raddtNgayChuyenDen.Value == null)
            {
                CommonFunction.ThongBaoChuaNhap(lblNgayChuyenDen.Content.ToString());
                raddtNgayChuyenDen.Focus();
                return false;
            }
            if (raddtNgayChuyenDen.Value == raddtNgayGiaoDich.Value)
            {
                LMessage.ShowMessage("Ngày chuyển đến phải khác ngày ngày giao dịch", LMessage.MessageBoxType.Warning);
                raddtNgayChuyenDen.Focus();
                return false;
            }
            return true;
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
                LDatatable.AddParameterList(ref dt, "MA_GDICH", txtMaGiaoDich.Text);

                DataSet ds = new HoTroProcess().LayDuLieu(dataQueryGet, dt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    raddtNgayGiaoDich.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_GDICH"].ToString(), "yyyyMMdd");
                    raddtNgayChuyenDen.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_GDICH"].ToString(), "yyyyMMdd");
                    txtDienGiai.Text = ds.Tables[0].Rows[0]["DIEN_GIAI"].ToString();

                    grid.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grid.ItemsSource = null;
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
            LDatatable.AddParameterList(ref dt, "MA_GDICH", txtMaGiaoDich.Text);
            LDatatable.AddParameterList(ref dt, "NGAY_GDICH", Convert.ToDateTime(raddtNgayGiaoDich.Value).ToString("yyyyMMdd"));
            LDatatable.AddParameterList(ref dt, "NGAY_CHUYEN_DEN", Convert.ToDateTime(raddtNgayChuyenDen.Value).ToString("yyyyMMdd"));

            return dt;
        }

    }
}
