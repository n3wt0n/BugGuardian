using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.TestCallerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += BugGuardianExceptionTrapper;

            //throw new Exception();

            //System.IO.File.Open(@"C:\NonExistentFile.docx", System.IO.FileMode.Open);

            //string aaaaa = null;
            //aaaaa.Substring(0, 12);

            string aaaaa = "sss";
            aaaaa.Substring(0, 12);

            //var innerExceptions = new List<Exception>();
            //innerExceptions.Add(new ArgumentNullException("FakeParam"));
            //innerExceptions.Add(new System.IO.FileNotFoundException("Missing file: ", @"c:\fakefile.doc"));
            //var aex = new AggregateException("This doesn't work", innerExceptions);
            //throw aex;            
        }

        static void BugGuardianExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            using (var creator = new DBTek.BugGuardian.Creator())
            {                
                creator.AddBug(e.ExceptionObject as Exception);
            }
        }

    }
}
