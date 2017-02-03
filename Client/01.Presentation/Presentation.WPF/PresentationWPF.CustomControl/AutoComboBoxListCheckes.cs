using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using System.ComponentModel;
using System.Windows;
using Telerik.Windows.Controls;
using System.Data;
using Presentation.Process;

namespace PresentationWPF.CustomControl
{
    public class AutoComboBoxListCheckes
    {
        /// <summary>
        /// Tạo AutocomboBox
        /// </summary>
        /// <param name="lstSource">Source cho Combobox</param>
        /// <param name="comboBox">Tên control Combobox được gen</param>
        /// <param name="maTruyVan">Mã truy vấn để lấy source cho combobox</param>
        /// <param name="lstDieuKien">Giá trị các điều kiện tương ứng với truy vấn</param>
        /// <param name="maChon">Mã Item sẽ được chọn nếu có</param>
        /// <param name="lstMaKhoangChon">Danh sách các Item sẽ được gen vào Combobox nếu có.</param>
        public void GenAutoComboBox(ref ListCheckBoxCombo lstSource, ref RadComboBox comboBox, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
            if (!string.IsNullOrEmpty(maTruyVan))
            {
                DataSet dataSetSource = new DataSet();
                TruyVanProcess process = new TruyVanProcess();
                DanhSachResponse response = process.getDanhSachInformation(maTruyVan, lstDieuKien);
                dataSetSource = response.DataSetSource;
                AutoCompleteCheckBox autocom = new AutoCompleteCheckBox();
                int icount = 0;
                lstSource.Add(new AutoCompleteCheckBox());
                foreach (DataRow row in dataSetSource.Tables[0].Rows)
                {
                    icount++;
                    string[] value = new string[row.Table.Columns.Count - 1];
                    if (row.Table.Columns.Count > 3)
                    {
                        value[0] = row[1].ToString();
                        value[1] = row[0].ToString();
                        for (int i = 3; i < row.Table.Columns.Count; i++)
                        {
                            value[i - 1] = row[i].ToString();
                        }
                        autocom.ValueMember = value;
                    }
                    else
                    {
                        value[0] = row[1].ToString();
                        value[1] = row[0].ToString();
                        autocom.ValueMember = value;
                    }
                    lstSource.Add(new AutoCompleteCheckBox(LLanguage.SearchResourceByKey(row[2].ToString()), value, true, icount));
                }
            }
            comboBox.ItemsSource = lstSource;
            comboBox.SelectedIndex = 0;
        }

        public void GenAutoComboBox(DataTable dt, ref RadComboBox comboBox)
        {
            ListCheckBoxCombo lstSource = new ListCheckBoxCombo();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))  return;

            if (!LObject.IsNullOrEmpty(dt) && dt.Rows.Count>0)
            {
                AutoCompleteCheckBox autocom = new AutoCompleteCheckBox();
                int icount = 0;
                lstSource.Add(new AutoCompleteCheckBox());
                foreach (DataRow row in dt.Rows)
                {
                    icount++;
                    string[] value = new string[row.Table.Columns.Count - 1];
                    if (row.Table.Columns.Count > 3)
                    {
                        value[0] = row[1].ToString();
                        value[1] = row[0].ToString();
                        for (int i = 3; i < row.Table.Columns.Count; i++)
                        {
                            value[i - 1] = row[i].ToString();
                        }
                        autocom.ValueMember = value;
                    }
                    else
                    {
                        value[0] = row[1].ToString();
                        value[1] = row[0].ToString();
                        autocom.ValueMember = value;
                    }
                    lstSource.Add(new AutoCompleteCheckBox(LLanguage.SearchResourceByKey(row[2].ToString()), value, true, icount));
                }

                comboBox.ItemsSource = lstSource;
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
