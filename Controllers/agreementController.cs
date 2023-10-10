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
        SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        CommonVeriables commonVerb = new CommonVeriables();
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
            public CommonVeriables.ResponseMeta meta { get; set; }
        }

        public class RequestPartyMasterObj
        {
            public string id { get; set; }
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
        public class RequestPartyMasterObjCreate
        {
            //public string agreement_id { get; set; }
            public string date { get; set; }
            public string scheme_name { get; set; }
            public string unit_no { get; set; }
            public string name { get; set; }
            public string inword_no { get; set; }
            public string is_signed { get; set; }
        }
        RequestPartyMasterObj requestPartyMasterObj = new RequestPartyMasterObj();
        RequestPartyMasterObjCreate requestPartyMasterObjCreate = new RequestPartyMasterObjCreate();
        ResponseObj responseObj = new ResponseObj();
        CommonVeriables.ResponseMeta responseMeta = new CommonVeriables.ResponseMeta();

        [Route("api/agreement")]
        [HttpGet]
        public HttpResponseMessage Getparty(string search = "", int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            string SQL = "select * from agreement where date like '%" + search + "%' or pde_number like '%" + search + "%'" +
                "or token_date_and_time like '%" + search + "%' ";
            String SQLOrderBy = "ORDER BY created_at ASC ";
            String limitedSQL = commonVerb.GetPaginatedSQL(skip, pageSize, SQL, SQLOrderBy);
            SqlCommand cmd = new SqlCommand(limitedSQL, cn);
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
                        string strintId = reader["scheme_name"].ToString();

                        string name_of_scheme = "";
                        string unit_no = "";
                        cn1.Open();
                        SqlCommand scmd = new SqlCommand("select * from party_master where id = '" + strintId.ToString() + "' ", cn1);
                        SqlDataReader sdr = scmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            name_of_scheme = sdr["name_of_scheme"].ToString();
                        }
                        scmd.Dispose();
                        cn1.Close();
                        cn1.Open();
                        SqlCommand inwardcmd = new SqlCommand("select * from inword_docs where id = '" + reader["unit_no"].ToString() + "' ", cn1);
                        SqlDataReader inwarddr = inwardcmd.ExecuteReader();
                        if (inwarddr.Read())
                        {
                            unit_no = inwarddr["unit_no"].ToString();
                        }
                        cn1.Close();
                        dt.Rows.Add(reader["id"].ToString(), reader["agreement_id"].ToString(),
                                    reader["date"].ToString(), name_of_scheme.ToString(),
                                    unit_no.ToString(), reader["name"].ToString(),
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
            dt.Columns.Add("scheme_id");
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
                        string strintId = reader["scheme_name"].ToString();

                        string name_of_scheme = "";
                        string unit_no = "";
                        cn1.Open();
                        SqlCommand scmd = new SqlCommand("select * from party_master where id = '" + strintId.ToString() + "' ", cn1);
                        SqlDataReader sdr = scmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            name_of_scheme = sdr["name_of_scheme"].ToString();
                        }
                        scmd.Dispose();
                        cn1.Close();
                        cn1.Open();
                        SqlCommand inwardcmd = new SqlCommand("select * from inword_docs where id = '" + reader["unit_no"].ToString() + "' ", cn1);
                        SqlDataReader inwarddr = inwardcmd.ExecuteReader();
                        if (inwarddr.Read())
                        {
                            unit_no = inwarddr["unit_no"].ToString();
                        }
                        cn1.Close();
                        dt.Rows.Add(reader["id"].ToString(), reader["agreement_id"].ToString(),
                                    reader["date"].ToString(), name_of_scheme.ToString(), strintId.ToString(),
                                    unit_no.ToString(), reader["name"].ToString(),
                                    reader["unit_no"].ToString(), reader["is_signed"].ToString(),
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
        public HttpResponseMessage CreateAgreement([FromBody] RequestPartyMasterObjCreate requestPartyMasterObj)
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
                string status = "pending";
                if (requestPartyMasterObj.is_signed == "true")
                {
                    status = "signed";
                }
                string paramsNamesc = "agreement_id,date,scheme_name,unit_no,name,inword_no,is_signed,status,created_at,update_at";
                //string commond = "insert into agreement (" + paramsNamesc + ") values " +
                //    "('" + r.ToString() + "','" + requestPartyMasterObj.date + "','" + requestPartyMasterObj.scheme_name + "'," +
                //    "'" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "'," +
                //    "'" + requestPartyMasterObj.inword_no + "','" + requestPartyMasterObj.is_signed + "'," +
                //    "'" + status.ToString() + "'," +
                //    "'" + DateTime.Now + "','" + DateTime.Now + "')";
                //if (requestPartyMasterObj.pde_number != ""&& requestPartyMasterObj.token_date_and_time != "" )
                //{
                //    paramsNames = "agreement_id,date,scheme_name,unit_no,name,inword_no,is_signed,pde_number,token_date_and_time,register_number,register_date,delivery_date,delivery_person,file_url,status,created_at,update_at";
                //    commond = "insert into agreement (" + paramsNames + ") values " +
                //    "('" + r.ToString() + "','" + requestPartyMasterObj.scheme_name + "'," +
                //    "'" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "'," +
                //    "'" + requestPartyMasterObj.inword_no + "','" + requestPartyMasterObj.is_signed + "'," +
                //    "'" + requestPartyMasterObj.pde_number + "','" + requestPartyMasterObj.token_date_and_time + "'," +
                //    "'" + requestPartyMasterObj.register_number + "','" + requestPartyMasterObj.register_date + "'," +
                //    "'" + requestPartyMasterObj.delivery_date + "','" + requestPartyMasterObj.delivery_person + "'," +
                //    "'" + requestPartyMasterObj.file_url + "','" + requestPartyMasterObj.status + "'," +
                //    "'" + DateTime.Now + "','" + DateTime.Now + "')";
                //}

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "insert into agreement (" + paramsNamesc + ") values " +
                    "('" + r.ToString() + "','" + requestPartyMasterObj.date + "','" + requestPartyMasterObj.scheme_name + "'," +
                    "'" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "'," +
                    "'" + requestPartyMasterObj.inword_no + "','" + requestPartyMasterObj.is_signed + "'," +
                    "'" + status.ToString() + "'," +
                    "'" + DateTime.Now + "','" + DateTime.Now + "')";
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Agreement added successfilly.";
                responseObj.data = dt;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "update inword_docs set (status,docs_id,update_at) values " +
                    "('" + status.ToString() + "','" + r.ToString() + "','" + DateTime.Now + "') where inword_no='"+ requestPartyMasterObj.inword_no + "'";
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
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

        [Route("api/agreement/{id}")]
        [HttpPut]
        public HttpResponseMessage updateAgreement(int id, [FromBody] RequestPartyMasterObj requestPartyMasterObj)
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
                string status = "pending";
                if (requestPartyMasterObj.is_signed == "true" && requestPartyMasterObj.pde_number == "" && requestPartyMasterObj.token_date_and_time == "")
                {
                    status = "signed";
                }else if (requestPartyMasterObj.is_signed == "true" && requestPartyMasterObj.pde_number != "" && requestPartyMasterObj.token_date_and_time != ""
                    && requestPartyMasterObj.register_number == "" && requestPartyMasterObj.register_date == "")
                {
                    status = "token_done";
                } else if (requestPartyMasterObj.register_number != "" && requestPartyMasterObj.register_date != "" && requestPartyMasterObj.delivery_date == "" && requestPartyMasterObj.delivery_person == "")
                {
                    status = "registered";
                }
                else if (requestPartyMasterObj.delivery_date != "" && requestPartyMasterObj.delivery_person != "")
                {
                    status = "deliver";
                }

                string commond = "update agreement set "+
                    "is_signed='" + requestPartyMasterObj.is_signed + "'," +
                    "pde_number='" + requestPartyMasterObj.pde_number + "',"+
                    "token_date_and_time='" + requestPartyMasterObj.token_date_and_time + "'," +
                    "register_number='" + requestPartyMasterObj.register_number + "'," +
                    "register_date='" + requestPartyMasterObj.register_date + "'," +
                    "delivery_date='" + requestPartyMasterObj.delivery_date + "'," +
                    "delivery_person='" + requestPartyMasterObj.delivery_person + "'," +
                    "status='" + status.ToString() + "'," +
                    "update_at='" + DateTime.Now + "'"+
                    "where id='" + requestPartyMasterObj.id + "'";
                //if (requestPartyMasterObj.pde_number != ""&& requestPartyMasterObj.token_date_and_time != "" )delivery_date,delivery_person
                //{
                //    paramsNames = "agreement_id,date,scheme_name,unit_no,name,inword_no,is_signed,pde_number,token_date_and_time,register_number,register_date,delivery_date,delivery_person,file_url,status,created_at,update_at";
                //    commond = "insert into agreement (" + paramsNames + ") values " +
                //    "('" + r.ToString() + "','" + requestPartyMasterObj.scheme_name + "'," +
                //    "'" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "'," +
                //    "'" + requestPartyMasterObj.inword_no + "','" + requestPartyMasterObj.is_signed + "'," +
                //    "'" + requestPartyMasterObj.pde_number + "','" + requestPartyMasterObj.token_date_and_time + "'," +
                //    "'" + requestPartyMasterObj.register_number + "','" + requestPartyMasterObj.register_date + "'," +
                //    "'" + requestPartyMasterObj.delivery_date + "','" + requestPartyMasterObj.delivery_person + "'," +
                //    "'" + requestPartyMasterObj.file_url + "','" + requestPartyMasterObj.status + "'," +
                //    "'" + DateTime.Now + "','" + DateTime.Now + "')";
                //}

                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = commond;
                cmd.ExecuteNonQuery();
                cmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Agreement Update successfilly.";
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
