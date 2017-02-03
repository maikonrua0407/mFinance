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
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using System.Data;

namespace PresentationWPF.KhaiThacDuLieu.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_DM_THU_GOC_LAI.xaml
    /// </summary>
    public partial class TDVM_DM_THU_GOC_LAI : UserControl
    {
        #region Khai bao
        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceCum = new ListCheckBoxCombo();
        #endregion

        #region Khoi tao

        public TDVM_DM_THU_GOC_LAI()
        {
            InitializeComponent();
            KhoiTaoThongTin();
        }

        private void KhoiTaoThongTin()
        {
            KhoiTaoComboBox();
            dtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfMonth();
            dtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();
        }
        
        private void KhoiTaoComboBox()
        {
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
            LoadComboboxPhongGD();
        }
        #endregion

        #region Xu ly giao dien
        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourcePGD = new ListCheckBoxCombo();
            string maChiNhanh = "";
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += "," + lstCN.ValueMember[1].ToString();
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1, maChiNhanh.Length - 1);
            else
                maChiNhanh = "0";
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
            LoadComboboxCum();
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }
        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceCum = new ListCheckBoxCombo();
            string maChiNhanh = "";
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1, maChiNhanh.Length - 1);
            else
                maChiNhanh = "0";

            string maPGD = "";
            foreach (AutoCompleteCheckBox lstPGD in lstSourcePGD)
            {
                if (lstPGD.CheckedMember)
                    maPGD += ",'" + lstPGD.ValueMember[0].ToString() + "'";
            }
            if (maPGD.Length > 0)
                maPGD = maPGD.Substring(1, maPGD.Length - 1);
            else
                maPGD = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUMLIST.getValue(), lstDieuKien);
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }

        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            LoadComboboxCum();
        }

        private void cmbCum_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceCum = cmbCum.ItemsSource as ListCheckBoxCombo;
        }
        #endregion

        #region Xy ly nghiep vu

        public void GetParameters(out List<Tuple<string,string>> dsDieuKien)
        {
            dsDieuKien = new List<Tuple<string,string>>();
            string tuNgay = LDateTime.DateToString(dtTuNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            string denNgay = LDateTime.DateToString(dtDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            dsDieuKien.Add(new Tuple<string,string>(tuNgay,"@TuNgay"));
            dsDieuKien.Add(new Tuple<string, string>(denNgay, "@DenNgay"));
            foreach (AutoCompleteCheckBox obj in lstSourceChiNhanh)
            {
                if (obj.CheckedMember && !obj.ValueMember[0].ToUpper().Equals("ALL"))
                {
                    dsDieuKien.Add(new Tuple<string, string>(obj.ValueMember[0], "@MaChiNhanh"));
                }
            }
            foreach (AutoCompleteCheckBox obj in lstSourcePGD)
            {
                if (obj.CheckedMember && !obj.ValueMember[0].ToUpper().Equals("ALL"))
                {
                    dsDieuKien.Add(new Tuple<string, string>(obj.ValueMember[0], "@MaPhongGD"));
                }
            }
            foreach (AutoCompleteCheckBox obj in lstSourceCum)
            {
                if (obj.CheckedMember && !obj.ValueMember[0].ToUpper().Equals("ALL"))
                {
                    dsDieuKien.Add(new Tuple<string, string>(obj.ValueMember[0], "@MaCum"));
                }
            }
        }

        public void BuildData(DataSet ds)
        {
                grvChiTiet.ItemsSource = ds.Tables[0].DefaultView;
                grvTongHop.ItemsSource = ds.Tables[1].DefaultView;
        }
        #endregion
    }
}
