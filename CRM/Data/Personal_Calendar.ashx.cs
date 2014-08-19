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
    /// Personal_Calendar 的摘要说明
    /// </summary>
    public class Personal_Calendar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Personal_Calendar calendar = new BLL.Personal_Calendar();
            Model.Personal_Calendar model = new Model.Personal_Calendar();

            if (request["Action"] == "get")
            {
                CalendarViewType viewType = (CalendarViewType)Enum.Parse(typeof(CalendarViewType), request["viewtype"]);
                string strshowday = request["showdate"];
                int clientzone = Convert.ToInt32(request["timezone"]);
                int serverzone = GetTimeZone();

                var zonediff = serverzone - clientzone;



                var format = new CalendarViewFormat(viewType, DateTime.Parse(strshowday), DayOfWeek.Monday);

                DataSet ds = calendar.GetList("emp_id=" + int.Parse(context.Request.Cookies["UserID"].Value) + " and StartTime>='" + format.StartDate.ToString("yyyy-MM-dd hh:mm:ss") + "' and EndTime<='" + format.EndDate.ToString("yyyy-MM-dd hh:mm:ss") + "'");
                string dt = DataToJSON(ds);

                var data = new JsonCalendarViewData(calendar.DataTableToList(ds.Tables[0]), format.StartDate, format.EndDate);
                context.Response.Write("{\"start\":\"\\/Date(" + MilliTimeStamp(format.StartDate) + ")\\/\",\"end\":\"\\/Date(" + MilliTimeStamp(format.EndDate) + ")\\/\",\"error\":null,\"issort\":true,\"events\":[" + dt + "]}");
                //context.Response.Write(dt);
            }

            if (request["Action"] == "quickadd")
            {
                int clientzone = Convert.ToInt32(request["timezone"]);
                int serverzone = GetTimeZone();
                var zonediff = serverzone - clientzone;

                model.Subject = PageValidate.InputText(request["CalendarTitle"], 4000);
                model.StartTime = DateTime.Parse(request["CalendarStartTime"]).AddHours(zonediff);
                model.EndTime = DateTime.Parse(request["CalendarEndTime"]).AddHours(zonediff);
                model.IsAllDayEvent = PageValidate.InputText(request["IsAllDayEvent"], 255) == "1" ? true : false;

                model.CalendarType = 1;
                model.InstanceType = 0;

                model.UPAccount = context.Request.Cookies["UserID"].Value;
                model.UPTime = DateTime.Now;
                model.MasterId = clientzone;

                model.emp_id = int.Parse(context.Request.Cookies["UserID"].Value);
                model.Category = PageValidate.InputText(context.Request.Cookies["UserID"].Value, 50);

                int n = calendar.Add(model);

                context.Response.Write("{\"IsSuccess\":true,\"Msg\":\"\u64cd\u4f5c\u6210\u529f!\",\"Data\":\"" + n + "\"}");
            }
            if (request["Action"] == "quickupdate")
            {
                string Id = request["calendarId"];

                int clientzone = Convert.ToInt32(request["timezone"]);
                int serverzone = GetTimeZone();
                var zonediff = serverzone - clientzone;

                model.StartTime = DateTime.Parse(request["CalendarStartTime"]).AddHours(zonediff);
                model.EndTime = DateTime.Parse(request["CalendarEndTime"]).AddHours(zonediff);

                model.UPAccount = context.Request.Cookies["UserID"].Value;
                model.UPTime = DateTime.Now;
                model.MasterId = clientzone;

                model.Id = int.Parse(Id);

                calendar.quickUpdate(model);

                context.Response.Write("{IsSuccess:true}");
            }
            if (request["Action"] == "quickdel")
            {
                int id = Convert.ToInt32(request["calendarId"]);
                calendar.Delete(id);

                context.Response.Write("{IsSuccess:true}");
            }
            if (request["Action"] == "form")
            {
                int id = Convert.ToInt32(request["calendarid"]);
                DataSet ds = calendar.GetList("Id=" + id);
                string dt = Common.DataToJson.DataToJSON(ds);
                context.Response.Write(dt);
            }
            if (request["Action"] == "save")
            {
                string Id = request["calendarid"];

                int clientzone = 8;
                int serverzone = GetTimeZone();
                var zonediff = serverzone - clientzone;

                model.StartTime = DateTime.Parse(request["T_starttime"]).AddHours(zonediff);
                model.EndTime = DateTime.Parse(request["T_endtime"]).AddHours(zonediff);

                model.Subject = Common.PageValidate.InputText(request["T_content"], 4000);

                model.emp_id = int.Parse(context.Request.Cookies["UserID"].Value);
                model.UPAccount = context.Request.Cookies["UserID"].Value;
                model.UPTime = DateTime.Now;
                model.MasterId = clientzone;
                model.CalendarType = 1;
                model.InstanceType = 0;
                model.IsAllDayEvent = PageValidate.InputText(request["allday"], 255) == "True" ? true : false;

                model.Id = int.Parse(Id);

                calendar.Update(model);

                context.Response.Write("{IsSuccess:true}");
            }
            if (request["Action"] == "Today")
            {
                DateTime starttime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 00:00:00");
                DateTime endtime = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString() + " 00:00:00");

                //DataSet ds = calendar.GetList(0, "datediff(day,[StartTime],getdate())=0 and datediff(day,[EndTime],getdate())=0 and emp_id=" + int.Parse(request.Cookies["UserID"].Value), "[StartTime] desc");

                DataSet ds = calendar.GetList(0, "'" + DateTime.Now.ToShortDateString() + " 23:59:50' >= [StartTime] and '" + DateTime.Now.ToShortDateString() + " 0:00:00' <= [EndTime] and emp_id=" + int.Parse(request.Cookies["UserID"].Value), "[StartTime] desc");
                context.Response.Write(GetGridJSON.DataTableToJSON(ds.Tables[0]));
            }
        }

        private static string DataToJSON(DataSet ds)
        {
            StringBuilder JsonString = new StringBuilder();
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("[");
                    JsonString.Append("\"" + (dt.Rows[i]["Id"].ToString()) + "\",");
                    JsonString.Append("\"" + System.Web.HttpUtility.UrlEncode((dt.Rows[i]["Subject"].ToString())) + "\",");
                    JsonString.Append("\"\\/Date(" + MilliTimeStamp(DateTime.Parse(dt.Rows[i]["StartTime"].ToString())) + ")\\/\",");
                    JsonString.Append("\"\\/Date(" + MilliTimeStamp(DateTime.Parse(dt.Rows[i]["EndTime"].ToString())) + ")\\/\",");
                    JsonString.Append("" + (dt.Rows[i]["IsAllDayEvent"].ToString() == "True" ? 1 : 0) + ",");
                    JsonString.Append("" + (dt.Rows[i]["StartTime"].ToString() == dt.Rows[i]["StartTime"].ToString() ? 0 : 1) + ",");
                    JsonString.Append("" + (dt.Rows[i]["InstanceType"].ToString() == "2" ? 1 : 0) + ",");
                    JsonString.Append("" + (string.IsNullOrEmpty(dt.Rows[i]["Category"].ToString()) ? "-1" : "" + dt.Rows[i]["Category"].ToString()) + ",");
                    JsonString.Append("1,\"" + dt.Rows[i]["companyid"] + "\",\"\"");

                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("]");
                    }
                    else
                    {
                        JsonString.Append("],");
                    }
                }
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
        private static int GetTimeZone()
        {
            DateTime now = DateTime.Now;
            var utcnow = now.ToUniversalTime();

            var sp = now - utcnow;

            return sp.Hours;
        }
        private static long MilliTimeStamp(DateTime theDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = theDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
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