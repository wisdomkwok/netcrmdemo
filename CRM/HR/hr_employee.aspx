<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />  

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '名字', name: 'name' },
                    { display: '入职日期', name: 'EntryDate' },
                    { display: '生日', name: 'birthday' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '默认岗位', name: 'post' },
                    { display: '默认职务', name: 'zhiwu' },
                    {
                        display: '可登陆', width: 50, render: function (item) {
                            var html = "<div style='margin-top:5px;'><input type='checkbox'";
                            if (item.canlogin == 1) html += "checked='checked'  ";
                            html += " disabled='disabled' /></div>";
                            return html;
                        }
                    },
                    { display: '状态', name: 'status' }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/hr_employee.ashx?Action=grid",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                onDblClickRow: function (data, rowindex, rowobj) {
                    edit();
                },
                rowtype: "status",
                onAfterShowData: function (grid) {
                    $("tr[rowtype='离职']").addClass("l-leaving").removeClass("l-grid-row-alt");
                },
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
                            columns: [
                              //{ display: 'ID', name: 'RoleID', type: 'int', width: 50 },
                              { display: '序号', width: 50, render: function (item, i) { return i + 1; } },
                              { display: '角色名', name: 'RoleName',width:300 },
                              { display: '角色描述', name: 'RoleDscript', width: 450 },
                              { display: '排序', name: 'RoleSort', width: 50 }
                            ],
                            title:'拥有角色',
                            usePager: false,
                            url: "../data/hr_employee.ashx?Action=getRole&empid=" + r.ID,
                             height: '100px',
                            heightDiff: 0
                        })

                    }
                }

            });



            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });
        var view_auth = false;
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=24&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {

                    if (arr[i].text == '查看' && arr[i].disable)
                        view_auth = true;
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'stext', text: '姓名：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#stext").ligerTextBox({ width: 200, nullText: "输入姓名搜索" })
                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        //查询
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../data/hr_employee.ashx?" + serchtxt);
            manager.loadData(true);
        }

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
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        function view() {
            if (view_auth) {
                var manager = $("#maingrid4").ligerGetGridManager();
                var row = manager.getSelectedRow();
                if (row) {
                    parent.jQuery.ligerDialog.open({ width: 770, height: 490, title: "查看员工", url: "HR/hr_employee_add.aspx?empid=" + row.ID, buttons: [{ text: '关闭', onclick: function (item, dialog) { dialog.close(); } }] });
                } else {
                    $.ligerDialog.warn('请选择行！');
                }
            }
            else {
                $.ligerDialog.warn('权限不够！');
            }

        }


        function add() {
            f_openWindow("hr/hr_employee_add.aspx", "新增账号", 730, 430);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('hr/hr_employee_add.aspx?empid=' + row.ID, "修改账号", 730, 430);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function changepwd() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            //alert(row.uid);
            if (row) {
                top.$.ligerDialog.open(
                    {
                        url: 'hr/hr_employee_changpwd.aspx?empid=' + row.ID,
                        title: "修改密码",
                        width: 400,
                        height: 200,
                        buttons:
                            [
                                {
                                    text: '保存', onclick: function (item, dialog) {
                                        dialog.frame.f_save();
                                    }
                                },
                                {
                                    text: '关闭', onclick: function (item, dialog) {
                                        dialog.close();
                                    }
                                }
                            ]
                    }
                );
            }
            else {
                top.$.ligerDialog.error('请选择行!');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("员工删除后不能恢复，且员工所在岗位都将清除，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../data/hr_employee.ashx", type: "POST",
                            data: { Action: "AdvanceDelete", id: row.ID, rnd: Math.random() },
                            success: function (responseText) {
                                top.$.ligerDialog.closeWaitting();
                                if (responseText == "true") {
                                    f_reload();
                                }
                                else if (responseText == "false:customer") {
                                    top.$.ligerDialog.error('删除失败！此员工下含有客户信息，请彻底删除之后再操作。', "删除失败！", null, 9003);
                                }
                                else {
                                    top.$.ligerDialog.error('删除失败！', "", null, 9003);
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('删除失败！', "", null, 9003);
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择员工");
            }
        }
        function f_save(item, dialog) {
            var cansave = dialog.frame.f_checkdefault();
            if (cansave) {
                var issave = dialog.frame.f_save();
                var postnum = dialog.frame.f_postnum();
                var postdata = dialog.frame.f_postdata();

                //alert(postnum + " @ " + postdata);
                if (issave) {
                    dialog.close();
                    $.ligerDialog.waitting('数据保存中,请稍候...');
                    $.ajax({
                        url: "../data/hr_employee.ashx", type: "POST",
                        data: issave + "&PostData=" + postdata,
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
            } else {
                alert('请新增并选择默认岗位！');
            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
    <style type="text/css">
        .l-leaving { background: #eee; color: #999; }
    </style>

</head>
<body>

    <form id="form1">
        <div>
            <div id="toolbar"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
