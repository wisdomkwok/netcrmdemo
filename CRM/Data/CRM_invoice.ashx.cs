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
    /// CRM_invoice 的摘要说明
    /// </summary>
    public class CRM_invoice : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_invoice cci = new BLL.CRM_invoice();
            Model.CRM_invoice model = new Model.CRM_invoice();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + emp_id);

            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                DataRow dremp = dsemp.Tables[0].Rows[0];
                model.invoice_num = PageValidate.InputText(request["T_invoice_num"], 255);
                string orderid = request["orderid"];

                BLL.CRM_order order = new BLL.CRM_order();
                DataSet dsorder = order.GetList("id=" + int.Parse(orderid));

                model.order_id = int.Parse(orderid);
                if (dsorder.Tables[0].Rows.Count > 0)
                {
                    model.Customer_id = int.Parse(dsorder.Tables[0].Rows[0]["Customer_id"].ToString());
                    model.Customer_name = PageValidate.InputText(dsorder.Tables[0].Rows[0]["Customer_name"].ToString(), 255);
                }

                model.C_depid = int.Parse(request["T_department_val"].ToString());
                model.C_depname = PageValidate.InputText(request["T_department"].ToString(), 255);
                model.C_empid = int.Parse(request["T_employee_val"].ToString());
                model.C_empname = PageValidate.InputText(request["T_employee"].ToString(), 255);

                model.invoice_amount = decimal.Parse(request["T_invoice_amount"]);
                model.invoice_date = DateTime.Parse(request["T_invoice_date"].ToString());
                model.invoice_type_id = int.Parse(request["T_invoice_type_val"].ToString());
                model.invoice_type = PageValidate.InputText(request["T_invoice_type"].ToString(), 255);
                model.invoice_content = PageValidate.InputText(request["T_content"].ToString(), 12000);

                string cid = request["invoiceid"];
                if (!string.IsNullOrEmpty(cid) && cid != "null")
                {
                    model.id = int.Parse(cid);

                    DataSet ds = cci.GetList(" id=" + model.id);
                    DataRow dr = ds.Tables[0].Rows[0];

                    model.create_id = int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
                    model.create_name = ds.Tables[0].Rows[0]["create_name"].ToString();
                    model.create_date = DateTime.Parse(ds.Tables[0].Rows[0]["create_date"].ToString());

                    cci.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.invoice_num;
                    string EventType = "开票修改";
                    int EventID = model.id;

                    if (dr["invoice_amount"].ToString() != request["T_invoice_amount"].Replace(",", "").Replace(".00", ""))
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "发票总额", dr["invoice_amount"].ToString(), request["T_invoice_amount"].Replace(",", "").Replace(".00", ""));
                    }
                    if (dr["invoice_type"].ToString() != request["T_invoice_type"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "发票类型", dr["invoice_type"].ToString(), request["T_invoice_type"]);
                    }
                    if (dr["invoice_num"].ToString() != request["T_invoice_num"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "发票号码", dr["invoice_num"].ToString(), request["T_invoice_num"]);
                    }
                    if (dr["invoice_date"].ToString() != request["T_invoice_date"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开票时间", dr["invoice_date"].ToString(), request["T_invoice_date"]);
                    }
                    if (dr["invoice_content"].ToString() != request["T_content"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开票内容", "原内容被修改", "原内容被修改");
                    }
                    if (dr["C_depname"].ToString() != request["T_department"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开票人部门", dr["C_depname"].ToString(), request["T_department"]);
                    }
                    if (dr["C_empname"].ToString() != request["T_employee"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开票人姓名", dr["C_empname"].ToString(), request["T_employee"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.create_id = int.Parse(request.Cookies["UserID"].Value);
                    model.create_name = dremp["name"].ToString();
                    model.create_date = DateTime.Now;

                    cci.Add(model);
                }
                //更新订单发票金额
                order.UpdateInvoice(orderid);
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
                string order_id = request["orderid"];

                if (!string.IsNullOrEmpty(order_id) && order_id != "null")
                    serchtxt += " and order_id=" + int.Parse(order_id);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText( request["company"],255) + "%'";

                if (!string.IsNullOrEmpty(request["receive_num"]))
                    serchtxt += " and invoice_num like N'%" + PageValidate.InputText( request["receive_num"] ,255)+ "%'";

                if (!string.IsNullOrEmpty(request["pay_type"]))
                    serchtxt += " and invoice_type_id =" + int.Parse( request["pay_type_val"]);

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and C_depid =" + int.Parse(request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and C_empid =" + int.Parse( request["employee_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and invoice_date >= '" + PageValidate.InputText( request["startdate"],255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]);
                    serchtxt += " and invoice_date  <= '" + DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                if (!string.IsNullOrEmpty(request["startdate_del"]))
                {
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText( request["startdate_del"],255) + "'";
                }
                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate + "'";
                }


                //权限
                DataSet ds = cci.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }



            if (request["Action"] == "form")
            {                
                int invoiceid = int.Parse(request["invoiceid"]);
                DataSet ds = cci.GetList("id=" + invoiceid);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "AdvanceDelete")
            {
                //参数安全过滤
                string c_id = request["id"];

                DataSet ds = cci.GetList("id=" + int.Parse(c_id));

                bool isdel = cci.AdvanceDelete(int.Parse(c_id), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //更新订单发票金额
                BLL.CRM_order order = new BLL.CRM_order();
                string orderid = ds.Tables[0].Rows[0]["order_id"].ToString();
                order.UpdateInvoice(orderid);

                if (isdel)
                {
                    //日志
                    string EventType = "开票预删除";


                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    int EventID = int.Parse(c_id);
                    string EventTitle = ds.Tables[0].Rows[0]["Customer_name"].ToString();
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
                string idlist = PageValidate.InputText( request["idlist"],100000);
                string[] arr = idlist.Split(',');

                DataSet ds = cci.GetList("id in (" + idlist.Trim() + ")");


                BLL.CRM_order order = new BLL.CRM_order();
                for (int i = 0; i < arr.Length; i++)
                {
                    cci.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {
                    //日志
                    string EventType = "恢复删除票据";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //更新订单发票金额
                        string orderid = ds.Tables[0].Rows[0]["order_id"].ToString();
                        order.UpdateInvoice(orderid);

                        int UserID = emp_id;
                        string UserName = empname;
                        int EventID = idlist[i];
                        string IPStreet = request.UserHostAddress;
                        string EventTitle = ds.Tables[0].Rows[i]["Customer_name"].ToString();
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
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "72");
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

                    string EventType = "彻底删除发票"; 

                    DataSet ds = cci.GetList("id in (" + idlist.Trim() + ")");  

                    BLL.CRM_order order = new BLL.CRM_order();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        cci.Delete(int.Parse(arr[i]));
                    }

                    if (true)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = idlist[i];
                            string EventTitle = ds.Tables[0].Rows[i]["Customer_name"].ToString();
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}