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
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '����', name: 'name' },
                    { display: '�Ա�', name: 'sex', width: 50 },
                    { display: '����', name: 'dname' },
                    { display: 'ְ��', name: 'zhiwu' }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/Sys_role_emp.ashx?Action=get&rid=" + str1,
                width: '100%',
                height: '100%',
                //title: "Ա���б�",
                heightDiff: -1
            });
            
            $("#serchbar1").ligerToolBar({
                items: [{
                    type: 'button',
                    text: '���',
                    icon: '../../images/add.gif',
                    disable: true,
                    click: function () {
                        f_add();
                    }
                },
                {
                    type: 'button',
                    text: '�Ƴ�',
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
                //    text: '��ѯ',
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
            [{ id: 1, text: 'δ����' },
            { id: 2, text: '������' },
            { id: 3, text: 'ȫ��'}];

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

            top.$.ligerDialog.open({ title: 'ѡ����ϵ��', width: 600, height: 300, url: 'system/getemp.aspx', buttons: [
                { text: 'ȷ��', onclick: f_selectContactOK },
                { text: 'ȡ��', onclick: f_selectContactCancel }
            ] ,zindex:9003
            });
            return false;
        }

        function f_selectContactOK(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('��ѡ����!');
                return;
            }
            else {
                rows = dialog.frame.f_select();
                //�����ظ�
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
                url: "../data/Sys_role_emp.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: "save", rid: str1, savestring: postdata }, /* ע������ĸ�ʽ������ */
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
