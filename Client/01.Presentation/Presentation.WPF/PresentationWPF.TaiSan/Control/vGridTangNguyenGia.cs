using System;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Collections.Generic;
using Telerik.Windows.Controls.GridView;
using System.Linq;
using System.Text.RegularExpressions;


namespace PresentationWPF.TaiSan.Control
{
    public class vGridTangNguyenGia : ViewModelBase
    {
        private readonly RadGridView gridView = null;
        private readonly bool isValidationEnabled = false;
        private List<string> allCountries;

        public static readonly DependencyProperty IsValidationEnabledProperty =
            DependencyProperty.RegisterAttached("IsValidationEnabled", typeof(bool), typeof(vGridTangNguyenGia),
            new PropertyMetadata(new PropertyChangedCallback(OnValidationSummaryPropertyChanged)));

        public static void SetIsValidationEnabled(DependencyObject dependencyObject, bool isEnabled)
        {
            dependencyObject.SetValue(IsValidationEnabledProperty, isEnabled);
        }

        public static bool GetIsValidationEnabled(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsValidationEnabledProperty);
        }

        public static void OnValidationSummaryPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            RadGridView gridView = dependencyObject as RadGridView;
            bool isEnabled = (bool)e.NewValue;

            if (gridView != null && isEnabled)
            {
                vGridTangNguyenGia behavior = new vGridTangNguyenGia(gridView, isEnabled);
            }
        }

        public vGridTangNguyenGia(RadGridView gridView, bool isEnabled)
        {
            this.gridView = gridView;
            this.isValidationEnabled = isEnabled;

            this.gridView.CellValidating += this.GridView_CellValidating;
            this.gridView.RowValidating += this.GridView_RowValidating;
        }

        void GridView_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            bool isValid = true;
            //string hinhThucTT = "";
            //string validationText = "Validation failed. ";
            string validationText = "Giá trị không phù hợp. ";
            GridViewCell cell = e.Cell;
            Grid grid = null;
            switch (cell.Column.UniqueName)
            {
                case "ContactName":
                    isValid = ValidateName((string)e.NewValue);
                    if (!isValid)
                    {
                        validationText += "The name of the customer may contain only Latin letters" +
                                            Environment.NewLine + "and empty spaces and must start with a letter.";
                    }
                    break;
                case "Country":
                    isValid = this.ValidateCountry((string)e.NewValue);
                    if (!isValid)
                    {
                        validationText += "The name of the country must match the name of an existing one.";
                    }
                    break;
                case "Phone":
                    isValid = ValidatePhone((string)e.NewValue);
                    if (!isValid)
                    {
                        validationText += "The phone must be in one of the formats X.X.X.X, Y or (X) Y, where " +
                                            Environment.NewLine +
                                            "X is a random sequence of numerals and Y is a random sequence of numerals, " +
                                            Environment.NewLine +
                                            "empty spaces and '-', which starts and ends with a numeral.";
                    }
                    break;
                case "PostalCode":
                    isValid = ValidatePostalCode(e.NewValue);
                    if (!isValid)
                    {
                        validationText += "The postal code of the customer must not be empty.";
                    }
                    break;

                // Truongnx add
                case "SO_CTU":

                    isValid = ValidateSO_CTU((string)e.NewValue);
                    if (!isValid)
                    {
                        validationText += "Số chứng từ không được để trống.";
                    }
                    break;

                //case "NGAY_CTU":
                //isValid = ValidateNGAY_CTU((string)e.NewValue);
                //if (!isValid)
                //{
                //    validationText += "Ngày chứng từ không được để trống.";
                //}
                //break;

                //grid = e.EditingElement as Grid;
                //RadDatePicker rdp = grid.FindChildByType<RadDatePicker>();
                //if (rdp != null && rdp.SelectedDate != null)
                //{
                //    isValid = ValidateNGAY_CTU(rdp.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                //    if (!isValid)
                //    {
                //        validationText += "Ngày chứng từ không được để trống.";
                //    }
                //}
                //else if (rdp.SelectedDate == null)
                //{
                //    isValid = false;
                //    if (!isValid)
                //    {
                //        validationText += "Ngày chứng từ không được để trống.";
                //    }
                //}
                //break;


                //case "GIA_TRI":

                //    isValid = ValidateGIA_TRI(((decimal)e.NewValue).ToString());
                //    if (!isValid)
                //    {
                //        validationText += "Giá trị nguyên giá không được để trống và chỉ là các ký tự số.";
                //    }
                //    break;
                //case "TKHOAN_TTOAN":
                //    grid = e.EditingElement as Grid;
                //    TextBox txt = grid.FindChildByType<TextBox>();
                //    if (txt != null)
                //    {
                //        isValid = ValidateTKHOAN_TTOAN((string)txt.Text);
                //        if (!isValid)
                //        {
                //            validationText += "Tài khoản thanh toán không được để trống và chỉ là các ký tự số.";
                //        }
                //    }
                //    break;
            }
            if (!isValid)
            {
                this.MarkCell(cell, validationText);
            }
            else
            {
                this.RestoreCell(cell);
            }

            e.ErrorMessage = validationText;
            e.IsValid = isValid;
        }

        private void GridView_RowValidating(object sender, GridViewRowValidatingEventArgs e)
        {
            bool isValid = true;
            //string validationText = "Validation failed. ";
            string validationText = "Giá trị không phù hợp. ";
            GridViewRow row = e.Row;

            foreach (GridViewCell cell in row.Cells)
            {
                object value = cell.Value;
                switch (cell.Column.UniqueName)
                {
                    case "ContactName":
                        isValid = ValidateName((string)value);
                        if (!isValid)
                        {
                            validationText += "The name of the customer may contain only Latin letters" +
                                                Environment.NewLine + "and empty spaces and must start with a letter.";
                        }
                        break;
                    case "Country":
                        isValid = this.ValidateCountry((string)value);
                        if (!isValid)
                        {
                            validationText += "The name of the country must match the name of an existing one.";
                        }
                        break;
                    case "Phone":
                        isValid = ValidatePhone((string)value);
                        if (!isValid)
                        {
                            validationText += "The phone must be in one of the formats X.X.X.X, Y or (X) Y, where " +
                                                Environment.NewLine +
                                                "X is a random sequence of numerals and Y is a random sequence of numerals, " +
                                                Environment.NewLine +
                                                "empty spaces and '-', which starts and ends with a numeral.";
                        }
                        break;
                    case "PostalCode":
                        isValid = ValidatePostalCode(value);
                        if (!isValid)
                        {
                            validationText += "The postal code of the customer must not be empty.";
                        }
                        break;

                    // Truongnx add
                    case "SO_CTU":
                        isValid = ValidateSO_CTU((string)value);
                        if (!isValid)
                        {
                            validationText += "Số chứng từ không được để trống.";
                        }
                        break;
                    case "CAU_THANH":
                        isValid = ValidateCAU_THANH((string)value);
                        if (!isValid)
                        {
                            validationText += "Chưa chọn cấu thành nguyên giá.";
                        }
                        break;
                    case "HTHUC_TTOAN":
                        isValid = ValidateHTHUC_TTOAN((string)value);
                        if (!isValid)
                        {
                            validationText += "Chưa chọn hình thức thanh toán.";
                        }
                        break;
                    case "NGAY_CTU":
                        //isValid = ValidateNGAY_CTU((string)value);
                        //if (!isValid)
                        //{
                        //    validationText += "Ngày chứng từ không được để trống.";
                        //}
                        break;

                    case "GIA_TRI":
                        isValid = ValidateGIA_TRI((string)value);
                        if (!isValid)
                        {
                            validationText += "Giá trị nguyên giá không được để trống và chỉ là các ký tự số.";
                        }
                        break;

                    //case "TKHOAN_TTOAN":
                    //    isValid = ValidateTKHOAN_TTOAN((string)value);
                    //    if (!isValid)
                    //    {
                    //        validationText += "Tài khoản thanh toán không được để trống và chỉ là các ký tự số.";
                    //    }
                    //    break;

                }
                if (!isValid)
                {
                    this.MarkCell(cell, validationText);
                    e.IsValid = isValid;
                    return;
                }
                else
                {
                    this.RestoreCell(cell);
                }
            }
            e.IsValid = isValid;
        }

        private static List<string> GetAllCountries()
        {
            List<string> allCountries = new List<string>(1);
            IEnumerable<string> customerCountries = new List<string>();
            //IEnumerable<string> customerCountries = from customer in new Northwind().CustomersCollection
            //                                        select customer.Country;
            foreach (string country in customerCountries)
            {
                allCountries.Add(country);
            }
            allCountries.Add("UK");
            allCountries.Sort();
            return allCountries;
        }

        private static bool ValidatePostalCode(object postalCode)
        {
            if (postalCode == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(postalCode.ToString());
        }

        private bool ValidateCountry(string country)
        {
            if (this.allCountries == null)
            {
                this.allCountries = GetAllCountries();
            }
            return this.allCountries.Contains(country);
        }

        private static bool ValidateName(string name)
        {
            if (name == null)
            {
                return false;
            }
            return Regex.IsMatch(name, @"^([A-Za-z]+\s*)+$");
        }

        private static bool ValidatePhone(string phone)
        {
            return string.IsNullOrEmpty(phone) ||
                   Regex.IsMatch(phone, @"^([0-9]+\.){3}[0-9]+$") || Regex.IsMatch(phone, @"^(\([0-9]+\))?\s?([0-9]+(-|\s)*)*[0-9]+\s*$");
        }

        private void MarkCell(System.Windows.Controls.Control cell, string validationText)
        {
            ToolTipService.SetToolTip(cell, validationText);
        }

        private void RestoreCell(System.Windows.Controls.Control cell)
        {
            ToolTipService.SetToolTip(cell, null);
        }

        // Truongnx add
        private static bool ValidateSO_CTU(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
        }
        private static bool ValidateCAU_THANH(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
        }
        private static bool ValidateHTHUC_TTOAN(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
        }
        private static bool ValidateNGAY_CTU(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
        }

        private static bool ValidateGIA_TRI(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
            //return Regex.IsMatch(input, @"^[0-9]+$");
        }

        private static bool ValidateTKHOAN_TTOAN(string input)
        {
            if (input == null)
            {
                return false;
            }
            return !string.IsNullOrEmpty(input);
            //return Regex.IsMatch(input, @"^[0-9]+$");
        }
    }
}
