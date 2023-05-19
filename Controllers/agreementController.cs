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
    public class agreementController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        string paramsNames = "agreement_id,date,scheme_name,unit_no,name,inword_no,is_signed,pde_number,token_date_and_time,register_number,register_date,delivery_date,delivery_person,file_url,status,created_at,update_at";
        public class inword_docs
        {
            public int id { get; set; }
            public string agreement_id { get; set; }
            public string date { get; set; }
            public string scheme_name { get; set; }
            public string unit_no { get; set; }
            public string name { get; set; }
            public string inword_no { get; set; }
            public string is_signed { get; set; }
            public string pde_number { get; set; }
            public string token_date_and_time { get; set; }
            public string register_number { get; set; }
            public string register_date { get; set; }
            public string delivery_date { get; set; }
            public string delivery_person { get; set; }
            public string file_url { get; set; }
            public string status { get; set; }
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
            public string agreement_id { get; set; }
            public string date { get; set; }
            public string scheme_name { get; set; }
            public string unit_no { get; set; }
            public string name { get; set; }
            public string inword_no { get; set; }
            public string is_signed { get; set; }
            public string pde_number { get; set; }
            public string token_date_and_time { get; set; }
            public string register_number { get; set; }
            public string register_date { get; set; }
            public string delivery_date { get; set; }
            public string delivery_person { get; set; }
            public string file_url { get; set; }
            public string status { get; set; }
        }
        RequestPartyMasterObj requestPartyMasterObj = new RequestPartyMasterObj();
        ResponseObj responseObj = new ResponseObj();

        [Route("api/agreement")]
        [HttpGet]
        public HttpResponseMessage Getparty()
        {
            SqlCommand cmd = new SqlCommand("select * from agreement", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("agreement_id");
            dt.Columns.Add("date");
            dt.Columns.Add("scheme_name");
            dt.Columns.Add("unit_no");
            dt.Columns.Add("name");
            dt.Columns.Add("inword_no");
            dt.Columns.Add("is_signed");
            dt.Columns.Add("pde_number");
            dt.Columns.Add("token_date_and_time");
            dt.Columns.Add("register_number");
            dt.Columns.Add("register_date");
            dt.Columns.Add("delivery_date");
            dt.Columns.Add("delivery_person");
            dt.Columns.Add("file_url");
            dt.Columns.Add("status");
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
                        dt.Rows.Add(reader["id"].ToString(), reader["agreement_id"].ToString(),
                                    reader["date"].ToString(), reader["scheme_name"].ToString(),
                                    reader["unit_no"].ToString(), reader["name"].ToString(),
                                    reader["inword_no"].ToString(), reader["is_signed"].ToString(), 
                                    reader["pde_number"].ToString(), reader["token_date_and_time"].ToString(),
                                    reader["register_number"].ToString(), reader["register_date"].ToString(),
                                    reader["delivery_date"].ToString(), reader["delivery_person"].ToString(),
                                    reader["file_url"].ToString(), reader["status"].ToString(),
                                    reader["created_at"].ToString(),reader["update_at"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "Agreement found";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                else
                {
                    responseObj.status = 401;
                    responseObj.message = "Agreement data not found.";
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

        [Route("api/agreement/{id}")]
        [HttpGet]
        public HttpResponseMessage Getparty(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from agreement where id='" + id + "'", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("agreement_id");
            dt.Columns.Add("date");
            dt.Columns.Add("scheme_name");
            dt.Columns.Add("unit_no");
            dt.Columns.Add("name");
            dt.Columns.Add("inword_no");
            dt.Columns.Add("is_signed");
            dt.Columns.Add("pde_number");
            dt.Columns.Add("token_date_and_time");
            dt.Columns.Add("register_number");
            dt.Columns.Add("register_date");
            dt.Columns.Add("delivery_date");
            dt.Columns.Add("delivery_person");
            dt.Columns.Add("file_url");
            dt.Columns.Add("status");
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
                        dt.Rows.Add(reader["id"].ToString(), reader["agreement_id"].ToString(),
                                    reader["date"].ToString(), reader["scheme_name"].ToString(),
                                    reader["unit_no"].ToString(), reader["name"].ToString(),
                                    reader["inword_no"].ToString(), reader["is_signed"].ToString(),
                                    reader["pde_number"].ToString(), reader["token_date_and_time"].ToString(),
                                    reader["register_number"].ToString(), reader["register_date"].ToString(),
                                    reader["delivery_date"].ToString(), reader["delivery_person"].ToString(),
                                    reader["file_url"].ToString(), reader["status"].ToString(),
                                    reader["created_at"].ToString(), reader["update_at"].ToString());
                    }
                    responseObj.status = 200;
                    responseObj.message = "Agreement found";
                    responseObj.data = dt;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObj);
                }
                else
                {
                    responseObj.status = 401;
                    responseObj.message = "Agreement data not found.";
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

        [Route("api/agreement")]
        [HttpPost]
        public HttpResponseMessage CreateParty([FromBody] RequestPartyMasterObj requestPartyMasterObj)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                int r = 1;
                cn.Open();
                SqlCommand scmd = new SqlCommand("Select max(agreement_id) from agreement", cn);
                SqlDataReader sdr = scmd.ExecuteReader();
                if (sdr.Read())
                {
                    string d = sdr[0].ToString();
                    if (d == "")
                    {

                    }
                    else
                    {
                        r = Convert.ToInt32(sdr[0].ToString());
                        r = r + 1;
                    }

                }
                cn.Close();

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "insert into agreement (" + paramsNames + ") values " +
                    "('" + r.ToString() + "','" + requestPartyMasterObj.scheme_name + "',"+
                    "'" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "',"+
                    "'" + requestPartyMasterObj.inword_no + "','" + requestPartyMasterObj.is_signed + "',"+
                    "'" + requestPartyMasterObj.pde_number + "','" + requestPartyMasterObj.token_date_and_time + "',"+
                    "'" + requestPartyMasterObj.register_number + "','" + requestPartyMasterObj.register_date + "'," +
                    "'" + requestPartyMasterObj.delivery_date + "','" + requestPartyMasterObj.delivery_person + "'," +
                    "'" + requestPartyMasterObj.file_url + "','" + requestPartyMasterObj.status + "',"+
                    "'" + DateTime.Now + "','" + DateTime.Now + "')";
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Agreement added successfilly.";
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            catch (Exception ex)
            {
                responseObj.status = 500;
                responseObj.message = "something went wrong." + ex.ToString();
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
