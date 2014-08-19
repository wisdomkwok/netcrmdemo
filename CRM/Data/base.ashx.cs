using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;

namespace XHD.CRM.Data
{
    /// <summary>
    /// _base 的摘要说明
    /// </summary>
    public class _base : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Sys_Menu menu = new BLL.Sys_Menu();

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(request.Cookies["UserID"].Value);
            DataSet dsemp = emp.GetList("id=" + int.Parse( request.Cookies["UserID"].Value));
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "GetSysApp")
            {
                DataSet ds = null;

                int appid = int.Parse(request["appid"]);

                if (dsemp.Tables[0].Rows.Count > 0)
                {
                    if (dsemp.Tables[0].Rows[0]["uid"].ToString() == "admin")
                    {
                        ds = menu.GetList(0, "App_id=" + appid, "Menu_order");
                    }
                    else
                    {
                        Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                        string menus = getauth.GetAuthority(request.Cookies["UserID"].Value, "Menus");
                        ds = menu.GetList(0, "App_id=" + appid + " and Menu_id in " + menus, "Menu_order");
                    }
                }

                string dt = "[" + GetTasksString(0, ds.Tables[0]) + "]";

                context.Response.Write(dt);
            }
            if (request["Action"] == "getUserTree")
            {
                BLL.Sys_online sol = new BLL.Sys_online();
                Model.Sys_online model = new Model.Sys_online();


                model.UserName = emp.GetList("ID =" + int.Parse(request.Cookies["UserID"].Value)).Tables[0].Rows[0]["name"].ToString();
                model.UserID = int.Parse(request.Cookies["UserID"].Value);
                model.LastLogTime = DateTime.Now;

                DataSet ds1 = sol.GetList(" UserID=" + request.Cookies["UserID"].Value);

                //添加当前用户信息
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    sol.Update(model, " UserID=" + request.Cookies["UserID"].Value);
                }
                else
                {
                    sol.Add(model);
                }
                //}

                //删除超时用户
                sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

                //context.Response.Write(Common.GetGridJSON.DataTableToJSON(sol.GetAllList().Tables[0]));

                BLL.hr_department dep = new BLL.hr_department();
                BLL.hr_post hp = new BLL.hr_post();

                DataSet ds = dep.GetList(0, "isDelete=0 ", " convert(int,[d_order])");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",pid:" + ds.Tables[0].Rows[i]["parentid"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["d_name"] + "',d_icon:'" + ds.Tables[0].Rows[i]["d_icon"] + "'},");
                    DataSet dsp = hp.GetList("dep_id=" + ds.Tables[0].Rows[i]["id"]);
                    for (int j = 0; j < dsp.Tables[0].Rows.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dsp.Tables[0].Rows[j]["emp_name"].ToString()))
                        {
                            DataSet dso = sol.GetList("UserID=" + dsp.Tables[0].Rows[j]["emp_id"]);
                            string posticon = "images/icon/93.png";
                            if (dso.Tables[0].Rows.Count > 0)
                            {
                                posticon = "images/icon/38.png";//95
                            }

                            str.Append("{id:-" + dsp.Tables[0].Rows[j]["post_id"].ToString() + ",pid:" + dsp.Tables[0].Rows[j]["dep_id"].ToString() + ",text:'" + dsp.Tables[0].Rows[j]["emp_name"] + "',d_icon:'" + posticon + "'},");
                        }
                    }
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);

            }
            if (request["Action"] == "GetUserInfo")
            {
                string dt = Common.DataToJson.DataToJSON(dsemp);

                context.Response.Write(dt);

            }
            if (request["Action"] == "GetOnline")
            {
                BLL.Sys_online sol = new BLL.Sys_online();
                Model.Sys_online model = new Model.Sys_online();


                model.UserName = empname;
                model.UserID = emp_id;
                model.LastLogTime = DateTime.Now;

                DataSet ds1 = sol.GetList(" UserID=" + int.Parse( request.Cookies["UserID"].Value));

                //添加当前用户信息
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    sol.Update(model, " UserID=" + int.Parse( request.Cookies["UserID"].Value));
                }
                else
                {
                    sol.Add(model);
                }
                //}

                //删除超时用户
                sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

                context.Response.Write(Common.GetGridJSON.DataTableToJSON(sol.GetAllList().Tables[0]));
            }
            if (request["Action"] == "GetIcons")
            {
                try
                {
                    var icontype = request["icontype"];

                    var rootPath = context.Server.MapPath("~/images/icon/");
                    Common.ObjectListToJSON objtojson = new Common.ObjectListToJSON();
                    List<FileInfo> lp = GetAllFilesInDirectory(rootPath);
                    string a = objtojson.toJSON(lp);
                    context.Response.Write(a);

                }
                catch (Exception err)
                {
                    context.Response.Write("系统错误:" + err.Message);
                }
            }
        }
        public List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>();
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
            }
            return listFiles;
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
                    str.Append("\"");
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append("\":\"");
                    str.Append(row[i].ToString());
                    str.Append("\"");
                }
                if (GetTasksString((int)row["Menu_id"], table).Length > 0)
                {
                    str.Append(",\"children\":[");
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