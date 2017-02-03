using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;
using DataServices.ZAMainApp;

namespace BusinessServices.ZAMainApp
{
    public class Login
    {
        public HT_NSD doLogin(string userName, string passWord)
        {
            DataServices.ZAMainApp.Login loginService = new DataServices.ZAMainApp.Login();
            return loginService.doLogin(userName, passWord);
        }
    }
}
