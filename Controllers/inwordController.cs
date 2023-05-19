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
    public class inwordController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);

        public class inword_docs
        {
            public int id { get; set; }
            public string inword_no { get; set; }
            public string date { get; set; }
            public string docs_type { get; set; }
            public string name { get; set; }
            public string scheme_name { get; set; }
            public string unit_no { get; set; }
            public string pan_no { get; set; }
            public string adhar_number { get; set; }
            public string contact_number { get; set; }
            public string address { get; set; }
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
            public string inword_no { get; set; }
            public string date { get; set; }
            public string docs_type { get; set; }
            public string scheme_name { get; set; }
            public string unit_no { get; set; }
            public string name { get; set; }
            public string pan_no { get; set; }
            public string adhar_number { get; set; }
            public string contact_number { get; set; }
            public string address { get; set; }
        }
        RequestPartyMasterObj requestPartyMasterObj = new RequestPartyMasterObj();
        ResponseObj responseObj = new ResponseObj();

        [Route("api/inwords")]
        [HttpGet]
        public HttpResponseMessage Getparty()
        {
            SqlCommand cmd = new SqlCommand("select * from inword_docs", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("date");
            dt.Columns.Add("docs_type");
            dt.Columns.Add("inword_no");
            dt.Columns.Add("scheme_name");
            dt.Columns.Add("unit_no");
            dt.Columns.Add("name");
            dt.Columns.Add("pan_no");
            dt.Columns.Add("adhar_number");
            dt.Columns.Add("contact_number");
            dt.Columns.Add("address");
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
                        dt.Rows.Add(reader["id"].ToString(), reader["date"].ToString(), reader["docs_type"].ToString(), reader["inword_no"].ToString(),
                            reader["scheme_name"].ToString(), reader["unit_no"].ToString(), reader["name"].ToString(), reader["pan_no"].ToString(),
                            reader["adhar_number"].ToString(), reader["contact_number"].ToString(), reader["address"].ToString(), reader["created_at"].ToString(), reader["update_at"].ToString());
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

        [Route("api/inwords/{id}")]
        [HttpGet]
        public HttpResponseMessage Getparty(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from inword_docs where id='" + id + "'", cn);
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("date");
            dt.Columns.Add("docs_type");
            dt.Columns.Add("inword_no");
            dt.Columns.Add("scheme_name");
            dt.Columns.Add("unit_no");
            dt.Columns.Add("name");
            dt.Columns.Add("pan_no");
            dt.Columns.Add("adhar_number");
            dt.Columns.Add("contact_number");
            dt.Columns.Add("address");
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
                        dt.Rows.Add(reader["id"].ToString(), reader["date"].ToString(), reader["docs_type"].ToString(), reader["inword_no"].ToString(),
                            reader["scheme_name"].ToString(), reader["unit_no"].ToString(), reader["name"].ToString(), reader["pan_no"].ToString(),
                            reader["adhar_number"].ToString(), reader["contact_number"].ToString(), reader["address"].ToString(), reader["created_at"].ToString(), reader["update_at"].ToString());
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

        [Route("api/inwords")]
        [HttpPost]
        public HttpResponseMessage CreateParty([FromBody] RequestPartyMasterObj requestPartyMasterObj)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                int r = 1;
                cn.Open();
                SqlCommand scmd = new SqlCommand("Select max(inword_no) from inword_docs", cn);
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
                cmd.CommandText = "insert into inword_docs (inword_no,scheme_name,unit_no,name,date,docs_type,pan_no,adhar_number,contact_number,address,created_at,update_at) values " +
                    "('"+ r.ToString() +"','" + requestPartyMasterObj.scheme_name + "','" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "','" + requestPartyMasterObj.date + "','" + requestPartyMasterObj.docs_type + "','" + requestPartyMasterObj.pan_no + "'," +
                    "'" + requestPartyMasterObj.adhar_number + "','" + requestPartyMasterObj.contact_number + "','" + requestPartyMasterObj.address + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Inword added successfilly.";
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
