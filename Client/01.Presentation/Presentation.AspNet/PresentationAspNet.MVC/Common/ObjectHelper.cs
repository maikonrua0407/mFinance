using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC
{
    public static class ObjectHelper<T>
    {
        public static int Compare(T x, T y)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var fields = type.GetFields();
            var compareValue = 0;

            foreach (var property in properties)
            {
                var valx = property.GetValue(x, null) as IComparable;
                if (valx == null)
                    continue;
                var valy = property.GetValue(y, null);
                compareValue = valx.CompareTo(valy);
                if (compareValue != 0)
                    return compareValue;
            }
            foreach (var field in fields)
            {
                var valx = field.GetValue(x) as IComparable;
                if (valx == null)
                    continue;
                var valy = field.GetValue(y);
                compareValue = valx.CompareTo(valy);
                if (compareValue != 0)
                    return compareValue;
            }

            return compareValue;
        }

        public static int CompareProperties(T x, T y)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var compareValue = 0;

            foreach (var property in properties)
            {
                var valx = property.GetValue(x, null) as IComparable;
                if (valx == null)
                    continue;
                var valy = property.GetValue(y, null);
                compareValue = valx.CompareTo(valy);
                if (compareValue != 0)
                    return compareValue;
            }

            return compareValue;
        }

        public static int ComparFields(T x, T y)
        {
            var type = typeof(T);
            var fields = type.GetFields();
            var compareValue = 0;

            foreach (var field in fields)
            {
                var valx = field.GetValue(x) as IComparable;
                if (valx == null)
                    continue;
                var valy = field.GetValue(y);
                compareValue = valx.CompareTo(valy);
                if (compareValue != 0)
                    return compareValue;
            }

            return compareValue;
        }

    }
}