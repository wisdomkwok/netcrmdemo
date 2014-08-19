using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using XHD.Common;
using System.Web.Script.Serialization;

namespace XHD.CRM.Data
{
    /// <summary>
    /// CRM_order 的摘要说明
    /// </summary>
    public class CRM_order : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;


            BLL.CRM_order order = new BLL.CRM_order();
            Model.CRM_order model = new Model.CRM_order();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                DataRow dremp = dsemp.Tables[0].Rows[0];

                model.Customer_id = int.Parse(request["T_Customer_val"]);
                model.Customer_name = PageValidate.InputText(request["T_Customer"], 255);

                model.Order_date = DateTime.Parse(request["T_date"]);
                model.pay_type_id = int.Parse(request["T_paytype_val"]);
                model.pay_type = PageValidate.InputText(request["T_paytype"], 255);
                model.Order_details = PageValidate.InputText(request["T_details"].ToString(), 4000);
                model.Order_status_id = int.Parse(request["T_status_val"]);
                model.Order_status = PageValidate.InputText(request["T_status"], 255);
                model.Order_amount = decimal.Parse(request["T_amount"]);

                model.create_id = int.Parse(dremp["ID"].ToString());
                model.create_date = DateTime.Now;

                model.C_dep_id = int.Parse(request["T_department_val"]);
                model.C_dep_name = PageValidate.InputText(request["T_department"], 255);
                model.C_emp_id = int.Parse(request["T_employee_val"]);
                model.C_emp_name = PageValidate.InputText(request["T_employee"], 255);

                model.F_dep_id = int.Parse(request["T_department1_val"]);
                model.F_dep_name = PageValidate.InputText(request["T_department1"], 255);
                model.F_emp_id = int.Parse(request["T_employee1_val"]);
                model.F_emp_name = PageValidate.InputText(request["T_employee1"], 255);

                int orderid;
                string pid = request["orderid"];
                if (!string.IsNullOrEmpty(pid) && pid != "null")
                {
                    model.id = int.Parse(PageValidate.IsNumber(pid) ? pid : "-1");
                    DataSet ds = order.GetList("id=" + model.id);
                    DataRow dr = ds.Tables[0].Rows[0];
                    orderid = model.id;


                    order.Update(model);
                    //context.Response.Write(model.id );
                    context.Response.Write("{success:success}");

                    C_Sys_log log = new C_Sys_log();
                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Customer_name;
                    string EventType = "订单修改";
                    int EventID = model.id;

                    if (dr["Customer_name"].ToString() != request["T_Customer"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "客户", dr["Customer_name"].ToString(), request["T_Customer"]);
                    }
                    if (dr["Order_details"].ToString() != request["T_details"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "订单详情", "原内容被修改", "原内容被修改");
                    }
                    if (dr["Order_date"].ToString() != request["T_date"].ToString() + " 0:00:00")
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "成交时间", dr["Order_date"].ToString(), request["T_date"].ToString() + " 0:00:00");
                    }
                    if (dr["Order_amount"].ToString() != request["T_amount"].Replace(",", "").Replace(".00", ""))
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "订单总额", dr["Order_amount"].ToString(), request["T_amount"].Replace(",", "").Replace(".00", ""));
                    }
                    if (dr["Order_status"].ToString() != request["T_status"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "订单状态", dr["Order_status"].ToString(), request["T_status"]);
                    }
                    if (dr["F_dep_name"].ToString() != request["T_department1"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "促成人员部门", dr["F_dep_name"].ToString(), request["T_department1"]);
                    }
                    if (dr["F_emp_name"].ToString() != request["T_employee1"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "促成人员", dr["F_emp_name"].ToString(), request["T_employee1"]);
                    }
                    if (dr["pay_type"].ToString() != request["T_paytype"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "支付方式", dr["pay_type"].ToString(), request["T_paytype"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.Serialnumber = DateTime.Now.AddMilliseconds(3).ToString("yyyyMMddHHmmssfff").Trim();
                    //model.arrears_invoice = decimal.Parse(request["T_amount"]);
                    orderid = order.Add(model);
                    context.Response.Write("{success:success}");
                }
                //更新订单收款金额
                order.UpdateReceive(orderid.ToString());
                //更新订单发票金额
                order.UpdateInvoice(orderid.ToString());

                string json = request["PostData"].ToLower();
                JavaScriptSerializer js = new JavaScriptSerializer();

                PostData[] postdata;
                postdata = js.Deserialize<PostData[]>(json);

                BLL.CRM_order_details cod = new BLL.CRM_order_details();
                Model.CRM_order_details modeldel = new Model.CRM_order_details();

                modeldel.order_id = orderid;
                cod.Delete(" order_id=" + modeldel.order_id);
                for (int i = 0; i < postdata.Length; i++)
                {
                    modeldel.product_id = postdata[i].Product_id;
                    modeldel.product_name = postdata[i].Product_name;
                    modeldel.quantity = postdata[i].Quantity;
                    modeldel.unit = postdata[i].Unit;
                    modeldel.price = postdata[i].Price;
                    modeldel.amount = postdata[i].Amount;

                    cod.Add(modeldel);
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
                    serchtxt += " isDelete=1 ";
                }
                else
                {
                    serchtxt += " isDelete=0 ";
                }
                string issar = request["issarr"];
                if (issar == "1")
                {
                    serchtxt += " and isnull( arrears_money,0)>0";
                }


                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText(request["company"], 100) + "%'";

                if (!string.IsNullOrEmpty(request["contact"]))
                    serchtxt += " and Order_status_id = " + int.Parse(request["contact_val"]);

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and F_dep_id = " + int.Parse(request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and F_emp_id = " + int.Parse(request["employee_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and Order_date >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]);
                    serchtxt += " and Order_date <= '" + DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time <= '" + enddate + "'";
                }

                //权限 
                serchtxt += DataAuth(request.Cookies["UserID"].Value);
                DataSet ds = order.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);



                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }

            if (request["Action"] == "gridbycustomerid")
            {
                string customerid = request["customerid"];

                DataSet ds = order.GetList(0, " isDelete=0 and Customer_id =" + int.Parse(customerid), " Order_date desc");
                context.Response.Write(Common.GetGridJSON.DataTableToJSON(ds.Tables[0]));
            }
            if (request["Action"] == "form")
            {
                int pid = int.Parse(request["orderid"]);
                DataSet ds = order.GetList("id=" + pid);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            if (request["Action"] == "AdvanceDelete")
            {
                //参数安全过滤
                string c_id = request["id"];

                DataSet ds = order.GetList("id=" + c_id);

                BLL.CRM_contract contract = new BLL.CRM_contract();
                BLL.CRM_invoice invoice = new BLL.CRM_invoice();
                BLL.CRM_receive receive = new BLL.CRM_receive();
                if (invoice.GetList("order_id=" + c_id).Tables[0].Rows.Count > 0)
                {
                    //invoice
                    context.Response.Write("false:invoice");
                }
                else if (receive.GetList("order_id=" + c_id).Tables[0].Rows.Count > 0)
                {
                    //receive
                    context.Response.Write("false:receive");
                }
                else
                {
                    bool canedel = true;
                    if (uid != "admin")
                    {
                        Data.GetDataAuth dataauth = new Data.GetDataAuth();
                        string txt = dataauth.GetDataAuthByid("3", "Sys_del", emp_id.ToString());

                        string[] arr = txt.Split(':');
                        switch (arr[0])
                        {
                            case "none":
                                canedel = false;
                                break;
                            case "my":
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["C_emp_id"].ToString() == arr[1])
                                        canedel = true;
                                    else
                                        canedel = false;
                                }
                                break;
                            case "dep":
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["C_dep_id"].ToString() == arr[1])
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
                        bool isdel = order.AdvanceDelete(int.Parse(c_id), 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (isdel)
                        {
                            //日志
                            string EventType = "订单预删除";


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
                    else
                    {
                        context.Response.Write("delfalse");
                    }
                }
            }

            //regain            
            if (request["Action"] == "regain")
            {
                string idlist = PageValidate.InputText(request["idlist"], 100000);
                string[] arr = idlist.Split(',');

                DataSet ds = order.GetList("id in (" + idlist.Trim() + ")");

                for (int i = 0; i < arr.Length; i++)
                {
                    order.AdvanceDelete(int.Parse(arr[i]), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                if (true)
                {

                    string EventType = "恢复删除订单";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
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
                        string delauth = getauth.GetBtnAuthority(request.Cookies["UserID"].Value, "66");
                        if (delauth == "false")
                            canDel = false;
                        else
                            canDel = true;
                    }
                }
                if (canDel)
                {
                    string idlist = PageValidate.InputText(request["idlist"], 100000);
                    string[] arr = idlist.Split(',');

                    string EventType = "彻底删除订单";

                    DataSet ds = order.GetList("id in (" + idlist.Trim() + ")");

                    bool cando = true;


                    for (int i = 0; i < arr.Length; i++)
                    {
                        bool deleted = order.Delete(int.Parse(arr[i]));

                        if (!deleted)
                            cando = false;
                    }

                    if (cando)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //日志
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


        private string DataAuth(string uid)
        {
            //权限
            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet dsemp = emp.GetList("ID=" + int.Parse(uid));

            string returntxt = " and 1=1 ";
            if (dsemp.Tables[0].Rows.Count > 0)
            {
                if (dsemp.Tables[0].Rows[0]["uid"].ToString() != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("3", "Sys_view", uid);

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            returntxt = " and 1=2 ";
                            break;
                        case "my":
                            returntxt = " and  C_emp_id=" + arr[1];
                            break;
                        case "dep":
                            if (string.IsNullOrEmpty(arr[1]))
                                returntxt = " and  C_emp_id=" + int.Parse(uid);
                            else
                                returntxt = " and  C_dep_id=" + arr[1];
                            break;
                        case "depall":
                            BLL.hr_department dep = new BLL.hr_department();
                            DataSet ds = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), ds.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            returntxt = " and  C_dep_id in (" + intext.TrimEnd(',') + ")";
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

        public class PostData
        {
            private int? product_id;
            private string product_name;
            private decimal? price;
            private int? quantity;
            private string unit;
            private decimal? amount;

            public int? Product_id
            {
                set { product_id = value; }
                get { return product_id; }
            }

            public string Product_name
            {
                set { product_name = value; }
                get { return product_name; }
            }
            public decimal? Price
            {
                set { price = value; }
                get { return price; }
            }
            public int? Quantity
            {
                set { quantity = value; }
                get { return quantity; }
            }
            public string Unit
            {
                set { unit = value; }
                get { return unit; }
            }
            public decimal? Amount
            {
                set { amount = value; }
                get { return amount; }
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