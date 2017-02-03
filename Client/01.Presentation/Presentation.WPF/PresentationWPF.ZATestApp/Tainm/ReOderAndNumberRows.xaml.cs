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
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.CustomControl;

namespace PresentationWPF.ZATestApp.Tainm
{
    /// <summary>
    /// Interaction logic for ReOderAndNumberRows.xaml
    /// </summary>
    public partial class ReOderAndNumberRows : UserControl
    {
        public class DATAVALUE
        {
            int _sID;

            public int SID
            {
                get { return _sID; }
                set { _sID = value; }
            }
            string _sTen;

            public string STen
            {
                get { return _sTen; }
                set { _sTen = value; }
            }
            int _iNhom;

            public int INhom
            {
                get { return _iNhom; }
                set { _iNhom = value; }
            }

            string _sNhom;

            public string SNhom
            {
                get { return _sNhom; }
                set { _sNhom = value; }
            }

            decimal _dSoTien1;

            public decimal DSoTien1
            {
                get { return _dSoTien1; }
                set { _dSoTien1 = value; }
            }

            decimal _dSoTien2;

            public decimal DSoTien2
            {
                get { return _dSoTien2; }
                set { _dSoTien2 = value; }
            }
            public class CLASSCHILD
            {
                int _sID;

                public int SID
                {
                    get { return _sID; }
                    set { _sID = value; }
                }
                string sTen;

                public string STen
                {
                    get { return sTen; }
                    set { sTen = value; }
                }
            }

            string _ngayThang;

            public string NgayThang
            {
                get { return _ngayThang; }
                set { _ngayThang = value; }
            }

            CLASSCHILD _objClasschild;

            public CLASSCHILD ObjClasschild
            {
                get { return _objClasschild; }
                set { _objClasschild = value; }
            }
        }
        public class LSTDATAVALUE
        {
            DATAVALUE[] _lstDataValue;

            public DATAVALUE[] LstDataValue
            {
                get { return _lstDataValue; }
                set { _lstDataValue = value; }
            }
        }
        public ReOderAndNumberRows()
        {
            InitializeComponent();
            LSTDATAVALUE lstDaTa = new LSTDATAVALUE();
            List<DATAVALUE> lstDataValue = new List<DATAVALUE>();
            for (int i = 0; i < 100; i++)
            {
                DATAVALUE objData = new DATAVALUE();
                objData.SID = i + 1;
                objData.STen = "Lớp cha thứ" + (i + 1);
                objData.ObjClasschild = new DATAVALUE.CLASSCHILD();
                objData.ObjClasschild.SID = i + 10;
                objData.ObjClasschild.STen = "20130302";
                objData.SNhom = "Nhom" + i % 3;
                objData.INhom = i % 3;
                objData.DSoTien1 = i / 4 * 20000;
                objData.DSoTien2 = i / 3 * 50000;
                objData.NgayThang = "10";
                lstDataValue.Add(objData);
            }
            CheckedCombo.ComboBoxSource cs = new CheckedCombo.ComboBoxSource();
            cs.Add(new CheckedCombo.MyDataItem("All", "All", true, cs.IndexBool));
            //Add other Items you want
            for (int i = 0; i < 5; i++)
            {
                cs.Add(new CheckedCombo.MyDataItem(i.ToString(), "GiaTri"+i.ToString(), true, i+1));
            }

            //DataBinding
            cmbTestIP.ItemsSource = cs;
            //dt = new DataTable();
            //dt.Columns.Add("STT", typeof(int));
            //dt.Columns.Add("GTRI_TTIN", typeof(string));
            //dt.Columns.Add("HTHI_DIEU_KHIEN", typeof(string));
            //dt.Columns.Add("HTHI_SQL", typeof(string));
            //dt.Columns.Add("Discontinued", typeof(string));
            //string sControl = "Combobox";
            
            //for (int i = 0; i < 10; i++)
            //{
            //    string sGiaTriTT = "";
            //    if (sControl.Equals("Combobox"))
            //        sGiaTriTT = "001";
            //    else
            //        sGiaTriTT = DateTime.Now.ToString("yyyyMMdd");
            //    dt.NewRow();
            //    dt.Rows.Add(new object[] { i, sGiaTriTT, sControl, "","" });
            //    if (sControl.Equals("Combobox"))
            //        sControl = "Datetime";
            //    else if (sControl.Equals("Datetime"))
            //        sControl = "Textbox";
            //    else
            //        sControl = "Combobox";
            //}
            lstDaTa.LstDataValue = lstDataValue.ToArray();
            pager.PageSize = (int)nudPageSize.Value;
            GridView.ItemsSource = null;
            GridView.ItemsSource = lstDaTa.LstDataValue;
            GridView.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(GridView_CellValidating);
            GridView.IsReadOnly = false;
            ucclThang1.SelectedDatesGridViewChanged += new SelectedDatesChangedGridViewEventHandler(ucclThang1_SelectedDatesGridViewChanged);
            ucclThang1.DisplayDate = DateTime.Now.AddYears(1);
        }

        void ucclThang1_SelectedDatesGridViewChanged(object sender, SelectedDatesChangedGridViewEventArgs e)
        {
            List<DateTime> lstDate = e.NewDates.ToList();
            GridViewRowItem grv = e.Cell.ParentRow;
            DATAVALUE data = grv.Item as DATAVALUE;
            data.NgayThang = ucclThang1.ValuesString;
            GridView.CurrentItem = data;
        }

        void GridView_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName != null && e.Cell.Column.UniqueName.Equals("ObjClasschild.STen"))
            {
                e.IsValid = true;
                if (e.NewValue.Equals("11"))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Lỗi to như quả bóng";
                }
                if (e.NewValue.Equals("22"))
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Lỗi to như quả khí cầu";
                }
            }
        }
        static DataTable dt;
        static bool isLoaded = false;
        private void nudRowNum_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (dt != null)
                {
                    pager.PageSize = (int)nudPageSize.Value;
                    GridView.ItemsSource = dt.DefaultView;
                    
                }
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            DATAVALUE[] lstDataValue = (DATAVALUE[])GridView.ItemsSource;
            lstDataValue.ToList();
        }

        private void BindDataTypeCell()
        {
            
        }

        private void GridView_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
            if (col.DisplayDate < DateTime.Now)
                e.Cancel = true;
        }

        private void GridView_PreparingCellForEdit(object sender, GridViewPreparingCellForEditEventArgs e)
        {
            //var tb = e.EditingElement as RadComboBox;
        }

        private void cmbTestIP_DropDownClosed(object sender, EventArgs e)
        {
            CheckedCombo.ComboBoxSource cs = (CheckedCombo.ComboBoxSource)cmbTestIP.ItemsSource;
        }
    }
}
