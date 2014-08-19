<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: 'ID', name: 'id', width: 30 },
                    { display: '产品', name: 'Product' }
                ],                
                dataAction: 'local', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "../data/Param_Product.ashx?Action=grid",
                width: '100%', height: '100%',
                //title: "big",
                heightDiff: -0
            });
            
            $("#pageloading").hide();

            var manager = $("#maingrid4").ligerGetGridManager();
            manager.onResize();
            toolbar();

            $("#pageloading").hide();
        });
        function toolbar() {
            $.get("../data/toolbar.ashx?Action=Get&mid=17&rnd=" + Math.random(), function (data, textStatus) {
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
            f_openWindow("System/Param_Product_add.aspx", "新增产品", 390, 200);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/Param_Product_add.aspx?pid=' + row.id, "修改产品", 390, 200);
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
                        url: "../data/Param_Product.ashx", 
                        data: { Action:'del',id:row.id },                        
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
                    url: "../data/Param_Product.ashx", type: "POST",
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
    <form id="mainform" >
        <div id="toolbar"></div>
        <div class="l-loading" style="display:block" id="pageloading"></div> 
        <div id="maingrid4" style="margin:-1px; "></div> 
    </form>
</body>
</html>
