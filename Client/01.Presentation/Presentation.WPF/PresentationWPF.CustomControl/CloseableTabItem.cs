using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    public class CloseableTabItem : TabItem
    {
        static CloseableTabItem()
        {
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabItem),
                new FrameworkPropertyMetadata(typeof(CloseableTabItem)));
        }

        public static readonly RoutedEvent CloseTabEvent =
            EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CloseableTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button closeButton = base.GetTemplateChild("PART_Close") as Button;
            closeButton.ToolTip = LLanguage.SearchResourceByKey("U.DungChung.Button.Dong");
            if (closeButton != null)
                closeButton.Click += new System.Windows.RoutedEventHandler(closeButton_Click);
        }

        void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string askBeforeClose = "0";
            try
            {
                UserControl uc = (UserControl)((Window)sender).Content;
                string thuocTinh = uc.Uid;
                askBeforeClose = thuocTinh.SplitByDelimiter("#")[4];
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "CloseUserControl: " + ex);
                askBeforeClose = "0";
            }

            if (askBeforeClose.Equals("1"))
            {
                MessageBoxResult retMessage = LMessage.ShowMessage("M.DungChung.HoiDongChucNang", LMessage.MessageBoxType.Question);
                if (retMessage == MessageBoxResult.Yes)
                {
                    this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
                }
            }
            else 
            {
                this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
            }
            //this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
