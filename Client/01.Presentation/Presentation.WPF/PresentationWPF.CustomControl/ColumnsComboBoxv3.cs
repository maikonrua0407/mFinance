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
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;

namespace PresentationWPF.CustomControl
{
    public class ColumnsComboBoxv3 : GridViewBoundColumnBase
    {
        private static readonly DependencyProperty GiaTriProperty = DependencyProperty.Register("GiaTri", typeof(string), typeof(ColumnsComboBoxv3), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        private string GiaTri
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv3.GiaTriProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv3.GiaTriProperty, (object)value);
            }
        }

        public static readonly DependencyProperty DieuKienProperty = DependencyProperty.Register("DieuKien", typeof(string), typeof(ColumnsComboBoxv3), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string DieuKien
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv3.DieuKienProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv3.DieuKienProperty, (object)value);
            }
        }

        private Binding _dieuKienDataBinding;

        public Binding DieuKienDataBinding
        {
            get { return _dieuKienDataBinding; }
            set { _dieuKienDataBinding = value; }
        }

        public static readonly DependencyProperty TruyVanProperty = DependencyProperty.Register("TruyVan", typeof(string), typeof(ColumnsComboBoxv3), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public string TruyVan
        {
            get
            {
                return (string)this.GetValue(ColumnsComboBoxv3.TruyVanProperty);
            }
            set
            {
                this.SetValue(ColumnsComboBoxv3.TruyVanProperty, (object)value);
            }
        }

        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            TextBlock txtBlock = new TextBlock();
            try
            {
                if (LObject.IsNullOrEmpty(DataMemberBinding))
                    DataMemberBinding = new Binding();
                if (LObject.IsNullOrEmpty(DataMemberBinding.Source))
                    DataMemberBinding.Source = dataItem;
                if (LObject.IsNullOrEmpty(DieuKienDataBinding))
                    DieuKienDataBinding = new Binding();
                if (LObject.IsNullOrEmpty(DieuKienDataBinding.Source))
                    DieuKienDataBinding.Source = dataItem;
                this.SetBinding(ColumnsComboBoxv3.DieuKienProperty, DieuKienDataBinding);
                this.SetBinding(ColumnsComboBoxv3.GiaTriProperty, DataMemberBinding);
                if (!LObject.IsNullOrEmpty(TruyVan))
                {

                }
                if (!LObject.IsNullOrEmpty(DieuKien))
                {

                }
                if (!LObject.IsNullOrEmpty(GiaTri))
                    txtBlock.Text = GiaTri;
            }
            catch (Exception ex)
            {

            }
            return txtBlock;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            RadComboBox cmbCommon= new RadComboBox();
            try
            {
            }
            catch (Exception ex)
            {
            }
            return cmbCommon as FrameworkElement;
        }

    }
}
