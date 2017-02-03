using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Utilities.Common
{
    public static class LSecurity
    {
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("NGVTKey1");

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the original string is null or empty.</exception>
        public static string EncryptStringBase64(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(str);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the crypted string is null or empty.</exception>
        public static string DecryptStringBase64(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        public static string MD5Encrypt(string String2Encrypt)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(String2Encrypt);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public static bool DESEncryptFile(string sInputFilename,
           string sOutputFilename,
           string sKey)
        {
            try
            {
                FileStream fsInput = new FileStream(sInputFilename,
                   FileMode.Open,
                   FileAccess.Read);

                FileStream fsEncrypted = new FileStream(sOutputFilename,
                   FileMode.Create,
                   FileAccess.Write);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted,
                   desencrypt,
                   CryptoStreamMode.Write);

                byte[] bytearrayinput = new byte[fsInput.Length];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("Mã hóa không thành công", LMessage.MessageBoxType.Warning);
                return false;
            }

        }

        public static bool DESDecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                //A 64 bit key and IV is required for this provider.
                //Set secret key For DES algorithm.
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //Set initialization vector.
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Create a file stream to read the encrypted file back.
                FileStream fsread = new FileStream(sInputFilename,
                   FileMode.Open,
                   FileAccess.Read);
                //Create a DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create crypto stream set to read and do a 
                //DES decryption transform on incoming bytes.
                CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                   desdecrypt,
                   CryptoStreamMode.Read);
                //Print the contents of the decrypted file.
                StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                fsDecrypted.Flush();
                fsDecrypted.Close();
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("Giải mã không thành công", LMessage.MessageBoxType.Warning);
                return false;
            }
        }

        public static MemoryStream DESDecryptFile(string sInputFilename, string sKey)
        {
            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                //A 64 bit key and IV is required for this provider.
                //Set secret key For DES algorithm.
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //Set initialization vector.
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Create a file stream to read the encrypted file back.
                FileStream fsread = new FileStream(sInputFilename,
                   FileMode.Open,
                   FileAccess.Read);
                //Create a DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create crypto stream set to read and do a 
                //DES decryption transform on incoming bytes.
                CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                   desdecrypt,
                   CryptoStreamMode.Read);
                //Print the contents of the decrypted file.
                var cryptoStreamReader = new StreamReader(cryptostreamDecr);

                var memoryStream = new MemoryStream();
                var memoryStreamWriter = new StreamWriter(memoryStream);
                string _decrypted = cryptoStreamReader.ReadToEnd();
                memoryStreamWriter.Write(_decrypted);
                byte[] byteArray = Encoding.UTF8.GetBytes(_decrypted);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream mStream = new MemoryStream(byteArray);
                memoryStreamWriter.Flush();
                memoryStreamWriter.Close();
                return mStream;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("Giải mã không thành công", LMessage.MessageBoxType.Warning);
                return null;
            }
        }

        public static string SHA1Encrypt(string EncryptString)
        {
            string result = null;
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                Byte[] hashBytes = UE.GetBytes(EncryptString);
                SHA1CryptoServiceProvider sha1c = new SHA1CryptoServiceProvider();
                Byte[] crypt = sha1c.ComputeHash(hashBytes);
                result  = BitConverter.ToString(crypt);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static string SHA256Encrypt(string EncryptString)
        {
            string result = null;
            try
            {
                byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(EncryptString);
                SHA256Managed hash = new SHA256Managed();
                byte[] hashBytes = hash.ComputeHash(inputBytes);
                result = Convert.ToBase64String(hashBytes);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static bool IsMacAddress(string macAddress)
        {
            char[] hex = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            macAddress = macAddress.Replace(" ", "");
            macAddress = macAddress.Replace("-", "");
            macAddress = macAddress.ToUpper();

            if (macAddress.Equals("*")) 
                return true;

            if (macAddress.Length != 12) 
                return false;                

            foreach (char c in macAddress.ToCharArray())
            {
                if (hex.Contains(c) == false)                
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Kiểm tra 1 chuỗi có phải là địa chỉ IPv4 hay không. 
        /// Quan niệm:
        /// Hợp lệ: "*" hay "192.169.*.*" hay "*.*.23.*" ...
        /// Không hợp lệ: "192.168.*" hay "1234.168.0.23" hay "192.168.0.2*" ...
        /// Trả về true nếu hợp lệ. 
        /// Trả về false nếu không hợp lệ
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPv4Address(string ipAddress)
        {
            ipAddress = ipAddress.Replace(" ", "");

            if (ipAddress.Equals("*")) 
                return true;

            string[] s = ipAddress.Split('.');

            if (s.Length != 4) return false;

            foreach (var item in s)
            {
                if (!item.Equals("*"))
                {
                    if (!LString.IsNumeric(item))
                        return false;
                    else
                    {
                        int num = Convert.ToInt32(item);
                        if (num < 0 || num > 255)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
