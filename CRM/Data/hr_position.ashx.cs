using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;

namespace XHD.CRM.Data
{
    /// <summary>
    /// hr_position 的摘要说明
    /// </summary>
    public class hr_position : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.hr_position zw = new BLL.hr_position();
            Model.hr_position model = new Model.hr_position();

            BLL.hr_employee emp = new BLL.hr_employee();
            int empid = int.Parse(request.Cookies["UserID"].Value.ToString());
            DataSet dsemp = emp.GetList("id=" + empid);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();
            if (request["Action"] == "grid")
            {
                string serchtxt = "";
                string serchtype = request["isdel"];
                if (serchtype == "1")
                {
                    serchtxt += "  isDelete=1";
                }
                else
                {
                    serchtxt += "  isDelete=0 ";
                }
                DataSet ds = zw.GetList(0, serchtxt, "convert(int,[position_order])");
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }             

            //save
            if (request["Action"] == "save")
            {
                model.position_name = Common.PageValidate.InputText(request["T_position"], 255);
                model.position_order = request["T_order"];
                model.position_level = request["T_level"];

                string id = Common.PageValidate.InputText(request["id"], 250);

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    model.id = int.Parse(id);
                    DataSet ds = zw.GetList(" id=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];
                    zw.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = empid;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.position_name;
                    string EventType = "职位修改";
                    int EventID = model.id;

                    if (dr["position_name"].ToString() != request["T_position"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "职务名称", dr["position_name"].ToString(), request["T_position"]);
                    }
                    if (dr["position_level"].ToString() != request["T_level"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "职务级别", dr["position_level"].ToString(), request["T_level"]);
                    }
                    if (dr["position_order"].ToString() != request["T_order"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "行号", dr["position_order"].ToString(), request["T_order"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.create_id = empid;
                    model.create_date = DateTime.Now;
                    zw.Add(model);
                }
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                int id = Common.PageValidate.IsNumber(request["id"]) ? int.Parse(request["id"]) : -1;

                DataSet ds = zw.GetList("id=" + id);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "AdvanceDelete")
            {
                int id = Common.PageValidate.IsNumber(request["id"]) ? int.Parse(request["id"]) : -1;
                string EventType = "职务预删除";
                DataSet ds = zw.GetList(" id=" + id);
                if (emp.GetList("zhiwuid=" + id).Tables[0].Rows.Count > 0)
                {
                    //含有员工信息不能删除
                    context.Response.Write("false:emp");
                }
                else
                {
                    bool isdel = zw.AdvanceDelete(int.Parse(request["id"]), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (isdel)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = empid;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = id;
                            string EventTitle = ds.Tables[0].Rows[i]["position_name"].ToString();
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
            }

            if (request["Action"] == "regain")
            {
                string idlist = Common.PageValidate.InputText( request["idlist"],100000);
                string[] arr = idlist.Split(',');

                DataSet ds = zw.GetList("id in (" + idlist.Trim() + ")");

                for (int i = 0; i < arr.Length; i++)
                {
                    zw.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {
                    //日志

                    string EventType = "恢复删除职位"; 

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int UserID = empid;
                        string UserName = empname;
                        int EventID = idlist[i];
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = ds.Tables[0].Rows[i]["position_name"].ToString();
                        string Original_txt = null;
                        string Current_txt = null;

                        C_Sys_log log = new C_Sys_log();
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                    }

                    context.Response.Write("true");
                }                    
            }

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
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "80");
                        if (delauth == "false")
                            canDel = false;
                        else
                            canDel = true;
                    }
                }
                if (canDel)
                {
                    string idlist = Common.PageValidate.InputText( request["idlist"],100000);
                    string[] arr = idlist.Split(',');

                    string EventType = "彻底删除职位";

                    DataSet ds = zw.GetList("id in (" + idlist.Trim() + ")");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        zw.Delete(int.Parse(arr[i]));
                    }

                    if (true)
                    {
                        //日志    
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = empid;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = idlist[i];
                            string EventTitle = ds.Tables[0].Rows[0]["position_name"].ToString();
                            string Original_txt = null;
                            string Current_txt = null;

                            C_Sys_log log = new C_Sys_log();

                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                        }
                        context.Response.Write("true");
                    }
                }
                else
                {
                    context.Response.Write("auth");
                }
            }


            if (request["Action"] == "combo")
            {
                DataSet ds = zw.GetList(0, " isDelete=0 or isDelete is null ", "position_level");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["position_name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);

            }
            if (request["Action"] == "getlevel")
            {
                int position_id = int.Parse(request["position_id"]);

                BLL.hr_position hz = new BLL.hr_position();
                DataSet ds = hz.GetList("id=" + position_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write(ds.Tables[0].Rows[0]["position_level"]);
                }
                else
                {
                    context.Response.Write("-1");
                }
            }
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