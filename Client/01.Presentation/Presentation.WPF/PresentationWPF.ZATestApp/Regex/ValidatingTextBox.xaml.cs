using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PresentationWPF.ZATestApp.Regex
{
    /// <summary>
    /// Interaction logic for ValidatingTextBox.xaml
    /// http://www.regular-expressions.info/examples.html
    /// http://www.regexbuddy.com/
    /// </summary>
    public partial class ValidatingTextBox : TextBox, IDisposable
    {
        #region IDisposable

        public virtual void Dispose()
        {
            RegexValidator.ValidationFailed -= OnValidationFailed;
            RegexValidator.FailureCleared -= OnFailureCleared;
        }

        #endregion

        #region TextBox Content Types

        public enum InputType { Any, Custom, Numeric, IpAddress, Url, Integer, EmailAddress, Visa, Mastercard, AmericanExpress, Discover };

        /// <summary>
        /// The names of all available content types (e.g. for use in populating ComboBox options)
        /// </summary>
        public InputType[] ContentTypes
        {
            get { return (InputType[])Enum.GetValues(typeof(InputType)); }
        }

        public static readonly DependencyProperty ContentTypeProperty =
            DependencyProperty.Register("ContentType", typeof(InputType), typeof(ValidatingTextBox),
                                        new PropertyMetadata(InputType.Any, ContentTypeChangedCallback));

        private static void ContentTypeChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ValidatingTextBox)
            {
                UpdateValidationRule(obj as ValidatingTextBox);
            }
        }

        /// <summary>
        /// The contraints this text box has on the valid kinds of user input.
        /// </summary>
        public InputType ContentType
        {
            get { return (InputType)GetValue(ContentTypeProperty); }
            set { SetValue(ContentTypeProperty, value); }
        }

        #endregion

        #region Validation Error Notification Type

        public enum ErrorNotificationType { Tooltip, Popup };

        private ErrorNotificationType notificationType = ErrorNotificationType.Tooltip;
        public ErrorNotificationType NotificationType
        {
            get { return notificationType; }
            set
            {
                if (value != notificationType)
                {
                    notificationType = value;
                }
                TooltipShowsError = (notificationType == ErrorNotificationType.Tooltip);
            }
        }

        /// <summary>
        /// Does this text box show a tooltip with validation error descriptions?
        /// </summary>
        public bool TooltipShowsError
        {
            get { return (bool)GetValue(TooltipShowsErrorProperty); }
            set { SetValue(TooltipShowsErrorProperty, value); }
        }

        public static readonly DependencyProperty TooltipShowsErrorProperty =
            DependencyProperty.Register("TooltipShowsError", typeof(bool), typeof(ValidatingTextBox),
                                        new PropertyMetadata(true, TooltipShowsErrorChangedCallback));

        private static void TooltipShowsErrorChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // nothing to do
        }

        #endregion

        /// <summary>
        /// An optional error message to display when validation fails.
        /// </summary>
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ValidatingTextBox),
                                        new PropertyMetadata(string.Empty, ErrorMessageChangedCallback));

        private static void ErrorMessageChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// Create a new ValidatingTextBox.
        /// </summary>
        public ValidatingTextBox()
        {
            InitializeComponent();

            ContentType = InputType.Any;
            NotificationType = ErrorNotificationType.Tooltip;

            // Defer this call until after load, since XAML may choose to set a different ContentType
            this.Loaded += OnTextBoxLoaded;
            RegexValidator.ValidationFailed += OnValidationFailed;
            RegexValidator.FailureCleared += OnFailureCleared;
        }

        #region Validation Callbacks

        private ValidationPopup popup;
        private void OnValidationFailed(object sender, EventArgs e)
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }

            if (NotificationType == ErrorNotificationType.Popup)
            {
                popup = new ValidationPopup();
                popup.PlacementTarget = this;
                popup.Title = "Invalid Input";
                popup.AutoClose = true;
                // If the client has specified a custom error message, use that.  Else, get the default one from the validator.
                popup.Message = (ErrorMessage != null) ? ErrorMessage : RegexValidator.GetErrorMessage(this);
            }
        }

        private void OnFailureCleared(object sender, EventArgs e)
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        #endregion

        private void OnTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            UpdateValidationRule(this);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
        }

        private static void UpdateValidationRule(ValidatingTextBox self)
        {
            if (self.GetBindingExpression(TextBox.TextProperty) != null)
            {
                string regex = null;

                switch (self.ContentType)
                {
                    case InputType.Numeric:
                        regex = @"^[0-9.-]+$";
                        break;
                    case InputType.IpAddress:
                        regex = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
                        break;
                    case InputType.Integer:
                        regex = @"^[0-9]+$";
                        break;
                    case InputType.EmailAddress:
                        regex = @"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
                        break;
                    case InputType.Visa:
                        regex = @"^4[0-9]{12}(?:[0-9]{3})?$";
                        break;
                    case InputType.Mastercard:
                        regex = @"^5[1-5][0-9]{14}$";
                        break;
                    case InputType.AmericanExpress:
                        regex = @"^3[47][0-9]{13}$";
                        break;
                    case InputType.Discover:
                        regex = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
                        break;
                    case InputType.Url:
                        regex = @"([a-zA-Z]{3,})://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                        break;
                    case InputType.Custom:
                        // TODO!
                        break;
                    default:
                        break;
                }

                if (regex != null)
                {
                    // Assign the regular expression.
                    RegexValidator.SetRegexText(self, regex);

                    // Assign the error message.
                    RegexValidator.SetErrorMessage(self, String.Format("Invalid {0}", self.ContentType.ToString()));
                }
            }
        }
    }
}
