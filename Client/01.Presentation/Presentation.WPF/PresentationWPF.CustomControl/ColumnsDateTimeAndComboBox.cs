using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Telerik.Windows.Controls;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using Utilities.Common;
using System.Windows.Controls;

namespace PresentationWPF.CustomControl
{
    public class ColumnsDateTimeAndComboBox : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        RadMaskedDateTimeInput teldtCommon;
        private string giaTriMemberBinding;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;
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
        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsDateTimeAndComboBox), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));
        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsDateTimeAndComboBox.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsDateTimeAndComboBox.DieuKienProperty, (object)value);
            }
        }

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsDateTimeAndComboBox), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));
        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsDateTimeAndComboBox.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsDateTimeAndComboBox.DieuKienDataMemberProperty, (object)value);
            }
        }

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsDateTimeAndComboBox), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));
        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsDateTimeAndComboBox.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsDateTimeAndComboBox.GiaTriDataMemberProperty, (object)value);
            }
        }


        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            teldtCommon = new RadMaskedDateTimeInput();
            try
            {
                teldtCommon.IsEnabled = false;
                teldtCommon.SetBinding(RadMaskedDateTimeInput.ValueProperty, DataMemberBinding);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return teldtCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            try
            {
                cmbCommon = new RadComboBox();
                if (lstComboBox != null || lstComboBox.Count > 0)
                {
                    new AutoComboBox().GenAutoComboBox(ref lstComboBox, ref cmbCommon, null, null, giaTri);
                }
                else
                {
                    if (!dataItem.IsNullOrEmpty())
                    {

                        PropertyInfo property = null;
                        if (!DieuKienDataMember.IsNullOrEmptyOrSpace())
                        {
                            property = dataItem.GetType().GetProperty(DieuKienDataMember);
                            if (property != null)
                                DieuKien = property.GetValue(dataItem, null).ToString();
                        }
                        property = dataItem.GetType().GetProperty(GiaTriDataMember);
                        if (property != null)
                        {
                            GiaTri = property.GetValue(dataItem, null).ToString();
                            lstComboBox = new List<AutoCompleteEntry>();

                            List<string> lstDieuKien = new List<string>();
                            lstDieuKien.Add(DieuKien.ToString());
                            if (!sTruyVan.Equals(""))
                                KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
                            cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(GiaTri)));
                            cmbCommon.Tag = lstComboBox;
                        }

                    }
                }
                cmbCommon.IsEnabled = true;
                cmbCommon.IsEditable = true;
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
            GiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings.First();
            LstgiaTri = lstComboBox.ElementAt(cmbCommon.SelectedIndex).KeywordStrings;
            if (!LObject.IsNullOrEmpty(EditCellEnd))
                EditCellEnd(sender, EventArgs.Empty);
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
}

