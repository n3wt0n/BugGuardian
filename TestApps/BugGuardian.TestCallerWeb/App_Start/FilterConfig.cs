using DBTek.BugGuardian.TestCallerWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBTek.BugGuardian.TestCallerWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            //OPTION 1
            filters.Add(new BugGuardianFilter());
        }
    }
}