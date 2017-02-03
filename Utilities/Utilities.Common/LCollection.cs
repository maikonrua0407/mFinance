using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities.Common
{
    /// <summary>
    /// Lớp lỗi trên server để trả lại cho client
    /// </summary>
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException(Exception innerException) : base("Server exception", innerException) { }

        public CustomException(string message, Exception innerException) : base(message, innerException) { }

        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Thư viện các hàm thao tác với kiểu mảng, danh sách
    /// </summary>
    public static class LCollection
    {
        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Mảng cần sắp xếp</param>
        /// <returns>Trả lại true nếu sắp xếp được. Trả lại false nếu lỗi</returns>
        public static bool SortByAsc<T>(this T[] value)
        {
            if (Object.ReferenceEquals(value, null)) return false;
            if (value.Length == 0) return false;
            try { Array.Sort(value); return true; } catch { return false; }
        }

        /// <summary>
        /// Sắp xếp tăng dần
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Danh sách cần sắp xếp</param>
        /// <returns>Trả lại true nếu sắp xếp được. Trả lại false nếu lỗi</returns>
        public static bool SortByAsc<T>(this List<T> value)
        {
            if (Object.ReferenceEquals(value, null)) return false;
            if (value.Count == 0) return false;
            try { value.Sort(); return true; }
            catch { return false; }
        }

        /// <summary>
        /// Sắp xếp giảm dần
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Mảng cần sắp xếp</param>
        /// <returns>Trả lại true nếu sắp xếp được. Trả lại false nếu lỗi</returns>
        public static bool SortByDesc<T>(this T[] value)
        {
            if (!value.SortByAsc()) return false;
            Array.Reverse(value);
            return true;
        }

        /// <summary>
        /// Sắp xếp giảm dần
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Danh sách cần sắp xếp</param>
        /// <returns>Trả lại true nếu sắp xếp được. Trả lại false nếu lỗi</returns>
        public static bool SortByDesc<T>(this List<T> value)
        {
            if (!value.SortByAsc()) return false;
            value.Reverse();
            return true;
        }

        /// <summary>
        /// Nối nhiều mảng thành 1 mảng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="values">Các mảng cần nối</param>
        /// <returns>Trả lại 1 mảng chứa các mảng cần nối. Trả lại null nếu tất cả các mảng đều null</returns>
        public static T[] Concat<T>(params T[][] values)
        {
            T[] result = new T[0];
            bool hasValue = false;
            foreach (T[] value in values)
            {
                if (!Object.ReferenceEquals(value, null))
                {
                    result = result.Concat(value).ToArray();
                    hasValue = true; // Nếu có ít nhất 1 mảng không null thì kết quả không null
                }
            }
            return hasValue ? result : null;
        }

        /// <summary>
        /// Nối nhiều danh sách thành 1 danh sách
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="values">Các danh sách cần nối</param>
        /// <returns>Trả lại 1 danh sách chứa các danh sách cần nối. Trả lại null nếu tất cả các danh sách đều null</returns>
        public static List<T> Concat<T>(params List<T>[] values)
        {
            List<T> result = new List<T>();
            bool hasValue = false;
            foreach (List<T> value in values)
            {
                if (!Object.ReferenceEquals(value, null))
                {
                    result = result.Concat(value).ToList();
                    hasValue = true; // Nếu có ít nhất 1 danh sách không null thì kết quả không null
                }
            }
            return hasValue ? result : null;
        }

        public static byte Serialize(this Object value)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, value);
            byte result = stream.Serialize();
            stream.Close();
            return result;
        }

        public static byte[] Serialize(this Exception value)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, value);
            byte[] result = stream.ToArray();
            stream.Close();
            return result;
        }

        public static Exception Deserialize(this byte[] value)
        {
            Exception result;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(value);
            result = (Exception)bf.Deserialize(stream);
            stream.Close();
            //return result.InnerException;
            return result;
        }
    }
}
