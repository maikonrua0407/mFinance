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
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.ZAMainApp
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        public string userName;

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    string oldPassword = LSecurity.MD5Encrypt(txtOldPassword.Password);
                    string newPassword = LSecurity.MD5Encrypt(txtNewPassword.Password);

                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    string responseMessage = "";
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

                    // Đổi mật khẩu lúc đăng nhập hay lúc đang vận hành
                    if (ClientInformation.TenDangNhap != null)
                    {
                        ret = process.DoiMatKhauNguoiDung(userName, oldPassword, newPassword, ref responseMessage);
                    }
                    else
                    {
                        ret = process.DoiMatKhauNguoiDungLucDangNhap(userName, oldPassword, newPassword, ref responseMessage);
                    }                    

                    if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.QuanTriHeThong.NguoiDung.DoiMatKhauThanhCong", LMessage.MessageBoxType.Information);
                        CustomControl.CommonFunction.CloseUserControl(this);
                    }
                    else
                    {
                        LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Warning);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtOldPassword.Password))
            {
                LMessage.ShowMessage("M.ZAMainApp.ChangePassword.ThieuMatKhauCu", LMessage.MessageBoxType.Warning);
                txtOldPassword.Focus();
                return false;
            }

            else if (string.IsNullOrEmpty(txtNewPassword.Password))
            {
                LMessage.ShowMessage("M.ZAMainApp.ChangePassword.ThieuMatKhauMoi", LMessage.MessageBoxType.Warning);
                txtOldPassword.Focus();
                return false;
            }

            else if (txtOldPassword.Password.Equals((txtNewPassword.Password)))
            {
                LMessage.ShowMessage("M.ZAMainApp.ChangePassword.MatKhauMoiPhaiKhacMatKhauCu", LMessage.MessageBoxType.Warning);
                txtOldPassword.Focus();
                return false;
            }

            else if (string.IsNullOrEmpty(txtNewPasswordConfirm.Password))
            {
                LMessage.ShowMessage("M.ZAMainApp.ChangePassword.ThieuXacNhanMatKhauMoi", LMessage.MessageBoxType.Warning);
                txtOldPassword.Focus();
                return false;
            }

            else if (!txtNewPasswordConfirm.Password.Equals((txtNewPassword.Password)))
            {
                LMessage.ShowMessage("M.ZAMainApp.ChangePassword.XacNhanMatKhauMoiKhongChinhXac", LMessage.MessageBoxType.Warning);
                txtOldPassword.Focus();
                return false;
            }

            // Kiểm tra độ dài mật khẩu
            UtilitiesProcess utilitiesProcess = new UtilitiesProcess();
            Presentation.Process.UtilitiesServiceRef.HT_TSO htTso = utilitiesProcess.LayThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_DODAI_MATKHAU, ClientInformation.MaDonViQuanLy);
            if (htTso != null)
            {
                string giaTriThamSo = htTso.GIA_TRI;
                if (giaTriThamSo != null && giaTriThamSo != "")
                {
                    if (txtNewPassword.Password.Length < Int32.Parse(giaTriThamSo))
                    {
                        LMessage.ShowMessage("Độ dài mật khẩu không hợp lệ, giá trị tối thiểu là: " + giaTriThamSo, LMessage.MessageBoxType.Warning);
                        txtNewPassword.Focus();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
