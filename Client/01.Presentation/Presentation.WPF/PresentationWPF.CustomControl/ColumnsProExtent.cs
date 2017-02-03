using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;

using Utilities.Common;

using Telerik.Windows.Controls;
using Presentation.Process.Common;

namespace PresentationWPF.CustomControl
{
    public class ColumnsProExtent : GridViewBoundColumnBase
    {
        List<AutoCompleteEntry> lstComboBox;
        RadComboBox cmbCommon;
        RadMaskedDateTimeInput radMaskDate;
        RadMaskedNumericInput ctrMaskText;
        TextBox ctrCommon;
        Grid grdControl=new Grid();
        DatePicker dtpPicker = new DatePicker();
        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            DataRow dr = null;
            if(dataItem.GetType().Equals(typeof(DataRow)))
                dr = (DataRow)dataItem;
            else
                dr = ((DataRowView)dataItem).Row;
            string sHienThiDKhien = dr["HTHI_DIEU_KHIEN"].ToString();
            string sHienThiTVan = dr["HTHI_SDUNG_TVAN"].ToString();
            string sGiaTriTVan = dr["HTHI_SQL"].ToString();
            string sGiaTri = dr["GTRI_TTIN"].ToString();
            string sKieuDuLieu = dr["KIEU_DU_LIEU"].ToString();
            var valueBinding = new System.Windows.Data.Binding(this.DataMemberBinding.Path.Path);
            if (sHienThiDKhien.Equals("Combobox"))
            {
                cmbCommon = new RadComboBox();
                lstComboBox = new List<AutoCompleteEntry>();
                KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sGiaTriTVan);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                else
                    cmbCommon.SelectedIndex = 0;
                cmbCommon.IsEnabled = false;
                return cmbCommon;
            }
            else if (sHienThiDKhien.Equals("Datetimebox"))
            {
                radMaskDate = new RadMaskedDateTimeInput();
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    radMaskDate.Value = LDateTime.StringToDate(sGiaTri, ApplicationConstant.defaultDateTimeFormat);
                else
                    radMaskDate.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                radMaskDate.IsEnabled = false;
                radMaskDate.HorizontalAlignment = HorizontalAlignment.Stretch;
                var Control = grdControl.FindChildByType<RadMaskedDateTimeInput>();
                if (Control != null)
                    grdControl.Children.Remove(Control);
                var ControlDTP = grdControl.FindChildByType<DatePicker>();
                if (ControlDTP != null)
                    grdControl.Children.Remove(ControlDTP);
                grdControl.Children.Add(radMaskDate);
                return grdControl;
            }
            else
            {
                if (sKieuDuLieu.Equals("Number"))
                {
                    ctrMaskText = new RadMaskedNumericInput();
                    ctrMaskText.Mask = "#18";
                    if (!sGiaTri.IsNullOrEmptyOrSpace())
                        ctrMaskText.Value = Convert.ToDouble(sGiaTri);
                    else
                        ctrMaskText.Value = null;
                    ctrMaskText.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ctrMaskText.IsEnabled = false;
                    return ctrMaskText;
                }
                else
                {
                    ctrCommon = new TextBox();
                    ctrCommon.IsEnabled = false;
                    if (!sGiaTri.IsNullOrEmptyOrSpace())
                        ctrCommon.Text = sGiaTri;
                    else
                        ctrCommon.Text = "";
                    return ctrCommon;
                }
            }

        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {

            DataRow dr = null;
            if (dataItem.GetType().Equals(typeof(DataRow)))
                dr = (DataRow)dataItem;
            else
                dr = ((DataRowView)dataItem).Row;
            string sHienThiDKhien = dr["HTHI_DIEU_KHIEN"].ToString();
            string sHienThiTVan = dr["HTHI_SDUNG_TVAN"].ToString();
            string sGiaTriTVan = dr["HTHI_SQL"].ToString();
            string sGiaTri = dr["GTRI_TTIN"].ToString();
            string sKieuDuLieu = dr["KIEU_DU_LIEU"].ToString();
            TextBox ctrCommon = new TextBox();
            Binding valueBinding = CreateValueBinding();
            if (sHienThiDKhien.Equals("Combobox"))
            {
                lstComboBox = new List<AutoCompleteEntry>();
                cmbCommon = new RadComboBox();
                KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, sGiaTriTVan);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                else
                    cmbCommon.SelectedIndex = 0;
                cmbCommon.Tag = lstComboBox;
                cmbCommon.SetBinding(DisplayIndexProperty, valueBinding);
                cmbCommon.IsEnabled = true;
                return cmbCommon as FrameworkElement;
            }
            else if (sHienThiDKhien.Equals("Datetimebox"))
            {
                var ControlRad = grdControl.FindChildByType<RadMaskedDateTimeInput>();
                if (ControlRad != null)
                    grdControl.Children.Remove(ControlRad);
                var ControlDTP = grdControl.FindChildByType<DatePicker>();
                if (ControlDTP != null)
                    grdControl.Children.Remove(ControlDTP);
                radMaskDate = new RadMaskedDateTimeInput();
                
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    radMaskDate.Value = LDateTime.StringToDate(sGiaTri, ApplicationConstant.defaultDateTimeFormat);
                else
                    radMaskDate.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                radMaskDate.SetBinding(DisplayIndexProperty, valueBinding);
                radMaskDate.IsEnabled = true;
                radMaskDate.HorizontalAlignment = HorizontalAlignment.Stretch;
                radMaskDate.Margin = new Thickness(0, 0, 40, 0);
                dtpPicker.HorizontalAlignment = HorizontalAlignment.Right;
                dtpPicker.Width = 30;
                dtpPicker.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtpPicker_SelectedDateChanged);
                grdControl.Children.Add(dtpPicker);
                grdControl.Children.Add(radMaskDate);
                return grdControl as FrameworkElement;
            }
            else
            {
                if (sKieuDuLieu.Equals("Number"))
                {
                    ctrMaskText = new RadMaskedNumericInput();
                    ctrMaskText.Mask = "#18";
                    if (!sGiaTri.IsNullOrEmptyOrSpace())
                        ctrMaskText.Value = Convert.ToDouble(sGiaTri);
                    else
                        ctrMaskText.Value = null;
                    ctrMaskText.SetBinding(DisplayIndexProperty, valueBinding);
                    ctrMaskText.HorizontalAlignment = HorizontalAlignment.Stretch;
                    return ctrMaskText as FrameworkElement;
                }
                else
                {
                    ctrCommon = new TextBox();
                    if (!sGiaTri.IsNullOrEmptyOrSpace())
                        ctrCommon.Text = sGiaTri;
                    else
                        ctrCommon.Text = "";
                    ctrCommon.SetBinding(DisplayIndexProperty, valueBinding);
                    return ctrCommon as FrameworkElement;
                }
            }
        }

        void dtpPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            radMaskDate.Value = dtpPicker.SelectedDate;
        }

        private Binding CreateValueBinding()
        {
            Binding valueBinding = new Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
            return valueBinding;
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
