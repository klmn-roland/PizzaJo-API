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
                result.Add(new all_orders(0, 0, 0, "", "", ' ', ex.ToString()));
            }

            conn.Close();

            return result;
        }

    }
}
