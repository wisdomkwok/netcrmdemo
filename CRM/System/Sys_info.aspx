
<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "GET",
                url: "../data/base.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getSysInfo1', rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#Label1").text(obj.company_name);
                    $("#Label2").text(obj.start_time.split(" ")[0]);
                    $("#Label3").text(obj.end_time.split(" ")[0]);
                    $("#Label4").text(obj.user_num);
                }
            });
            $.ajax({
                type: "GET",
                url: "../data/base.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getSysInfo2', rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var data = obj.Rows;
                    var html;
                    for (var i = 0; i < data.length; i++) {
                        //data[i] = urldecode(data[i]);
                        html += "<img src='" + data[i].App_icon + "'/>";
                        html += "<span>" + data[i].App_name + "</span>";
                        html += "　";
                    }
                    $("#Label5").append($(html));
                }
            });
            $.ajax({
                type: "GET",
                url: "../data/base.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getLogo', rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $("#logo").attr("src", "../images/logo/" + result);
                }
            });
            $("#uploadbtn").click(function () {
                var filename = $("#File1").val();
                if (filename && filename != "null" && filename != "" && filename != null) {
                    if (confirm("上传会替换掉原logo，\n您确定要替换？")) {
                        $("#form1").ajaxSubmit({
                            type: "post",
                            url: "../../data/base.ashx", /* 注意后面的名字对应CS的方法名称 */
                            data: { Action: 'upload', filename: filename, rnd: Math.random() }, /* 注意参数的格式和名称 */
                            contentType: "application/json; charset=utf-8",
                            dataType: "text",
                            beforeSend: function (xmlHttp) {
                                xmlHttp.setRequestHeader("If-Modified-Since", "0");
                                xmlHttp.setRequestHeader("Cache-Control", "no-cache");
                            },
                            success: function (result) {
                                $("#logo").attr("src", "../images/logo/" + result);
                                top.getlogo();
                            }
                        });
                    }
                }
                else {
                    alert("请选择图片！")
                }
                
            })

        });

    </script>
</head>
<body style="padding: 0px">
    <form runat="server" id="form1">
        <table class="bodytable0" style="width: 100%; margin: -1px">

            <tr>
                <td height="23" width="150" class="title" colspan="2" style="border-top:none;">系统信息</td>
            </tr>

            <tr>
                <td height="23" width="150" class="table_label">公司名称：</td>
                <td height="23">
                    <span id="Label1">1</span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">开通时间：</td>
                <td height="23">
                    <span id="Label2">2</span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">到期时间：</td>
                <td height="23">
                    <span id="Label3">3</span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">授权用户数：</td>
                <td height="23">
                    <span id="Label4">4</span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">开通项目：</td>
                <td height="23">
                    <span id="Label5"></span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">公司log：</td>
                <td height="23">
                    <img id="logo" style="height: 54px" />
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">&nbsp;</td>
                <td height="23">
                     <div style="width:300px;float:left"><input id="File1" type="file" runat="server" validate="{required:true}" style="height:21px;width:295px;"/></div>
                    
                    <div style="width:200px;float:left;"><input type="button" value="上传" id="uploadbtn"  style="width:60px;height:21px;"/></div>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
