using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Windows.Controls.Ribbon;

namespace PresentationWPF.CustomControl
{
    public class FormDetailBase : UserControl
    {
        public FormDetailBase()
        {
            tlbAdd.Label = "XXXX";
            tlbModify.Label = "YYYY";
            tlbDelete.Label = "ZZZZ";

            tlbAdd.ToolTip = "XXXX Tooltip";
            tlbModify.Visibility = Visibility.Hidden;
            tlbDelete.Visibility = Visibility.Collapsed;

        }

        public RoutedCommand AddCommand = new RoutedCommand();
        public RoutedCommand ModifyCommand = new RoutedCommand();
        public RoutedCommand DeleteCommand = new RoutedCommand();
        public RoutedCommand ApproveCommand = new RoutedCommand();
        public RoutedCommand RefuseCommand = new RoutedCommand();
        public RoutedCommand CancelCommand = new RoutedCommand();
        public RoutedCommand SearchCommand = new RoutedCommand();
        public RoutedCommand PreviewCommand = new RoutedCommand();
        public RoutedCommand ExportCommand = new RoutedCommand();
        public RoutedCommand CloseCommand = new RoutedCommand();

        public WrapPanel Toolbar;

        public RibbonButton tlbAdd = new RibbonButton();
        public RibbonButton tlbModify = new RibbonButton();
        public RibbonButton tlbDelete = new RibbonButton();
        public RibbonButton tlbApprove = new RibbonButton();
        public RibbonButton tlbRefuse = new RibbonButton();
        public RibbonButton tlbCancel = new RibbonButton();
        public RibbonButton tlbSearch = new RibbonButton();
        public RibbonButton tlbPreview = new RibbonButton();
        public RibbonButton tlbExport = new RibbonButton();
        public RibbonButton tlbClose = new RibbonButton();

        #region Đăng ký Command
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
                            if (btl.Name.Equals("tlbAdd"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                                key = new KeyBinding(this.AddCommand, keyg);
                                key.Gesture = keyg;
                            }
                            else if (btl.Name.Equals("tlbModify"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                                key = new KeyBinding(this.ModifyCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbDelete"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.None);
                                key = new KeyBinding(this.DeleteCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbApprove"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                                key = new KeyBinding(this.ApproveCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbRefuse"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control);
                                key = new KeyBinding(this.RefuseCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbCancel"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
                                key = new KeyBinding(this.CancelCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbSearch"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                                key = new KeyBinding(this.SearchCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbPreview"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                                key = new KeyBinding(this.PreviewCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbExport"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control);
                                key = new KeyBinding(this.ExportCommand, keyg);
                            }
                            else if (btl.Name.Equals("tlbClose"))
                            {
                                KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                                key = new KeyBinding(this.CloseCommand, keyg);
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
                        key = new KeyBinding(this.AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (btl.Name.Equals("tlbModify"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(this.ModifyCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbRemove"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.None);
                        key = new KeyBinding(this.DeleteCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbApprove"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(this.ApproveCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbRefuse"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control);
                        key = new KeyBinding(this.RefuseCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbCancel"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
                        key = new KeyBinding(this.CancelCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbSearch"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(this.SearchCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbPreview"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(this.PreviewCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbExport"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control);
                        key = new KeyBinding(this.ExportCommand, keyg);
                    }
                    else if (btl.Name.Equals("tlbClose"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(this.CloseCommand, keyg);
                    }
                    InputBindings.Add(key);
                }
            }
        }
        public void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        }

        public void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbPreview.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbExport.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        #endregion

        public void tlbShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            MessageBox.Show(tlb.Label + " OK");
        }
    }
}
