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
    public class mastersController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);

        public class party_master
        {
            public int id { get; set; }
            public string name_of_company { get; set; }
            public string name_of_scheme { get; set; }
            public string contact_person { get; set; }
            public string contact_number { get; set; }
            public string address { get; set; }
            public string no_of_units { get; set; }
            public string created_at { get; set; }
            public string update_at { get; set; }
        }

        public class ResponseObj
        {
            public int status { get; set; }
            public string message { get; set; }
            public DataTable data { get; set; }
        }

        public class RequestPartyMasterObj
        {
            public int id { get; set; }
            public string name_of_company { get; set; }
            public string name_of_scheme { get; set; }
            public string contact_person { get; set; }
            public string contact_number { get; set; }
            public string address { get; set; }
            public string no_of_units { get; set; }
        }
        RequestPartyMasterObj requestPartyMasterObj = new RequestPartyMasterObj();
        ResponseObj responseObj = new ResponseObj();

        [Route("api/masters/party")]
        [HttpGet]
        public HttpResponseMessage Getparty()
        {
            SqlCommand cmd = new SqlCommand("select * from party_master", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name_of_company");
            dt.Columns.Add("name_of_scheme");
            dt.Columns.Add("contact_person");
            dt.Columns.Add("contact_number");
            dt.Columns.Add("address");
            dt.Columns.Add("no_of_units");
            dt.Columns.Add("created_at");
            dt.Columns.Add("update_at");


            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["id"].ToString(), reader["name_of_company"].ToString(), reader["name_of_scheme"].ToString(), reader["contact_person"].ToString(),
                            reader["contact_number"].ToString(), reader["address"].ToString(), reader["no_of_units"].ToString(), reader["created_at"].ToString(), reader["update_at"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "Data found";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                else
                {
                    responseObj.status = 401;
                    responseObj.message = "Party data not found.";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
            }
            catch (Exception ex)
            {
                responseObj.status = 500;
                responseObj.message = "something went wrong.";
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }

        [Route("api/masters/party/{id}")]
        [HttpGet]
        public HttpResponseMessage Getparty(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from party_master where id='" + id + "'", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name_of_company");
            dt.Columns.Add("name_of_scheme");
            dt.Columns.Add("contact_person");
            dt.Columns.Add("contact_number");
            dt.Columns.Add("address");
            dt.Columns.Add("no_of_units");
            dt.Columns.Add("created_at");
            dt.Columns.Add("update_at");
            

            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dt.Rows.Add(reader["id"].ToString(), reader["name_of_company"].ToString(), reader["name_of_scheme"].ToString(), reader["contact_person"].ToString(),
                            reader["contact_number"].ToString(), reader["address"].ToString(), reader["no_of_units"].ToString(), reader["created_at"].ToString(), reader["update_at"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "Data found";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                else
                {
                    responseObj.status = 401;
                    responseObj.message = "Party data not found.";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
            }
            catch (Exception ex)
            {
                responseObj.status = 500;
                responseObj.message = "something went wrong.";
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }

        [Route("api/masters/party")]
        [HttpPost]
        public HttpResponseMessage CreateParty([FromBody] RequestPartyMasterObj requestPartyMasterObj)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "insert into party_master (name_of_company,name_of_scheme,contact_person,contact_number,address,no_of_units,created_at,update_at) values " +
                    "('" + requestPartyMasterObj.name_of_company + "','" + requestPartyMasterObj.name_of_scheme + "','" + requestPartyMasterObj.contact_person + "',"+
                    "'" + requestPartyMasterObj.contact_number + "','" + requestPartyMasterObj.address + "','" + requestPartyMasterObj.no_of_units + "','"+ DateTime.Now + "','" + DateTime.Now + "')";
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Party added successfilly.";
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            catch (Exception ex)
            {
                responseObj.status = 500;
                responseObj.message = "something went wrong.";
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
