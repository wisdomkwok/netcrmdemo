using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Data;
using XHD.Common;
using System.Text;

namespace XHD.CRM.Data
{
    /// <summary>
    /// CRM_Contact 的摘要说明
    /// </summary>
    public class CRM_Contact : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_Contact contact = new BLL.CRM_Contact();
            Model.CRM_Contact model = new Model.CRM_Contact();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                string customerid = request["T_company_val"];

                model.C_customerid = int.Parse(customerid);
                model.C_customername = Common.PageValidate.InputText(request["T_company"], 250);
                model.C_name = Common.PageValidate.InputText(request["T_contact"], 250);
                model.C_sex = Common.PageValidate.InputText(request["T_sex"], 250);
                model.C_birthday = Common.PageValidate.InputText(request["T_birthday"], 250);
                model.C_department = Common.PageValidate.InputText(request["T_dep"], 250);
                model.C_position = Common.PageValidate.InputText(request["T_position"], 250);

                model.C_tel = Common.PageValidate.InputText(request["T_tel"], 250);
                model.C_mob = Common.PageValidate.InputText(request["T_mobil"], 250);
                model.C_fax = Common.PageValidate.InputText(request["T_fax"], 250);
                model.C_email = Common.PageValidate.InputText(request["T_email"], 250);
                model.C_QQ = Common.PageValidate.InputText(request["T_qq"], 250);
                model.C_add = Common.PageValidate.InputText(request["T_add"], 250);

                model.C_hobby = Common.PageValidate.InputText(request["T_hobby"], 250);
                model.C_remarks = Common.PageValidate.InputText(request["T_remarks"], 250);

                string contact_id = request["contact_id"];
                if (!string.IsNullOrEmpty(contact_id) && contact_id != "null")
                {
                    DataSet ds = contact.GetList("id=" + int.Parse(contact_id));
                    DataRow dr = ds.Tables[0].Rows[0];

                    model.C_createId = int.Parse(ds.Tables[0].Rows[0]["C_createId"].ToString());
                    model.C_createDate = DateTime.Parse(ds.Tables[0].Rows[0]["C_createDate"].ToString());
                    model.id = int.Parse(contact_id);

                    contact.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.C_name; ;
                    string EventType = "联系人修改";
                    int EventID = model.id;

                    if (dr["C_customername"].ToString() != request["T_company"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "公司名称", dr["C_customer_name"].ToString(), request["T_company"]);
                    }
                    if (dr["C_name"].ToString() != request["T_contact"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人", dr["C_name"].ToString(), request["T_contact"]);
                    }
                    if (dr["C_sex"].ToString() != request["T_sex"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人性别", dr["C_sex"].ToString(), request["T_sex"]);
                    }
                    if (dr["C_birthday"].ToString() != request["T_birthday"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人生日", dr["C_birthday"].ToString(), request["T_birthday"]);
                    }
                    if (dr["C_department"].ToString() != request["T_dep"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人部门", dr["C_department"].ToString(), request["T_dep"]);
                    }
                    if (dr["C_position"].ToString() != request["T_position"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人职位", dr["C_position"].ToString(), request["T_position"]);
                    }
                    if (dr["C_tel"].ToString() != request["T_tel"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人电话", dr["C_tel"].ToString(), request["T_tel"]);
                    }
                    if (dr["C_mob"].ToString() != request["T_mobil"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人手机", dr["C_mob"].ToString(), request["T_mobil"]);
                    }
                    if (dr["C_fax"].ToString() != request["T_fax"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人传真", dr["C_fax"].ToString(), request["T_fax"]);
                    }
                    if (dr["C_email"].ToString() != request["T_email"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人邮箱", dr["C_email"].ToString(), request["T_email"]);
                    }
                    if (dr["C_QQ"].ToString() != request["T_qq"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人QQ", dr["C_QQ"].ToString(), request["T_qq"]);
                    }
                    if (dr["C_add"].ToString() != request["T_add"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人地址", dr["C_add"].ToString(), request["T_add"]);
                    }
                    if (dr["C_hobby"].ToString() != request["T_hobby"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "联系人爱好", dr["C_hobby"].ToString(), request["T_hobby"]);
                    }
                    if (dr["C_remarks"].ToString() != request["T_remarks"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "备注", dr["C_remarks"].ToString(), request["T_remarks"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.C_createId = int.Parse(request.Cookies["UserID"].Value);
                    model.C_createDate = DateTime.Now;

                    contact.Add(model);
                }
            }
            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " id ";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = null;
                string serchtype = request["isdel"];
                if (serchtype == "1")
                {
                    serchtxt += "isDelete=1";
                }
                else
                {
                    serchtxt += "isDelete=0 ";
                }

                if (!string.IsNullOrEmpty(request["customerid"]))
                    serchtxt += " and C_customerid=" + int.Parse(request["customerid"]);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and C_customername like N'%" + PageValidate.InputText(request["company"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["contact"]))
                    serchtxt += " and C_name like N'%" + PageValidate.InputText(request["contact"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["tel"]))
                    serchtxt += " and C_mob like N'%" + PageValidate.InputText(request["tel"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["qq"]))
                    serchtxt += " and C_QQ like N'%" + PageValidate.InputText(request["qq"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and C_createDate >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and C_createDate  <= '" + enddate + "'";
                }

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";
                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate1 = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate1 + "'";
                }
                //权限
                serchtxt += " and C_customerid in (select id from CRM_Customer where  " + DataAuth(emp_id.ToString()) + ")";

                //context.Response.Write(serchtxt);

                DataSet ds = contact.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }




            if (request["Action"] == "form")
            {
                string contact_id = request["contact_id"];

                DataSet ds = contact.GetList("id=" + int.Parse(contact_id));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "AdvanceDelete")
            {
                //参数安全过滤
                string c_id = request["id"];

                DataSet ds = contact.GetList("id=" + int.Parse(c_id));
                string EventType = "客户联系人预删除";

                bool isdel = contact.AdvanceDelete(int.Parse(c_id), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (isdel)
                {
                    //日志


                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    int EventID = int.Parse(c_id);
                    string EventTitle = ds.Tables[0].Rows[0]["C_name"].ToString();
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
            if (request["Action"] == "regain")
            {
                string idlist = PageValidate.InputText(request["idlist"], 100000);
                string[] arr = idlist.Split(',');

                DataSet ds = contact.GetList("id in (" + idlist.Trim() + ")");

                for (int i = 0; i < arr.Length; i++)
                {
                    contact.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {
                    //日志
                    string EventType = "恢复删除客户联系人";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int UserID = emp_id;
                        string UserName = empname;
                        int EventID = idlist[i];
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = ds.Tables[0].Rows[i]["C_name"].ToString();
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
            //del
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
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "55");
                        if (delauth == "false")
                            canDel = false;
                        else
                            canDel = true;
                    }
                }
                if (canDel)
                {
                    string idlist = PageValidate.InputText(request["idlist"], 255);
                    string[] arr = idlist.Split(',');

                    string EventType = "彻底删除客户联系人";

                    DataSet ds = contact.GetList("id in (" + idlist.Trim() + ")");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        contact.Delete(int.Parse(arr[i]));
                    }

                    if (true)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = idlist[i];
                            string EventTitle = ds.Tables[0].Rows[0]["C_name"].ToString();
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
        }


        private string DataAuth(string uid)
        {
            //权限
            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet dsemp = emp.GetList("ID=" + int.Parse(uid));

            string returntxt = " 1=1";
            if (dsemp.Tables[0].Rows.Count > 0)
            {
                if (dsemp.Tables[0].Rows[0]["uid"].ToString() != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("1", "Sys_view", uid);

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            returntxt = " 1=2";
                            break;
                        case "my":
                            returntxt = " ( privatecustomer='公客' or Employee_id=" + arr[1] + ")";
                            break;
                        case "dep":
                            if (string.IsNullOrEmpty(arr[1])) 
                                returntxt = " ( privatecustomer='公客' or Employee_id=" + int.Parse(uid) + ")";
                            else
                                returntxt = " ( privatecustomer='公客' or Department_id=" + arr[1] + ")";
                            break;
                        case "depall":
                            BLL.hr_department dep = new BLL.hr_department();
                            DataSet ds = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), ds.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            returntxt = " ( privatecustomer='公客' or Department_id in (" + intext.TrimEnd(',') + "))";
                            break;
                    }
                }
            }
            return returntxt;
        }
        private static string GetDepTask(int Id, DataTable table)
        {
            DataRow[] rows = table.Select("parentid=" + Id.ToString());

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append(row["id"] + ",");
                if (GetDepTask((int)row["id"], table).Length > 0)
                    str.Append(GetDepTask((int)row["id"], table));
            }
            return str.ToString();
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