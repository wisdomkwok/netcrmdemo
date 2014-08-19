<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script> 
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '名字', name: 'name' },
                    { display: '入职日期', name: 'EntryDate', align: 'left' },
                    { display: '生日', name: 'birthday', align: 'left' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '职务', name: 'zhiwu' },
                    { display: '状态', name: 'status', align: 'left' }
                ],
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],

                url: "../data/hr_employee.ashx?Action=grid",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: 0
            });

            $("#pageloading").hide();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });
        function toolbar() {
            $.get("../data/toolbar.ashx?Action=Get&mid=22&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var toolbarscript = "var toolbar = new Toolbar({renderTo: 'toolbar', items: [";
                toolbarscript += data;
                toolbarscript += "],active: 'ALL'}); toolbar.render();";
                eval(toolbarscript);
            });
        }

        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = { width: width, height: height, title: title, url: url, buttons: [
                    { text: '保存', onclick: function (item, dialog) {
                        f_save(item, dialog);
                    }
                    },
                    { text: '关闭', onclick: function (item, dialog) {
                        dialog.close();
                    }
                    }
                    ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        function add() {            
            f_openWindow("System/hr_employee_add.aspx", "新增账号", 770, 430);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                //$.ligerDialog.open({ width: 770, height: 430, url: 'addemp.aspx?empid=' + row.ID, title: "修改账号" });
                f_openWindow('System/hr_employee_add.aspx?empid=' + row.ID, "修改账号", 770, 430);
            } else {
                top.$.ligerDialog.error('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                if (confirm("删除后不能恢复，\n您确定要删除？")) {
                    $.ajax({
                        type: "POST",
                        url: "employeelist.aspx/Del", /* 注意后面的名字对应CS的方法名称 */
                        data: "{rid:'" + row.ID + "'}", /* 注意参数的格式和名称 */
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            f_reload();
                        }
                    });
                }
            } else {
                alert("请选择行");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "../data/hr_employee.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
   
</head>
<body> 

  <form id="form1" > 
    <div>
        <div id="toolbar"></div>
        <div class="l-loading" style="display:block" id="pageloading"></div>    
        <div id="maingrid4" style="margin:-1px; "></div>   
    </div>     
  </form>

 
</body>
</html>
