using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace pizzajo_api.Controllers
{
    public class OrderController : ApiController
    {
        //this class is used when all orders are fetched
        public class all_orders
        {
            public int order_id { get; set; }
            public string order_time { get; set; }
            public string order_note { get; set; }
            public char order_status { get; set; }
            public string food_name { get; set; }
            public string food_group_name { get; set; }
            public int order_item_quantity { get; set; }
            public int order_item_price { get; set; }
            public string customer_fname { get; set; }
            public string customer_lname { get; set; }
            public string customer_phone { get; set; }
            public string zipcode { get; set; }
            public string city_name { get; set; }
            public string street_name { get; set; }
            public int house_number { get; set; }
            public int payment_amount { get; set; }
            public char payment_type { get; set; }
            public string error { get; set; }

            public all_orders(int order_id, string order_time, string order_note, char order_status, string food_name, string food_group_name, int order_item_quantity, int order_item_price, string customer_fname, string customer_lname, string customer_phone, string zipcode, string city_name, string street_name, int house_number, int payment_amount, char payment_type, string error)
            {
                this.order_id = order_id;
                this.order_time = order_time;
                this.order_note = order_note;
                this.order_status = order_status;
                this.food_name = food_name;
                this.food_group_name = food_group_name;
                this.order_item_quantity = order_item_quantity;
                this.order_item_price = order_item_price;
                this.customer_fname = customer_fname;
                this.customer_lname = customer_lname;
                this.customer_phone = customer_phone;
                this.zipcode = zipcode;
                this.city_name = city_name;
                this.street_name = street_name;
                this.house_number = house_number;
                this.payment_amount = payment_amount;
                this.payment_type = payment_type;
                this.error = error;
            }
        }

        // GET: fetch all orders
        public List<all_orders> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `select_all_order`();";

            var result = new List<all_orders>();


            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {
                    result.Add(new all_orders(Convert.ToInt32(fetch_query["order_id"]),
                    fetch_query["order_time"].ToString(),
                    fetch_query["order_note"].ToString(),
                    Convert.ToChar(fetch_query["order_status"]),
                    fetch_query["food_name"].ToString(),
                    fetch_query["food_group_name"].ToString(),
                    Convert.ToInt32(fetch_query["order_item_quantity"]),
                    Convert.ToInt32(fetch_query["order_item_price"]),
                    fetch_query["customer_fname"].ToString(),
                    fetch_query["customer_lname"].ToString(),
                    fetch_query["customer_phone"].ToString(),
                    fetch_query["zipcode"].ToString(),
                    fetch_query["city_name"].ToString(),
                    fetch_query["street_name"].ToString(),
                    Convert.ToInt32(fetch_query["house_number"]),
                    Convert.ToInt32(fetch_query["payment_amount"]),
                    Convert.ToChar(fetch_query["payment_type"]),
                    ""));
                }
            }
            catch (MySqlException ex)
            {
                //int , string , string , char , string , int , int , string , string , string , string , string , string , int , int , char
                result.Add(new all_orders(0, null, null, ' ', null, null, 0, 0, null, null, null, null, null, null, 0, 0, ' ', ex.ToString()));
            }

            conn.Close();

            return result;
        }

        // POST: change order status
        public string Post(string id, string status)
        {
            id = id.Trim();
            status = status.Trim();

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `set_orderStatus`(" + id + "," + status + ");";

            try
            {
                conn.Open();
                query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return "error: " + ex.ToString();
            }

            conn.Close();

            return "Order #" + id + " status changed to " + status;
        }

        // POST: create_order_item
        public string Post(string foodID, string quan, string price)
        {
            foodID = foodID.Trim();
            quan = quan.Trim();
            price = price.Trim();

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `create_order_item`(" + foodID + "," + quan + "," + price + ");";

            try
            {
                conn.Open();
                query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return "error: " + ex.ToString();
            }

            conn.Close();

            return "order_item created";
        }

        // POST: create orderWithCustomer
        public string Post(string fname, string lname, string phone, string email, string zip, string cityN, string streetN, int hnumber, int amount, string type, string note)
        {
            fname = TrimAndQuote(fname);
            lname = TrimAndQuote(lname);
            phone = TrimAndQuote(phone);
            email = TrimAndQuote(email);
            zip = TrimAndQuote(zip);
            cityN = TrimAndQuote(cityN);
            streetN = TrimAndQuote(streetN);
            //hnumber is an integer and works without trimming
            //streetN is an integer and works without trimming
            type = TrimAndQuote(type);
            note = TrimAndQuote(note);

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `create_orderWithCustomer`(" + fname + "," + lname + "," + phone + "," + email + "," + zip + "," + cityN + "," + streetN + "," + hnumber + "," + amount + "," + type + "," + note + ");";

            try
            {
                conn.Open();
                query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return "error: " + ex.ToString();
            }

            conn.Close();

            return "orderWithCustomer created";
        }

        //Trim and quote data for mysql server
        private string TrimAndQuote(string input)
        {
            return '"' + input.Trim() + '"';
        }

        // DELETE: delete_order
        public string Delete(int id)
        {
            //delete order_item
            //delete order

            //fontos a sorrend !

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();


            try
            {
                conn.Open();


                query.CommandText = "CALL `delete_order_item`(" + id + ");";
                query.ExecuteNonQuery();


                query.CommandText = "CALL `delete_order`(" + id + ");";
                query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return "error: " + ex.ToString();
            }

            conn.Close();

            return "order and related order items deleted";
        }
    }
}
