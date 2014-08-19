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
    /// CRM_order_details 的摘要说明
    /// </summary>
    public class CRM_order_details : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
           
            if (request["Action"] == "grid")
            {
                BLL.CRM_order_details cod = new BLL.CRM_order_details();
                string orderid = request["orderid"];
               

                DataSet ds = cod.GetList(" order_id=" + int.Parse( orderid));
                context.Response.Write(Common.GetGridJSON.DataTableToJSON(ds.Tables[0]));
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