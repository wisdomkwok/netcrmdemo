<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script> 
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '名字', name: 'name' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '职务', name: 'zhiwu' }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/Sys_role_emp.ashx?Action=get&rid=" + str1,
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -1
            });
            
            $("#serchbar1").ligerToolBar({
                items: [{
                    type: 'button',
                    text: '添加',
                    icon: '../../images/add.gif',
                    disable: true,
                    click: function () {
                        f_add();
                    }
                },
                {
                    type: 'button',
                    text: '移除',
                    icon: '../../images/delete.gif',
                    disable: true,
                    click: function () {
                        f_delete();
                    }
                }
                //{
                //    type: 'textbox',
                //    id: "Serchtext",
                //    disable:true
                //},
                //{
                //    type: 'button',
                //    text: '查询',
                //    icon: '../../images/search.gif',
                //    disable: true,
                //    click: function () {
                //        f_search();
                //    }
                //}
                ]
            })

                      
        });

        var SerchData =
            [{ id: 1, text: '未设置' },
            { id: 2, text: '已设置' },
            { id: 3, text: '全部'}];

        function f_search() {
            var grid = $("#maingrid4").ligerGetGridManager();
            grid.loadData(f_getWhere());
        }
        function f_getWhere() {
            var grid = $("#maingrid4").ligerGetGridManager();
            if (!grid) return null;
            var clause = function (rowdata, rowindex) {
                var key = $("#Serchtext").val();
                return rowdata.name.indexOf(key) > -1;
            };
            return clause;
        }

        function f_add() {

            top.$.ligerDialog.open({ title: '选择联系人', width: 600, height: 300, url: 'system/getemp.aspx', buttons: [
                { text: '确定', onclick: f_selectContactOK },
                { text: '取消', onclick: f_selectContactCancel }
            ] ,zindex:9003
            });
            return false;
        }

        function f_selectContactOK(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('请选择行!');
                return;
            }
            else {
                rows = dialog.frame.f_select();
                //过滤重复
                var manager = $("#maingrid4").ligerGetGridManager();
                var data = manager.getCurrentData();

                for (var i = 0; i < rows.length; i++) {
                    var add = 1;
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].ID == data[j].ID) {
                            add = 0;
                        }
                    }
                    if (add == 1)
                        manager.addRow(rows[i]);
                }
                dialog.close();
            }
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }

        function f_delete() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.deleteSelectedRow();
        }

        function f_save() {
            var str1 = getparastr("rid");
            var manager = $("#maingrid4").ligerGetGridManager();
            var savedata = manager.getCurrentData();
            var postdata = JSON.stringify(savedata);

            $.ajax({
                type: "POST",
                url: "../data/Sys_role_emp.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: "save", rid: str1, savestring: postdata }, /* 注意参数的格式和名称 */
                success: function (result) {
                    setTimeout(function () { parent.$.ligerDialog.close(); }, 100);
                    
                }
            });
        }
    </script>
   
</head>
<body> 

  <form id="form1" > 
    <div>
        <div id="serchbar1"></div>
           
        <div id="maingrid4" style="margin:-1px; "></div>   
    </div>     
  </form>
   
</body>
</html>
