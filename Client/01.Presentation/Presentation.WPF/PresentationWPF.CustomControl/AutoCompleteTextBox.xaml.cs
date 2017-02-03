using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for AutoCompleteTextBox.xaml
    /// </summary>    
    public partial class AutoCompleteTextBox : Canvas
    {
        #region Members
        private VisualCollection controls;
        private TextBox textBox;
        private ComboBox comboBox;
        private ObservableCollection<AutoCompleteEntry> autoCompletionList;
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;
        #endregion

        #region Constructor
        public AutoCompleteTextBox()
        {
            controls = new VisualCollection(this);
            InitializeComponent();

            autoCompletionList = new ObservableCollection<AutoCompleteEntry>();
            searchThreshold = 2;        // default threshold to 2 char

            // set up the key press timer
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            // set up the text box and the combo box
            comboBox = new ComboBox();
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);

            textBox = new TextBox();
            //textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.PreviewKeyDown += textBox_KeyDown;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            controls.Add(comboBox);
            controls.Add(textBox);
        }
        #endregion

        #region Methods
        public string Text
        {
            get { return textBox.Text; }
            set 
            {
                insertText = true;
                textBox.Text = value; 
            }
        }

        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }

        public void AddItem(AutoCompleteEntry entry)
        {
            autoCompletionList.Add(entry);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;

                string keyWork = "";
                string[] keyWorks = textBox.Text.Split(',');
                bool exist = false;
                if (keyWorks != null && keyWorks.Count() > 1)
                {
                    for (int i = 0; i < keyWorks.Count() - 1; i++)
                    {
                        keyWork += keyWorks[i] + ", ";
                        if (keyWorks[i].Trim().Equals(cbItem.Content.ToString().Trim()))
                            exist = true;
                    }
                }
                else
                    keyWork = "";
                if(keyWork.Equals(""))
                    textBox.Text = cbItem.Content.ToString() + ", ";
                else if (exist)
                    textBox.Text = keyWork;
                else
                    textBox.Text = keyWork + cbItem.Content.ToString() + ", ";
                textBox.Select(textBox.Text.Length, 0);
                textBox.Focus();
            }
        }

        private void TextChanged()
        {
            try
            {
                comboBox.Items.Clear();
                string keyWork="";
                    string[] keyWorks = textBox.Text.Split(',');
                if(keyWorks!=null&&keyWorks.Count()>1)
                    keyWork = keyWorks[keyWorks.Count() - 1].Trim();
                else
                    keyWork=textBox.Text;
                if (keyWork.Length >= searchThreshold)
                {
                    foreach (AutoCompleteEntry entry in autoCompletionList)
                    {
                        foreach (string word in entry.KeywordStrings)
                        {
                            if (word.StartsWith(keyWork, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ComboBoxItem cbItem = new ComboBoxItem();
                                cbItem.Content = entry.ToString();
                                comboBox.Items.Add(cbItem);
                                break;
                            }
                        }
                    }
                    comboBox.IsDropDownOpen = comboBox.HasItems;
                }
                else
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
            catch { }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;
            
            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Down)
                {
                    if (comboBox.Items.Count > 0)
                        comboBox.Focus();
                }
                else
                {
                    // text was not typed, do nothing and consume the flag
                    if (insertText == true) insertText = false;

                    // if the delay time is set, delay handling of text changed
                    else
                    {
                        if (delayTime > 0)
                        {
                            keypressTimer.Interval = delayTime;
                            keypressTimer.Start();
                        }
                        else TextChanged();
                    }
                }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            textBox.Arrange(new Rect(arrangeSize));
            comboBox.Arrange(new Rect(arrangeSize));
            return base.ArrangeOverride(arrangeSize);
        }

        protected override Visual GetVisualChild(int index)
        {
            return controls[index];
        }

        protected override int VisualChildrenCount
        {
            get { return controls.Count; }
        }
        #endregion
    }
}