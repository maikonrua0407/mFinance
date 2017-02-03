using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PresentationWPF.CustomControl
{
    public class UserControlRibbonMenu
    {

        /// <summary>
        /// Dựng menu ribbon của hệ thống theo user đăng nhập
        /// </summary>
        /// <param name="rbMenu">Control Menu ribbon được tham chiếu, xử lý rồi trả về</param>
        public void KhoiTaoMenu(ref Ribbon rbMenu)
        {
            try
            {
                string toolTip;
                ChucNangDto[] ChucNanglst = ClientInformation.ListChucNang.Select(e => e).ToArray();
                IQueryable<ChucNangDto> sItem;
                string pathFolderImages = ApplicationConstant.defaultImageSource;
                // Lấy danh sách menu theo tên đăng nhập và mã đơn vị
                var sTab = ChucNanglst.Where(e => e.IDChucNangCha == 0).OrderBy(e => e.STT).ToList();
                foreach (var Tab in sTab)
                {
                    if (Tab.ThuocTinh.SplitByDelimiter("#")[2] == "RibbonApplicationMenu")
                    {   // Application menu
                        RibbonApplicationMenu ram = new RibbonApplicationMenu();
                        ram.Label = Utilities.Common.LLanguage.SearchResourceByKey(Tab.TieuDe);
                        if (!Tab.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Tab.ThuocTinh.SplitByDelimiter("#")[0];
                        else toolTip = Tab.TieuDe;
                        ram.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                        ram.KeyTip = Tab.ThuocTinh.SplitByDelimiter("#")[3];
                        ram.Uid = Tab.IDChucNang.ToString();
                        if (!Tab.BieuTuong.IsNullOrEmptyOrSpace())
                        {
                            try
                            {
                                BitmapImage bmp = new BitmapImage();
                                if (Tab.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                {
                                    bmp.BeginInit();
                                    bmp.UriSource = new Uri(pathFolderImages + Tab.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                    bmp.EndInit();
                                    // RibbonMenuItem icon always small
                                    ram.SmallImageSource = bmp;
                                }
                                else
                                {   // Lagre or Small
                                    bmp.BeginInit();
                                    bmp.UriSource = new Uri(pathFolderImages + Tab.BieuTuong, UriKind.RelativeOrAbsolute);
                                    bmp.EndInit();
                                    ram.SmallImageSource = bmp;
                                }
                            }
                            catch { }
                        }
                        var sMenu = ChucNanglst.Where(e => e.IDChucNangCha == Tab.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                        foreach (var Menu in sMenu)
                        {
                            switch (Menu.ThuocTinh.SplitByDelimiter("#")[2])
                            {
                                case "RibbonApplicationSplitMenuItem":
                                    if (Menu.Quyen == 0) goto case "RibbonApplicationMenuItem";
                                    var rasmi = new RibbonApplicationSplitMenuItem();
                                    rasmi.Name = "ID" + Menu.IDChucNang.ToString();
                                    rasmi.Header = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                    if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                    else toolTip = Menu.TieuDe;
                                    rasmi.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                    rasmi.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                    rasmi.Uid = Menu.IDChucNang.ToString();
                                    if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                    {
                                        try
                                        {
                                            BitmapImage bmp = new BitmapImage();
                                            if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                            {
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                // RibbonApplicationSplitMenuItem icon always large
                                                rasmi.ImageSource = bmp;

                                                bmp = new BitmapImage();
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                // QuickAccessToolBar icon always small
                                                rasmi.QuickAccessToolBarImageSource = bmp;
                                            }
                                            else
                                            {   // Lagre or Small
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                rasmi.ImageSource = bmp;
                                                rasmi.QuickAccessToolBarImageSource = bmp;
                                            }
                                        }
                                        catch { }
                                    }
                                    rasmi.Tag = Menu;
                                    //rasmi.IsEnabled = Menu.Quyen > 0;
                                    // Items
                                    sItem = ChucNanglst.Where(e => e.IDChucNangCha == Menu.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                                    foreach (var Item in sItem)
                                    {
                                        var item = new RibbonApplicationMenuItem();
                                        item.Name = "ID" + Item.IDChucNang.ToString();
                                        item.Header = Utilities.Common.LLanguage.SearchResourceByKey(Item.TieuDe);
                                        if (!Item.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Item.ThuocTinh.SplitByDelimiter("#")[0];
                                        else toolTip = Item.TieuDe;
                                        item.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                        item.KeyTip = Item.ThuocTinh.SplitByDelimiter("#")[3];
                                        item.Uid = Item.IDChucNang.ToString();
                                        if (!Item.BieuTuong.IsNullOrEmptyOrSpace())
                                        {
                                            try
                                            {
                                                BitmapImage bmp = new BitmapImage();
                                                if (Item.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                {
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    // RibbonApplicationMenuItem icon always large
                                                    item.ImageSource = bmp;

                                                    bmp = new BitmapImage();
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    // QuickAccessToolBar icon always small
                                                    item.QuickAccessToolBarImageSource = bmp;
                                                }
                                                else
                                                {   // Lagre or Small
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong, UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    item.ImageSource = bmp;
                                                    item.QuickAccessToolBarImageSource = bmp;
                                                }
                                            }
                                            catch { }
                                        }
                                        item.Tag = Item;
                                        item.IsEnabled = Item.Quyen > 0;
                                        rasmi.Items.Add(item);
                                    }
                                    ram.Items.Add(rasmi);
                                    break;
                                case "RibbonApplicationMenuItem":
                                    var rami = new RibbonApplicationMenuItem();
                                    rami.Name = "ID" + Menu.IDChucNang.ToString();
                                    rami.Header = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                    if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                    else toolTip = Menu.TieuDe;
                                    rami.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                    rami.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                    rami.Uid = Menu.IDChucNang.ToString();
                                    if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                    {
                                        try
                                        {
                                            BitmapImage bmp = new BitmapImage();
                                            if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                            {
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                // RibbonApplicationMenuItem icon always large
                                                rami.ImageSource = bmp;

                                                bmp = new BitmapImage();
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                // QuickAccessToolBar icon always small
                                                rami.QuickAccessToolBarImageSource = bmp;
                                            }
                                            else
                                            {   // Lagre or Small
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                rami.ImageSource = bmp;
                                                rami.QuickAccessToolBarImageSource = bmp;
                                            }
                                        }
                                        catch { }
                                    }
                                    if (!Menu.ChucNang.IsNullOrEmptyOrSpace()) rami.Tag = Menu;
                                    //rami.IsEnabled = Menu.Quyen > 0;
                                    // Items
                                    sItem = ChucNanglst.Where(e => e.IDChucNangCha == Menu.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                                    foreach (var Item in sItem)
                                    {
                                        var item = new RibbonApplicationMenuItem();
                                        item.Name = "ID" + Item.IDChucNang.ToString();
                                        item.Header = Utilities.Common.LLanguage.SearchResourceByKey(Item.TieuDe);
                                        if (!Item.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Item.ThuocTinh.SplitByDelimiter("#")[0];
                                        else toolTip = Item.TieuDe;
                                        item.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                        item.KeyTip = Item.ThuocTinh.SplitByDelimiter("#")[3];
                                        item.Uid = Item.IDChucNang.ToString();
                                        if (!Item.BieuTuong.IsNullOrEmptyOrSpace())
                                        {
                                            try
                                            {
                                                BitmapImage bmp = new BitmapImage();
                                                if (Item.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                {
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    // RibbonApplicationMenuItem icon always large
                                                    item.ImageSource = bmp;

                                                    bmp = new BitmapImage();
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    // QuickAccessToolBar icon always small
                                                    item.QuickAccessToolBarImageSource = bmp;
                                                }
                                                else
                                                {   // Lagre or Small
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong, UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    item.ImageSource = bmp;
                                                    item.QuickAccessToolBarImageSource = bmp;
                                                }
                                            }
                                            catch { }
                                        }
                                        item.Tag = Item;
                                        item.IsEnabled = Item.Quyen > 0;
                                        rami.Items.Add(item);
                                    }
                                    ram.Items.Add(rami);
                                    break;
                                default:
                                    Grid grid;
                                    if (ram.FooterPaneContent.IsNullOrEmpty())
                                    {
                                        grid = new Grid();
                                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                                        ColumnDefinition cd = new ColumnDefinition();
                                        cd.Width = GridLength.Auto;
                                        grid.ColumnDefinitions.Add(cd);
                                        ram.FooterPaneContent = grid;
                                    }
                                    else grid = (Grid)(ram.FooterPaneContent);

                                    var rb = new RibbonButton();
                                    rb.Name = "ID" + Menu.IDChucNang.ToString();
                                    rb.Label = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                    if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                    else toolTip = Menu.TieuDe;
                                    rb.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                    rb.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                    rb.Uid = Menu.IDChucNang.ToString();
                                    if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                    {
                                        try
                                        {
                                            BitmapImage bmp = new BitmapImage();
                                            if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                            {
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                rb.LargeImageSource = bmp;

                                                bmp = new BitmapImage();
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                rb.SmallImageSource = bmp;
                                            }
                                            else
                                            {   // Lagre or Small
                                                bmp.BeginInit();
                                                bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                bmp.EndInit();
                                                if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Large") rb.LargeImageSource = bmp;
                                                else rb.SmallImageSource = bmp;
                                            }
                                        }
                                        catch { }
                                    }
                                    rb.Tag = Menu;
                                    rb.IsEnabled = Menu.Quyen > 0;
                                    grid.Children.Add(rb);
                                    Grid.SetColumn(rb, 1);
                                    break;
                            }

                        }
                        rbMenu.ApplicationMenu = ram;
                    }
                    else
                    {   // Tabs
                        RibbonTab tab = new RibbonTab();
                        Type rthType = new RibbonTabHeader().GetType();
                        tab.Header = Utilities.Common.LLanguage.SearchResourceByKey(Tab.TieuDe);
                        if (!Tab.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Tab.ThuocTinh.SplitByDelimiter("#")[0];
                        else toolTip = Tab.TieuDe;
                        tab.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                        tab.HeaderStyle = new Style(rthType);
                        tab.HeaderStyle.Setters.Add(new Setter(RibbonWindow.ToolTipProperty, tab.ToolTip));
                        tab.IsEnabled = Tab.Quyen > 0;
                        tab.Uid = Tab.IDChucNang.ToString();
                        // Groups
                        var sGroup = ChucNanglst.Where(e => e.IDChucNangCha == Tab.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                        foreach (var Group in sGroup)
                        {
                            RibbonGroup group = new RibbonGroup();
                            group.Header = Utilities.Common.LLanguage.SearchResourceByKey(Group.TieuDe);
                            group.IsEnabled = Group.Quyen > 0;
                            // Buttons
                            var sMenu = ChucNanglst.Where(e => e.IDChucNangCha == Group.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                            foreach (var Menu in sMenu)
                            {
                                switch (Menu.ThuocTinh.SplitByDelimiter("#")[2])
                                {
                                    case "RibbonSplitButton":
                                        if (Menu.Quyen == 0) goto case "RibbonMenuButton";
                                        var rsb = new RibbonSplitButton();
                                        rsb.Name = "ID" + Menu.IDChucNang.ToString();
                                        rsb.Label = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                        if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                        else toolTip = Menu.TieuDe;
                                        rsb.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                        rsb.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                        rsb.Uid = Menu.IDChucNang.ToString();
                                        if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                        {
                                            try
                                            {
                                                BitmapImage bmp = new BitmapImage();
                                                if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                {
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rsb.LargeImageSource = bmp;

                                                    bmp = new BitmapImage();
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rsb.SmallImageSource = bmp;
                                                }
                                                else
                                                {   // Lagre or Small
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Large") rsb.LargeImageSource = bmp;
                                                    else rsb.SmallImageSource = bmp;
                                                }
                                            }
                                            catch { }
                                        }
                                        rsb.Tag = Menu;
                                        //rsb.IsEnabled = Menu.Quyen > 0;
                                        // Items
                                        sItem = ChucNanglst.Where(e => e.IDChucNangCha == Menu.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                                        foreach (var Item in sItem)
                                        {
                                            var item = new RibbonMenuItem();
                                            item.Name = "ID" + Item.IDChucNang.ToString();
                                            item.Header = Utilities.Common.LLanguage.SearchResourceByKey(Item.TieuDe);
                                            if (!Item.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Item.ThuocTinh.SplitByDelimiter("#")[0];
                                            else toolTip = Item.TieuDe;
                                            item.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                            item.KeyTip = Item.ThuocTinh.SplitByDelimiter("#")[3];
                                            item.Uid = Item.IDChucNang.ToString();
                                            if (!Item.BieuTuong.IsNullOrEmptyOrSpace())
                                            {
                                                try
                                                {
                                                    BitmapImage bmp = new BitmapImage();
                                                    if (Item.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                    {
                                                        bmp.BeginInit();
                                                        bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                        bmp.EndInit();
                                                        // RibbonMenuItem icon always small
                                                        item.ImageSource = bmp;
                                                        // QuickAccessToolBar icon always small
                                                        item.QuickAccessToolBarImageSource = bmp;
                                                    }
                                                    else
                                                    {   // Lagre or Small
                                                        bmp.BeginInit();
                                                        bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong, UriKind.RelativeOrAbsolute);
                                                        bmp.EndInit();
                                                        item.ImageSource = bmp;
                                                        item.QuickAccessToolBarImageSource = bmp;
                                                    }
                                                }
                                                catch { }
                                            }
                                            item.Tag = Item;
                                            item.IsEnabled = Item.Quyen > 0;
                                            rsb.Items.Add(item);
                                        }
                                        group.Items.Add(rsb);
                                        break;
                                    case "RibbonMenuButton":
                                        var rmb = new RibbonMenuButton();
                                        rmb.Name = "ID" + Menu.IDChucNang.ToString();
                                        rmb.Label = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                        if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                        else toolTip = Menu.TieuDe;
                                        rmb.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                        rmb.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                        rmb.Uid = Menu.IDChucNang.ToString();
                                        if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                        {
                                            try
                                            {
                                                BitmapImage bmp = new BitmapImage();
                                                if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                {
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rmb.LargeImageSource = bmp;

                                                    bmp = new BitmapImage();
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rmb.SmallImageSource = bmp;
                                                }
                                                else
                                                {   // Lagre or Small
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Large") rmb.LargeImageSource = bmp;
                                                    else rmb.SmallImageSource = bmp;
                                                }
                                            }
                                            catch { }
                                        }
                                        //rmb.IsEnabled = Menu.Quyen > 0;
                                        // Items
                                        sItem = ChucNanglst.Where(e => e.IDChucNangCha == Menu.IDChucNang).OrderBy(e => e.STT).AsQueryable();
                                        foreach (var Item in sItem)
                                        {
                                            var item = new RibbonMenuItem();
                                            item.Name = "ID" + Item.IDChucNang.ToString();
                                            item.Header = Utilities.Common.LLanguage.SearchResourceByKey(Item.TieuDe);
                                            if (!Item.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Item.ThuocTinh.SplitByDelimiter("#")[0];
                                            else toolTip = Item.TieuDe;
                                            item.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                            item.KeyTip = Item.ThuocTinh.SplitByDelimiter("#")[3];
                                            item.Uid = Item.IDChucNang.ToString();
                                            if (!Item.BieuTuong.IsNullOrEmptyOrSpace())
                                            {
                                                try
                                                {
                                                    BitmapImage bmp = new BitmapImage();
                                                    if (Item.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                    {
                                                        bmp.BeginInit();
                                                        bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                        bmp.EndInit();
                                                        // RibbonMenuItem icon always small
                                                        item.ImageSource = bmp;
                                                        // QuickAccessToolBar icon always small
                                                        item.QuickAccessToolBarImageSource = bmp;
                                                    }
                                                    else
                                                    {   // Lagre or Small
                                                        bmp.BeginInit();
                                                        bmp.UriSource = new Uri(pathFolderImages + Item.BieuTuong, UriKind.RelativeOrAbsolute);
                                                        bmp.EndInit();
                                                        item.ImageSource = bmp;
                                                        item.QuickAccessToolBarImageSource = bmp;
                                                    }
                                                }
                                                catch { }
                                            }
                                            item.Tag = Item;
                                            item.IsEnabled = Item.Quyen > 0;
                                            rmb.Items.Add(item);
                                        }
                                        group.Items.Add(rmb);
                                        break;
                                    default:
                                        var rb = new RibbonButton();
                                        rb.Name = "ID" + Menu.IDChucNang.ToString();
                                        rb.Label = Utilities.Common.LLanguage.SearchResourceByKey(Menu.TieuDe);
                                        if (!Menu.ThuocTinh.SplitByDelimiter("#")[0].IsNullOrEmptyOrSpace()) toolTip = Menu.ThuocTinh.SplitByDelimiter("#")[0];
                                        else toolTip = Menu.TieuDe;
                                        rb.ToolTip = Utilities.Common.LLanguage.SearchResourceByKey(toolTip);
                                        rb.KeyTip = Menu.ThuocTinh.SplitByDelimiter("#")[3];
                                        rb.Uid = Menu.IDChucNang.ToString();
                                        if (!Menu.BieuTuong.IsNullOrEmptyOrSpace())
                                        {
                                            try
                                            {
                                                BitmapImage bmp = new BitmapImage();
                                                if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Both")
                                                {
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[0], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rb.LargeImageSource = bmp;

                                                    bmp = new BitmapImage();
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong.SplitByDelimiter("#")[1], UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    rb.SmallImageSource = bmp;
                                                }
                                                else
                                                {   // Lagre or Small
                                                    bmp.BeginInit();
                                                    bmp.UriSource = new Uri(pathFolderImages + Menu.BieuTuong, UriKind.RelativeOrAbsolute);
                                                    bmp.EndInit();
                                                    if (Menu.ThuocTinh.SplitByDelimiter("#")[1] == "Large") rb.LargeImageSource = bmp;
                                                    else rb.SmallImageSource = bmp;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                                            }
                                        }
                                        rb.Tag = Menu;
                                        rb.IsEnabled = Menu.Quyen > 0;
                                        group.Items.Add(rb);
                                        break;
                                }
                            }
                            tab.Items.Add(group);
                        }
                        rbMenu.Items.Add(tab);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi log Exception
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
            }
        }
        public bool GetImage(string uri, ref BitmapImage bmp)
        {
            try
            {
                bmp.BeginInit();
                bmp.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log Exception
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
            
        }

        //public List<TinhNangDto> LayDsTinhNangTheoChucNang(string keyFunction)
        //{
        //    List<TinhNangDto> lstTinhNang = new List<TinhNangDto>();
        //    ChucNangDto[] ChucNanglst = ClientInformation.ListChucNang.Where(e => true).ToArray();//e.PathFunction.Equals(keyFunction)
        //    foreach (ChucNangDto cnang in ChucNanglst)
        //    {
        //        foreach (TinhNangDto tnang in cnang.lstTinhNang)
        //        {
        //            if (lstTinhNang.Select(e => e.MaTinhNang).Contains(tnang.MaTinhNang))
        //                break;
        //            else
        //            {
        //                TinhNangDto tinhNang = new TinhNangDto();
        //                tinhNang.MaTinhNang = tnang.MaTinhNang;
        //                //tinhNang.MaNgonNgu = tnang.MaNgonNgu;
        //                lstTinhNang.Add(tinhNang);
        //            }
        //        }
        //    }
        //    return lstTinhNang;
        //}

        public List<TinhNangDto> LayDsTinhNangTheoChucNang(string keyFunction)
        {
            List<TinhNangDto> lstTinhNang = new List<TinhNangDto>();
            
            lstTinhNang = ClientInformation.ListChucNang.FirstOrDefault(e => e.ChucNang.Equals(keyFunction)).lstTinhNang.ToList();

            return lstTinhNang;
        }
    }
}
