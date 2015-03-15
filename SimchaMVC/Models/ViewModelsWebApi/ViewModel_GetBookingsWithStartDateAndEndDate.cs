using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModel_GetBookingsWithStartDateAndEndDate
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int?[] Hall_Ids { get; set; }
        public string ZipCode { get; set; }
        public int? ZipAprox { get; set; }
        public int?[] LocationIds { get; set; }
        public int?[] SimchaTypeIds { get; set; }
        public int? Capacity { get; set; }
   
        
    }
}