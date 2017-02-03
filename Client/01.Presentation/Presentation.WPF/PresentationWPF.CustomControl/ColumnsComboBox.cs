using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Utilities.Common;

using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.GridView.Clipboard;
using Telerik.Windows.Controls.GridView.Columns;
using Telerik.Windows.Data;
using Telerik.Windows.Controls;
using System.Reflection;
using Presentation.Process.TaiSanServiceRef;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;


namespace PresentationWPF.CustomControl
{
    public class ColumnsComboBox : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        private string giaTriMemberBinding;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public object objDataItem;
        public event EventHandler EditCellEnd;
        public int index;
        public string GiaTriMemberBinding
        {
            get { return giaTriMemberBinding; }
            set { giaTriMemberBinding = value; }
        }
        private string dieuKienMemberBinding;

        public string DieuKienMemberBinding
        {
            get { return dieuKienMemberBinding; }
            set { dieuKienMemberBinding = value; }
        }
        private string giaTri;

        public string GiaTri
        {
            get { return giaTri; }
            set { giaTri = value; }
        }

        private string truyvanMemberBinding;

        public string TruyVanMemberBinding
        {
            get { return truyvanMemberBinding; }
            set { truyvanMemberBinding = value; }
        }

        string sTruyVan = "";
        public string STruyVan
        {
            get { return sTruyVan; }
            set { sTruyVan = value; }
        }
        private string[] lstgiaTri;

        public string[] LstgiaTri
        {
            get { return lstgiaTri; }
            set { lstgiaTri = value; }
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                DataRow dr = null;
                if (dataItem.GetType() == typeof(DataRow))
                    dr = (DataRow)dataItem;
                else if (dataItem.GetType() == typeof(DataRowView))
                    dr = ((DataRowView)dataItem).Row;
                objDataItem = dataItem;
                lstComboBox = new List<AutoCompleteEntry>();
                cmbCommon = new RadComboBox();
                List<string> lstDieuKien = new List<string>();
                string sDieuKien = "";
                string sGiaTri = "";
                if (dr.Table.Columns.Contains(DieuKienMemberBinding))
                    sDieuKien = dr[DieuKienMemberBinding].ToString();
                else
                    sDieuKien = DieuKienMemberBinding;
                if (!GiaTriMemberBinding.IsNullOrEmptyOrSpace() && dr.Table.Columns.Contains(GiaTriMemberBinding))
                    sGiaTri = dr[GiaTriMemberBinding].ToString();
                lstDieuKien.Add(sDieuKien.ToString());
                if (!truyvanMemberBinding.IsNullOrEmptyOrSpace())
                {
                    if(dr.Table.Columns.Contains(truyvanMemberBinding))
                        sTruyVan = dr[truyvanMemberBinding].ToString();
                }
                if (!sTruyVan.Equals(""))
                    KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                else
                    cmbCommon.SelectedIndex = -1;
                cmbCommon.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return cmbCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                DataRow dr = null;
                if (dataItem.GetType() == typeof(DataRow))
                    dr = (DataRow)dataItem;
                else if (dataItem.GetType() == typeof(DataRowView))
                    dr = ((DataRowView)dataItem).Row;
                objDataItem = dataItem;
                lstComboBox = new List<AutoCompleteEntry>();
                cmbCommon = new RadComboBox();
                List<string> lstDieuKien = new List<string>();
                string sDieuKien = "";
                string sGiaTri = "";
                if (!truyvanMemberBinding.IsNullOrEmptyOrSpace())
                {
                    if (dr.Table.Columns.Contains(truyvanMemberBinding))
                        sTruyVan = dr[truyvanMemberBinding].ToString();
                }
                if (dr.Table.Columns.Contains(DieuKienMemberBinding))
                    sDieuKien = dr[DieuKienMemberBinding].ToString();
                else
                    sDieuKien = DieuKienMemberBinding;
                if (!GiaTriMemberBinding.IsNullOrEmptyOrSpace() && dr.Table.Columns.Contains(GiaTriMemberBinding))
                    sGiaTri = dr[GiaTriMemberBinding].ToString();
                lstDieuKien.Add(sDieuKien.ToString());
                if (!truyvanMemberBinding.IsNullOrEmptyOrSpace())
                {
                    if (dr.Table.Columns.Contains(truyvanMemberBinding))
                        sTruyVan = dr[truyvanMemberBinding].ToString();
                }
                lstDieuKien.Add(sDieuKien.ToString());
                if (!sTruyVan.Equals(""))
                    KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
                cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                cmbCommon.Tag = lstComboBox;
                cmbCommon.IsEnabled = true;
                cmbCommon.SelectionChanged += new SelectionChangedEventHandler(cmbCommon_SelectionChanged);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cellEdit = cell;
            return cmbCommon as FrameworkElement;
        }

        void cmbCommon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCommon.SelectedIndex >= 0)
            {
                GiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings.First();
                LstgiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings;
                index = cmbCommon.SelectedIndex;
                if (!LObject.IsNullOrEmpty(EditCellEnd))
                    EditCellEnd(sender, EventArgs.Empty);
            }
        }

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, string maTruyVan, List<string> lstDieuKien = null)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
        }
    }

    public class ColumnsComboBoxDmucTSan : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;

        public static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxDmucTSan), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxDmucTSan), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsComboBoxDmucTSan), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsComboBoxDmucTSan), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucTSan.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucTSan.GiaTriProperty, (object)value);
            }
        }

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucTSan.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucTSan.DieuKienProperty, (object)value);
            }
        }

        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucTSan.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucTSan.GiaTriDataMemberProperty, (object)value);
            }
        }

        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucTSan.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucTSan.DieuKienDataMemberProperty, (object)value);
            }
        }

        string sMaLoaiDM = "";
        public string SMaLoaiDM
        {
            get { return sMaLoaiDM; }
            set { sMaLoaiDM = value; }
        }

        bool isEditable = false;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string str in DieuKienDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                DieuKien = property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }

                    }
                    if (!sMaLoaiDM.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sMaLoaiDM);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        foreach (string str in GiaTriDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cmbCommon.IsEnabled = false;
            return cmbCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string str in DieuKienDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                DieuKien = property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }

                    }
                    if (!sMaLoaiDM.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sMaLoaiDM);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        foreach (string str in GiaTriDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = -1;
                }
                cmbCommon.Tag = lstComboBox;
                cmbCommon.IsEnabled = true;
                cmbCommon.SelectionChanged += new SelectionChangedEventHandler(cmbCommon_SelectionChanged);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cellEdit = cell;
            return cmbCommon as FrameworkElement;
        }

        void cmbCommon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCommon.SelectedIndex >= 0)
            {
                GiaTri = ((AutoCompleteEntry)cmbCommon.SelectedItem).KeywordStrings.First();
                if (!LObject.IsNullOrEmpty(EditCellEnd))
                    EditCellEnd(sender, EventArgs.Empty);
            }
        }

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, string maDanhMuc)
        {
            List<DatabaseConstant.LOAI_DMUC_TSAN> lstDanhMucLoai = new List<DatabaseConstant.LOAI_DMUC_TSAN>();
            lstDanhMucLoai.Add(DatabaseConstant.layLoaiDanhMucTS(maDanhMuc));
            List<DMUC_TSAN_DTO> lstDanhMucDto = new List<DMUC_TSAN_DTO>();
            TaiSanProcess process = new TaiSanProcess();
            if (process.LayDanhMucTaiSanTheoLoai(ref lstDanhMucDto, lstDanhMucLoai))
            {
                foreach (DMUC_TSAN_DTO item in lstDanhMucDto)
                {
                    foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                    {
                        AutoCompleteEntry entry = new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI);
                        cmbCommon.Items.Add(entry);
                        lstAutoComplete.Add(entry);
                    }
                }
            }
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
            cmbCommon.IsEditable = isEditable;
        }

    }

    public class ColumnsComboBoxDoiTuongSD : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;

        public static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxDoiTuongSD), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxDoiTuongSD), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsComboBoxDoiTuongSD), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsComboBoxDoiTuongSD), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDoiTuongSD.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDoiTuongSD.GiaTriProperty, (object)value);
            }
        }

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDoiTuongSD.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDoiTuongSD.DieuKienProperty, (object)value);
            }
        }

        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDoiTuongSD.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDoiTuongSD.GiaTriDataMemberProperty, (object)value);
            }
        }

        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDoiTuongSD.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDoiTuongSD.DieuKienDataMemberProperty, (object)value);
            }
        }

        string sPhongBan = "";
        public string SPhongBan
        {
            get { return sPhongBan; }
            set { sPhongBan = value; }
        }

        bool isEditable = false;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string str in DieuKienDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                DieuKien = property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }

                    }
                    if (!sPhongBan.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sPhongBan);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        if (oObject.GetType() == typeof(DataRow))
                        {
                            GiaTri = ((DataRow)oObject)[GiaTriDataMember].ToString();
                        }
                        else
                        {
                            foreach (string str in GiaTriDataMember.Split('.').ToList())
                            {
                                property = oObject.GetType().GetProperty(str);
                                if (property != null)
                                {
                                    GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                    oObject = property.GetValue(oObject, null);
                                }
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cmbCommon.IsEnabled = false;
            return cmbCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        if (oObject.GetType() == typeof(DataRow))
                        {
                            GiaTri = ((DataRow)oObject)[GiaTriDataMember].ToString();
                        }
                        else
                        {
                            foreach (string str in GiaTriDataMember.Split('.').ToList())
                            {
                                property = oObject.GetType().GetProperty(str);
                                if (property != null)
                                {
                                    GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                    oObject = property.GetValue(oObject, null);
                                }
                            }
                        }
                    }
                    if (!sPhongBan.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sPhongBan);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        foreach (string str in GiaTriDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = -1;
                }
                cmbCommon.Tag = lstComboBox;
                cmbCommon.IsEnabled = true;
                cmbCommon.SelectionChanged += new SelectionChangedEventHandler(cmbCommon_SelectionChanged);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cellEdit = cell;
            return cmbCommon as FrameworkElement;
        }

        void cmbCommon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GiaTri = ((AutoCompleteEntry)cmbCommon.SelectedItem).KeywordStrings.First();
            if (!LObject.IsNullOrEmpty(EditCellEnd))
                EditCellEnd(sender, EventArgs.Empty);
        }

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, string maPhongBan)
        {
            var process = new TruyVanProcess();
            DataSet dataSetSource = new DataSet();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Clear();
            lstDieuKien.Add(maPhongBan);
            lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.THOI_VIEC.layGiaTri());
            lstDieuKien.Add(ClientInformation.MaDonVi);
            DanhSachResponse response = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUOI_BAN_GIAO.getValue(), lstDieuKien);
            dataSetSource = response.DataSetSource;
            if (dataSetSource.Tables.Count > 0)
            {
                foreach (DataRow row in dataSetSource.Tables[0].Rows)
                {
                    AutoCompleteEntry entry = new AutoCompleteEntry(row["TEN_HSO"].ToString(), row["MA_HSO"].ToString(), row["CHUC_VU"].ToString());
                    cmbCommon.Items.Add(entry);
                    lstAutoComplete.Add(entry);
                }
            }
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
            cmbCommon.IsEditable = isEditable;
        }

    }

    public class ColumnsComboBoxDmucNhanSu : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;

        public static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxDmucNhanSu), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxDmucNhanSu), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsComboBoxDmucNhanSu), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsComboBoxDmucNhanSu), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucNhanSu.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucNhanSu.GiaTriProperty, (object)value);
            }
        }

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucNhanSu.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucNhanSu.DieuKienProperty, (object)value);
            }
        }

        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucNhanSu.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucNhanSu.GiaTriDataMemberProperty, (object)value);
            }
        }

        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxDmucNhanSu.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxDmucNhanSu.DieuKienDataMemberProperty, (object)value);
            }
        }

        string sMaLoaiDM = "";
        public string SMaLoaiDM
        {
            get { return sMaLoaiDM; }
            set { sMaLoaiDM = value; }
        }

        bool isEditable = false;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string str in DieuKienDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                DieuKien = property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }

                    }
                    if (!sMaLoaiDM.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sMaLoaiDM);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        foreach (string str in GiaTriDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cmbCommon.IsEnabled = false;
            return cmbCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string str in DieuKienDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                DieuKien = property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }

                    }
                    if (!sMaLoaiDM.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sMaLoaiDM);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        oObject = dataItem;
                        foreach (string str in GiaTriDataMember.Split('.').ToList())
                        {
                            property = oObject.GetType().GetProperty(str);
                            if (property != null)
                            {
                                GiaTri = property.GetValue(oObject, null).IsNullOrEmpty() ? "" : property.GetValue(oObject, null).ToString();
                                oObject = property.GetValue(oObject, null);
                            }
                        }
                    }
                    if (!GiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                    else
                        cmbCommon.SelectedIndex = -1;
                }
                cmbCommon.Tag = lstComboBox;
                cmbCommon.IsEnabled = true;
                cmbCommon.SelectionChanged += new SelectionChangedEventHandler(cmbCommon_SelectionChanged);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cellEdit = cell;
            return cmbCommon as FrameworkElement;
        }

        void cmbCommon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCommon.SelectedIndex >= 0)
            {
                GiaTri = ((AutoCompleteEntry)cmbCommon.SelectedItem).KeywordStrings.First();
                if (!LObject.IsNullOrEmpty(EditCellEnd))
                    EditCellEnd(sender, EventArgs.Empty);
            }
        }

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, string maDanhMuc)
        {
            var process = new TruyVanProcess();
            DataSet dataSetSource = new DataSet();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Clear();
            lstDieuKien.Add(maDanhMuc);
            lstDieuKien.Add(ClientInformation.MaDonVi);
            DanhSachResponse response = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_NHAN_SU_THEO_BANG.getValue(), lstDieuKien);
            dataSetSource = response.DataSetSource;
            if (dataSetSource.Tables.Count > 0)
            {
                foreach (DataRow row in dataSetSource.Tables[0].Rows)
                {
                    AutoCompleteEntry entry = new AutoCompleteEntry(row["TEN"].ToString(), row["ID"].ToString(), row["MA"].ToString());
                    cmbCommon.Items.Add(entry);
                    lstAutoComplete.Add(entry);
                }
            }
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
            cmbCommon.IsEditable = isEditable;
        }

    }
}
