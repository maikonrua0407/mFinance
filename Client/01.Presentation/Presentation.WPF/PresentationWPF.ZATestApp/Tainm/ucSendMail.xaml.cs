using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

namespace PresentationWPF.ZATestApp.Tainm
{
    /// <summary>
    /// Interaction logic for ucSendMail.xaml
    /// </summary>
    public partial class ucSendMail : UserControl
    {
        public ucSendMail()
        {
            InitializeComponent();
            btnSend.Click += BtnSend_Click;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MailMessage mail = new MailMessage("tainm.ngv@gmail.com", "tainm.ngv@gmail.com");
                //SmtpClient client = new SmtpClient();
                //client.Port = 465;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.EnableSsl = true;
                //client.Host = "smtp.gmail.com";
                //mail.Subject = "this is a test email.";
                //mail.Body = "this is my test email body";
                //client.Credentials = new NetworkCredential();
                //client.Send(mail);
                ManagedInstallerClass.InstallHelper(new[] { @"C:\Program Files (x86)\NGV GROUP\NGV BACKUP 2.0\NG.BACKUP.FTP.SERVICE.exe" });
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage(ex.Message,LMessage.MessageBoxType.Error);
            }
        }
    }
}
