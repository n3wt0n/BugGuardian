using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BugGuardian.TestCallerWeb.WebForms
{
    public partial class PageWithError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            int i = int.Parse("this is not an int");
        }      
    }
}