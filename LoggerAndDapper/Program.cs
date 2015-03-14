using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogHelper;
using System.Threading;

namespace LoggerAndDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                ThreadModel th = new ThreadModel(); 
                taskList.Add(Task.Factory.StartNew(th.Test)); 
            }
            Task.WaitAll(taskList.ToArray());
            Console.ReadKey();

        }
    }


    class ThreadModel
    {  
        public void Test()
        {
            ILogCommon Ilog = AdisonLog.LogInstance;
            Ilog.SetLogLevel(2);
            Thread.Sleep(new Random().Next(1, 100));
            Ilog.ErrorLog(Thread.CurrentThread.ManagedThreadId.ToString(), new Exception("bad index1 " + Thread.CurrentThread.ManagedThreadId));
            Thread.Sleep(new Random().Next(1, 100));
            Ilog.ErrorLog(Thread.CurrentThread.ManagedThreadId.ToString(), new Exception("bad index2 " + Thread.CurrentThread.ManagedThreadId));
            Thread.Sleep(new Random().Next(1, 100));
            Ilog.ErrorLog(Thread.CurrentThread.ManagedThreadId.ToString(), new Exception("bad index3 " + Thread.CurrentThread.ManagedThreadId));
            Thread.Sleep(new Random().Next(1, 100));
            Ilog.ErrorLog(Thread.CurrentThread.ManagedThreadId.ToString(), new Exception("bad index4 " + Thread.CurrentThread.ManagedThreadId));

        }
    }
}
