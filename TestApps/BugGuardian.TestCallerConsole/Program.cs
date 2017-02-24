using System;
using System.Collections.Generic;

namespace DBTek.BugGuardian.TestCallerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += BugGuardianExceptionTrapper;

            //throw new Exception();

            //System.IO.File.Open(@"C:\NonExistentFile.docx", System.IO.FileMode.Open);

            string aaaaa = null;
            aaaaa.Substring(0, 12);

            //string aaaaa = "sss";
            //aaaaa.Substring(0, 12);

            //var innerExceptions = new List<Exception>();
            //innerExceptions.Add(new ArgumentNullException("FakeParam"));
            //innerExceptions.Add(new System.IO.FileNotFoundException("Missing file: ", @"c:\fakefile.doc"));
            //var aex = new AggregateException("This doesn't work", innerExceptions);
            //throw aex;            
        }

        static void BugGuardianExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            BugGuardian.Factories.ConfigurationFactory.SetConfiguration("http://MY_TFS_SERVER:8080/Tfs", "MY_USERNAME", "MY_PASSWORD", "MY_PROJECT");
            using (var manager = new BugGuardianManager())
            {
                manager.AddBug(e.ExceptionObject as Exception, message: "Unknown exception", tags: new List<string> { "Operation" });

                manager.AddTask(e.ExceptionObject as Exception, message: "Unknown exception", tags: new List<string> { "Operation" });
            }
        }
    }
}
