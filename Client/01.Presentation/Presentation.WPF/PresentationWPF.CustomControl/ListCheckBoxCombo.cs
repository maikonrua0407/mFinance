using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    public class AutoCompleteCheckBox : INotifyPropertyChanged
    {
        string dislayMember;

        public string DislayMember
        {
            get { return dislayMember; }
            set { dislayMember = value; }
        }
        bool checkedMember;

        public bool CheckedMember
        {
            get { return checkedMember; }
            set
            {
                if (this.checkedMember != value)
                {
                    this.checkedMember = value;

                    //if we changed the all checked item (first item)
                    if (this.valueMember[0].Equals("All") && getloadAllNeeded(indexLoadNeeded))
                        OnPropertyChanged("CheckedMemberAll");
                    else
                        OnPropertyChanged("CheckedMember");
                }
            }
        }
        string[] valueMember;

        public string[] ValueMember
        {
            get { return valueMember; }
            set { valueMember = value; }
        }

        //bool used to know if we access to every checkbox
            //if it's enabled, we don't process the property changed of CheckedMember
            //see setter of CheckedMember property
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
            public AutoCompleteCheckBox(string dislay, string[] val, bool selected, int index)
            {
                this.dislayMember = dislay;
                this.valueMember = val;
                this.checkedMember = selected;
                this.indexLoadNeeded = index;

                if (index <= loadsAllNeeded.Count)
                {
                    loadsAllNeeded.Add(true);
                }
            }

            //default constructor
            public AutoCompleteCheckBox()
            {
                this.valueMember = new string[2] { "All","0" };
                this.dislayMember = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
                this.checkedMember = true;
                this.indexLoadNeeded = 0;
                loadsAllNeeded.Add(true);
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
    }
    public class ListCheckBoxCombo : ObservableCollection<AutoCompleteCheckBox>
    {
            private int indexBoolStat = 0;
            private string text;
            private List<string[]> value;


            /// <summary>   
            /// Gets or sets SelectedItemsText.   
            /// </summary>   
            /// 
            public ListCheckBoxCombo()
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


            public List<string[]> SelectedItemsValue
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
                    foreach (AutoCompleteCheckBox item in e.NewItems)
                    {
                        item.PropertyChanged += new PropertyChangedEventHandler(OnItemPropertyChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (AutoCompleteCheckBox item in e.OldItems)
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
                this.SelectedItemsValue = new List<string[]>();

                if (this.Items[0].CheckedMember)
                {
                    this.SelectedItemsText = this.Items[0].DislayMember;
                    this.SelectedItemsValue.Add(this.Items[0].ValueMember);
                }
                else
                {
                    int iCount = 0;
                    List<AutoCompleteCheckBox> lst = this.Items.Where(e => e.CheckedMember == true).ToList();
                    foreach (AutoCompleteCheckBox item in lst)
                    {
                        sb.AppendFormat("{0}, ", item.DislayMember);
                        SelectedItemsValue.Add(item.ValueMember);
                        iCount++;
                    }
                    if (sb.Length > 2)
                    {
                        sb.Remove(sb.Length - 2, 2);
                    }
                    if (iCount > 20)
                    {
                        sb.Clear();
                        sb.Append(LLanguage.SearchResourceByKey("U.DungChung.SoGiaTriDuocChon", new string[1] { iCount.ToString() }));
                    }
                    this.SelectedItemsText = sb.ToString();
                }
            }


            //function used to update the content of the combobox source
            private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                //this case means the user changed the all item (first item)
                if (e.PropertyName == "CheckedMemberAll")
                {
                    this.UpdateAll();
                }
                else
                    if (e.PropertyName == "CheckedMember")
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
                AutoCompleteCheckBox.setloadAllNeeded(false, indexBool);
                int itemsCount = this.Items.Count;

                bool allChecked = true;
                for (int i = 1; i < itemsCount && allChecked; i++)
                {
                    AutoCompleteCheckBox item = this.Items[i];
                    if (!item.CheckedMember)
                    {
                        allChecked = false;
                    }
                }
                if (allChecked && !this.Items[0].CheckedMember) this.Items[0].CheckedMember = true;
                else
                    if (!allChecked && this.Items[0].CheckedMember) this.Items[0].CheckedMember = false;

                AutoCompleteCheckBox.setloadAllNeeded(true, indexBool);
            }



            //function used to check or uncheck all the other item
            //when the user checked or unchecked the all item (first item)
            public void UpdateAll()
            {

                //in order to avoid the behavior the property changed of the other items,
                // if we change its state
                AutoCompleteCheckBox.setloadAllNeeded(false, indexBool);
                int itemsCount = this.Items.Count;

                if (this.Items[0].CheckedMember)
                {
                    for (int i = 1; i < itemsCount; i++)
                    {
                        AutoCompleteCheckBox item = this.Items[i];
                        this.Items[i].CheckedMember = true;
                    }

                }
                else
                {
                    for (int i = 1; i < itemsCount; i++)
                    {
                        AutoCompleteCheckBox item = this.Items[i];
                        this.Items[i].CheckedMember = false;
                    }
                }
                this.UpdateSelectedText();
                AutoCompleteCheckBox.setloadAllNeeded(true, indexBool);
            }
    }
}
