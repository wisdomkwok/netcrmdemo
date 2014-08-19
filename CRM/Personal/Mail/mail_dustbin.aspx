<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../../../CSS/input.css" rel="stylesheet" />

    <script src="../../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                   { display: '序号', width: 50, render: function (item, i) { return i + 1; } },
                   {
                       display: '状态', name: 'isView', width: 50, render: function (item) {
                           var html = "<img src='../../images/icon/";
                           if (item.isView == 0)
                               html += 47;
                           else
                               html += 96;
                           html += ".png' />";
                           return html;
                       }
                   },
                   {
                       display: '附件', name: 'atta_count', width: 60, render: function (item) {
                           if (item.atta_count == 0) {
                               return "";
                           }
                           else {
                               var html = "<img src='../../images/IsAccessary.gif'/>"
                               html += item.atta_count;
                               return html;
                           }
                       }
                   },
                   {
                       display: '主题', name: 'mail_title', width: 180, align: 'left', render: function (item) {
                           return "<a href='javascript:void(0)' onclick=view(" + item.mail_id + ")>" + item.mail_title + "</a>";
                       }
                   },
                   { display: '发件人', name: 'sender_name', width: 100 },
                   {
                       display: '发送时间', name: 'sender_time', width: 150, render: function (item) {
                           return formatTimebytype(item.sender_time, 'yyyy-MM-dd hh:mm');
                       }
                   },
                   {
                       display: '阅读时间', name: 'view_time', width: 150, render: function (item) {
                           if (item.isView == 1)
                               return formatTimebytype(item.view_time, 'yyyy-MM-dd hh:mm:ss');
                       }
                   },
                   {
                       display: '删除时间', name: 'Delete_time', width: 150, render: function (item) {
                           return formatTimebytype(item.Delete_time, 'yyyy-MM-dd hh:mm');
                       }
                   }
                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../../data/mail.ashx?Action=recive_grid&isdel=1&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,
                checkbox: true,
                
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('form').ligerForm();
            initSerchForm();
            toolbar();
        });

        function toolbar() {
            $.getJSON("../../../data/toolbar.ashx?Action=GetSys&mid=80&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    items.push(arr[i]);
                }
                items.push({
                    type: 'serchbtn',
                    text: '高级搜索',
                    icon: '../../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#pageloading").fadeOut(400);
                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        function initSerchForm() {
           
        }
        function serchpanel() {
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
        }
        function doserch() {
            var sendtxt = "&Action=recive_grid&isdel=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);

            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../../../data/mail.ashx?" + serchtxt);
            manager.loadData(true);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#serchform").each(function () {
                this.reset();
            });
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13) {
                doserch();
            }
        });
        function regain() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length > 0) {
                var rowid = "";
                for (var i = 0; i < row.length; i++) {
                    if (i == (row.length - 1)) {
                        rowid += row[i].id;
                    }
                    else {
                        rowid += row[i].id + ",";
                    }
                }
                $.ajax({
                    url: "../../../data/mail.ashx", type: "POST",
                    data: { Action: "regain", idlist: rowid, rnd: Math.random() },
                    success: function (responseText) {
                        if (responseText == "true") {
                            f_reload();
                        }
                        else if (responseText == "false") {
                            $.ligerDialog.error('恢复失败!')
                        }
                        else if (responseText == "delfalse") {
                            $.ligerDialog.error('没有权限恢复!');
                        }
                        else {
                            $.ligerDialog.error('未知错误!');
                        }

                    },
                    error: function () {
                        $.ligerDialog.error('删除失败!');
                    }
                });
                
            }
            else {
                $.ligerDialog.warn("请选择行!");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length > 0) {
                $.ligerDialog.confirm("删除后不能恢复，请谨慎操作！\n您确定要删除？",function(yes){
                if (yes) {
                    var rowid = "";
                    for (var i = 0; i < row.length; i++) {
                        if (i == (row.length - 1)) {
                            rowid += row[i].id;
                        }
                        else {
                            rowid += row[i].id + ",";
                        }
                    }
                    //alert(rowid);
                    $.ajax({
                        url: "../../../data/mail.ashx", type: "POST",
                        data: { Action: "del", idlist: rowid, rnd: Math.random() },
                        success: function (responseText) {
                            if (responseText == "true") {
                                f_reload();
                            }
                            else if (responseText == "false") {
                                $.ligerDialog.error('删除失败!')
                            }
                            else if (responseText == "delfalse") {
                                $.ligerDialog.error('没有权限删除!');
                            }
                            else {
                                $.ligerDialog.error('未知错误!');
                            }
                        },
                        error: function () {
                            $.ligerDialog.error('删除失败!');
                        }
                    });
                }
                })
            }
            else {
                $.ligerDialog.warn("请选择行!");
            }
        }

        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }; 
    </script>
</head>
<body>
    <form id="form1">
        <div id="toolbar"></div>
        <div class="l-loading" style="display: block" id="pageloading"></div>
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 900px' class="bodytable1">
                <tr>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>发送时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>主题：</div>
                    </td>
                    <td>
                        <input id='mail_title' name="mail_title" type='text' ltype='text' ligerui='{width:120}' />
                    </td>
                    <%--<td>
                        <div style='width: 60px; text-align: right; float: right'>状态：</div>
                    </td>
                    <td>
                        <input type='text' id='mail_status' name='mail_status' ltype='text' ligerui='{width:196}' />
                    </td>--%>
                    
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>删除时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate2' name='startdate2' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate2' name='enddate2' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                    

                </tr>
                <tr>
                    
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>阅读时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate1' name='startdate1' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate1' name='enddate1' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>发件人：</div>
                    </td>
                    <td>
                        <input type='text' id='sender' name='sender' ltype='text' ligerui='{width:120}' />
                    </td>

                    
                    
                    <td></td>
                    <td>
                        <input id='Button2' type='button' value='重置' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='搜索' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>
