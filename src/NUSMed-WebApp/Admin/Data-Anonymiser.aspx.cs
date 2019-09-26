using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUSMed_WebApp.Classes.BLL;

namespace NUSMed_WebApp.Admin
{
  public partial class Data_Anonymiser : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void LinkButtonGenerate_Click(object sender, EventArgs e)
    {
      DataBLL dataBLL = new DataBLL();

      dataBLL.InsertAnonymizedTableToDb();
    }
  }
}
