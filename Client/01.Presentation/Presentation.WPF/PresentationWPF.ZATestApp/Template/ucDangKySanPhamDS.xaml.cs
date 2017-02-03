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
using Microsoft.Windows.Controls.Ribbon;

namespace PresentationWPF.ZATestApp.Template
{
    /// <summary>
    /// Interaction logic for TemplateDanhSach.xaml
    /// </summary>
    public partial class TemplateDanhSach : UserControl
    {
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public TemplateDanhSach()
        {
            InitializeComponent();
        }
        #region Đăng ký Command
        private void BindShortkey()
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
                            if (btl.Name.Equals("tlbAdd"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.AddCommand, keyg);
                                key.Gesture = keyg;
                            }
                            else if (btl.Name.Equals("tlbModify"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.ModifyCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbDelete"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.None);
                                key = new KeyBinding(TemplateDanhSach.DeleteCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbApprove"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.ApproveCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbRefuse"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.RefuseCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbCancel"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.CancelCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbSearch"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.SearchCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbExport"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control);
                                key = new KeyBinding(TemplateDanhSach.ExportCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbClose"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                                key = new KeyBinding(TemplateDanhSach.CloseCommand, keyg);
                            }
                            InputBindings.Add(key);
                        }
                    }
                }
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton btl = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    if (btl.Name.Equals("tlbAdd"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (btl.Name.Equals("tlbModify"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.ModifyCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbDelete"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.None);
                        key = new KeyBinding(TemplateDanhSach.DeleteCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbApprove"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.ApproveCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbRefuse"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.RefuseCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbCancel"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.CancelCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbSearch"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.SearchCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbExport"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control);
                        key = new KeyBinding(TemplateDanhSach.ExportCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbClose"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(TemplateDanhSach.CloseCommand, keyg);
                    }
                    InputBindings.Add(key);
                }
            }
        }
        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbExport.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (raddgrKhachHangDS.SelectedItems.Count > 0)
            {
                tlbClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }

        #endregion

        private void tlbShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            MessageBox.Show(tlb.Label + " OK");
        }
    }
}
