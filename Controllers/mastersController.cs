using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace associet_backend.Controllers
{
    public class mastersController : ApiController
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        CommonVeriables commonVerb = new CommonVeriables();

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
            public CommonVeriables.ResponseMeta meta { get; set; }
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
        CommonVeriables.ResponseMeta responseMeta = new CommonVeriables.ResponseMeta();



        [Route("api/masters/party")]
        [HttpGet]
        public HttpResponseMessage Getparty(string search = "", int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            string SQL = "select * from party_master where name_of_company like '%" + search + "%' or name_of_scheme like '%" + search + "%'" +
                "or contact_person like '%" + search + "%' or contact_number like '%" + search + "%' ";
            String SQLOrderBy = "ORDER BY created_at ASC ";
            String limitedSQL = commonVerb.GetPaginatedSQL(skip, pageSize, SQL, SQLOrderBy);
            SqlCommand cmd = new SqlCommand(limitedSQL, cn);
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

            int totalRows = 0;
            cn1.Open();
            SqlCommand scmd = new SqlCommand(SQL, cn1);
            SqlDataReader sdr = scmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    totalRows++;
                }

            }
            scmd.Dispose();
            cn1.Close();

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

                    responseMeta.per_page = pageSize;
                    responseMeta.current_page = page;
                    responseMeta.last_page = totalRows / pageSize;
                    responseMeta.total = totalRows;
                    responseMeta.current_page_record = dt.Rows.Count;

                    responseObj.status = 200;
                    responseObj.message = "Data found";
                    responseObj.data = dt;
                    responseObj.meta = responseMeta;
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
                responseObj.message = "something went wrong." + ex.ToString();
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
                responseObj.message = "something went wrong." + ex.ToString();
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
                //int r = 1;
                //cn.Open();
                //SqlCommand scmd = new SqlCommand("Select max(id) from party_master", cn);
                //SqlDataReader sdr = scmd.ExecuteReader();
                //if (sdr.Read())
                //{
                //    string d = sdr[0].ToString();
                //    if (d == "")
                //    {

                //    }
                //    else
                //    {
                //        r = Convert.ToInt32(sdr[0].ToString());
                //        r = r + 1;
                //    }

                //}
                //cn.Close();


                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "insert into party_master (name_of_company,name_of_scheme,contact_person,contact_number,address,no_of_units,created_at,update_at) values " +
                    "('" + requestPartyMasterObj.name_of_company + "','" + requestPartyMasterObj.name_of_scheme + "','" + requestPartyMasterObj.contact_person + "'," +
                    "'" + requestPartyMasterObj.contact_number + "','" + requestPartyMasterObj.address + "','" + Convert.ToInt32(requestPartyMasterObj.no_of_units) + "','" + DateTime.Now + "','" + DateTime.Now + "')";


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
                responseObj.message = "something went wrong." + ex.ToString();
                responseObj.data = dt;
                return Request.CreateResponse(HttpStatusCode.OK, responseObj);
            }
            finally
            {
                cn.Close();
            }
        }

        [Route("api/masters/party/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateParty(int id, [FromBody] RequestPartyMasterObj requestPartyMasterObj)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                SqlCommand ucmd = new SqlCommand("UPDATE party_master SET name_of_company=@name_of_company,name_of_scheme=@name_of_scheme,contact_person=@contact_person," +
                    "contact_number=@contact_number,address=@address,no_of_units=@no_of_units,update_at=@update_at  Where  id='" + id + "'", cn);

                ucmd.Parameters.Add("@name_of_company", SqlDbType.NVarChar).Value = requestPartyMasterObj.name_of_company;
                ucmd.Parameters.Add("@name_of_scheme", SqlDbType.NVarChar).Value = requestPartyMasterObj.name_of_scheme;
                ucmd.Parameters.Add("@contact_person", SqlDbType.NVarChar).Value = requestPartyMasterObj.contact_person;
                ucmd.Parameters.Add("@contact_number", SqlDbType.NVarChar).Value = requestPartyMasterObj.contact_number;
                ucmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = requestPartyMasterObj.address;
                ucmd.Parameters.Add("@no_of_units", SqlDbType.NVarChar).Value = requestPartyMasterObj.no_of_units;
                ucmd.Parameters.Add("@update_at", SqlDbType.NVarChar).Value = DateTime.Now;

                ucmd.Connection = cn;
                cn.Open();
                ucmd.ExecuteNonQuery();
                ucmd.Clone();
                cn.Close();
                responseObj.status = 200;
                responseObj.message = "Party details updated successfilly.";
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

        [Route("api/masters/schemes")]
        [HttpGet]
        public HttpResponseMessage GetSchemes()
        {
            var paramsD = Request.RequestUri.Query;
            string[] paramsDArray = paramsD.Split('?');
            string searchQuery = "";
            if (paramsDArray.Length > 1)
            {
                string[] paramsSplitComa = paramsDArray[1].Split(',');
                string[] paramsFinal = paramsDArray[1].Split('=');
                searchQuery = paramsFinal[1].ToString();
            }
            SqlCommand cmd = new SqlCommand("select * from party_master where name_of_scheme like '%" + searchQuery + "%'", cn);
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
