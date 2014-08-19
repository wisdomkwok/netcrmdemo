<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <link href="../../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />  
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
    <script src="../../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '组织架构名称', name: 'd_name', width: 140 },
                    { display: '负责人', name: 'd_fuzeren', width: 100 },
                    { display: '电话', name: 'd_tel', width: 100 },
                    { display: '邮箱', name: 'd_email', width: 100 },
                    { display: '传真', name: 'd_fax', width: 100 },
                    { display: '地址', name: 'd_add', width: 100 },
                    { display: '描述', name: 'd_miaoshu', width: 100 },
                    {
                        display: '删除时间', name: 'Delete_time', width: 180, render: function (item) {
                            return formatTimebytype(item.Delete_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }

                ],
                checkbox: true,
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../../data/hr_department.ashx?Action=delgrid&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,
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
            //toolbar1();

        });

        function toolbar() {
            $.getJSON("../../../data/toolbar.ashx?Action=GetSys&mid=80&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../../" + arr[i].icon;
                    items.push(arr[i]);
                } 
                
                $("#toolbar").ligerToolBar({
                    items: items
                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                
                $("#maingrid4").ligerGetGridManager().onResize();
                $("#stext").ligerTextBox({ width: 200, nullText: "输入关键词搜索" })
            });
        }
        function initSerchForm() {
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("../../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
       
       
        function regain() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row) {
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
                    url: "../../../data/hr_department.ashx", type: "POST",
                    data: { Action: "regain", idlist: rowid, rnd: Math.random() },
                    success: function (responseText) {
                        if (responseText == "true") {
                            f_reload();
                        }
                        else {
                            top.$.ligerDialog.error('恢复失败！');
                        }

                    },
                    error: function () {
                        top.$.ligerDialog.error('删除失败！');
                    }
                });

            }
            else {
                $.ligerDialog.warn("情选择行");
            }
        }


        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row) {
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
                        url: "../../../data/hr_department.ashx", type: "POST",
                        data: { Action: "del", idlist: rowid, rnd: Math.random() },
                        success: function (responseText) {
                            if (responseText == "true") {
                                f_reload();
                            }
                            else {
                                top.$.ligerDialog.error('删除失败！');
                            }
                        },
                        error: function () {
                            top.$.ligerDialog.error('删除失败！');
                        }
                    });
                }
                })
            }
            else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
        };


    </script>
</head>
<body>
    <form id="form1">

        
        <div style="position: relative; z-index: 9999">
            <div id="toolbar"></div>
        </div>

        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>

    </form>

</body>
</html>
