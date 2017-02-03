using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Utilities.Common
{
    public static class LMessage
    {
        public enum MessageBoxType
        {
            Information,
            Warning,
            Error,
            Question
        }
        public static string LayGiaTri(this MessageBoxType messageType)
        {
            switch (messageType)
            {
                case MessageBoxType.Information: return LLanguage.SearchResourceByKey("Utilities.Common.Information");
                case MessageBoxType.Warning: return LLanguage.SearchResourceByKey("Utilities.Common.Warning");
                case MessageBoxType.Error: return LLanguage.SearchResourceByKey("Utilities.Common.Error");
                case MessageBoxType.Question: return LLanguage.SearchResourceByKey("Utilities.Common.Question");
                default: return "";
            }
        }

        /// <summary>
        /// Hiển thị MessageBox theo key resource và loại MessageBox ứng với ngôn ngữ mặc định của hệ thống
        /// </summary>
        /// <param name="key">Key resource</param>
        /// <param name="type">Loại MessageBox</param>
        public static MessageBoxResult ShowMessage(string key, MessageBoxType type)
        {

            //Chuyển key resource sang value theo dict hiện tại
            string header = LLanguage.SearchResourceByKey(type.ToString());
            // truonglq sửa : xác nhận dấu xuống dòng
            key = LLanguage.SearchResourceByKey(key).Replace("\\n","\n");

            //Hiển thị message
            if (type == MessageBoxType.Information)
            {
                return MessageBox.Show(key, type.LayGiaTri(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (type == MessageBoxType.Warning)
            {
                return MessageBox.Show(key, type.LayGiaTri(), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (type == MessageBoxType.Error)
            {
                return MessageBox.Show(key, type.LayGiaTri(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (type == MessageBoxType.Question)
            {
                return MessageBox.Show(key, type.LayGiaTri(), MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }

            return MessageBoxResult.None;
        }

        /// <summary>
        /// Hiển thị MessageBox theo key resource, loại MessageBox và ngôn ngữ lựa chọn
        /// </summary>
        /// <param name="key">Key resource</param>
        /// <param name="type">Loại message</param>
        /// <param name="cultureName">Tên viết tắt ngôn ngữ</param>
        /// <param name="pathFolderLanguages">đường dẫn tới folder ngôn ngữ</param>
        public static void ShowMessageByLanguage(string key, MessageBoxType type, string cultureName, string pathFolderLanguages)
        {
            //Chuyển key resource sang value theo các tham số truyền vào
            string header = LLanguage.SearchResourceByKey(type.ToString(),pathFolderLanguages,cultureName);
            key = LLanguage.SearchResourceByKey(key,pathFolderLanguages,cultureName);

            //Hiển thị message
            switch (type)
            {
                case MessageBoxType.Information:
                    MessageBox.Show(key, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case MessageBoxType.Warning:
                    MessageBox.Show(key, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case MessageBoxType.Error:
                    MessageBox.Show(key, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case MessageBoxType.Question:
                    MessageBox.Show(key, type.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question);
                    break;
            }

            header = null;
        }

        /// <summary>
        /// Hiển thị message theo key và các tham số truyền vào
        /// </summary>
        /// <param name="key">key resource</param>
        /// <param name="obj">Danh sách các tham số truyền vào</param>
        /// <param name="type">Loại MessageBox</param>
        public static void ShowMessage(string key, string[] obj, MessageBoxType type)
        {
            key = LLanguage.SearchResourceByKey(key);
            key = String.Format(key, obj);
            ShowMessage(key, type);
        }

        /// <summary>
        /// Hiển thị message box theo message truyền vào và không phụ thuộc vào language
        /// </summary>
        /// <param name="message">string cần hiển thị</param>
        /// <param name="type">loại message box</param>
        public static void ShowMessageWithoutKey(string message, MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.Information:
                    MessageBox.Show(message, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case MessageBoxType.Warning:
                    MessageBox.Show(message, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case MessageBoxType.Error:
                    MessageBox.Show(message, type.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                case MessageBoxType.Question:
                    MessageBox.Show(message, type.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question);
                    break;
            }
        }
    }
}
