using DataGov_API_Intro_6.Models;
using Microsoft.AspNetCore.Mvc;
using DataGov_API_Intro_6.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataGov_API_Intro_6.Controllers
{
    public class HomeController : Controller
    {
        HttpClient httpClient;

        static string BASE_URL = "https://developer.nps.gov/api/v1";
        static string API_KEY = "mdBybOievMdeX3eYSC0MhFu3U7xRV18xHAPG04qb"; //Add your API key here inside ""
        string constr = "Server=tcp:ism6225-group2.database.windows.net,1433;Initial Catalog=TravelTampa;Persist Security Info=False;User ID=traveltpa;Password=Bolts2022;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm

        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NATIONAL_PARK_API_PATH = BASE_URL + "/parks?limit=20";
            string parksData = "";

            Parks parks = null;

            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);

            try
            {
                //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                //                                        .GetAwaiter().GetResult();
                HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!parksData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    parks = JsonConvert.DeserializeObject<Parks>(parksData);
                }

                dbContext.Parks.Add(parks);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            // Table to store the query results
            DataTable table = new DataTable();

            // Creates a SQL connection
            using (var connection = new SqlConnection("Data Source = ism6225-group2.database.windows.net;initial Catalog=TravelTampa;User ID=traveltpa;Password=Bolts2022"))
            {
                connection.Open();
                // Creates a SQL command
                using (var command = new SqlCommand("SELECT * from vw_county_info", connection))
                {
                    // Loads the query results into the table
                    table.Load(command.ExecuteReader());
                }
                connection.Close();
            }

                //return View(parks);
                return View(table);
        }

        public async Task<IActionResult> CovidResources()
        {

            List<Health> health = new List<Health>();
            //string query = "SELECT * FROM vw_county_info";
            string query = "SELECT * FROM health_service_area";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            health.Add(new Health
                            {
                                health_service_num = Convert.ToInt32(sdr["health_service_num"]),
                                health_service_area = Convert.ToString(sdr["health_service_area"]),
                                health_service_pop = Convert.ToDecimal(sdr["health_service_pop"])
                            }); ;
                        }
                    }
                    con.Close();
                }
            }
            return View(health);
        }

        [HttpPost]
        public ActionResult UpdateCovid([FromBody] Health health)
        {
            string query = "UPDATE health_service_area SET health_service_area=@health_service_area WHERE health_service_num=@health_service_num";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {                    
                    cmd.Parameters.AddWithValue("@health_service_area", health.health_service_area);
                    cmd.Parameters.AddWithValue("@health_service_num", health.health_service_num);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }            
            return Json("");
        }

        [HttpPost]
        public JsonResult InsertCovid([FromBody] Health health)
        {
            string query = "INSERT INTO health_service_area (health_service_num,health_service_area,health_service_pop,covid_inpatient_bed_utilization,covid_hospital_admissions_per_100k,create_date) VALUES(888,@health_service_area,0,0,0,@create_date)";
            //query += "SELECT SCOPE_IDENTITY()";            
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@health_service_area", health.health_service_area);
                    cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }            
            return Json(health);
        }

        [HttpPost]
        public IActionResult DeleteCovid([FromBody] int HealthId)
        {
            string query = "DELETE FROM health_service_area WHERE health_service_num=@health_service_num";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@health_service_num", HealthId);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }            

            return Json("");
        }

        public async Task<IActionResult> Interest()
        {
            return View();
        }

        public async Task<IActionResult> Recommendations()
        {
            return View();
        }
    }
}