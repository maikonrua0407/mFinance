using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System;
using System.Windows.Controls;

namespace PresentationWPF.CustomControl
{
    public class ColumnsDateTime : GridViewBoundColumnBase
    {
        public TimeSpan TimeInterval
		{
			get
			{
				return (TimeSpan) GetValue(TimeIntervalProperty);
			}
			set
			{
				SetValue(TimeIntervalProperty, value);
			}
		}

		public static readonly DependencyProperty TimeIntervalProperty =
			DependencyProperty.Register("TimeInterval", typeof(TimeSpan), typeof(ColumnsDateTime), new PropertyMetadata(TimeSpan.FromHours(1d)));


        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            this.BindingTarget = RadMaskedDateTimeInput.ValueProperty;

            RadMaskedDateTimeInput picker = new RadMaskedDateTimeInput();
            picker.FormatString = "dd/MM/yyyy";
            picker.SetBinding(this.BindingTarget, this.CreateValueBinding());
            picker.HorizontalAlignment = HorizontalAlignment.Stretch;
            picker.IsEnabled = false;
            return picker;
        }

		public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
		{
            this.SetBinding(ColumnsMaskDateTime.ValueProperty, CreateValueBinding(dataItem));

			this.BindingTarget = RadDateTimePicker.SelectedValueProperty;

			RadDatePicker picker = new RadDatePicker();
			picker.IsTooltipEnabled = false;

			picker.TimeInterval = this.TimeInterval;

            picker.DisplayFormat = DateTimePickerFormat.Short;

			//picker.SetBinding(this.BindingTarget, this.CreateValueBinding());
            picker.SetBinding(this.BindingTarget, CreateValueBinding(dataItem));

			return picker;
		}

		public override object GetNewValueFromEditor(object editor)
		{
            RadDatePicker picker = editor as RadDatePicker;
			if (picker != null)
			{
				picker.DateTimeText = picker.CurrentDateTimeText;
			}

			return base.GetNewValueFromEditor(editor);
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

        private System.Windows.Data.Binding CreateValueBinding(object dataItem)
        {
            System.Windows.Data.Binding valueBinding = new System.Windows.Data.Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Source = dataItem;
            valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
            return valueBinding;
        }
    }
}
