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
    /// Personal_notes 的摘要说明
    /// </summary>
    public class Personal_notes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Personal_notes notes = new BLL.Personal_notes();
            Model.Personal_notes model = new Model.Personal_notes();
            if (request["Action"] == "Get")
            {

                DataSet ds = notes.GetList("emp_id=" + int.Parse(context.Request.Cookies["UserID"].Value));

                context.Response.Write(GetGridJSON.DataTableToJSON2(ds.Tables[0]));
            }
            if (request["Action"] == "save")
            {
                model.emp_id = int.Parse(context.Request.Cookies["UserID"].Value);
                model.note_content = request["body"];
                model.note_time = DateTime.Now;
                model.note_color = request["color"];
                model.xyz = request["left"] + "," + request["top"] + "," + request["zindex"];

                int id = notes.Add(model);

                context.Response.Write(id);
            }
            if (request["Action"] == "update")
            {
                model.xyz = request["x"] + "," + request["y"] + "," + request["z"];
                model.id = int.Parse(request["id"]);

                notes.Update(model);
            }
            if (request["Action"] == "delete")
            {       
                bool a = notes.Delete(int.Parse(request["id"]));
                context.Response.Write(a);
            }
            if (request["Action"] == "grid")
            {    
                DataSet ds = notes.GetList(0, "emp_id=" + int.Parse(context.Request.Cookies["UserID"].Value), "note_time desc");
                DataTable dt = ds.Tables[0];

                context.Response.Write(GetGridJSON.DataTableToJSON(dt));
            }

            if (request["Action"] == "notesremind")
            {
                DataSet ds = notes.GetList(7, "emp_id=" + int.Parse(request.Cookies["UserID"].Value), " note_time desc");
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
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