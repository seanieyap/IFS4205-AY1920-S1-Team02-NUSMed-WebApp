using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Therapist.My_Patients
{
    public partial class View : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyPatients();
            Master.LiActiveTherapistMyPatientsView();

        }
    }
}