using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.ZAMainAppServiceRef;
using Utilities.Common;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Xml.Linq;

namespace PresentationWPF.CustomControl
{
    public class HeThong
    {
        public void DuyetQuyenTinhNangToolbar(string keyFunction, ref WrapPanel panel, ref ContextMenu contextMenu)
        {
            try
            {
                //string pathFolderImages = ApplicationConstant.defaultImageSource;
                string toolTip = "";
                List<TinhNangDto> listTN = new UserControlRibbonMenu().LayDsTinhNangTheoChucNang(keyFunction);
                bool isFullControl = false;
                if (listTN.Where(e => e.MaTinhNang.Equals(DatabaseConstant.Action.TOAN_QUYEN.getValue())).Count() > 0)
                    isFullControl = true;
                foreach (var item in panel.Children)
                {
                    if (item is RibbonButton)
                    {
                        RibbonButton rb = (RibbonButton)item;
                        string strTinhNang = rb.Name.Substring(3, rb.Name.Length - 3);

                        toolTip = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.TOOLTIP." + strTinhNang);
                        rb.ToolTip = toolTip;

                        var menuItem = new MenuItem
                        {
                            Header = toolTip //Utilities.Common.LLanguage.SearchResourceByKey("ACTION." + strTinhNang)
                            ,
                            Name = "ctm" + strTinhNang
                            ,
                            Icon = new Image { Source = new BitmapImage(new Uri(rb.SmallImageSource.ToString(), UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
                            //,
                            //ToolTip = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.TOOLTIP." + strTinhNang)

                        };
                        menuItem.Visibility = rb.Visibility;
                        menuItem.IsEnabled = rb.IsEnabled;
                        contextMenu.Items.Add(menuItem);
                        if (!isFullControl)
                        {
                            if (listTN.Select(e => e.MaTinhNang).Contains(strTinhNang) != true 
                                &&
                                (
                                strTinhNang.Equals(DatabaseConstant.Action.XEM.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.THEM.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.SUA.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.XOA.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.DUYET.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.TU_CHOI_DUYET.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.THOAI_DUYET.getValue()) ||
                                strTinhNang.Equals(DatabaseConstant.Action.XU_LY.getValue())
                                )
                                )
                            {
                                rb.IsEnabled = false;
                                rb.Tag = "false";
                                menuItem.IsEnabled = false;
                                menuItem.Opacity = .5;
                            }
                        }
                    }
                    if (item is Label)
                    {
                        if (((Label)item).Visibility == Visibility.Visible)
                        {
                            Separator sp = new Separator();
                            contextMenu.Items.Add(sp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        public bool KiemTraQuyenTheoChucNang(string keyFunction, DatabaseConstant.Action quyen)
        {
            try
            {
                bool isRole = false;

                List<TinhNangDto> listTN = new UserControlRibbonMenu().LayDsTinhNangTheoChucNang(keyFunction);

                if (listTN.Where(e => e.MaTinhNang.Equals(quyen.getValue())).Count() > 0)
                    isRole = true;
                else
                    isRole = false;

                return isRole;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            return false;
        }

        public ArrayList SetVisibleControl(string formNameSpace, string strloai)
        {
            try
            {
                ArrayList lst = new ArrayList();
                List<string> lstItem = new List<string>();
                XElement xe = XElement.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\ShowConfig.xml");
                var form = xe.Elements("form").Where(a => a.Attribute("Name").Value == formNameSpace);
                var loai = form.Elements("case").Where(a => a.Attribute("Name").Value == strloai).AsQueryable();
                foreach (var item in loai.Elements("control"))
                {
                    lstItem = new List<string>();
                    lstItem.Add(item.Attribute("Name").Value);
                    lstItem.Add(item.Element("property").Attribute("Name").Value);
                    lstItem.Add(item.Element("property").Value);
                    lst.Add(lstItem);
                }
                return lst;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }
    }
}
