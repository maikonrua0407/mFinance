using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Data;
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
using Utilities.Common;
using Telerik.Windows.Controls;


namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Thư viện các hàm thao tác với kiểu object
    /// </summary>
    public static class CustomerObject
    {
        /// <summary>
        /// Tìm kiếm control
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);
                    if (!foundChild.IsNullOrEmpty())
                        return foundChild;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        return foundChild;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    return foundChild;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Extension method for a FrameworkElement that searches for a child element by type and name.
        /// </summary>
        /// <typeparam name="T">The type of the child element to search for.</typeparam>
        /// <param name="element">The parent framework element.</param>
        /// <param name="sChildName">The name of the child element to search for.</param>
        /// <returns>The matching child element, or null if none found.</returns>
        public static T FindElementByName<T>(this FrameworkElement element, string sChildName) where T : FrameworkElement
        {
            T childElement = null;

            //
            // Spin through immediate children of the starting element.
            //
            var nChildCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < nChildCount; i++)
            {
                // Get next child element.
                FrameworkElement child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                // Do we have a child?
                if (child == null)
                    continue;

                // Is child of desired type and name?
                if (child is T && child.Name.Equals(sChildName))
                {
                    // Bingo! We found a match.
                    childElement = (T)child;
                    break;
                } // if

                // Recurse and search through this child's descendants.
                childElement = FindElementByName<T>(child, sChildName);

                // Did we find a matching child?
                if (childElement != null)
                    break;
            } // for

            return childElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="logicalCollection"></param>
        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }
    }
}
