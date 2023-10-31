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
        SqlConnection cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["database_ConnectionString"].ConnectionString);
        CommonVeriables commonVerb = new CommonVeriables();
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
            public CommonVeriables.ResponseMeta meta { get; set; }
        }


        public class RequestPartyMasterObj
        {
            public int id{ get; set; }
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
        public class listInwordResObj
        {
            public int id { get; set; }
            public string inword_no { get; set; }
            public string date { get; set; }
            public string docs_type { get; set; }
            public object scheme_name;
            public string scheme_name_id { get; set; }
            public string unit_no { get; set; }
            public string name { get; set; }
            public string pan_no { get; set; }
            public string adhar_number { get; set; }
            public string contact_number { get; set; }
            public string status { get; set; }
            public string address { get; set; }
            public string docs_id { get; set; }
            public string created_at { get; set; }
            public string update_at { get; set; }
        }
        public class ResponseObjnew
        {
            public int status { get; set; }
            public string message { get; set; }
            public List<listInwordResObj> data { get; set; }
            public CommonVeriables.ResponseMeta meta { get; set; }
        }

        RequestPartyMasterObj requestPartyMasterObj = new RequestPartyMasterObj();
        ResponseObj responseObj = new ResponseObj();
        ResponseObjnew responseObjNew = new ResponseObjnew();
        CommonVeriables.ResponseMeta responseMeta = new CommonVeriables.ResponseMeta();

        [Route("api/inwords")]
        [HttpGet]
        public HttpResponseMessage Getparty(string search = "", int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            string SQL = "select * from inword_docs where unit_no like '%" + search + "%' or name like '%" + search + "%'" +
                "or contact_number like '%" + search + "%' ";
            String SQLOrderBy = "ORDER BY created_at ASC ";
            String limitedSQL = commonVerb.GetPaginatedSQL(skip, pageSize, SQL, SQLOrderBy);
            SqlCommand cmd = new SqlCommand(limitedSQL, cn);
            DataTable dt = new DataTable();

            int totalRows = 0;
            cn1.Open();
            SqlCommand countcmd = new SqlCommand(SQL, cn1);
            SqlDataReader countsdr = countcmd.ExecuteReader();
            if (countsdr.HasRows)
            {
                while (countsdr.Read())
                {
                    totalRows++;
                }

            }
            countcmd.Dispose();
            cn1.Close();

            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<listInwordResObj> data = new List<listInwordResObj>();
                    while (reader.Read())
                    {
                        string strintId = reader["scheme_name"].ToString();

                        string name_of_scheme = "";
                        cn1.Open();
                        SqlCommand scmd = new SqlCommand("select * from party_master where id = '" + strintId.ToString() + "' ", cn1);
                        SqlDataReader sdr = scmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            name_of_scheme = sdr["name_of_scheme"].ToString();
                        }
                        cn1.Close();

                        listInwordResObj LlistInwordResObj = new listInwordResObj();
                        LlistInwordResObj.id = Convert.ToInt32(reader["id"]);
                        LlistInwordResObj.date = reader["date"].ToString();
                        LlistInwordResObj.docs_type = reader["docs_type"].ToString();
                        LlistInwordResObj.inword_no = reader["inword_no"].ToString();
                        LlistInwordResObj.scheme_name = name_of_scheme;
                        LlistInwordResObj.unit_no = reader["unit_no"].ToString();
                        LlistInwordResObj.name = reader["name"].ToString();
                        LlistInwordResObj.pan_no = reader["pan_no"].ToString();
                        LlistInwordResObj.adhar_number = reader["adhar_number"].ToString();
                        LlistInwordResObj.contact_number = reader["contact_number"].ToString();
                        LlistInwordResObj.address = reader["address"].ToString();
                        LlistInwordResObj.created_at = reader["created_at"].ToString();
                        LlistInwordResObj.update_at = reader["update_at"].ToString();
                        LlistInwordResObj.status = reader["status"].ToString();
                        LlistInwordResObj.docs_id = reader["docs_id"].ToString();

                        data.Add(LlistInwordResObj);
                    }
                    responseMeta.per_page = pageSize;
                    responseMeta.current_page = page;
                    responseMeta.last_page = totalRows / pageSize;
                    responseMeta.total = totalRows;
                    responseMeta.current_page_record = data.Count;

                    responseObjNew.status = 200;
                    responseObjNew.message = "Data found";
                    responseObjNew.data = data.ToList();
                    responseObjNew.meta = responseMeta;
                    return Request.CreateResponse(HttpStatusCode.OK, responseObjNew);
                    
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
            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<listInwordResObj> data = new List<listInwordResObj>();
                    while (reader.Read())
                    {
                        string strintId = reader["scheme_name"].ToString();

                        string name_of_scheme = "";
                        cn1.Open();
                        SqlCommand scmd = new SqlCommand("select * from party_master where id = '" + strintId.ToString() + "' ", cn1);
                        SqlDataReader sdr = scmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            name_of_scheme = sdr["name_of_scheme"].ToString();
                        }
                        cn1.Close();
                        //dt.Rows.Add(reader["id"].ToString(), reader["date"].ToString(), reader["docs_type"].ToString(), reader["inword_no"].ToString(),
                        //    reader["scheme_name"].ToString(), reader["unit_no"].ToString(), reader["name"].ToString(), reader["pan_no"].ToString(),
                        //    reader["adhar_number"].ToString(), reader["contact_number"].ToString(), reader["address"].ToString(), reader["created_at"].ToString(), reader["update_at"].ToString());

                        listInwordResObj LlistInwordResObj = new listInwordResObj();
                        LlistInwordResObj.id = Convert.ToInt32(reader["id"]);
                        LlistInwordResObj.date = reader["date"].ToString();
                        LlistInwordResObj.docs_type = reader["docs_type"].ToString();
                        LlistInwordResObj.inword_no = reader["inword_no"].ToString();
                        LlistInwordResObj.scheme_name = name_of_scheme;
                        LlistInwordResObj.scheme_name_id = strintId;
                        LlistInwordResObj.unit_no = reader["unit_no"].ToString();
                        LlistInwordResObj.name = reader["name"].ToString();
                        LlistInwordResObj.pan_no = reader["pan_no"].ToString();
                        LlistInwordResObj.adhar_number = reader["adhar_number"].ToString();
                        LlistInwordResObj.contact_number = reader["contact_number"].ToString();
                        LlistInwordResObj.address = reader["address"].ToString();
                        LlistInwordResObj.created_at = reader["created_at"].ToString();
                        LlistInwordResObj.update_at = reader["update_at"].ToString();
                        data.Add(LlistInwordResObj);
                    }
                    responseObjNew.status = 200;
                    responseObjNew.message = "Data found";
                    responseObjNew.data = data.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, responseObjNew);
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
                cmd.CommandText = "insert into inword_docs "+
                    "(inword_no,scheme_name,unit_no,name,date,docs_type,pan_no,adhar_number,contact_number,address,created_at,update_at) values " +
                    "('"+ r.ToString() +"','" + requestPartyMasterObj.scheme_name + "','" + requestPartyMasterObj.unit_no + "','" + requestPartyMasterObj.name + "','" + requestPartyMasterObj.date + "',"+
                    "'" + requestPartyMasterObj.docs_type + "','" + requestPartyMasterObj.pan_no + "','" + requestPartyMasterObj.adhar_number + "',"+
                    "'" + requestPartyMasterObj.contact_number + "','" + requestPartyMasterObj.address + "','" + DateTime.Now + "','" + DateTime.Now + "')";
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

        [Route("api/inwords/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateParty(int id, [FromBody] RequestPartyMasterObj requestPartyMasterObj)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = "update inword_docs set " +
                    "scheme_name='"+ requestPartyMasterObj.scheme_name + "',"+
                    "unit_no='" + requestPartyMasterObj.unit_no + "'," +
                    "name='" + requestPartyMasterObj.name + "'," +
                    "date='" + requestPartyMasterObj.date + "'," +
                    "docs_type='" + requestPartyMasterObj.docs_type + "'," +
                    "pan_no='" + requestPartyMasterObj.pan_no + "'," +
                    "adhar_number='" + requestPartyMasterObj.adhar_number + "'," +
                    "contact_number='" + requestPartyMasterObj.contact_number + "'," +
                    "address='" + requestPartyMasterObj.address + "'," +
                    "update_at='" + DateTime.Now + "' where id='"+ requestPartyMasterObj.id + "'";

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

        [Route("api/inwords/unit_no")]
        [HttpGet]
        public HttpResponseMessage GetunitNo()
        {
            string schemeId = "";
            var paramsD = Request.RequestUri.Query;
            string[] paramsDArray = paramsD.Split('?');
            string searchQuery = "";
            if (paramsDArray.Length > 1)
            {
                string[] paramsSplitComa = paramsDArray[1].Split('&');
                string[] paramsFinal = paramsSplitComa[0].Split('=');
                searchQuery = paramsFinal[1].ToString();
                if (paramsSplitComa.Length > 1)
                {
                    string[] paramsFinalSchemeId = paramsSplitComa[1].Split('=');
                    if (paramsFinalSchemeId.Length > 1)
                    {
                        schemeId = paramsFinalSchemeId[1].ToString();
                    }
                }
            }
            SqlCommand cmd = new SqlCommand("select * from inword_docs where (unit_no like '%" + searchQuery + "%') and scheme_name='" + schemeId.ToString() + "'", cn);
            DataTable dt = new DataTable();
            try
            {
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<listInwordResObj> data = new List<listInwordResObj>();
                    while (reader.Read())
                    {
                        
                        listInwordResObj LlistInwordResObj = new listInwordResObj();
                        LlistInwordResObj.id = Convert.ToInt32(reader["id"]);
                        LlistInwordResObj.date = reader["date"].ToString();
                        LlistInwordResObj.docs_type = reader["docs_type"].ToString();
                        LlistInwordResObj.inword_no = reader["inword_no"].ToString();
                        LlistInwordResObj.scheme_name = reader["scheme_name"].ToString();
                        LlistInwordResObj.unit_no = reader["unit_no"].ToString();
                        LlistInwordResObj.name = reader["name"].ToString();
                        LlistInwordResObj.pan_no = reader["pan_no"].ToString();
                        LlistInwordResObj.adhar_number = reader["adhar_number"].ToString();
                        LlistInwordResObj.contact_number = reader["contact_number"].ToString();
                        LlistInwordResObj.address = reader["address"].ToString();
                        LlistInwordResObj.created_at = reader["created_at"].ToString();
                        LlistInwordResObj.update_at = reader["update_at"].ToString();

                        data.Add(LlistInwordResObj);
                    }
                    responseObjNew.status = 200;
                    responseObjNew.message = "Inward Update Succesfully!";
                    responseObjNew.data = data.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, responseObjNew);

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
    }
}
