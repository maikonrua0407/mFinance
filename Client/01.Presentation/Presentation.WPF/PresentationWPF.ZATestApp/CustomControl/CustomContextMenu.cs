using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Windows.Media.Imaging;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.Common;
using System.Windows.Input;
using Utilities.Common;
using PresentationWPF.CustomControl;

namespace PresentationWPF.ZATestApp.CustomControl
{
    public class CustomContextMenu
    {
        public void CreateContextMenu(ref ContextMenu contextMenu, string keyFunction)
        {
            string pathFolderImages = ApplicationConstant.defaultImageSource;
            var helpItem = new MenuItem
            {
                Header = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.Help")
                ,
                Name = "cmtHelp"
                ,
                Icon = new Image { Source = new BitmapImage(new Uri(pathFolderImages + "Action/help.png", UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
            };
            contextMenu.Items.Add(helpItem);

            List<TinhNangDto> listTN = new UserControlRibbonMenu().LayDsTinhNangTheoChucNang(keyFunction);
            foreach (TinhNangDto tn in listTN)
            {
                var itemMenu = new MenuItem
                {
                    Header = "Viết hàm lấy mã ngôn ngữ theo mã tính năng" //Utilities.Common.LLanguage.SearchResourceByKey(tn.MaNgonNgu)
                    ,
                    Name = "cmt" + tn.MaTinhNang
                    //,Icon = new Image { Source = new BitmapImage(new Uri("Utilities.Common;component/myicon.png")) }
                };
                contextMenu.Items.Add(itemMenu);
            }

            var cutItem = new MenuItem
            {
                Header = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.Cut")
                ,
                Name = "cmtCut"
                ,
                Icon = new Image { Source = new BitmapImage(new Uri(pathFolderImages + "Action/help.png", UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
            };
            contextMenu.Items.Add(cutItem);

            var copyItem = new MenuItem
            {
                Header = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.Copy")
                ,
                Name = "cmtCopy"
                ,
                Icon = new Image { Source = new BitmapImage(new Uri(pathFolderImages + "Action/help.png", UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
            };
            contextMenu.Items.Add(copyItem);

            var patseItem = new MenuItem
            {
                Header = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.Paste")
                ,
                Name = "cmtPaste"
                ,
                Icon = new Image { Source = new BitmapImage(new Uri(pathFolderImages + "Action/help.png", UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
            };
            contextMenu.Items.Add(patseItem);

            var closeItem = new MenuItem
            {
                Header = Utilities.Common.LLanguage.SearchResourceByKey("ACTION.Close")
                ,
                Name = "cmtClose"
                ,
                Icon = new Image { Source = new BitmapImage(new Uri(pathFolderImages + "Action/close.png", UriKind.RelativeOrAbsolute)), Width = 16, Height = 16 }
            };
            contextMenu.Items.Add(closeItem);
        }
    }
}
