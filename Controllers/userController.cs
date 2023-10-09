using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace associet_backend.Controllers
{
    public class userController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        CommonVeriables commonVerb = new CommonVeriables();
        public class user_master
        {
            public int id { get; set; }
            public string user_name { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }

        public class ResponseObj
        {
            public int status { get; set; }
            public string message { get; set; }
            public DataTable user { get; set; }
        }

        public class RequestLoginObj
        {
            public string user_name { get; set; }
            public string password { get; set; }
        }
        RequestLoginObj requestLoginObj = new RequestLoginObj();
        ResponseObj responseObj = new ResponseObj();

        [HttpGet]
        public HttpResponseMessage Get(string search = "", int page = 1, int pageSize = 10)
        {
            SqlCommand cmd = new SqlCommand("select * from user_master", cn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("user_name");
            dt.Columns.Add("password");
            dt.Columns.Add("role");

            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(reader["id"].ToString(), reader["user_name"].ToString(), reader["password"].ToString(), reader["role"].ToString());
                }
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //sda.Fill(ds);
            }
            catch (Exception ex)
            {
                //...
            }
            finally
            {
                cn.Close();
            }
            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found.");
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from user_master where id='"+ id +"'", cn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("user_name");
            dt.Columns.Add("password");
            dt.Columns.Add("role");

            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["id"].ToString(), reader["user_name"].ToString(), reader["password"].ToString(), reader["role"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "User found";
                    responseObj.user = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                else
                {
                    responseObj.status = 401;
                    responseObj.message = "User not found.";
                    responseObj.user = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
            }
            catch (Exception ex)
            {
                responseObj.status = 500;
                responseObj.message = "something went wrong.";
                responseObj.user = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }

        [Route("api/user/login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody] RequestLoginObj requestLoginObj)
        {
            SqlCommand cmd = new SqlCommand("select * from user_master where user_name='"+ requestLoginObj.user_name+ "' and password='"+ requestLoginObj.password+"'", cn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("user_name");
            dt.Columns.Add("password");
            dt.Columns.Add("role");

            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["id"].ToString(), reader["user_name"].ToString(), reader["password"].ToString(), reader["role"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "Login success.";
                    responseObj.user = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                } else
                {
                    responseObj.status = 401;
                    responseObj.message = "User and password incorrect. try again.";
                    responseObj.user = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //sda.Fill(ds);
            }
            catch (Exception ex)
            {
                //...
                responseObj.status = 500;
                responseObj.message = "something went wrong.";
                responseObj.user = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }

    }
}
