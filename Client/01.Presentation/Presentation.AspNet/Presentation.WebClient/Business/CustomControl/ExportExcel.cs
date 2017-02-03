using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;
namespace Presentation.WebClient.Business.CustomControl
{
    public class ExportExcel
    {
        // phan quyen truy cap C:\Windows\SysWOW64\config\systemprofile\Desktop cho IIS

        public void ExportToExcel(DataSet mDataset, string sPath)
        {
            ApplicationClass class2 = new ApplicationClass();
            class2.Application.Workbooks.Add(true);
            System.Data.DataTable table = mDataset.Tables[0];
            int num = 0;
            foreach (DataColumn column in table.Columns)
            {
                num++;
                class2.Cells[1, num] = column.ColumnName;
            }
            int num2 = 0;
            foreach (DataRow row in table.Rows)
            {
                num = 0;
                num2++;
                foreach (DataColumn column in table.Columns)
                {
                    num++;
                    try
                    {
                        class2.Cells[num2 + 1, num] = table.Rows[num2 - 1][column.ColumnName].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            class2.Visible = true;
            object fileFormat = Missing.Value;
            try
            {
                try
                {
                    System.IO.File.Delete(sPath);
                }
                catch
                {
                }
                ((Worksheet)class2.ActiveSheet).SaveAs(sPath, fileFormat, fileFormat, fileFormat, fileFormat, true, fileFormat, fileFormat, fileFormat, fileFormat);
            }
            catch
            {
            }
            class2.Quit();
           
        }
        public void ExportToExcel(System.Data.DataTable mDataset, string sPath)
        {
            ApplicationClass class2 = new ApplicationClass();
            class2.Application.Workbooks.Add(true);
            System.Data.DataTable table = mDataset;
            int num = 0;
            foreach (DataColumn column in table.Columns)
            {
                num++;
                class2.Cells[1, num] = column.ColumnName;
            }
            int num2 = 0;
            foreach (DataRow row in table.Rows)
            {
                num = 0;
                num2++;
                foreach (DataColumn column in table.Columns)
                {
                    num++;
                    try
                    {
                        class2.Cells[num2 + 1, num] = table.Rows[num2 - 1][column.ColumnName].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            class2.Visible = true;
            object fileFormat = Missing.Value;
            try
            {
                try
                {
                    System.IO.File.Delete(sPath);
                }
                catch
                {
                }
                ((Worksheet)class2.ActiveSheet).SaveAs(sPath, fileFormat, fileFormat, fileFormat, fileFormat, true, fileFormat, fileFormat, fileFormat, fileFormat);
            }
            catch
            {
            }
            class2.Quit();

        }
    }
}