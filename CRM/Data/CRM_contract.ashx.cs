using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using XHD.Common;

namespace XHD.CRM.Data
{
    /// <summary>
    /// CRM_contract 的摘要说明
    /// </summary>
    public class CRM_contract : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_contract cc = new BLL.CRM_contract();
            Model.CRM_contract model = new Model.CRM_contract();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                DataRow dremp = dsemp.Tables[0].Rows[0];

                model.Serialnumber = PageValidate.InputText(request["T_contract_num"], 255);
                model.Contract_name = PageValidate.InputText(request["T_contract_name"], 255);
                model.Customer_id = int.Parse(request["T_Customer_val"]);
                model.Customer_name = PageValidate.InputText(request["T_Customer"], 255);

                model.C_depid = int.Parse(request["T_department_val"].ToString());
                model.C_depname = PageValidate.InputText(request["T_department"].ToString(), 255);
                model.C_empid = int.Parse(request["T_employee_val"].ToString());
                model.C_empname = PageValidate.InputText(request["T_employee"].ToString(), 255);

                model.Contract_amount = decimal.Parse(request["T_contract_amount"]);
                model.Pay_cycle = int.Parse(request["T_pay_cycle"]);

                model.Start_date = PageValidate.InputText(request["T_start_date"].ToString(), 255);
                model.End_date = PageValidate.InputText(request["T_end_date"].ToString(), 255);
                model.Sign_date = PageValidate.InputText(request["T_contract_date"].ToString(), 255);
                model.Customer_Contractor = PageValidate.InputText(request["T_contractor"].ToString(), 255);
                model.Our_Contractor_depid = int.Parse(request["T_department1_val"].ToString());
                model.Our_Contractor_depname = PageValidate.InputText(request["T_department1"], 255);
                model.Our_Contractor_id = int.Parse(request["T_employee1_val"].ToString());
                model.Our_Contractor_name = PageValidate.InputText(request["T_employee1"].ToString(), 255);

                model.Main_Content = PageValidate.InputText(request["T_content"].ToString(), 12000);
                model.Remarks = PageValidate.InputText(request["T_remarks"].ToString(), 12000);

                string cid = request["cid"];
                if (!string.IsNullOrEmpty(cid) && cid != "null")
                {
                    model.id = int.Parse(cid);

                    DataSet ds = cc.GetList(" id=" + model.id);
                    DataRow dr = ds.Tables[0].Rows[0]; 

                    cc.Update(model);

                    C_Sys_log log = new C_Sys_log();
                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Contract_name;
                    string EventType = "合同修改";
                    int EventID = model.id;

                    if (dr["Customer_name"].ToString() != request["T_Customer"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "客户", dr["Customer_name"].ToString(), request["T_Customer"]);
                    }
                    if (dr["Contract_name"].ToString() != request["T_contract_name"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同名称", dr["Contract_name"].ToString(), request["T_contract_name"]);
                    }
                    if (dr["Serialnumber"].ToString() != request["T_contract_num"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同编号", dr["Serialnumber"].ToString(), request["T_contract_num"]);
                    }
                    if (dr["Contract_amount"].ToString() != request["T_contract_amount"].Replace(",", "").Replace(".00", ""))
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同金额", dr["Contract_amount"].ToString(), request["T_contract_amount"].Replace(",", "").Replace(".00", ""));
                    }
                    if (dr["Customer_Contractor"].ToString() != request["T_contractor"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "对方签约人", dr["Customer_Contractor"].ToString(), request["T_contractor"]);
                    }
                    if (dr["Our_Contractor_depname"].ToString() != request["T_department1"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "我方签约人部门", dr["Our_Contractor_depname"].ToString(), request["T_department1"]);
                    }
                    if (dr["Our_Contractor_name"].ToString() != request["T_employee1"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "我方签约人名字", dr["Our_Contractor_name"].ToString(), request["T_employee1"]);
                    }
                    if (dr["Main_Content"].ToString() != request["T_content"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "主要条款", "原内容被修改", "原内容被修改");
                    }
                    if (dr["Remarks"].ToString() != request["T_remarks"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "备注", "原内容被修改", "原内容被修改");
                    }
                    if (dr["Start_date"].ToString() != request["T_start_date"].ToString())
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开始时间", dr["Start_date"].ToString(), request["T_start_date"].ToString());
                    }
                    if (dr["End_date"].ToString() != request["T_end_date"].ToString())
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "结束时间", dr["End_date"].ToString(), request["T_end_date"].ToString());
                    }
                    if (dr["Sign_date"].ToString() != request["T_contract_date"].ToString())
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "签约时间", dr["Sign_date"].ToString(), request["T_contract_date"].ToString());
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.Creater_id = int.Parse(request.Cookies["UserID"].Value);
                    model.Creater_name = dremp["name"].ToString();
                    model.Create_time = DateTime.Now;

                    cc.Add(model);
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
                    sortorder = "desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = null;
                string serchtype = request["isdel"];
                if (serchtype == "1")
                {
                    serchtxt += " isDelete=1";
                }
                else
                {
                    serchtxt += " isDelete=0";
                }

                string customer_id = request["cid"];
                if (!string.IsNullOrEmpty(customer_id) && customer_id != "null")
                    serchtxt += " and Customer_id=" + int.Parse(customer_id);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText(request["company"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["contact"]))
                    serchtxt += " and Contract_name like N'%" + PageValidate.InputText(request["contact"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and C_depid =" + int.Parse(request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and C_empid =" + int.Parse(request["employee_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and Create_time >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Create_time  <= '" + enddate + "'";
                }

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate + "'";
                }
                //权限 
                serchtxt += DataAuth(request.Cookies["UserID"].Value);

                DataSet ds = cc.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                context.Response.Write(Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total));
            }


            if (request["Action"] == "form")
            {
                string contract_id = request["cid"];

                DataSet ds = cc.GetList("id=" + int.Parse(contract_id) + DataAuth(request.Cookies["UserID"].Value));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "AdvanceDelete")
            {
                string c_id = request["id"];
                DataSet ds = cc.GetList("id=" + int.Parse(c_id));

                bool canedel = true;
                if (uid != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("4", "Sys_del", emp_id.ToString());

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            canedel = false;
                            break;
                        case "my":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["C_empid"].ToString() == arr[1])
                                    canedel = true;
                                else
                                    canedel = false;
                            }
                            break;
                        case "dep":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["C_depid"].ToString() == arr[1])
                                    canedel = true;
                                else
                                    canedel = false;
                            }
                            break;
                        case "all":
                            canedel = true;
                            break;
                    }
                }
                if (canedel)
                {
                    bool isdel = cc.AdvanceDelete(int.Parse(c_id), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (isdel)
                    {
                        //日志
                        string EventType = "合同预删除";


                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(c_id);
                        string EventTitle = ds.Tables[0].Rows[0]["Contract_name"].ToString();
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
            if (request["Action"] == "regain")
            {
                string idlist = PageValidate.InputText(request["idlist"], 100000);
                string[] arr = idlist.Split(',');

                DataSet ds = cc.GetList("id in (" + idlist.Trim() + ")");

                for (int i = 0; i < arr.Length; i++)
                {
                    cc.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {
                    //日志
                    string EventType = "恢复删除合同";


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int UserID = emp_id;
                        string UserName = empname;
                        int EventID = idlist[i];
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = ds.Tables[0].Rows[i]["Contract_name"].ToString();
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
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "68");
                        if (delauth == "false")
                        {
                            canDel = false;
                        }
                        else
                        {
                            canDel = true;
                        }
                    }
                }
                if (canDel)
                {
                    string idlist = request["idlist"];
                    string[] arr = idlist.Split(',');

                    string EventType = "彻底删除合同";

                    DataSet ds = cc.GetList("id in (" + idlist.Trim() + ")");

                    bool cando = true;

                    for (int i = 0; i < arr.Length; i++)
                    {
                        bool deleted = cc.Delete(int.Parse(arr[i]));
                        if (!deleted)
                            cando = false;
                    }

                    if (cando)
                    {
                        //日志

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = idlist[i];
                            string EventTitle = ds.Tables[0].Rows[i]["Contract_name"].ToString();
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

            if (request["Action"] == "Compared_empcuscontract")
            {
                var idlist = PageValidate.InputText(request["idlist"].Replace(";", ",").Replace("-", ""), 100000);
                string dt1 = request["date1"];
                string dt2 = request["date2"];

                BLL.hr_post post = new BLL.hr_post();
                DataSet dspost = post.GetList("post_id in(" + idlist + ")");

                string emplist = "(";

                for (int i = 0; i < dspost.Tables[0].Rows.Count - 1; i++)
                {
                    emplist += dspost.Tables[0].Rows[i]["emp_id"] + ",";
                }
                emplist += dspost.Tables[0].Rows[dspost.Tables[0].Rows.Count - 1]["emp_id"] + ")";

                //context.Response.Write(emplist);

                DataSet ds = cc.Compared_empcuscontract(DateTime.Parse(dt1), DateTime.Parse(dt2), emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            if (request["Action"] == "emp_cuscontract")
            {
                var idlist = PageValidate.InputText(request["idlist"].Replace(";", ",").Replace("-", ""), 100000);
                var syear = request["syear"];

                BLL.hr_post post = new BLL.hr_post();
                DataSet dspost = post.GetList("post_id in(" + idlist + ")");

                string emplist = "(";

                for (int i = 0; i < dspost.Tables[0].Rows.Count - 1; i++)
                {
                    emplist += dspost.Tables[0].Rows[i]["emp_id"] + ",";
                }
                emplist += dspost.Tables[0].Rows[dspost.Tables[0].Rows.Count - 1]["emp_id"] + ")";

                //context.Response.Write(emplist);

                DataSet ds = cc.report_empcontract(int.Parse(syear), emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }


        }


        private string DataAuth(string uid)
        {
            //权限
            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet dsemp = emp.GetList("ID=" + int.Parse(uid));

            string returntxt = " and 1=1";
            if (dsemp.Tables[0].Rows.Count > 0)
            {
                if (dsemp.Tables[0].Rows[0]["uid"].ToString() != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("4", "Sys_view", uid);

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            returntxt = " and 1=2 ";
                            break;
                        case "my":
                            returntxt = " and  C_empid=" + arr[1];
                            break;
                        case "dep":
                            if (string.IsNullOrEmpty(arr[1]))
                                returntxt = " and  C_empid=" + int.Parse(uid);
                            else
                                returntxt = " and  C_depid=" + arr[1];
                            break;
                        case "depall":
                            BLL.hr_department dep = new BLL.hr_department();
                            DataSet ds = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), ds.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            returntxt = " and  C_depid in (" + intext.TrimEnd(',') + ")";
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