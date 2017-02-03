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
using System.Runtime.Serialization;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.CustomControl;

namespace PresentationWPF.ZATestApp.CustomControl
{
    /// <summary>
    /// Interaction logic for ucMenuBar.xaml
    /// </summary>
    public partial class ucMenuBar : UserControl
    {
        public static string assem;
        public ucMenuBar()
        {
            InitializeComponent();
            hienThiTinhNang();
        }
        void hienThiTinhNang()
        {
            assem = ClientInformation.ChucNangVuaChon;
            if (!string.IsNullOrEmpty(assem))
            {
                List<TinhNangDto> listTN = new UserControlRibbonMenu().LayDsTinhNangTheoChucNang(assem);
                bool isAdd = false, isEdit = false, isRemove = false, isApproved = false, isReject = false, isInApproved = false, isSearch = false, isExport = false;
                foreach (TinhNangDto tn in listTN)
                {
                    if (tn.MaTinhNang.Equals("Add"))
                        isAdd = true;
                    if (tn.MaTinhNang.Equals("Edit"))
                        isEdit = true;
                    if (tn.MaTinhNang.Equals("Remove"))
                        isRemove = true;
                    if (tn.MaTinhNang.Equals("Approved"))
                        isApproved = true;
                    if (tn.MaTinhNang.Equals("Reject"))
                        isReject = true;
                    if (tn.MaTinhNang.Equals("InApproved"))
                        isInApproved = true;
                    if (tn.MaTinhNang.Equals("Search"))
                        isSearch = true;
                    if (tn.MaTinhNang.Equals("Export"))
                        isExport = true;
                }
                //btnAdd.IsEnabled = isAdd;
                //btnEdit.IsEnabled = isEdit;
                //btnRemove.IsEnabled = isRemove;
                //btnApproved.IsEnabled = isApproved;
                //btnReject.IsEnabled = isReject;
                //btnInApproved.IsEnabled = isInApproved;
                //btnSearch.IsEnabled = isSearch;
                //btnExport.IsEnabled = isExport;
            }
        }
    }
}
