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
using Utilities.Common;
using Presentation.Process;
using PresentationWPF.CustomControl;

namespace PresentationWPF.QuanTriHeThong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            calendar.DisplayDate = "01/01/2012".StringToDate();
        }

        private void mnuThoat_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void mnuTinhThanh_Click(object sender, RoutedEventArgs e)
        {
            //frmDMTinhThanh frm = new frmDMTinhThanh();
            //frm.Show();
        }

        private void mnuHeThong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = "30/12/2010".StringToDate().Lunar2Solar().DateToString();

                //if (b) MessageBox.Show("not working day");
                //else MessageBox.Show("working day");            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.TargetSite.Name + "\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            try
            {
                if (((DateTime)e.AddedDate).ToString("yyyy").StringToInt32() > 1753 && (((DateTime)e.AddedDate).ToString("yyyyMM") != ((DateTime)e.RemovedDate).ToString("yyyyMM")))
                {
                    calendar.BlackoutDates.Clear();
                    DateTime dt = (DateTime)e.AddedDate;
                    List<bool> workingDays = QuanTriHeThongProcess.IsWorkingDays(dt.GetYear(), dt.GetMonth());
                    for (int i = 0; i < workingDays.Count; i++)
                    {
                        if (!workingDays[i]) calendar.BlackoutDates.Add(new CalendarDateRange(dt));
                        dt = dt.PlusDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(LLanguage.SearchResourceByKey(ex.Message), ex.InnerException).ShowDialog();
                else
                    new frmThongBaoLoi(LLanguage.SearchResourceByKey("M"), ex).ShowDialog();
                //LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
