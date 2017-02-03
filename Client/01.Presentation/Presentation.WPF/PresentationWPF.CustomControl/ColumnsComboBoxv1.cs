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
    public class ColumnsComboBoxv1 : GridViewBoundColumnBase
    {
        public List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        public Telerik.Windows.Controls.GridView.GridViewCell cellEdit;
        public event EventHandler EditCellEnd;

        public static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxv1), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxv1), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty GiaTriDataMemberProperty = DependencyProperty.Register("GiaTriDataMember", typeof(string), typeof(ColumnsComboBoxv1), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public static readonly DependencyProperty DieuKienDataMemberProperty = DependencyProperty.Register("DieuKienDataMember", typeof(string), typeof(ColumnsComboBoxv1), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv1.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv1.GiaTriProperty, (object)value);
            }
        }

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv1.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv1.DieuKienProperty, (object)value);
            }
        }

        public string GiaTriDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv1.GiaTriDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv1.GiaTriDataMemberProperty, (object)value);
            }
        }

        public string DieuKienDataMember
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv1.DieuKienDataMemberProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv1.DieuKienDataMemberProperty, (object)value);
            }
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
                    if (!sTruyVan.Equals(""))
                    {
                        List<string> lstDieuKien = new List<string>();
                        lstDieuKien.Add(DieuKien.ToString());
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        this.SetBinding(GiaTriProperty,CreateValueBinding(dataItem));
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
                    if (!sTruyVan.Equals(""))
                    {
                        List<string> lstDieuKien = new List<string>();
                        lstDieuKien.Add(DieuKien.ToString());
                        lstComboBox = new List<AutoCompleteEntry>();
                        KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sTruyVan, lstDieuKien);
                    }
                    if (!GiaTriDataMember.IsNullOrEmptyOrSpace())
                    {
                        this.SetBinding(GiaTriProperty, CreateValueBinding(dataItem));
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
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
        }

        public override IList<string> UpdateSourceWithEditorValue(GridViewCell gridViewCell)
        {
            List<String> errors = new List<String>();
            RadComboBox editor = gridViewCell.GetEditingElement() as RadComboBox;
            BindingExpression bindingExpression = this.ReadLocalValue(ColumnsComboBoxv1.GiaTriProperty) as BindingExpression;
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
                foreach (ValidationError error in Validation.GetErrors(editor))
                {
                    errors.Add(error.ErrorContent.ToString());
                }
            }
            return errors.ToList();
        }

        public override object GetNewValueFromEditor(object editor)
        {
            RadComboBox colorPicker = editor as RadComboBox;
            if (colorPicker != null)
            {
                AutoCompleteEntry au = lstComboBox.ElementAt(colorPicker.SelectedIndex);
                return au.KeywordStrings.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private Binding CreateValueBinding(object dataItem)
        {
            Binding valueBinding = new Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Source = dataItem;
            valueBinding.Path = new PropertyPath(GiaTriDataMember);
            return valueBinding;
        }
    }
}
