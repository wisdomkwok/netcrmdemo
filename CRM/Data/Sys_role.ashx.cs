using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using XHD.Common;

namespace XHD.CRM.Data
{
    /// <summary>
    /// Sys_role 的摘要说明
    /// </summary>
    public class Sys_role : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Sys_role role = new BLL.Sys_role();
            Model.Sys_role model = new Model.Sys_role();
            BLL.hr_employee emp = new BLL.hr_employee();

            //save
            if (request["Action"] == "SysSave")
            {

                model.RoleName = PageValidate.InputText(request["T_role"], 250);
                model.RoleSort = PageValidate.InputText(request["T_RoleOrder"], 10);
                model.RoleDscript = PageValidate.InputText(request["T_Descript"], 255);

                string id = request["id"];

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    DataSet ds = role.GetList("RoleID=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];
                    model.RoleID = int.Parse(id);
                    model.CreateID = int.Parse(dr["CreateID"].ToString());
                    model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                    model.UpdateDate = DateTime.Now;
                    model.UpdateID = int.Parse(request.Cookies["UserID"].Value);
                    role.Update(model);
                }
                else
                {
                    model.CreateID = int.Parse(request.Cookies["UserID"].Value);
                    model.CreateDate = DateTime.Now;
                    int rid = role.Add(model);

                    BLL.Sys_data_authority auth = new BLL.Sys_data_authority();
                    Model.Sys_data_authority modelsda = new Model.Sys_data_authority();

                    //默认数据权限
                    modelsda.Role_id = rid;
                    modelsda.Sys_view = 1;
                    modelsda.Sys_add = 1;
                    modelsda.Sys_edit = 1;
                    modelsda.Sys_del = 1;

                    modelsda.option_id = 1;
                    modelsda.Sys_option = "客户管理"; 
                    auth.Add(modelsda);

                    modelsda.option_id = 2;
                    modelsda.Sys_option = "跟进管理";
                    auth.Add(modelsda);

                    modelsda.option_id = 3;
                    modelsda.Sys_option = "订单管理";
                    auth.Add(modelsda);

                    modelsda.option_id = 4;
                    modelsda.Sys_option = "合同管理";
                    auth.Add(modelsda);

                }
            }

            //validate
            if (request["Action"] == "Exist")
            {
                DataSet ds1 = role.GetList(" RoleName='" + Common.PageValidate.InputText(request["T_role"], 250) + "'");
                context.Response.Write(ds1.Tables[0].Rows.Count > 0 ? "false" : "true");
            }

            //表格json
            if (request["Action"] == "grid")
            {
                DataSet ds = role.GetList(0, "", " RoleSort");

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);

                context.Response.Write(dt);
            }

            //Form JSON
            if (request["Action"] == "form")
            {

                DataSet ds = role.GetList(" RoleID=" + int.Parse(request["id"]));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {
                string rid = request["id"];
                bool isdel = role.Delete(int.Parse(rid));
                if (isdel)
                    context.Response.Write("true");
                else
                    context.Response.Write("false");

                //角色下员工删除
                BLL.Sys_role_emp rm = new BLL.Sys_role_emp();
                rm.Delete("RoleID=" + int.Parse(rid));

                //角色下数据权限删除
                BLL.Sys_data_authority auth = new BLL.Sys_data_authority();
                auth.Delete("Role_id=" + int.Parse(rid));
            }
            //auth
            if (request["Action"] == "treegrid")
            {
                int appid = int.Parse(request["appid"]);
                BLL.Sys_Menu menu = new BLL.Sys_Menu();

                //string dt1 = 
                DataTable dt = menu.GetList("App_id=" + appid).Tables[0];
                dt.Columns.Add(new DataColumn("Sysroler", typeof(string)));

                BLL.Sys_Button btn = new BLL.Sys_Button();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataSet ds = btn.GetList(0, "Menu_id=" + dt.Rows[i]["Menu_id"].ToString(), " convert(int,[Btn_order])");
                    string roler = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            roler += ds.Tables[0].Rows[j]["Btn_id"].ToString() + "|" + ds.Tables[0].Rows[j]["Btn_name"].ToString();
                            roler += ",";
                        }
                    }
                    dt.Rows[i][dt.Columns.Count - 1] = roler;
                }
                string dt1 = "{Rows:[" + GetTasksString(0, dt) + "]}";
                context.Response.Write(dt1);
                context.Response.End();
            }
            //get auth
            if (request["Action"] == "getauth")
            {
                string postdata = Convert.ToString(HttpContext.Current.Request.QueryString["postdata"]);
                JavaScriptSerializer json = new JavaScriptSerializer();
                save sa = json.Deserialize<save>(postdata);
                Model.Sys_authority modelauth = new Model.Sys_authority();
                modelauth.Role_id = int.Parse(sa.role_id);
                modelauth.App_ids = sa.app;
                modelauth.Menu_ids = sa.menu;
                modelauth.Button_ids = sa.btn;

                BLL.Sys_authority sysau = new BLL.Sys_authority();

                string roledata = "0|0";
                DataSet ds = sysau.GetList("Role_id=" + modelauth.Role_id + " and App_ids='a" + modelauth.App_ids + ",'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    roledata = dr["Menu_ids"] + "|" + dr["Button_ids"];
                }
                context.Response.Write(roledata);
            }
            // save auth
            if (request["Action"] == "saveauth")
            {
                string postdata = Convert.ToString(HttpContext.Current.Request.QueryString["postdata"]);
                JavaScriptSerializer json = new JavaScriptSerializer();
                save sa = json.Deserialize<save>(postdata);
                Model.Sys_authority modelauth = new Model.Sys_authority();
                modelauth.Role_id = int.Parse(sa.role_id);
                modelauth.App_ids = sa.app;
                modelauth.Menu_ids = sa.menu;
                modelauth.Button_ids = sa.btn;

                BLL.Sys_authority sysau = new BLL.Sys_authority();

                if (!string.IsNullOrEmpty(postdata))
                {
                    sysau.DeleteWhere("Role_id=" + modelauth.Role_id + " and App_ids='" + modelauth.App_ids + "'");
                    sysau.Add(modelauth);

                    context.Response.Write("{sucess:sucess}");

                    //日志
                    BLL.Sys_log log = new BLL.Sys_log();
                    Model.Sys_log modellog = new Model.Sys_log();

                    DataSet dsemp = emp.GetList("id=" + request.Cookies["UserID"].Value);
                    modellog.EventDate = DateTime.Now;
                    modellog.UserID = int.Parse(request.Cookies["UserID"].Value);
                    modellog.UserName = dsemp.Tables[0].Rows[0]["name"].ToString();
                    modellog.IPStreet = request.UserHostAddress;

                    modellog.EventType = "权限修改";
                    modellog.EventID = modelauth.Role_id.ToString();
                    log.Add(modellog);
                }
            }
        }

        [Serializable]
        private class save
        {
            public string role_id;
            public string app;
            public string menu;
            public string btn;
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
                if (GetTasksString((int)row["Menu_id"], table).Length > 0)
                {
                    str.Append(",children:[");
                    str.Append(GetTasksString((int)row["Menu_id"], table));
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