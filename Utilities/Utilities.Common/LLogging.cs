using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using log4net;

namespace Utilities.Common
{
    public static class LLogging
    {
        public static ILog bus = LogManager.GetLogger("BusLogger");
        public static ILog err = LogManager.GetLogger("ErrLogger");
        public static ILog sys = LogManager.GetLogger("SysLogger");

        /// <summary>
        /// Log Type
        /// </summary>
        public enum LogType
        {
            SYS,
            BUS,
            ERR
        }

        /// <summary>
        /// Ghi log với type
        /// </summary>
        /// <param name="header">Thông tin thêm</param>
        /// <param name="type">Loại level</param>
        /// <param name="ex">Exception</param>
        public static void WriteLog(string message, LogType type, Exception ex)
        {
            // log4net.ThreadContext.Properties["LogType"] = type.ToString();
            // log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\log.conf"));
            log4net.ILog log = log4net.LogManager.GetLogger(message);
            if (type.ToString().Equals("BUS"))
            {
                log = bus;
            }
            else if (type.ToString().Equals("ERR"))
            {
                log = err;
            }
            else if (type.ToString().Equals("SYS"))
            {
                log = sys;
            }
            else
            {
                log = sys;
            }

            //log4net.ILog log = log4net.LogManager.GetLogger(message);
            string exOuput = message + " " + ex.ToString();

            if (log.IsDebugEnabled)
            {
                log.Debug(exOuput);
            }
            else if (log.IsInfoEnabled)
            {
                log.Info(exOuput);
            }
            else if (log.IsWarnEnabled)
            {
                log.Warn(exOuput);
            }
            else if (log.IsErrorEnabled)
            {
                log.Error(exOuput);
            }
            else if (log.IsFatalEnabled)
            {
                log.Fatal(exOuput);
            }
            else 
            {
                log.Debug(exOuput);
            }
        }

        /// <summary>
        /// Ghi log với type
        /// </summary>
        /// <param name="header">Thông tin thêm</param>
        /// <param name="type">Loại level</param>
        /// <param name="ex">Exception</param>
        public static void WriteLog(string message, LogType type, string ex)
        {
            //log4net.ThreadContext.Properties["LogType"] = type.ToString();
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\log.conf"));
            //log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(message);
            if (type.ToString().Equals("BUS"))
            {
                log = bus;
            }
            else if (type.ToString().Equals("ERR"))
            {
                log = err;
            }
            else if (type.ToString().Equals("SYS"))
            {
                log = sys;
            }
            else
            {
                log = sys;
            }
            
            //log4net.ILog log = log4net.LogManager.GetLogger(message);
            string exOuput = message + " " + ex;

            if (log.IsDebugEnabled)
            {
                log.Debug(exOuput);
            }
            else if (log.IsInfoEnabled)
            {
                log.Info(exOuput);
            }
            else if (log.IsWarnEnabled)
            {
                log.Warn(exOuput);
            }
            else if (log.IsErrorEnabled)
            {
                log.Error(exOuput);
            }
            else if (log.IsFatalEnabled)
            {
                log.Fatal(exOuput);
            }
            else
            {
                
                log.Debug(exOuput);
            }
        }
    }
}


