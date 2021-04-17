using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using pizzajo_api;

namespace PizzaJo_API.Controllers
{
    public class AddressController : ApiController
    {
        // this class is used when all orders are fetched
        public class all_address
        {
            public int house_number { get; set; }
            public string street_name { get; set; }
            public string city_name { get; set; }
            public string zipcode { get; set; }
            public string error { get; set; }

            public all_address(int house_number, string street_name, string city_name, string zipcode, string error)
            {
                this.house_number = house_number;
                this.street_name = street_name;
                this.city_name = city_name;
                this.zipcode = zipcode;
                this.error = error;
            }
        }

        // GET: fetch all food (by category)
        public List<all_address> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `getAddress`();";
           
            var result = new List<all_address>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {

                    result.Add(new all_address(Convert.ToInt32(fetch_query["house_number"]), fetch_query["street_name"].ToString(), fetch_query["city_name"].ToString(), fetch_query["zipcode"].ToString(), ""));
                }
            }
            catch (MySqlException ex)
            {
                result.Add(new all_address(0, null, null, null, ex.ToString()));
            }

            conn.Close();

            return result;
        }
    }
}
