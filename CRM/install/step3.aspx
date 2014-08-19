<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="IE=edge" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>小黄豆CRM-安装</title>

    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("form").ligerForm();
            $("#btn_test").click(function () { check(); })
            $("#btn_next").click(function () { runconfig() })
        });
        function check() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&Action=checkconnect&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: '../Data/install.ashx',
                    data: sendtxt,
                    success: function (result) {
                        if (result == "false:connect") {
                            alert("连接错误，请检查服务器名，用户名和密码！")
                        }
                        else if (result == "false:configed") {
                            alert('检测失败，系统已经配置过，如需重新配置，请在conn.config文件里，把CompleteConfig节配置为false。');
                        }
                        else if (result == "false:dbname") {
                            alert('已存在名为"' + $("#t_db_name").val() + '"的数据库');
                        }
                        else if (result == "false:dbfile") {
                            alert('App_Data文件夹下已存在名为"' + $("#t_db_name").val() + '.mdf"的文件');
                        }
                        else if (result == "success") {
                            alert("连接成功，可以开始配置。");
                            $("#btn_next").attr("disabled", "");
                        }
                        else {
                            alert("系统错误！");
                        }
                        $("#btn_test").attr("disabled", "");

                    },
                    error: function () {
                        alert("检测失败");
                    },
                    beforeSend: function () {
                        $("#btn_test").attr("disabled", "disabled");
                    }

                });

            }
            else {
                $("#btn_next").attr("disabled", "disabled");
            }  
        }
        function runconfig() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&Action=startconfig&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: '../Data/install.ashx',
                    data: sendtxt,
                    beforeSend: function () {
                        $.ligerDialog.waitting("系统配置中,请稍后...");
                        $("#btn_next").attr("disabled", "disabled");
                    },
                    success: function (result) {                           
                        window.location.href = "success.aspx";
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        alert("配置失败！系统错误。");
                    }  
                });  
            }
            else {
                $("#btn_next").attr("disabled", "disabled");
            }
        }
        function btn_reset()
        {
            $("#btn_next").attr("disabled", "disabled");
        }
    </script>
    <style type="text/css">
        span { font-weight: bolder; }
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/loginbackground1.jpg) repeat-x; font-size: 12px; }
        .auto-style1 { color: #FF0000; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">

        <div style="width: 731px; margin-left: 50px; margin-top: 100px;">
            <table class="bodytable3" id="Table1" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <h2>欢迎使用小黄豆CRM</h2>

                        <table>
                            <tr>
                                <td style="width: 400px;">
                                    <p>
                                        <span class="auto-style1"><strong>Step3:</strong></span>现在对您的运行环境进行配置。
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/xhd.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                        <table class="bodytable0" style="width: 100%; margin: -1px; line-height: 25px;">

                            <tr>
                                <td class="table_title" colspan="3" height="25">数据库配置：</td>
                            </tr>

                            <tr>
                                <td width="150px" class="table_label">服务器名：</td>
                                <td height="25">
                                    <input type="text" id="t_name" name="t_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">如：.\sqlexpress，您的SQL服务器名</td>
                            </tr>
                            <tr>
                                <td class="table_label">登录名：</td>
                                <td height="25">
                                    <input type="text" id="t_uid" name="t_uid" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">如：sa</td>
                            </tr>
                            <tr>
                                <td class="table_label">密码：</td>
                                <td height="25">
                                    <input type="text" id="t_pwd" name="t_pwd" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">数据库账号的密码</td>
                            </tr>
                            <tr>
                                <td class="table_label">是否加密：</td>
                                <td height="25">
                                    <input type="text" id="t_Encrypt" name="t_Encrypt" ltype="select" ligerui="{width:200,data:[{'text':'加密',id:0},{'text':'不加密',id:1}],initValue:0}" validate="{required:true}" /></td>
                                <td height="25">连接字符串是否加密</td>
                            </tr>
                            <tr>
                                <td class="table_label">数据库名：</td>
                                <td height="25">
                                    <input type="text" id="t_db_name" name="t_db_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" /></td>
                                <td height="25">数据库名称</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">&nbsp;
                        <input type="button" value="测试连接" style="width: 80px; height: 25px;" id="btn_test" />&nbsp;
                        <input type="button" value="开始安装" style="width: 80px; height: 25px;" id="btn_next" disabled="disabled" /></td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
