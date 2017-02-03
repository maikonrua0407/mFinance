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
using System.Windows.Input;

namespace PresentationWPF.CustomControl
{
    public class ColumnsMaskDateTime : GridViewBoundColumnBase
    {
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask",
        typeof(string),
        typeof(ColumnsMaskDateTime),
        new PropertyMetadata(null));

        public String Mask
        {
            get
            {
                return (string)GetValue(MaskProperty);
            }
            set
            {
                SetValue(MaskProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
        typeof(string),
        typeof(ColumnsMaskDateTime),
        new PropertyMetadata(null));

        public String Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue",
        typeof(DateTime),
        typeof(ColumnsMaskDateTime),
        new PropertyMetadata(null));

        public DateTime DefaultValue
        {
            get
            {
                return (DateTime)GetValue(DefaultValueProperty);
            }
            set
            {
                SetValue(DefaultValueProperty, value);
            }
        }

        public static readonly DependencyProperty VisibilityCalendarProperty = DependencyProperty.Register("VisibilityCalendar",
        typeof(bool),
        typeof(ColumnsMaskDateTime),
        new PropertyMetadata(null));

        public Boolean VisibilityCalendar
        {
            get
            {
                return (Boolean)GetValue(VisibilityCalendarProperty);
            }
            set
            {
                SetValue(VisibilityCalendarProperty, value);
            }
        }

        public ColumnsMaskDateTime()
        {
            if (LObject.IsNullOrEmpty(DefaultValueProperty))
                DefaultValue = DateTime.Now;
            if (LObject.IsNullOrEmpty(VisibilityCalendar))
                VisibilityCalendar = true;
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            Binding newBinding = CreateValueBinding();
            if (LObject.IsNullOrEmpty(newBinding.Source))
                newBinding.Source = dataItem;
            this.SetBinding(ColumnsMaskDateTime.ValueProperty, newBinding);
            TextBlock txt = new TextBlock();
            if (!LObject.IsNullOrEmpty(Value))
                txt.Text = LDateTime.StringToDate(Value, ApplicationConstant.defaultDateTimeFormat).ToString(Mask);
            return txt;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            Binding newBinding = CreateValueBinding();
            if (LObject.IsNullOrEmpty(newBinding.Source))
                newBinding.Source = dataItem;
            this.SetBinding(ColumnsMaskDateTime.ValueProperty, newBinding);
            Grid gridPanel = new Grid();
            RadMaskedDateTimeInput radMask = new RadMaskedDateTimeInput();
            DatePicker datePicker = new DatePicker();
            Binding datePickerBinding = new Binding();
            datePickerBinding.Mode = BindingMode.TwoWay;
            datePickerBinding.NotifyOnValidationError = true;
            datePickerBinding.ValidatesOnExceptions = true;
            datePickerBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            datePickerBinding.Source = radMask;
            datePickerBinding.Path = new PropertyPath(RadMaskedDateTimeInput.ValueProperty);
            datePicker.SetBinding(DatePicker.SelectedDateProperty, datePickerBinding);
            datePicker.IsTabStop = false;
            if (!LObject.IsNullOrEmpty(Value))
                radMask.Value = LDateTime.StringToDate(Value, ApplicationConstant.defaultDateTimeFormat);
            else
                radMask.Value = DefaultValue;
            if (!Mask.IsNullOrEmptyOrSpace())
                radMask.Mask = Mask;
            radMask.HorizontalAlignment = HorizontalAlignment.Stretch;
            radMask.IsClearButtonVisible = false;
            var ControlDate = gridPanel.FindChildByType<RadMaskedDateTimeInput>();
            if (ControlDate == null)
                gridPanel.Children.Add(radMask);
            if (VisibilityCalendar)
            {
                radMask.Margin = new Thickness(0, 0, 35, 0);
                datePicker.HorizontalAlignment = HorizontalAlignment.Right;
                datePicker.Width = 30;
                var ControlDatePicker = gridPanel.FindChildByType<DatePicker>();
                if (ControlDatePicker == null)
                    gridPanel.Children.Add(datePicker);
            }
            return gridPanel;
        }

        public Binding CreateValueBinding()
        {
            System.Windows.Data.Binding valueBinding = new System.Windows.Data.Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
            return valueBinding;
        }

        public override IList<string> UpdateSourceWithEditorValue(GridViewCell gridViewCell)
        {
            List<String> errors = new List<String>();
            Grid gridPanel = gridViewCell.GetEditingElement() as Grid;
            DatePicker editor = gridPanel.FindChildByType<DatePicker>();
            if (editor.SelectedDate.HasValue)
                Value = LDateTime.DateToString(editor.SelectedDate.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            else
                Value = "";
            BindingExpression bindingExpression = this.ReadLocalValue(ColumnsMaskDateTime.ValueProperty) as BindingExpression;
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
            Grid grid = editor as Grid;
            RadMaskedDateTimeInput datePicker = grid.FindChildByType<RadMaskedDateTimeInput>();
            if (datePicker != null)
            {
                return datePicker.Value.GetValueOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
