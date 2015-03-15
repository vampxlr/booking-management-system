using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelSearchCriteriaBookAdd
    {
        public DateTime search_date { get; set; }
        public int zip_code { get; set; }
        public int miles { get; set; }
        public string hall_name { get; set; }
        public string service_area { get; set; }
        public string type_name { get; set; }
    }
}