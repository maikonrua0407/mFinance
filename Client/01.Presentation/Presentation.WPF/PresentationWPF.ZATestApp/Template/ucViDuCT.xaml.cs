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
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;

namespace PresentationWPF.ZATestApp.Template
{
    /// <summary>
    /// Interaction logic for ucViDuCT.xaml
    /// </summary>
    public partial class ucViDuCT : FormDetailBase
    {
        public ucViDuCT()
        {
            InitializeComponent();
            MessageBox.Show("Welcome");

            tlbAdd.Label = "XXXX";
            tlbModify.Label = "YYYY";
            tlbDelete.Label = "ZZZZ";

            tlbAdd.ToolTip = "XXXX Tooltip";
            tlbModify.Visibility = Visibility.Hidden;
            tlbDelete.Visibility = Visibility.Collapsed;
        }

        public Visibility tlbSaveVisibility
        {
            get; set;
        }

        private void tlbApprove_Click(object sender, RoutedEventArgs e)
        {
            tlbSave.Visibility = Visibility.Hidden;
        }

        private void formReload()
        {
            tlbSave.Visibility = Visibility.Hidden;
        }
     }
}
