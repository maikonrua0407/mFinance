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
    public class ColumnsNummeric : GridViewBoundColumnBase
    {
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask",
        typeof(string),
        typeof(ColumnsNummeric),
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

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            RadMaskedNumericInput radMask = new RadMaskedNumericInput();
            radMask.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(dataItem));
            radMask.Mask = Mask;
            radMask.HorizontalAlignment = HorizontalAlignment.Stretch;
            radMask.IsEnabled = false;
            return radMask;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            RadMaskedNumericInput radMask = new RadMaskedNumericInput();
            radMask.SetBinding(RadMaskedNumericInput.ValueProperty, CreateValueBinding(dataItem));
            radMask.Mask = Mask;
            radMask.HorizontalAlignment = HorizontalAlignment.Stretch;
            return radMask;
        }

        private Binding CreateValueBinding(object dataItem)
        {
            Binding valueBinding = new Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Source = dataItem;
            valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
            return valueBinding;
        }

        public override IList<string> UpdateSourceWithEditorValue(GridViewCell gridViewCell)
        {
            List<String> errors = new List<String>();
            RadMaskedNumericInput editor = gridViewCell.GetEditingElement() as RadMaskedNumericInput;
            BindingExpression bindingExpression = editor.ReadLocalValue(RadMaskedNumericInput.ValueProperty) as BindingExpression;
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
            RadMaskedNumericInput colorPicker = editor as RadMaskedNumericInput;
            if (colorPicker != null)
            {
                return colorPicker.Value.GetValueOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
