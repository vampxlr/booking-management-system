using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelAddBooking
    {
        public booking booking { get; set; }
        public calendar calendar { get; set; }
        public time_slots time_slots { get; set; }
        public hall hall { get; set; }
    
    }
}