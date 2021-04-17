using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using pizzajo_api;

namespace pizzajo_api.Controllers
{
    public class FoodController : ApiController
    {
        // this class is used when all orders are fetched
        public class all_food
        {
            public string food_name { get; set; }
            public int food_price { get; set; }
            public string food_ingredients { get; set; }
            public byte[] food_picture { get; set; }
            public string error { get; set; }

            public all_food(string food_name, int food_price, string food_ingredients, byte[] food_picture, string error)
            {
                this.food_name = food_name;
                this.food_price = food_price;
                this.food_ingredients = food_ingredients;
                this.food_picture = food_picture;
                this.error = error;
            }
        }

        // GET: fetch all food (by category)
        public List<all_food> Get(int food_group_id)
        {
            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            if (food_group_id == 1) //pizza 
            {
                query.CommandText = "CALL `select_all_pizza`();";
            }
            if (food_group_id == 2) //hamburger 
            {
                query.CommandText = "CALL `select_all_hamburger`();";
            }
            if (food_group_id == 3) //gyros 
            {
                query.CommandText = "CALL `select_all_gyros`();";
            }


            var result = new List<all_food>();


            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {
                    byte[] picture = (byte[])fetch_query.GetValue(3); //4th column

                    result.Add(new all_food(fetch_query["food_name"].ToString(), Convert.ToInt32(fetch_query["food_price"]), fetch_query["food_ingredients"].ToString(), picture, ""));
                }
            }
            catch (MySqlException ex)
            {
                result.Add(new all_food(null, 0, null, null, ex.ToString()));
            }

            conn.Close();

            return result;
        }
    }
}
