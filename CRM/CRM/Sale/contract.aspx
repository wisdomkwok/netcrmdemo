<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
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
                    { display: '合同编号', name: 'Serialnumber', width: 140 },
                    { display: '合同名称', name: 'Contract_name', width: 200, align: 'left' },
                    { display: '客户名称', name: 'Customer_name', width: 200 },
                    {
                        display: '合同金额(￥)', name: 'Contract_amount', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.Contract_amount);
                        }
                    },
                    {
                        display: '合同归属', width: 100, render: function (item) {
                            return item.C_depname + "." + item.C_empname;
                        }
                    },

                    { display: '签订日期', name: 'Sign_date', width: 90 }

                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/Crm_contract.ashx?Action=grid&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,
                onDblClickRow: function (data, rowindex, rowobj) {
                    edit();
                },
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        $(p).append(
                            "<table class='bodytable0'  style='width:99%;margin:5px'>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>签约人：</td><td height='23'  width='15%'>" + r.Customer_Contractor + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>我方签约：</td><td height='23'  width='15%'>" + r.Our_Contractor_name + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>开始时间：</td><td height='23'  width='15%'>" + r.Start_date + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>结束时间：</td><td height='23'  width='15%'>" + r.End_date + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>主要条款：</td><td height='23' colspan='7'>" + r.Main_Content.replace(/\n/g, "<br />") + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>备注：</td><td height='23'  colspan='7'>" + r.Remarks + "</td>" +
                                "</tr>" +
                            "</table>");
                    }
                },
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

        var view_auth = false;
        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=35&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].text == '查看' && arr[i].disable)
                        view_auth = true;
                    arr[i].icon = "../../" + arr[i].icon;
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

                
                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        function initSerchForm() {
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
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
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../../../data/Crm_contract.ashx?" + serchtxt);
            manager.loadData(true);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#serchform").each(function () {
                this.reset();
                $(".l-selected").removeClass("l-selected");
            });
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13) {
                doserch();
            }
        });
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                f_save(item, dialog);
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }

        function view() {
            if (view_auth) {
                var manager = $("#maingrid4").ligerGetGridManager();
                var row = manager.getSelectedRow();
                if (row) {
                    parent.jQuery.ligerDialog.open({ width: 770, height: 490, title: "合同详情", url: "CRM/Sale/contract_add.aspx?cid=" + row.id, buttons: [{ text: '关闭', onclick: function (item, dialog) { dialog.close(); } }] });
                } else {
                    $.ligerDialog.warn('请选择行！');
                }
            }
            else {
                $.ligerDialog.warn('权限不够！');
            }

        }
        function add() {
            f_openWindow("CRM/sale/contract_add.aspx", "新增合同", 770, 490);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('CRM/sale/contract_add.aspx?cid=' + row.id, "修改合同", 770, 490);
            }
            else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/Crm_contract.ashx", type: "POST",
                            data: { Action: "AdvanceDelete", id: row.id, rnd: Math.random() },
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
                $.ligerDialog.warn("请选择客户");
            }
        }


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "../../data/Crm_contract.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        $.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！');
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
    <form id="form1">
        <div id="toolbar"></div>
        
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            <div id="toolbar1"></div>
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 750px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>签订日期：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>

                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>合同名：</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>业务员：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    <td></td>
                    <td>
                        <input id='Button2' type='button' value='重置' style='width: 80px; height: 24px' onclick="doclear()" />
                        <input id='Button1' type='button' value='搜索' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>
