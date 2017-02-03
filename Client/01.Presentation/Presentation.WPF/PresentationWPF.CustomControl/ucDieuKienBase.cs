using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows;
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    public class ucDieuKienBase : UserControl
    {
        public RoutedCommand ViewReportCommand = new RoutedCommand();
        public WrapPanel Toolbar;
        public RibbonButton tlbViewReport = new RibbonButton();

        public ucDieuKienBase()
        {
            tlbViewReport.Label = "View";
        }

        public void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(StackPanel))
                {
                    foreach (var subchild in ((StackPanel)child).Children)
                    {
                        if (subchild.GetType() == typeof(RibbonButton))
                        {
                            RibbonButton btl = (RibbonButton)subchild;
                            KeyBinding key = new KeyBinding();
                            if (btl.Name.Equals("tlbViewReport"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                                key = new KeyBinding(this.ViewReportCommand, keyg);
                                key.Gesture = keyg;
                            }
                            InputBindings.Add(key);
                        }
                    }
                }
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton btl = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    if (btl.Name.Equals("tlbViewReport"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(this.ViewReportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    InputBindings.Add(key);
                }
            }
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals("ViewReport"))
            {
                
            }
        }

        public void tlbShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            MessageBox.Show(tlb.Label + " OK");
        }

        public void ViewReportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void ViewReportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbViewReport.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
