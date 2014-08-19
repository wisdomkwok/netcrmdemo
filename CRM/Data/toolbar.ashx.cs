using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace XHD.CRM.Data
{
    /// <summary>
    /// toolbar 的摘要说明
    /// </summary>
    public class toolbar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
            //sys toolbar
            if (request["Action"] == "GetSys")
            {
                BLL.Sys_Button btn = new BLL.Sys_Button();

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
                DataSet ds = btn.GetList(0, "Menu_id = " + int.Parse(request["mid"]), "convert(int,[Btn_order])");
                Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                string toolbarscript = "{Items:[";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    toolbarscript += "{";
                    toolbarscript += "type: 'button',";
                    toolbarscript += "text: '" + ds.Tables[0].Rows[i]["Btn_name"].ToString() + "',";
                    toolbarscript += "icon: '" + ds.Tables[0].Rows[i]["Btn_icon"].ToString() + "',";
                    if (BtnAble)
                    {
                        toolbarscript += "disable: true,";
                    }
                    else
                    {
                        toolbarscript += "disable: " + getauth.GetBtnAuthority(request.Cookies["UserID"].Value, ds.Tables[0].Rows[i]["Btn_id"].ToString()) + ",";
                    }
                    toolbarscript += "click: function () {";
                    toolbarscript += ds.Tables[0].Rows[i]["Btn_handler"].ToString().Replace("()", "(" + int.Parse(request["mid"]) + ")");
                    toolbarscript += "}";
                    toolbarscript += "},";

                }
                toolbarscript = toolbarscript.Substring(0, toolbarscript.Length - 1);
                toolbarscript += "]}";
                context.Response.Write(toolbarscript);
            }
            else
            {
                context.Response.Write("none");
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