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
    public class AdminController : ApiController
    {
        // this class is used when all orders are fetched
        public class all_admin
        {
            public string user { get; set; }
            public string pass { get; set; }
            public string error { get; set; }

            public all_admin(string user, string pass, string error)
            {
                this.user = user;
                this.pass = pass;
                this.error = error;
            }
        }

        // POST: set_admin
        public string Post(string pass, string user)
        {
            pass = TrimAndQuote(pass);
            user = TrimAndQuote(user);

            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "CALL `set_admin`(" + pass + "," + user + ");";

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

            return "new admin created";
        }

        // GET: fetch all admins
        public List<all_admin> Get()
        {
            MySqlConnection conn = WebApiConfig.conn();

            MySqlCommand query = conn.CreateCommand();

            query.CommandText = "SELECT * FROM admin WHERE 1;";

            var result = new List<all_admin>();

            try
            {
                conn.Open();
                MySqlDataReader fetch_query = query.ExecuteReader();

                while (fetch_query.Read())
                {

                    result.Add(new all_admin(fetch_query["username"].ToString(), fetch_query["password"].ToString(), ""));
                }
            }
            catch (MySqlException ex)
            {
                result.Add(new all_admin(null, null, ex.ToString()));
            }

            conn.Close();

            return result;
        }

        //Trim and quote data for mysql server
        private string TrimAndQuote(string input)
        {
            return '"' + input.Trim() + '"';
        }
    }
}
