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
            public int customer_customer_id { get; set; }
            public int payment_payment_id { get; set; }
            public string order_time { get; set; }
            public string order_note { get; set; }
            public char order_status { get; set; }
            public string error { get; set; }

            public all_orders(int order_id, int customer_customer_id, int payment_payment_id, string order_time,
                string order_note, char order_status, string error)
            {
                this.order_id = order_id;
                this.customer_customer_id = customer_customer_id;
                this.payment_payment_id = payment_payment_id;
                this.order_time = order_time;
                this.order_note = order_note;
                this.order_status = order_status;
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
                    result.Add(new all_orders(Convert.ToInt32(fetch_query["order_id"]), Convert.ToInt32(fetch_query["customer_customer_id"]), Convert.ToInt32(fetch_query["payment_payment_id"]), fetch_query["order_time"].ToString(), fetch_query["order_note"].ToString(), Convert.ToChar(fetch_query["order_status"]), ""));
                }
            }
            catch (MySqlException ex)
            {
                result.Add(new all_orders(0,0,0, "", "", ' ', ex.ToString()));
            }

            conn.Close();

            return result;
        }

        // POST: change order status
        public string Post(string orderId, string status)
        {
            orderId = orderId.Trim();
            status = status.Trim();

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `set_orderStatus`(" + orderId + ","+ status + ");";

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

            return "Order #" + orderId + " status changed to " + status;
        }

        // POST: create order_food
        public string Post(string foodID, string quan, string price)
        {
            foodID = foodID.Trim();
            quan = quan.Trim();
            price = price.Trim();

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `create_order_food`(" + foodID + "," + quan + "," + price + ");";

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

            return "order_food created";
        }

        // POST: create orderWithCustomer
        public string Post(string fname, string lname, string phone, string email, string zip, string cityN, string streetN, string hnumber, string amount, string type, string note)
        {
            fname = TrimAndQuote(fname);
            lname = TrimAndQuote(lname);
            phone = TrimAndQuote(phone);
            email = TrimAndQuote(email);
            zip = TrimAndQuote(zip);
            cityN = TrimAndQuote(cityN);
            streetN = TrimAndQuote(streetN); ;
            // hnumber is an integer and works without trimming
            // amount is an integer and works without trimming
            hnumber = hnumber.Trim();
            streetN = streetN.Trim();
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

        // DELETE: delete order
        public string Delete(int id)
        {
            return "deleted: " + id;
        }
        private string TrimAndQuote(string input)
        {
            return '"' + input.Trim() + '"';
        }
    }
}
