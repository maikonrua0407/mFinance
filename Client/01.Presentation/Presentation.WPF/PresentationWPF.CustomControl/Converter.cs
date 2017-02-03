using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
using System.Text.RegularExpressions;
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = System.Convert.ToBoolean(value);
            return val ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TrueValue.Equals(value) ? true : false;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public NullToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class NullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Value converter that performs arithmetic calculations over its argument(s)
    /// </summary>
    /// <remarks>
    /// MathConverter can act as a value converter, or as a multivalue converter (WPF only).
    /// It is also a markup extension (WPF only) which allows to avoid declaring resources,
    /// ConverterParameter must contain an arithmetic expression over converter arguments. Operations supported are +, -, * and /
    /// Single argument of a value converter may referred as x, a, or {0}
    /// Arguments of multi value converter may be referred as x,y,z,t (first-fourth argument), or a,b,c,d, or {0}, {1}, {2}, {3}, {4}, ...
    /// The converter supports arithmetic expressions of arbitrary complexity, including nested subexpressions
    /// </remarks>
    public class MathConverter :
#if !SILVERLIGHT
 MarkupExtension,
        IMultiValueConverter,
#endif
 IValueConverter
    {
        Dictionary<string, IExpression> _storedExpressions = new Dictionary<string, IExpression>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new object[] { value }, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                decimal result = Parse(parameter.ToString()).Eval(values);
                if (targetType == typeof(decimal)) return result;
                if (targetType == typeof(string)) return result.ToString();
                if (targetType == typeof(int)) return (int)result;
                if (targetType == typeof(double)) return (double)result;
                if (targetType == typeof(long)) return (long)result;
                throw new ArgumentException(String.Format("Unsupported target type {0}", targetType.FullName));
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

#if !SILVERLIGHT
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
#endif
        protected virtual void ProcessException(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private IExpression Parse(string s)
        {
            IExpression result = null;
            if (!_storedExpressions.TryGetValue(s, out result))
            {
                result = new Parser().Parse(s);
                _storedExpressions[s] = result;
            }

            return result;
        }

        interface IExpression
        {
            decimal Eval(object[] args);
        }

        class Constant : IExpression
        {
            private decimal _value;

            public Constant(string text)
            {
                if (!decimal.TryParse(text, out _value))
                {
                    throw new ArgumentException(String.Format("'{0}' is not a valid number", text));
                }
            }

            public decimal Eval(object[] args)
            {
                return _value;
            }
        }

        class Variable : IExpression
        {
            private int _index;

            public Variable(string text)
            {
                if (!int.TryParse(text, out _index) || _index < 0)
                {
                    throw new ArgumentException(String.Format("'{0}' is not a valid parameter index", text));
                }
            }

            public Variable(int n)
            {
                _index = n;
            }

            public decimal Eval(object[] args)
            {
                if (_index >= args.Length)
                {
                    throw new ArgumentException(String.Format("MathConverter: parameter index {0} is out of range. {1} parameter(s) supplied", _index, args.Length));
                }

                return System.Convert.ToDecimal(args[_index]);
            }
        }

        class BinaryOperation : IExpression
        {
            private Func<decimal, decimal, decimal> _operation;
            private IExpression _left;
            private IExpression _right;

            public BinaryOperation(char operation, IExpression left, IExpression right)
            {
                _left = left;
                _right = right;
                switch (operation)
                {
                    case '+': _operation = (a, b) => (a + b); break;
                    case '-': _operation = (a, b) => (a - b); break;
                    case '*': _operation = (a, b) => (a * b); break;
                    case '/': _operation = (a, b) => (a / b); break;
                    default: throw new ArgumentException("Invalid operation " + operation);
                }
            }

            public decimal Eval(object[] args)
            {
                return _operation(_left.Eval(args), _right.Eval(args));
            }
        }

        class Negate : IExpression
        {
            private IExpression _param;

            public Negate(IExpression param)
            {
                _param = param;
            }

            public decimal Eval(object[] args)
            {
                return -_param.Eval(args);
            }
        }

        class Parser
        {
            private string text;
            private int pos;

            public IExpression Parse(string text)
            {
                try
                {
                    pos = 0;
                    this.text = text;
                    var result = ParseExpression();
                    RequireEndOfText();
                    return result;
                }
                catch (Exception ex)
                {
                    string msg =
                        String.Format("MathConverter: error parsing expression '{0}'. {1} at position {2}", text, ex.Message, pos);

                    throw new ArgumentException(msg, ex);
                }
            }

            private IExpression ParseExpression()
            {
                IExpression left = ParseTerm();

                while (true)
                {
                    if (pos >= text.Length) return left;

                    var c = text[pos];

                    if (c == '+' || c == '-')
                    {
                        ++pos;
                        IExpression right = ParseTerm();
                        left = new BinaryOperation(c, left, right);
                    }
                    else
                    {
                        return left;
                    }
                }
            }

            private IExpression ParseTerm()
            {
                IExpression left = ParseFactor();

                while (true)
                {
                    if (pos >= text.Length) return left;

                    var c = text[pos];

                    if (c == '*' || c == '/')
                    {
                        ++pos;
                        IExpression right = ParseFactor();
                        left = new BinaryOperation(c, left, right);
                    }
                    else
                    {
                        return left;
                    }
                }
            }

            private IExpression ParseFactor()
            {
                SkipWhiteSpace();
                if (pos >= text.Length) throw new ArgumentException("Unexpected end of text");

                var c = text[pos];

                if (c == '+')
                {
                    ++pos;
                    return ParseFactor();
                }

                if (c == '-')
                {
                    ++pos;
                    return new Negate(ParseFactor());
                }

                if (c == 'x' || c == 'a') return CreateVariable(0);
                if (c == 'y' || c == 'b') return CreateVariable(1);
                if (c == 'z' || c == 'c') return CreateVariable(2);
                if (c == 't' || c == 'd') return CreateVariable(3);

                if (c == '(')
                {
                    ++pos;
                    var expression = ParseExpression();
                    SkipWhiteSpace();
                    Require(')');
                    SkipWhiteSpace();
                    return expression;
                }

                if (c == '{')
                {
                    ++pos;
                    var end = text.IndexOf('}', pos);
                    if (end < 0) { --pos; throw new ArgumentException("Unmatched '{'"); }
                    if (end == pos) { throw new ArgumentException("Missing parameter index after '{'"); }
                    var result = new Variable(text.Substring(pos, end - pos).Trim());
                    pos = end + 1;
                    SkipWhiteSpace();
                    return result;
                }

                const string decimalRegEx = @"(\d+\.?\d*|\d*\.?\d+)";
                var match = Regex.Match(text.Substring(pos), decimalRegEx);
                if (match.Success)
                {
                    pos += match.Length;
                    SkipWhiteSpace();
                    return new Constant(match.Value);
                }
                else
                {
                    throw new ArgumentException(String.Format("Unexpeted character '{0}'", c));
                }
            }

            private IExpression CreateVariable(int n)
            {
                ++pos;
                SkipWhiteSpace();
                return new Variable(n);
            }

            private void SkipWhiteSpace()
            {
                while (pos < text.Length && Char.IsWhiteSpace((text[pos]))) ++pos;
            }

            private void Require(char c)
            {
                if (pos >= text.Length || text[pos] != c)
                {
                    throw new ArgumentException("Expected '" + c + "'");
                }

                ++pos;
            }

            private void RequireEndOfText()
            {
                if (pos != text.Length)
                {
                    throw new ArgumentException("Unexpected character '" + text[pos] + "'");
                }
            }
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    public class ConverterStringToDataTime : IValueConverter
    {
        public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {

                if (LDateTime.IsDate(value.ToString(), ApplicationConstant.defaultDateTimeFormat))
                    return LDateTime.StringToDate(value.ToString(), ApplicationConstant.defaultDateTimeFormat);
                else
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                if (typeof(DateTime).Equals(value.GetType()))
                {
                    return LDateTime.DateToString(System.Convert.ToDateTime(value), ApplicationConstant.defaultDateTimeFormat);
                }
                else
                    return null;
            }
        }
    }

    [ValueConversion(typeof(decimal),typeof(string))]
    public class ConverterDecimalToNull : IValueConverter
    {
        public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {

                if (!value.Equals(0))
                    return "{}{0:N0}";
                else
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(string), typeof(Boolean))]
    public class ConverterStringToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {

                return System.Convert.ToBoolean(value.ToString().ToLower());
            }
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    public class ConverterApplicationConstantToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                if (value.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    return true;
                else if (value.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri()))
                    return false;
                else
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (value.Equals(true))
                return BusinessConstant.CoKhong.CO.layGiaTri();
            else if (value.Equals(false))
                return BusinessConstant.CoKhong.KHONG.layGiaTri();
            else
                return "";
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    public class ConverterNullToDecimal : IValueConverter
    {
        public object Convert(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value) || !value.ToString().IsNumeric())
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDecimal(value);
            }
        }
        public object ConvertBack(object value, Type targetType,
      object parameter, CultureInfo culture)
        {
            if (LObject.IsNullOrEmpty(value) || !value.ToString().IsNumeric())
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDecimal(value);
            }
        }
    }
}
