using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugGuardian.TestCaller
{
    class Program
    {
        static void Main(string[] args)
        {
            var creator = new DBTek.BugGuardian.Creator();

            //creator.AddBug(new Exception());

            //try
            //{
            //    System.IO.File.Open(@"C:\NonExistentFile.docx", System.IO.FileMode.Open);
            //}
            //catch (Exception ex)
            //{
            //    creator.AddBug(ex);               
            //}

            //try
            //{
            //    string aaaaa = null;
            //    aaaaa.Substring(0, 12);
            //}
            //catch (Exception ex)
            //{
            //    creator.AddBug(ex);                
            //}


            //try
            //{
            //    string aaaaa = "sss";
            //    aaaaa.Substring(0, 12);
            //}
            //catch (Exception ex)
            //{
            //    creator.AddBug(ex);
            //}

            var innerExceptions = new List<Exception>();
            innerExceptions.Add(new ArgumentNullException("FakeParam"));
            innerExceptions.Add(new System.IO.FileNotFoundException("Missing file: ", @"c:\fakefile.doc"));
            var aex = new AggregateException("This doesn't work", innerExceptions);
            creator.AddBug(aex);
            

            Console.WriteLine("Done");
            Console.ReadLine();
        }       
    }
}
