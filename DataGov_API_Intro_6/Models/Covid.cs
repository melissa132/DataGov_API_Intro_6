using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataGov_API_Intro_6.Models
{
    public class Covid
    {
        public int county_fips { get; set; }
        public string? county_name { get; set; }
        public string? county_name2 { get; set; }
        public decimal covid_case_per_100k { get; set; }
    }
    public class Health
    {
        public int health_service_num { get; set; }
        public string? health_service_area { get; set; }        
        public decimal health_service_pop { get; set; }
    }

}
