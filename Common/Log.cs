using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Log
    {
        static string logDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        static string logName = string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        static string logpath = System.IO.Path.Combine(logDir, logName);
        private static readonly Object thisLock = new Object();
        public static void Error(string msg)
        {
            CreateLogFile();
            lock (thisLock)
            {
                using (StreamWriter SW = File.AppendText(logpath))
                {
                    SW.WriteLine(buildMessage(msg));
                    SW.Close();
                }
            }

        }


        public static void Error(string tag, string msg)
        {
            CreateLogFile();
            lock (thisLock)
            {
                using (StreamWriter SW = File.AppendText(logpath))
                {
                    SW.WriteLine(buildMessage(tag, msg));
                    SW.Close();
                }
            }

        }
        private static void CreateLogFile()
        {
            if (!System.IO.Directory.Exists(logDir))
            {
                System.IO.Directory.CreateDirectory(logDir);
            }
            if (!File.Exists(logpath))
            {
                StreamWriter SW;
                SW = File.CreateText(logpath);
                SW.WriteLine("Log created at: " +
                                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SW.Close();
            }
        }
        private static string buildMessage(string msg)
        {
            StackTrace trace = new StackTrace();
            StringBuilder sb = new StringBuilder();
            MethodBase methodName = null;
            if (trace.FrameCount > 1)
            {
                methodName = trace.GetFrame(2).GetMethod();
            }
            else
            {
                methodName = trace.GetFrame(1).GetMethod();
            }
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append("[").Append(methodName.ReflectedType.Name).Append(".").Append(methodName.Name).Append("]");
            sb.Append(msg);

            return sb.ToString();
        }

        private static string buildMessage(string tag, string msg)
        {
            StackTrace trace = new StackTrace();
            StringBuilder sb = new StringBuilder();
            MethodBase methodName = null;
            if (trace.FrameCount > 1)
            {
                methodName = trace.GetFrame(2).GetMethod();
            }
            else
            {
                methodName = trace.GetFrame(1).GetMethod();
            }
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append("[").Append(tag).Append("]");
            sb.Append("[");
            sb.Append("[").Append(methodName.ReflectedType.Name).Append(".").Append(methodName.Name).Append("]");

            sb.Append(msg);

            return sb.ToString();
        }
    }
}
