<%@ Page Language="C#" AutoEventWireup="true" %>

<!--���ҳ��->
<%
    //ˢ�¾�̬��������  
    CRM.Data.install inss = new CRM.Data.install();
    string filename = Server.MapPath("/conn.config");
    int check_config = inss.CheckConfig(filename);

    //�ж��Ƿ�������
    CRM.Data.install ins = new CRM.Data.install();
    int configed = ins.configed();

    if (configed == 1)
    {
        //�ж��Ƿ��½
        HttpCookie cookie = Request.Cookies["UserID"]; 
        if (Request.IsAuthenticated && null!=cookie)
            Response.Redirect("main.aspx");

        else
            Response.Redirect("login.aspx");
        //Response.Redirect("login.aspx");
    }
    else
        Response.Redirect("install/index.aspx");
 %>
