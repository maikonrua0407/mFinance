using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Utilities.Common
{
    /// <summary>
    /// Thư viện các hàm thao tác với kiểu object
    /// </summary>
    public static class LObject
    {
        /// <summary>
        /// Lấy tên đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Đối tượng cần lấy tên</param>
        /// <returns>Trả lại tên của đối tượng</returns>
        public static string GetObjectName<T>(this T value)
        {
            return typeof(T).FullName;
        }

        /// <summary>
        /// Lấy danh sách tên các thuộc tính của đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Đối tượng cần lấy tên các thuộc tính</param>
        /// <returns>Mảng kiểu string chứa tên các thuộc tính</returns>
        public static string[] GetListPropertyName<T>(this T value)
        {
            List<string> propertyName = new List<string>();
            foreach (PropertyInfo property in typeof(T).GetProperties())
                propertyName.Add(property.Name);

            return propertyName.ToArray<string>();
        }

        /// <summary>
        /// Lấy danh sách tên các thuộc tính của đối tượng
        /// </summary>
        /// <param name="obj">Đối tượng cần lấy tên các thuộc tính</param>
        /// <returns>Mảng kiểu string chứa tên các thuộc tính</returns>
        public static string[] GetListPropertyName(object obj)
        {
            List<string> propertyName = new List<string>();
            Type objType = obj.GetType();
            foreach (PropertyInfo property in objType.GetProperties())
                propertyName.Add(property.Name);

            return propertyName.ToArray<string>();
        }

        /// <summary>
        /// Lấy danh sách tên các sự kiện của đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Đối tượng cần lấy tên các sự kiện</param>
        /// <returns>Mảng kiểu string chứa tên các sự kiện</returns>
        public static string[] GetListEventName<T>(this T value)
        {
            List<string> eventName = new List<string>();
            foreach (EventInfo evt in typeof(T).GetEvents())
                eventName.Add(evt.Name);

            return eventName.ToArray<string>();
        }

        /// <summary>
        /// Lấy danh sách tên các sự kiện của đối tượng
        /// </summary>
        /// <param name="obj">Đối tượng cần lấy tên các sự kiện</param>
        /// <returns>Mảng kiểu string chứa tên các sự kiện</returns>
        public static string[] GetListEventName(object obj)
        {
            List<string> eventName = new List<string>();
            Type objType = obj.GetType();
            foreach (EventInfo evt in objType.GetEvents())
                eventName.Add(evt.Name);

            return eventName.ToArray<string>();
        }

        /// <summary>
        /// Kiểm tra đối tượng null hoặc rỗng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu</typeparam>
        /// <param name="value">Đối tượng cần kiểm tra</param>
        /// <returns>Trả lại true nếu đối tượng null hoặc rỗng. Trả lại false nếu ngược lại</returns>
        public static bool IsNullOrEmpty<T>(this T value)
        {
            // Kiểm tra null
            if (ReferenceEquals(value, null)) return true;
            // Kiểm tra kiểu string
            if (typeof(T) == typeof(String)) return value.Equals(String.Empty);
            try
            {   // Kiểm tra các object có khởi tạo không cần tham số (vd: new())
                T obj = Activator.CreateInstance<T>();
                if (value.Equals(obj)) return true;
            }
            catch
            {
                // Các object khởi tạo có tham số coi như <> empty
                return false;
            }
            return false;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T Map<T>(object entity)
        {
            return BoHelper.TranslateObject<T>(entity);
        }

        public static List<T> Maps<T>(IEnumerable entity)
        {
            return BoHelper.TranslateListObject<T>(entity);
        }

        public static string JsonSerialize(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            //create a memory stream
            var ms = new MemoryStream();
            //serialize the object to memory stream
            serializer.WriteObject(ms, obj);
            //convert the serizlized object to string
            var jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //close the memory stream
            ms.Close();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (T)serializer.ReadObject(stream);
        }

        public static string ObjectToXmlString(object pv_obj)
        {
            StringBuilder v_stRet = new StringBuilder();
            string v_strVal = "";
            if (null != pv_obj)
            {
                v_stRet.AppendLine("<" + pv_obj.GetType().Name + ">");
                PropertyInfo[] v_arrInfo = pv_obj.GetType().GetProperties();
                for (int i = 0; i < v_arrInfo.Length; i++)
                {
                    try
                    {
                        v_strVal = (v_arrInfo[i].GetValue(pv_obj, null) == null ? "" : v_arrInfo[i].GetValue(pv_obj, null).ToString());
                        v_stRet.AppendLine("<" + v_arrInfo[i].Name + ">" + v_strVal + "</" + v_arrInfo[i].Name + ">");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                v_stRet.AppendLine("</" + pv_obj.GetType().Name + ">");
            }

            return v_stRet.ToString();
            //StringWriter v_sw = new StringWriter();
            //try
            //{
            //    XmlTextWriter v_stw = new XmlTextWriter(v_sw);
            //    XmlSerializer v_sr = new XmlSerializer(pv_obj.GetType(),
            //    v_sr.Serialize(v_stw, pv_obj);
                
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return v_sw.ToString();
        }
    }
}
