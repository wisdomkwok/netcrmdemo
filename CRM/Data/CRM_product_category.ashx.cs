using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using XHD.Common;

namespace XHD.CRM.Data
{
    /// <summary>
    /// CRM_product_category 的摘要说明
    /// </summary>
    public class CRM_product_category : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_product_category ccpc = new BLL.CRM_product_category();
            Model.CRM_product_category model = new Model.CRM_product_category();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                string parentid = request["T_category_parent_val"];
                model.parentid = int.Parse(parentid);
                model.product_category = Common.PageValidate.InputText(request["T_category_name"], 250);
                model.product_icon = Common.PageValidate.InputText(request["T_category_icon"], 250);

                string id = request["id"];
                string pid = request["T_category_parent_val"];
                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    model.id = int.Parse(id);

                    DataSet ds = ccpc.GetList(" id=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (int.Parse(id) == int.Parse(pid))
                    {
                        context.Response.Write("false:type");
                    }
                    else
                    {
                        ccpc.Update(model);


                        //日志
                        C_Sys_log log = new C_Sys_log();

                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = model.product_category;
                        string EventType = "产品类别修改";
                        int EventID = model.id;
                        if (dr["product_category"].ToString() != request["T_category_name"])
                        {
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "产品类别", dr["product_category"].ToString(), request["T_category_name"]);
                        }
                        if (dr["product_icon"].ToString() != request["T_category_icon"])
                        {
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "类别图标", dr["product_icon"].ToString(), request["T_category_icon"]);
                        }
                        if (dr["parentid"].ToString() != request["T_category_parent_val"])
                        {
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "上级类别", dr["parentid"].ToString(), request["T_category_parent_val"]);
                        }
                    }
                }

                else
                {
                    model.isDelete = 0;
                    ccpc.Add(model);
                }
            }

            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " id";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = null;
                string serchtype = request["isdel"];
                if (serchtype == "1")
                {
                    serchtxt += " isDelete=1 ";
                }
                else
                {
                    serchtxt += " isDelete=0 ";
                }
                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and product_category like N'%" + request["company"] + "%'";

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                {
                    serchtxt += " and Delete_time >= '" + request["startdate_del"] + "'";
                }
                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]);
                    serchtxt += " and Delete_time  <= '" + enddate.AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                //权限
                

                string dt = "";
                if (request["grid"] == "tree")
                {
                    DataSet ds = ccpc.GetList(0, serchtxt, sorttext);
                    dt = "{Rows:[" + GetTasksString(0, ds.Tables[0]) + "]}";
                }
                else
                {  
                    DataSet ds = ccpc.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
                    dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                }
                context.Response.Write(dt);
            }
            if (request["Action"] == "tree")
            {
                DataSet ds = ccpc.GetList(" isDelete=0 ");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",pid:" + ds.Tables[0].Rows[i]["parentid"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["product_category"] + "',d_icon:'../../" + ds.Tables[0].Rows[i]["product_icon"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            if (request["Action"] == "combo")
            {
                DataSet ds = ccpc.GetList(" isDelete=0");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append("{id:0,pid:0,text:'无',d_icon:''},");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",pid:" + ds.Tables[0].Rows[i]["parentid"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["product_category"] + "',d_icon:'" + ds.Tables[0].Rows[i]["product_icon"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            if (request["Action"] == "form")
            {
                int cid = int.Parse(request["id"]);
                DataSet ds = ccpc.GetList("id=" + cid);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }

            //del
            if (request["Action"] == "AdvanceDelete")
            {
                //参数安全过滤
                string c_id = request["id"];

                DataSet ds = ccpc.GetList(" id=" + int.Parse(c_id));

                BLL.CRM_product product = new BLL.CRM_product();
                if (product.GetList(" category_id=" + int.Parse(c_id)).Tables[0].Rows.Count > 0)
                {
                    context.Response.Write("false:product");
                }
                else if(ccpc.GetList("parentid="+int.Parse(c_id)).Tables[0].Rows.Count>0){
                    context.Response.Write("false:parent");
                }
                else
                {
                    bool isdel = ccpc.AdvanceDelete(int.Parse(c_id), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (isdel)
                    {
                        //日志

                        string EventType = "产品类别预删除";

                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(c_id);
                        string EventTitle = ds.Tables[0].Rows[0]["product_category"].ToString();
                        string Original_txt = null;
                        string Current_txt = null;

                        C_Sys_log log = new C_Sys_log();

                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);

                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }

            }
            //regain            
            if (request["Action"] == "regain")
            {
                string idlist = PageValidate.InputText( request["idlist"],100000);
                string[] arr = idlist.Split(',');

                DataSet ds = ccpc.GetList("id in (" + idlist.Trim() + ")");

                for (int i = 0; i < arr.Length; i++)
                {
                    ccpc.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {
                    string EventType = "恢复删除产品类别";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int UserID = emp_id;
                        string UserName = empname;
                        int EventID = idlist[i];
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = ds.Tables[0].Rows[i]["product_category"].ToString();
                        string Original_txt = null;
                        string Current_txt = null;

                        C_Sys_log log = new C_Sys_log();
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                    }

                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }

            }
            ////del
            if (request["Action"] == "del")
            {

                bool canDel = false;
                if (dsemp.Tables[0].Rows.Count > 0)
                {
                    if (dsemp.Tables[0].Rows[0]["uid"].ToString() == "admin")
                    {
                        canDel = true;
                    }
                    else
                    {
                        Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "74");
                        if (delauth == "false")
                            canDel = false;
                        else
                            canDel = true;
                    }
                }
                if (canDel)
                {
                    string idlist = PageValidate.InputText( request["idlist"],100000);
                    string[] arr = idlist.Split(',');

                    string EventType = "彻底删除产品类别";

                    DataSet ds = ccpc.GetList("id in (" + idlist.Trim() + ")");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        ccpc.Delete(int.Parse(arr[i]));
                    }

                    if (true)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = idlist[i];
                            string EventTitle = ds.Tables[0].Rows[i]["product_category"].ToString();
                            string Original_txt = null;
                            string Current_txt = null;
                            C_Sys_log log = new C_Sys_log();
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                        }
                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
                else
                {
                    context.Response.Write("auth");
                }
            }

        }
        private static string GetTasksString(int Id, DataTable table)
        {
            DataRow[] rows = table.Select("parentid=" + Id.ToString());

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{");
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (i != 0) str.Append(",");
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append(":'");
                    str.Append(row[i].ToString());
                    str.Append("'");
                }
                if (GetTasksString((int)row["id"], table).Length > 0)
                {
                    str.Append(",children:[");
                    str.Append(GetTasksString((int)row["id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}