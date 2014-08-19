using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace XHD.CRM.Data
{
    /// <summary>
    /// sys_app 的摘要说明
    /// </summary>
    public class sys_app : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
            if (request["Action"] == "GetSysApp")
            {
                BLL.Sys_App app = new BLL.Sys_App();
                Data.GetAuthorityByUid getappauth = new Data.GetAuthorityByUid();
                string apps = getappauth.GetAuthority(request.Cookies["UserID"].Value, "Apps");

                BLL.hr_employee emp = new BLL.hr_employee();
                DataSet dsemp = emp.GetList("ID=" + int.Parse( request.Cookies["UserID"].Value));
                bool BtnAble = false;
                if (dsemp.Tables[0].Rows.Count > 0)
                {
                    if (dsemp.Tables[0].Rows[0]["uid"].ToString() == "admin")
                    {
                        BtnAble = true;
                    }
                }

                DataSet ds = app.GetList(0, "", "App_order");
                string toolbarscript = "{Items:[";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    toolbarscript += "{";
                    toolbarscript += "type: 'button',";
                    toolbarscript += "text: '" + ds.Tables[0].Rows[i]["App_name"].ToString() + "',";
                    toolbarscript += "icon: '" + ds.Tables[0].Rows[i]["App_icon"].ToString() + "',";

                    if (BtnAble)
                    {
                        toolbarscript += "disable: true,";
                    }
                    else
                    {
                        toolbarscript += "disable: " + getappauth.GetAppAuthority(request.Cookies["UserID"].Value, ds.Tables[0].Rows[i]["id"].ToString()) + ",";
                    }
                    toolbarscript += "click: function () {";
                    toolbarscript += "f_according(" + ds.Tables[0].Rows[i]["id"].ToString() + ")";
                    toolbarscript += "}";
                    toolbarscript += "},";
                }
                toolbarscript = toolbarscript.Substring(0, toolbarscript.Length - 1);
                toolbarscript += "]}";
                context.Response.Write(toolbarscript);
            }
            if (request["Action"] == "GetAppList")
            {
                BLL.Sys_App app = new BLL.Sys_App();
                DataSet ds = app.GetList(0, " ", "App_order");

                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["App_name"] + "',App_icon:'../" + ds.Tables[0].Rows[i]["App_icon"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
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