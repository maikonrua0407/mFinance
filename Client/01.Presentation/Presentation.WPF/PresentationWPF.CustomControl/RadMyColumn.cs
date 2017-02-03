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

namespace PresentationWPF.CustomControl
{
    public class RadMyColumn : GridViewBoundColumnBase
    {
        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            DataRow dr = (DataRow)dataItem;
            string sHienThiDKhien = dr["HTHI_DIEU_KHIEN"].ToString();
            string sGiaTri = dr["GTRI_TTIN"].ToString();
            TextBlock ctrCommon = new TextBlock();
            var valueBinding = new System.Windows.Data.Binding(this.DataMemberBinding.Path.Path);
            if (sHienThiDKhien.Equals("Combobox"))
            {
                RadComboBox cmbCommon = new RadComboBox();
                List<AutoCompleteEntry> lstComboBox = new List<AutoCompleteEntry>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DON_VI));
                KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, maTruyVan,lstDieuKien);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                else
                    cmbCommon.SelectedIndex = 0;
                return cmbCommon;
            }
            else if (sHienThiDKhien.Equals("Datetime"))
            {
                RadMaskedDateTimeInput radMaskDate = new RadMaskedDateTimeInput();
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    radMaskDate.Value = LDateTime.StringToDate(sGiaTri,ApplicationConstant.defaultDateTimeFormat);
                else
                    radMaskDate.Value = LDateTime.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                return radMaskDate;
            }
            else
            {
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    ctrCommon.Text = sGiaTri;
                else
                    ctrCommon.Text = "";
                return ctrCommon;
            }
            
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {

            DataRow dr = (DataRow)dataItem;
            string sHienThiDKhien = dr["HTHI_DIEU_KHIEN"].ToString();
            string sGiaTri = dr["GTRI_TTIN"].ToString();
            TextBlock ctrCommon = new TextBlock();
            Binding valueBinding = CreateValueBinding();
            if (sHienThiDKhien.Equals("Combobox"))
            {
                RadComboBox cmbCommon = new RadComboBox();
                List<AutoCompleteEntry> lstComboBox = new List<AutoCompleteEntry>();
                string maTruyVan = dr["HTHI_SQL"].ToString();
                if (dr["HTHI_SDUNG_TVAN"].Equals("CO"))
                {
                    KhoiTaoGiaTriComboBox(ref lstComboBox, ref cmbCommon, maTruyVan);
                    cmbCommon.SetBinding(DataContextProperty, valueBinding);
                    if (!sGiaTri.IsNullOrEmptyOrSpace())
                        cmbCommon.SelectedIndex = lstComboBox.IndexOf(lstComboBox.FirstOrDefault(i => i.KeywordStrings.First().Equals(sGiaTri)));
                    else
                        cmbCommon.SelectedIndex = 0;
                }
                return cmbCommon as FrameworkElement;
            }
            else if (sHienThiDKhien.Equals("Datetime"))
            {
                RadMaskedDateTimeInput radMaskDate = new RadMaskedDateTimeInput();
                radMaskDate.SetBinding(DataContextProperty, valueBinding);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    radMaskDate.Value = LDateTime.StringToDate(sGiaTri);
                else
                    radMaskDate.Value = LDateTime.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                return radMaskDate as FrameworkElement;
            }
            else
            {
                ctrCommon.SetBinding(DataContextProperty, valueBinding);
                if (!sGiaTri.IsNullOrEmptyOrSpace())
                    ctrCommon.Text = sGiaTri;
                else
                    ctrCommon.Text = "";
                return ctrCommon as FrameworkElement;
            }
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

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete,ref Telerik.Windows.Controls.RadComboBox cmbCommon,string maTruyVan,  List<string> lstDieuKien=null)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
        }
        //public override object GetNewValueFromEditor(object editor)
        //{
        //    TextBox colorPicker = editor as TextBox;
        //    if (colorPicker != null)
        //    {
        //        return colorPicker.Text;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
