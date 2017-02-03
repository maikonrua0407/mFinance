using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for ucMandatoryLabel.xaml
    /// </summary>
    public partial class ucMandatoryLabel : UserControl
    {
        public static readonly DependencyProperty ContentLabelProperty = DependencyProperty.Register("ContentLabel", typeof(string), typeof(ucMandatoryLabel), (PropertyMetadata)new FrameworkPropertyMetadata((object)""));

        public ucMandatoryLabel()
        {
            InitializeComponent();
        }

        public string ContentLabel
        {
            get
            {
                return (string)this.GetValue(ucMandatoryLabel.ContentLabelProperty);
            }
            set
            {
                this.SetValue(ucMandatoryLabel.ContentLabelProperty, (object)value);
            }
        }
    }
}
