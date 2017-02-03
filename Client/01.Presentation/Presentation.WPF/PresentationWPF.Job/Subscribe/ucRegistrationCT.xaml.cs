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
using Presentation.Process.JobServiceRef;
using Utilities.Common;
using Presentation.Process;
using PresentationWPF.CustomControl;

namespace PresentationWPF.Job.Subscribe
{
    /// <summary>
    /// Interaction logic for ucRegistrationCT.xaml
    /// </summary>
    public partial class ucRegistrationCT : UserControl
    {
        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string loaiDoiTuong;
        public string LOAIDOITUONG
        {
            get { return loaiDoiTuong; }
            set { loaiDoiTuong = value; }
        }

        public ucRegistrationCT()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    SYS_JOB_SUBSCRIBE obj = new SYS_JOB_SUBSCRIBE();
                    List<SYS_JOB_SUBSCRIBE> lstDoiTuong = new List<SYS_JOB_SUBSCRIBE>();
                    GetFormData(ref obj);

                    JobProcess process = new JobProcess();
                    bool ret = false;
                    string sMessage = "";

                    ret = process.SysJobSubscribe(DatabaseConstant.Action.CAU_HINH, ref loaiDoiTuong, ref obj, ref lstDoiTuong, ref sMessage);

                    if (ret)
                    {
                        LMessage.ShowMessage("Insert/Update successfull", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        LMessage.ShowMessage("Insert/Update failed", LMessage.MessageBoxType.Warning);
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void GetFormData(ref SYS_JOB_SUBSCRIBE obj)
        {
            try
            {
                obj.ID = id;
                obj.SUB_NAME = txtName.Text;
                obj.DESCRIPTION = txtDescription.Text;
                obj.SUB_EMAIL = txtEmail.Text;
                string status = chkStatusDeactive.IsChecked == true ? "DEACTIVE" : "ACTIVE";
                obj.SUB_STATUS = status;
                obj.JOB_CODE = loaiDoiTuong;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            try
            {
                SYS_JOB_SUBSCRIBE obj = new SYS_JOB_SUBSCRIBE();
                List<SYS_JOB_SUBSCRIBE> lstDoiTuong = new List<SYS_JOB_SUBSCRIBE>();
                obj.ID = id;

                JobProcess process = new JobProcess();
                bool ret = false;
                string sMessage = "";

                ret = process.SysJobSubscribe(DatabaseConstant.Action.LOAD, ref loaiDoiTuong, ref obj, ref lstDoiTuong, ref sMessage);

                if (obj != null)
                {
                    txtName.Text = obj.SUB_NAME;
                    txtDescription.Text = obj.DESCRIPTION;
                    txtEmail.Text = obj.SUB_EMAIL;
                    bool status = obj.SUB_STATUS.Equals("DEACTIVE") ? true : false;
                    chkStatusDeactive.IsChecked = status;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void beforeModifyFromList()
        {
            SetFormData();
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                LMessage.ShowMessage("Missing Name value", LMessage.MessageBoxType.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                LMessage.ShowMessage("Missing EmailAddress value", LMessage.MessageBoxType.Warning);
                txtEmail.Focus();
                return false;
            }            

            return true;
        }

        private void chkStatusDeactive_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkStatusDeactive_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
