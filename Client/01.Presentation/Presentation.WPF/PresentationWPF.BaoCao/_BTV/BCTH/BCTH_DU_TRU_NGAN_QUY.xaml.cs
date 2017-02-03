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
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.DungChung;
using System.Data;
using Presentation.Process;

namespace PresentationWPF.BaoCao._BTV.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_DU_TRU_NGAN_QUY.xaml
    /// </summary>
    public partial class BCTH_DU_TRU_NGAN_QUY : UserControl
    {
        #region Khai báo
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        string maChiNhanh = "";
        string tenChiNhanh = "";
        string thangDuTru = "";
        string thangBaoCao = "";
        string maPhongGD = "";
        int tyLe = 0;
        string _maBaoCao = "";
        
        DataSet ds = null;
        public string MaBaoCao
        {
            get { return _maBaoCao; }
            set { _maBaoCao = value; }
        }

        int _idBaoCao = 0;

        public int IDBaoCao
        {
            get { return _idBaoCao; }
            set { _idBaoCao = value; }
        }

        HT_BAOCAO _objBaoCao = new HT_BAOCAO();

        public HT_BAOCAO ObjBaoCao
        {
            get { return _objBaoCao; }
            set { _objBaoCao = value; }
        }

        List<HT_BAOCAO_TSO> _lstHtBaoCaoTso = null;

        public List<HT_BAOCAO_TSO> LstHtBaoCaoTso
        {
            get { return _lstHtBaoCaoTso; }
            set { _lstHtBaoCaoTso = value; }
        }
        #endregion

        #region Khoi tao
        public BCTH_DU_TRU_NGAN_QUY()
        {
            InitializeComponent();
            raddtThangBC.Value = raddtNgayBC.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            KhoiTaoComboBox();
            tlbTongHop.Click += new RoutedEventHandler(tlbTongHop_Click);
            rdoTyLeChung.Checked += new RoutedEventHandler(rdoTyLeChung_Checked);
            rdoTyLeRieng.Checked += new RoutedEventHandler(rdoTyLeRieng_Checked);
            grdSoTien.CellEditEnded += new EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdSoTien_CellEditEnded);
            grdThuTien.CellEditEnded += new EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdThuTien_CellEditEnded);
            grdChiTien.CellEditEnded += new EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdChiTien_CellEditEnded);
            numTyLe.ValueChanged += new EventHandler<Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs>(numTyLe_ValueChanged);
            grdSoTien.CellValidating += new EventHandler<Telerik.Windows.Controls.GridViewCellValidatingEventArgs>(grdSoTien_CellValidating);
            grdChiTien.CellValidating += new EventHandler<Telerik.Windows.Controls.GridViewCellValidatingEventArgs>(grdChiTien_CellValidating);
            grdThuTien.CellValidating += new EventHandler<Telerik.Windows.Controls.GridViewCellValidatingEventArgs>(grdThuTien_CellValidating);

        }

        void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();
            //Tao combobox chi nhanh
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            TaoComboboxPGD();
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
        }

        
        #endregion

        #region Xu ly giao dien
        private void TaoComboboxPGD()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourcePhongGD = new List<AutoCompleteEntry>();
            cmbPhongGD.Items.Clear();
            string smaCN = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            lstDieuKien.Add(smaCN);
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            if (cmbPhongGD.SelectedIndex == -1) cmbPhongGD.SelectedIndex = 0;
        }

        void tlbTongHop_Click(object sender, RoutedEventArgs e)
        {
            TongHopDuLieu();
        }

        void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaoComboboxPGD();
        }


        void rdoTyLeRieng_Checked(object sender, RoutedEventArgs e)
        {
            numTyLe.Value = 100;
            numTyLe.Visibility = System.Windows.Visibility.Collapsed;
            grdSoTien.Columns["TY_LE"].IsVisible = true;
            grdThuTien.Columns["TY_LE"].IsVisible = true;
            grdChiTien.Columns["TY_LE"].IsVisible = true;
        }

        void rdoTyLeChung_Checked(object sender, RoutedEventArgs e)
        {
            numTyLe.Value = 100;
            numTyLe.Visibility = System.Windows.Visibility.Visible;
            grdSoTien.Columns["TY_LE"].IsVisible = false;
            grdThuTien.Columns["TY_LE"].IsVisible = false;
            grdChiTien.Columns["TY_LE"].IsVisible = false;
        }

        void numTyLe_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            tyLe = (int)numTyLe.Value.GetValueOrDefault();
            DataView dr = grdSoTien.ItemsSource as DataView;
            if (!dr.IsNullOrEmpty())
            {
                foreach (DataRowView r in dr)
                {
                    r["TY_LE"] = tyLe;
                    r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(tyLe) / 100;
                }
                grdSoTien.ItemsSource = dr;
                grdSoTien.Rebind();
            }

            dr = grdThuTien.ItemsSource as DataView;
            if (!dr.IsNullOrEmpty())
            {
                foreach (DataRowView r in dr)
                {
                    r["TY_LE"] = tyLe;
                    r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(tyLe) / 100;
                }
                grdThuTien.ItemsSource = dr;
                grdThuTien.Rebind();
            }

            dr = grdChiTien.ItemsSource as DataView;
            if (!dr.IsNullOrEmpty())
            {
                foreach (DataRowView r in dr)
                {
                    r["TY_LE"] = tyLe;
                    r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(tyLe) / 100;
                }
                grdChiTien.ItemsSource = dr;
                grdChiTien.Rebind();
            }
        }

        void grdChiTien_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmpty() && e.Cell.Column.UniqueName.Equals("TY_LE") && !e.NewData.Equals(e.OldData))
            {
                DataRowView r = e.Cell.ParentRow.Item as DataRowView;
                r["TY_LE"] = e.NewData;
                r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(r["TY_LE"]) / 100;
                grdChiTien.CurrentItem = r;
            }
        }

        void grdThuTien_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmpty() && e.Cell.Column.UniqueName.Equals("TY_LE") && !e.NewData.Equals(e.OldData))
            {
                DataRowView r = e.Cell.ParentRow.Item as DataRowView;
                r["TY_LE"] = e.NewData;
                r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(r["TY_LE"]) / 100;
                grdThuTien.CurrentItem = r;
            }
        }

        void grdSoTien_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmpty() && e.Cell.Column.UniqueName.Equals("TY_LE") && !e.NewData.Equals(e.OldData))
            {
                DataRowView r = e.Cell.ParentRow.Item as DataRowView;
                r["TY_LE"] = e.NewData;
                r["DU_TRU"] = Convert.ToDecimal(r["GIA_TRI"]) * Convert.ToDecimal(r["TY_LE"]) / 100;
                grdSoTien.CurrentItem = r;
            }
            
        }

        void grdThuTien_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("TY_LE"))
            {
                if (e.NewValue.IsNullOrEmpty() || !e.NewValue.ToString().IsNumeric() || e.NewValue.ToString().StringToDecimal() < 1)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "The entered value must greater than 0";
                }
            }
        }

        void grdChiTien_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("TY_LE"))
            {
                if (e.NewValue.IsNullOrEmpty() || !e.NewValue.ToString().IsNumeric() || e.NewValue.ToString().StringToDecimal() < 1)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "The entered value must greater than 0";
                }
            }
        }

        void grdSoTien_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("TY_LE"))
            {
                if (e.NewValue.IsNullOrEmpty() || !e.NewValue.ToString().IsNumeric() || e.NewValue.ToString().StringToDecimal() < 1)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "The entered value must greater than 0";
                }
            }
        }
        #endregion

        #region Xy ly nghiep vu
        void TongHopDuLieu()
        {
            DataTable dt = null;
            DataRow[] drresult = null;
            LDatatable.MakeParameterTable(ref dt);
            AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            AutoCompleteEntry auPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex);
            maChiNhanh = auChiNhanh.KeywordStrings.FirstOrDefault();
            tenChiNhanh = auChiNhanh.DisplayName;
            maPhongGD = auPhongGD.KeywordStrings.FirstOrDefault();
            thangDuTru = raddtThangBC.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
            thangBaoCao = raddtNgayBC.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
            tyLe = (int)numTyLe.Value.GetValueOrDefault();
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "string", maChiNhanh);
            LDatatable.AddParameter(ref dt, "@MaPhongGD", "string", maPhongGD);
            LDatatable.AddParameter(ref dt, "@TenChiNhanh", "string", tenChiNhanh);
            LDatatable.AddParameter(ref dt, "@ThangDuTru", "string", thangDuTru);
            LDatatable.AddParameter(ref dt, "@NgayBCao", "string", thangBaoCao);
            LDatatable.AddParameter(ref dt, "@TyLe", "string", tyLe.ToString());
            LDatatable.AddParameter(ref dt, "@UserName", "string", ClientInformation.TenDangNhap);
            
            ds = new TruyVanProcess().TruyVanUDTT(_objBaoCao.DATA_QUERY, dt);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                drresult = ds.Tables[0].Select("LOAI_DLIEU = 'SDU_DAUTHANG'");
                grdSoTien.ItemsSource = drresult.CopyToDataTable().DefaultView;
                grdSoTien.Rebind();
                drresult = ds.Tables[0].Select("LOAI_DLIEU = 'THU_TRONGTHANG'");
                grdThuTien.ItemsSource = drresult.CopyToDataTable().DefaultView;
                grdThuTien.Rebind();
                drresult = ds.Tables[0].Select("LOAI_DLIEU LIKE 'CHI_TRONGTHANG_%'");
                grdChiTien.ItemsSource = drresult.CopyToDataTable().DefaultView;
                grdChiTien.Rebind();
            }
        }

        private bool Validation()
        {
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", maChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maPhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", tenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@ThangDuTru", thangDuTru, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", thangBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TyLe", tyLe.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@UserName", ClientInformation.TenDangNhap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            DataTable dt = null;
            DataSet ds = new DataSet();
            DataView drvGiaTri = grdSoTien.ItemsSource as DataView;
            dt = drvGiaTri.Table.Clone();
            foreach (DataRowView drv in drvGiaTri)
            {
                dt.Rows.Add(drv.Row.ItemArray);
            }
            drvGiaTri = grdThuTien.ItemsSource as DataView;
            foreach (DataRowView drv in drvGiaTri)
            {
                dt.Rows.Add(drv.Row.ItemArray);
            }
            drvGiaTri = grdChiTien.ItemsSource as DataView;
            foreach (DataRowView drv in drvGiaTri)
            {
                dt.Rows.Add(drv.Row.ItemArray);
            }
            ds.Tables.Add(dt);
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_Result", ds.GetXml(), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            return listThamSoBaoCao;
        }
        #endregion
    }
}
