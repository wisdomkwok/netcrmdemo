<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script> 
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: 'ID', name: 'id', type: 'int', width: 50 },
                    { display: '职务', name: 'zhiweiname', align: 'left' },
                    { display: '行号', name: 'order' }

                ],
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                onSelectRow: function (row, index, data) {
                    //alert('onSelectRow:' + index + " | " + data.ProductName); 
                },
                url: "../data/hr_position.ashx?Action=grid",
                width: '100%',
                height: '100%',
                heightDiff: 0
            });

            toolbar();

            $("#pageloading").hide();
        });
        function toolbar() {
            $.get("../data/toolbar.ashx?Action=Get&mid=20&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var toolbarscript = "var toolbar = new Toolbar({renderTo: 'toolbar', items: [";
                toolbarscript += data;
                toolbarscript += "],active: 'ALL'}); toolbar.render();";
                eval(toolbarscript);
            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
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
            f_openWindow('System/hr_position_add.aspx', "新建职务", 390, 200);
        }
        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {                
                f_openWindow('System/hr_position_add.aspx?pid=' + row.id, "修改职务", 390, 200);
            } else {
                alert("请选择行");
            }
        }
        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                if (confirm("角色删除后不能恢复，\n您确定要移除？")) {
                    $.ajax({
                        url: "../data/hr_position.ashx", type: "POST",
                        data: { Action: "del", id: row.id, rnd: Math.random() },
                        success: function (responseText) {
                            top.$.ligerDialog.closeWaitting();
                            if (responseText == "true") {
                                f_reload();
                            }
                            else {
                                top.$.ligerDialog.error('删除失败！');
                            }

                        },
                        error: function () {
                            top.$.ligerDialog.closeWaitting();
                            top.$.ligerDialog.error('删除失败！');
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
                    url: "../data/hr_position.ashx", type: "POST",
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
    </script>
   
</head>
<body> 

  <form id="form1"> 
    <div>
        <div id="toolbar"></div>
        <div class="l-loading" style="display:block" id="pageloading"></div>    
        <div id="maingrid4" style="margin:-1px; "></div>   
    </div>     
  </form>

 
</body>
</html>
