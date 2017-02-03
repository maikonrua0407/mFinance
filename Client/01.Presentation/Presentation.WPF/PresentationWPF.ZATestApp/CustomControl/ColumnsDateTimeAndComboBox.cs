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
using System.Globalization;


namespace PresentationWPF.ZATestApp.CustomControl
{
    public class ColumnsDateTimeAndComboBox : GridViewBoundColumnBase
    {
        RadComboBox cmbCommon;
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
        public string State
          {
            get { return (string)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); } 
          }
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
    "State", typeof(string), typeof(ColumnsDateTimeAndComboBox), new PropertyMetadata(""));
        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            RadMaskedDateTimeInput txtbCommon = new RadMaskedDateTimeInput();
            try
            {
                txtbCommon.Mask = "dd/MM/yyyy";
                txtbCommon.SetBinding(RadMaskedDateTimeInput.ValueProperty, DataMemberBinding);
                txtbCommon.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return txtbCommon;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            RadMaskedDateTimeInput txtbCommon = new RadMaskedDateTimeInput();
            try
            {
                txtbCommon.Mask = "dd/MM/yyyy";
                txtbCommon.SetBinding(RadMaskedDateTimeInput.ValueProperty, DataMemberBinding);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            cellEdit = cell;
            return txtbCommon as FrameworkElement;
        }
    }
    [ValueConversion(typeof(string),typeof(DateTime))]
    public class DataTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, 
      object parameter, CultureInfo culture)
        {
            return LDateTime.StringToDate(value.ToString(),ApplicationConstant.defaultDateTimeFormat);
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
