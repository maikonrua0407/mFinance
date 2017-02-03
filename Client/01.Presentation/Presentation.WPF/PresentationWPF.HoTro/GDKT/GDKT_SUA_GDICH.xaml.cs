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
using Telerik.Windows.Data;
using Utilities.Common;
using PresentationWPF.CustomControl;
using PresentationWPF.HoTro.DungChung;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.HoTro.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_SUA_GDICH.xaml
    /// </summary>
    public partial class GDKT_SUA_GDICH : UserControl
    {
        #region Khai bao
        public string dataQueryGet { get; set; }

        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        #endregion

        public GDKT_SUA_GDICH()
        {
            InitializeComponent();

            cmbNguonVon.SelectionChanged += new SelectionChangedEventHandler(cmbNguonVon_SelectionChanged);
        }

        private void LoadComboBoxNguonVon(string maDonVi)
        {
            AutoComboBox auto = new AutoComboBox();
            // Combobox nguon von
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(maDonVi);
            if (!lstSourceNguonVon.IsNullOrEmpty())
                lstSourceNguonVon.Clear();
            if (!cmbNguonVon.Items.IsNullOrEmpty())
                cmbNguonVon.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON_DVI.getValue(), lstDieuKien);
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

        private void raddgrHachToan_KeyDown(object sender, KeyEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            DataRowView dr = (DataRowView)raddgrHachToan.CurrentItem;
            if (e.Key == Key.F3)
            {
                switch (raddgrHachToan.CurrentCell.Column.UniqueName)
                {
                    case "SO_TAI_KHOAN":
                    case "MA_PLOAI":
                        //if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                        //{
                        //    DataRowView drv = raddgrHachToan.CurrentCell.ParentRow.Item as DataRowView;
                        //    if (drv["MA_KY_HIEU"].Equals("TIENMAT"))
                        //    {
                        //        return;
                        //    }
                        //}
                        AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                        
                        lstDieuKien.Add("%"); //MA_KY_HIEU
                        lstDieuKien.Add("NOI_BANG");
                        lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                        lstDieuKien.Add(auNguonVon.KeywordStrings.FirstOrDefault());
                        HienThiPopup(DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET, lstDieuKien);
                        break;
                    case "MA_DTUONG":
                        if (dr["LOAI_DTUONG"].ToString().IsNullOrEmptyOrSpace())
                            break;
                        lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonVi);
                        lstDieuKien.Add(dr["LOAI_DTUONG"].ToString());
                        HienThiPopup(DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG, lstDieuKien);
                        break;
                }
            }

        }

        private void HienThiPopup(DatabaseConstant.DanhSachTruyVan tenPopUp, List<string> lstDieuKien)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();

                process.getPopupInformation(tenPopUp.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = simplePopupResponse.PopupTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    DataRowView drCurrent = (DataRowView)raddgrHachToan.SelectedItem;
                    //if (LString.IsNullOrEmptyOrSpace(drCurrent["SO_TAI_KHOAN"].ToString()) && LString.IsNullOrEmptyOrSpace(drCurrent["MA_PLOAI"].ToString()) && LString.IsNullOrEmptyOrSpace(drCurrent["MA_DTUONG"].ToString()))
                    //{
                    //    // Them dong moi
                    //    DataRow drNew = _dtSource.NewRow();
                    //    drNew["STT"] = _dtSource.Rows.Count + 1;
                    //    if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                    //        drNew["NHOM_DKHOAN"] = "1";
                    //    _dtSource.Rows.Add(drNew);
                    //}

                    switch (tenPopUp)
                    {
                        case DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET:
                            drCurrent["ID_PLOAI"] = dr["ID_PLOAI"];
                            drCurrent["MA_PLOAI"] = dr["MA_PLOAI"];
                            drCurrent["SO_TAI_KHOAN"] = dr[2];
                            drCurrent["TEN_TAI_KHOAN"] = dr[3];                                        
                            break;
                        case DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG:
                            drCurrent["MA_DTUONG"] = dr[2];
                            break;
                    }
                    raddgrHachToan.CommitEdit();
                    lstPopup.Clear();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //GetsTaiKhoanHachToan();
            //LayDanhSachPloaiTheoLoaiGD(txtMaGD.Text);
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
                    txtDienGiai.Text = ds.Tables[0].Rows[0]["DIEN_GIAI"].ToString();
                    string maDonVi = ds.Tables[0].Rows[0]["MA_DVI"].ToString();
                    string maNguonVon = ds.Tables[0].Rows[0]["NV_LOAI_NVON"].ToString();

                    LoadComboBoxNguonVon(maDonVi);

                    cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(maNguonVon)));

                    raddgrHachToan.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    raddgrHachToan.ItemsSource = null;
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

            foreach (DataRowView drv in raddgrHachToan.Items)
            {
                LDatatable.AddParameterList(ref dt,
                                            "PHAT_SINH_CT",
                                            drv["ID"].ToString(),
                                            drv["ID_PLOAI"].ToString(),
                                            drv["MA_PLOAI"].ToString(),
                                            drv["SO_TAI_KHOAN"].ToString(),
                                            drv["GHI_NO"].ToString(),
                                            drv["GHI_CO"].ToString()
                                            );
            }

            return dt;
        }

    }
}
