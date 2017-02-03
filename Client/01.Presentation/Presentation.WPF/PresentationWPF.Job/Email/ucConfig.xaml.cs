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
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.JobServiceRef;
using Telerik.Windows.Controls;

namespace PresentationWPF.Job.Email
{
    /// <summary>
    /// Interaction logic for ucConfig.xaml
    /// </summary>
    public partial class ucConfig : UserControl
    {
        public ucConfig()
        {
            InitializeComponent();
            SetFormData();
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
                    SYS_JOB_EMAIL obj = new SYS_JOB_EMAIL();
                    GetFormData(ref obj);                   

                    JobProcess process = new JobProcess();
                    bool ret = false;
                    string sMessage = "";

                    ret = process.SysJobEmail(DatabaseConstant.Action.CAU_HINH, ref obj, ref sMessage);

                    if (ret)
                    {
                        LMessage.ShowMessage("Update successful", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        LMessage.ShowMessage("Update failed", LMessage.MessageBoxType.Warning);
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

        private void GetFormData(ref SYS_JOB_EMAIL obj)
        {
            try
            {
                obj.ID = 1;
                obj.NAME = txtName.Text;
                obj.DESCRIPTION = txtDescription.Text;
                obj.MAIL_FROM = txtUserName.Text;
                obj.MAIL_HOST_ADDRESS = txtHostAddress.Text;
                string pro = ((RadComboBoxItem)cboProtocol.SelectedItem).Tag.ToString();
                obj.MAIL_HOST_PROTOCOL = pro;
                obj.MAIL_HOST_SMTP_PORT = txtPort.Text;
                obj.MAIL_HOST_TIMEOUT = 1000;
                obj.MAIL_USERNAME = txtUserName.Text;
                obj.MAIL_PASSWORD = txtPassWord.Password;
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
                SYS_JOB_EMAIL obj = new SYS_JOB_EMAIL();
                obj.ID = 1;

                JobProcess process = new JobProcess();
                bool ret = false;
                string sMessage = "";

                ret = process.SysJobEmail(DatabaseConstant.Action.LAY_LAI, ref obj, ref sMessage);

                if (obj != null)
                {
                    txtName.Text = obj.NAME;
                    txtDescription.Text = obj.DESCRIPTION;
                    txtHostAddress.Text = obj.MAIL_FROM;
                    txtHostAddress.Text = obj.MAIL_HOST_ADDRESS;
                    txtPort.Text = obj.MAIL_HOST_SMTP_PORT;
                    txtUserName.Text = obj.MAIL_USERNAME;
                    txtPassWord.Password = obj.MAIL_PASSWORD;
                    string pro = obj.MAIL_HOST_PROTOCOL;
                    if (!pro.IsNullOrEmpty())
                    {
                        if (pro.Equals("SMTP")) cboProtocol.SelectedIndex = 0;
                        if (pro.Equals("SMTPS")) cboProtocol.SelectedIndex = 1;
                        else cboProtocol.SelectedIndex = 0;
                    }
                    else
                    {
                        cboProtocol.SelectedIndex = 0;
                    }
                }                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                LMessage.ShowMessage("Missing Name value", LMessage.MessageBoxType.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtHostAddress.Text))
            {
                LMessage.ShowMessage("Missing HostAddress value", LMessage.MessageBoxType.Warning);
                txtHostAddress.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPort.Text))
            {
                LMessage.ShowMessage("Missing Port value", LMessage.MessageBoxType.Warning);
                txtPort.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                LMessage.ShowMessage("Missing UserName value", LMessage.MessageBoxType.Warning);
                txtUserName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPassWord.Password))
            {
                LMessage.ShowMessage("Missing PassWord value", LMessage.MessageBoxType.Warning);
                txtPassWord.Focus();
                return false;
            }

            return true;
        }
    }
}
