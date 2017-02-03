using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC
{
    public class FileUtils
    {
        public static void ReSetReadOnlyFile(string path)
        {
            FileInfo[] d = new DirectoryInfo(path).GetFiles();
            foreach (FileInfo f in d)
            {
                f.IsReadOnly = false;
            }
        }

        public static void ReSetReadOnlyFolder(string path)
        {
            new DirectoryInfo(path).GetDirectories("*", SearchOption.AllDirectories).ToList().ForEach(
            di =>
            {
                di.Attributes &= ~FileAttributes.ReadOnly;
                di.GetFiles("*", SearchOption.TopDirectoryOnly).ToList().ForEach(fi => fi.IsReadOnly = false);
            }
        );
        }

        public static bool CheckFileExist(string pathFolder, string fileName)
        {
            var fileInfo = new DirectoryInfo(pathFolder).GetFiles();
            foreach (var f in fileInfo)
            {
                if (f.Name == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        public static void CopyFile(string pathOrigin, string pathNew)
        {
            try
            {
                File.Copy(pathOrigin, pathNew);
            }
            catch { }
        }

        public static void RenameFile(string oldName, string newName)
        {
            try
            {
                File.Move(oldName, newName);
            }
            catch { }
        }

        public static void RemoveFileTemp(string pathFolder)
        {
            if (Directory.Exists(pathFolder))
            {
                foreach (var f in new DirectoryInfo(pathFolder).GetFiles())
                {
                    File.Delete(f.FullName);
                }
            }
        }

        public static void RemoveFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}