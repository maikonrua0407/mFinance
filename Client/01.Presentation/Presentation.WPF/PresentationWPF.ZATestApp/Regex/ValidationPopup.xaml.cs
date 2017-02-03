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
using System.Threading;


namespace PresentationWPF.ZATestApp.Regex
{
     /// <summary>
    /// Interaction logic for ValidationPopup.xaml
    /// </summary>
    public partial class ValidationPopup : System.Windows.Controls.Primitives.Popup
    {
        public ValidationPopup()
        {
            InitializeComponent();

            AutoCloseTimeout = 4000;  // default value = 4 seconds

            // Defer creation of Timer to allow client code to set AutoCloseTimeout to a different value.
            // Using the Popup.Loaded event does not seem to work!
            this.MessageTextBlock.Loaded += OnLoad;
        }

        private Timer timer;
        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AutoClose)
            {
                timer = new Timer(OnTimeout, this, AutoCloseTimeout, Timeout.Infinite);
            }
        }

        private void OnTimeout(System.Object stateInfo)
        {
            // Closing the popup must occur on the UI thread
            Dispatcher.Invoke((System.Action)ClosePopup, (object[])null);
            timer.Dispose();
            timer = null;
        }

        private void ClosePopup()
        {
            IsOpen = false;
        }

        /// <summary>
        /// If AutoClose is True, the amount of time in milliseconds before the popup is automatically dismissed.
        /// </summary>
        public int AutoCloseTimeout { get; set; }

        /// <summary>
        /// If True, the popup will automatically close itself after AutoCloseTimeout amount of time.
        /// </summary>
        public bool AutoClose { get; set; }

        public string Title
        {
            get { return this.TitleTextBlock.Text; }
            set
            {
                this.TitleTextBlock.Text = value;
            }
        }

        public string Message
        {
            get { return this.MessageTextBlock.Text; }
            set
            {
                this.MessageTextBlock.Text = value;
            }
        }

        /// <summary>
        /// Workaround for problems with StaysOpen="False" alone not allowing popup to close.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">The mouse button event (mouse down).</param>
        private void Popup_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClosePopup();
            e.Handled = true;
        }
    }
}
