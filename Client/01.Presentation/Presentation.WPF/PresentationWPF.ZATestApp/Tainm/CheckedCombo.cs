using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace PresentationWPF.ZATestApp.Tainm
{
    public class CheckedCombo
    {
        public class MyDataItem : INotifyPropertyChanged
        {
            //bool used to know if we access to every checkbox
            //if it's enabled, we don't process the property changed of IsSelected
            //see setter of IsSelected property
            static private List<bool> loadsAllNeeded = new List<bool>();

            private int indexLoadNeeded;

            //getter
            static public bool getloadAllNeeded(int index)
            {
                return loadsAllNeeded[index];
            }

            //setter
            static public void setloadAllNeeded(bool value, int index)
            {
                loadsAllNeeded[index] = value;
            }

            //constructor
            public MyDataItem(String val, String txt, bool selected, int index)
            {
                this.value = val;
                this.text = txt;
                this.selected = selected;
                this.indexLoadNeeded = index;

                if (index <= loadsAllNeeded.Count)
                {
                    loadsAllNeeded.Add(true);
                }
            }

            //default constructor
            public MyDataItem()
            {
                this.value = "All";
                this.text = "All";
                this.selected = true;
                this.indexLoadNeeded = 0;
            }

            /// <summary>   
            ///     Called when the value of a property changes.   
            /// </summary>   
            /// <param name="propertyName">The name of the property that has changed.</param>   
            protected virtual void OnPropertyChanged(String propertyName)
            {
                if (String.IsNullOrEmpty(propertyName))
                {
                    return;
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            /// <summary>   
            ///     Raised when the value of one of the properties changes.   
            /// </summary>   
            public event PropertyChangedEventHandler PropertyChanged;





            private string text;
            /// <summary>   
            /// Gets or sets Text.   
            /// </summary>   
            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    if (this.text != value)
                    {
                        this.text = value;
                        OnPropertyChanged("Text");
                    }
                }
            }

            private string value = "";
            /// <summary>   
            /// Gets or sets Text.   
            /// </summary>   
            public string Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        OnPropertyChanged("Value");
                    }
                }
            }

            private bool selected;
            /// <summary>   
            /// Gets or sets IsSelected.   
            /// </summary>   
            public bool IsSelected
            {
                get
                {

                    return this.selected;
                }
                set
                {
                    if (this.selected != value)
                    {
                        this.selected = value;

                        //if we changed the all checked item (first item)
                        if (this.value.Equals("All") && getloadAllNeeded(indexLoadNeeded))
                            OnPropertyChanged("IsSelectedAll");
                        else
                            OnPropertyChanged("IsSelected");
                    }
                }
            }


        }

        public class ComboBoxSource : ObservableCollection<MyDataItem>
        {

            private static int indexBoolStat = 0;
            private string text;
            private List<string> value;


            /// <summary>   
            /// Gets or sets SelectedItemsText.   
            /// </summary>   
            /// 
            public ComboBoxSource()
            {
                this.indexBool = indexBoolStat;
                indexBoolStat++;

            }

            private int indexBool;
            public int IndexBool
            {
                get
                {
                    return this.indexBool;
                }
                set
                {
                    if (this.indexBool != value)
                    {
                        this.indexBool = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("indexBool"));
                    }
                }
            }


            public string SelectedItemsText
            {
                get
                {
                    return this.text;
                }
                set
                {
                    if (this.text != value)
                    {
                        this.text = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemsText"));
                    }
                }
            }


            public List<string> SelectedItemsValue
            {
                get
                {
                    return this.value;
                }
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemsValue"));
                    }
                }
            }



            protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems != null)
                {
                    foreach (MyDataItem item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(OnItemPropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (MyDataItem item in e.OldItems)
                    {
                        item.PropertyChanged -= new PropertyChangedEventHandler(OnItemPropertyChanged);
                    }
                }
                base.OnCollectionChanged(e);
                this.UpdateSelectedText();
            }

            public void UpdateSelectedText()
            {
                int itemsCount = this.Items.Count;
                StringBuilder sb = new StringBuilder();
                StringBuilder sbValue = new StringBuilder();
                this.SelectedItemsValue = new List<string>();

                for (int i = 1; i < itemsCount; i++)
                {
                    MyDataItem item = this.Items[i];
                    if (item.IsSelected)
                    {
                        sb.AppendFormat("{0}, ", item.Text);
                        SelectedItemsValue.Add(item.Value);
                    }
                }

                if (sb.Length > 2)
                {
                    sb.Remove(sb.Length - 2, 2);
                }

                this.SelectedItemsText = sb.ToString();


            }


            //function used to update the content of the combobox source
            private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                //this case means the user changed the all item (first item)
                if (e.PropertyName == "IsSelectedAll")
                {
                    this.UpdateAll();
                }
                else
                    if (e.PropertyName == "IsSelected")
                    {
                        this.UpdateSelectedText();
                        this.checkAll();
                    }
            }


            //function used to verify, if all the other items are checked or not
            //if yes the all item(first item) will be set to the checked state
            //otherwise the state will be unchecked
            public void checkAll()
            {

                //in order to avoid the behavior the property changed of the all item (first item),
                // if we change its state
                MyDataItem.setloadAllNeeded(false, indexBool);
                int itemsCount = this.Items.Count;

                bool allChecked = true;
                for (int i = 1; i < itemsCount && allChecked; i++)
                {
                    MyDataItem item = this.Items[i];
                    if (!item.IsSelected)
                    {
                        allChecked = false;
                    }
                }
                if (allChecked && !this.Items[0].IsSelected) this.Items[0].IsSelected = true;
                else
                    if (!allChecked && this.Items[0].IsSelected) this.Items[0].IsSelected = false;

                MyDataItem.setloadAllNeeded(true, indexBool);
            }



            //function used to check or uncheck all the other item
            //when the user checked or unchecked the all item (first item)
            public void UpdateAll()
            {

                //in order to avoid the behavior the property changed of the other items,
                // if we change its state
                MyDataItem.setloadAllNeeded(false, indexBool);
                int itemsCount = this.Items.Count;

                if (this.Items[0].IsSelected)
                {
                    for (int i = 1; i < itemsCount; i++)
                    {
                        MyDataItem item = this.Items[i];
                        this.Items[i].IsSelected = true;
                    }

                }
                else
                {
                    for (int i = 1; i < itemsCount; i++)
                    {
                        MyDataItem item = this.Items[i];
                        this.Items[i].IsSelected = false;
                    }
                }
                this.UpdateSelectedText();
                MyDataItem.setloadAllNeeded(true, indexBool);
            }


        }
    }
}
