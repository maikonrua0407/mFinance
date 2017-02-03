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


namespace PresentationWPF.CustomControl
{
    public class ColumnsComboBoxv2 : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;

        public static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxv2), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxv2), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsComboBoxv2), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsComboBoxv2), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv2.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv2.GiaTriProperty, (object)value);
            }
        }

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv2.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv2.DieuKienProperty, (object)value);
            }
        }

        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv2.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv2.GiaTriDataMemberProperty, (object)value);
            }
        }

        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv2.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv2.DieuKienDataMemberProperty, (object)value);
            }
        }

        string sTruyVan = "";
        public string STruyVan
        {
            get { return sTruyVan; }
            set { sTruyVan = value; }
        }

        bool isEditable = false;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
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
                cmbCommon = new RadComboBox();
                List<string> lstDieuKien = new List<string>();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string strspplit in DieuKienDataMember.Split('#').ToList())
                        {
                            oObject = dataItem;
                            foreach (string str in strspplit.Split('.').ToList())
                            {
                                property = oObject.GetType().GetProperty(str);
                                if (property != null)
                                {
                                    DieuKien = property.GetValue(oObject, null).ToString();
                                    oObject = property.GetValue(oObject, null);
                                }
                            }
                            if (!strspplit.IsNullOrEmptyOrSpace())
                                lstDieuKien.Add(DieuKien);
                        }
                    }
                    else if (!DieuKien.IsNullOrEmptyOrSpace())
                    {
                        lstDieuKien.Add(DieuKien.ToString());
                    }
                    if (!sTruyVan.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
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
                List<string> lstDieuKien = new List<string>();
                if (!dataItem.IsNullOrEmpty())
                {
                    PropertyInfo property = null;
                    object oObject = dataItem;
                    if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                    {
                        foreach (string strspplit in DieuKienDataMember.Split('#').ToList())
                        {
                            oObject = dataItem;
                            foreach (string str in strspplit.Split('.').ToList())
                            {
                                property = oObject.GetType().GetProperty(str);
                                if (property != null)
                                {
                                    DieuKien = property.GetValue(oObject, null).ToString();
                                    oObject = property.GetValue(oObject, null);
                                }
                            }
                            if (!strspplit.IsNullOrEmptyOrSpace())
                                lstDieuKien.Add(DieuKien);
                        }
                    }
                    else if (!DieuKien.IsNullOrEmptyOrSpace())
                    {
                        lstDieuKien.Add(DieuKien.ToString());
                    }
                    if (!sTruyVan.Equals(""))
                    {
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
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
                GiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings.First();
                LstgiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings;
                if (!LObject.IsNullOrEmpty(EditCellEnd))
                    EditCellEnd(sender, EventArgs.Empty);
            }
        }

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, string maTruyVan, List<string> lstDieuKien = null)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBoxDirectly(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
            cmbCommon.IsEditable = isEditable;
        }

    }
}
