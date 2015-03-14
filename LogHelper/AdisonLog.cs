using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace LogHelper
{
    /// <summary>
    /// log 记录组建
    /// </summary>
    public class AdisonLog : ILogCommon
    {
        private static object Mux = "_LogInstance";
        private static object WriteMux = "_WriteMux";
        private static AdisonLog _LogInstance;
        public static AdisonLog LogInstance
        {
            get
            {
                lock (Mux)
                {
                    if (_LogInstance == null)
                    {
                        lock (Mux)
                        {
                            _LogInstance = new AdisonLog();
                            _LogInstance.CurrentLog = LogLevel.Debug;
                        }
                    }
                    return _LogInstance;
                }
            }
        }

        private LogLevel CurrentLog
        {
            get;
            set;
        }

        private AdisonLog() { }

        /// <summary>
        /// level 可取之 0,1,2
        /// </summary>
        /// <param name="level"></param>
        public void SetLogLevel(int level)
        {
            CurrentLog = (LogLevel)level;
        }

        public void ErrorLog(string error)
        {
            if (CurrentLog >= LogLevel.Error)
            {
                string message = string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), error);
                Console.WriteLine(message);
                WriteLogToStorage(LogLevel.Error, message);
            }
        }

        public void ErrorLog(string userdescription, Exception ex)
        {
            if (CurrentLog >= LogLevel.Error)
            {
                string message = string.Format("{0} : {1} {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), userdescription, ex.Message, ex.StackTrace);
                Console.WriteLine(message);
                WriteLogToStorage(LogLevel.Error, message);
            }
        }

        public void DebugLog(string debug)
        {
            if (CurrentLog >= LogLevel.Debug)
            {
                string message = string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), debug);
                Console.WriteLine(message);
                WriteLogToStorage(LogLevel.Debug, message);
            }
        }

        public void InfoLog(string info)
        {
            if (CurrentLog >= LogLevel.Info)
            {
                string message = string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), info);
                Console.WriteLine(message);
                WriteLogToStorage(LogLevel.Info, message);
            }
        }

        private void WriteLogToStorage(LogLevel levelenum, string message)
        {
            string filePath = string.Empty;
            switch (levelenum)
            {
                case LogLevel.Debug: filePath = ConfigurationManager.AppSettings["debugPath"]; break;
                case LogLevel.Error: filePath = ConfigurationManager.AppSettings["errorPath"]; break;
                case LogLevel.Info: filePath = ConfigurationManager.AppSettings["infoPath"]; break;
            }
            if (string.IsNullOrEmpty(filePath))
            { 
                throw new Exception("文件路径(debugPath errorPath infoPath)配置不正确");
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }
            lock (WriteMux)
            {
                using (StreamWriter sr = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    sr.WriteLine(message);
                }
            }
        }
    }

    /// <summary>
    /// 通用的日志组建接口
    /// </summary>
    public interface ILogCommon
    {
        void ErrorLog(string error);
        void ErrorLog(string userdescription, Exception ex);
        void DebugLog(string debug);
        void InfoLog(string info);
        void SetLogLevel(int level);
    }

    public enum LogLevel
    {
        Debug = 0,
        Info,
        Error
    }
}
