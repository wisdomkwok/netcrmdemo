<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
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
    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 250, allowLeftResize: false, allowLeftCollapse: true, space: 2 });
            $("#tree1").ligerTree({
                url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#pageloading").hide();

            $.get("../data/hr_department.ashx?Action=department&did=1", function (data, textStatus) {
                //alert(data);
                var arrstr = new Array();
                arrstr = data.split(",");

                $("#Label1").text(arrstr[1]);
                $("#Label2").text(arrstr[2]);
                $("#Label3").text(arrstr[3]);
                $("#Label4").text(arrstr[6]);
                $("#Label5").text(arrstr[4]);
                $("#Label6").text(arrstr[5]);
                $("#Label7").text(arrstr[7]);
                $("#Label8").text(arrstr[8]);

            });
            toolbar();
        });
        function toolbar() {
            $.get("../data/toolbar.ashx?Action=Get&mid=19&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var toolbarscript = "var toolbar = new Toolbar({renderTo: 'toolbar', items: [";
                toolbarscript += data;
                toolbarscript += "],active: 'ALL'}); toolbar.render();";
                eval(toolbarscript);
            });
        }

        
        function onSelect(note) {
            $.get("../data/hr_department.ashx?Action=department&did=" + note.data.id + '&rdm=' + Math.random(), function (data, textStatus) {
                //alert(data);
                var arrstr = new Array();
                arrstr = data.split(",");

                $("#Label1").text(arrstr[1]);
                $("#Label2").text(arrstr[2]);
                $("#Label3").text(arrstr[3]);
                $("#Label4").text(arrstr[6]);
                $("#Label5").text(arrstr[4]);
                $("#Label6").text(arrstr[5]);
                $("#Label7").text(arrstr[7]);
                $("#Label8").text(arrstr[8]);
            });
                
        }

        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = { width: width, height: height, title: title, url: url, buttons: [
                    { text: '����', onclick: function (item, dialog) {
                        f_save(item, dialog);
                    }
                    },
                    { text: '�ر�', onclick: function (item, dialog) {
                        dialog.close();
                    }
                    }
                    ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        function edit() {
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('System/hr_department_add.aspx?depid=' + notes.data.id, "��������", 620, 280);
            }
            else {
                $.ligerDialog.error('��ѡ���ţ�');
            }
        }
        function add() {
            f_openWindow('System/hr_department_add.aspx', "��������", 620, 280);
        }

        function del() {
            var manager = $("#tree1").ligerGetTreeManager();
            var node = manager.getSelected();
            if (node) {
                if (manager.isChildren(node)) {
                    $.ligerDialog.warn("�����¼����ţ�������ɾ����");
                }
                else {
                    if (confirm("��ɫɾ�����ָܻ���\n��ȷ��Ҫ�Ƴ���")) {
                        $.ajax({
                            url: "../data/hr_department.ashx", type: "POST",
                            data: { Action: "del", id: node.data.id, rnd: Math.random() },
                            success: function (responseText) {
                                top.$.ligerDialog.closeWaitting();
                                if (responseText == "true") {
                                    treereload();
                                }
                                else {
                                    top.$.ligerDialog.error('ɾ��ʧ�ܣ��˲����º���Ա����¼��');
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                            }
                        });
                    }
                }
            } else {
                $.ligerDialog.warn("��ѡ���ţ�");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../data/hr_department.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function treereload() {
            treemanager = $("#tree1").ligerGetTreeManager();
            treemanager.clear();
            treemanager.FlushData();
        }
    </script>
</head>
<body style="padding:0px">
    <form id="form1">
    <div id="toolbar"></div>
    <div id="layout1" style="margin-top:-1px">
        <div position="left" title="��֯�ܹ�">
            <div id="treediv" style="width:250px; height:100%; margin:-1px; float:left; border:1px solid #ccc; overflow:auto;  ">
                <ul id="tree1"></ul>
            </div>
        </div> 
        <div position="center">
            
                    <table class="bodytable0"  style="width:100%;margin:-1px">
                        
                        <tr>
                            <td height="23" width="20%" class="table_label">���ƣ�</td>
                            <td height="23" >
                                <span id="Label1">1</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">�����ˣ�</td>
                            <td height="23" >
                                <span id="Label2">2</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">�绰��</td>
                            <td height="23" >
                                <span id="Label3">3</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">���䣺</td>
                            <td height="23" >
                                <span id="Label4">4</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">���棺</td>
                            <td height="23" >
                                <span id="Label5">5</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">��ַ��</td>
                            <td height="23">
                                <span id="Label6">6</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">������</td>
                            <td height="23">
                                <span id="Label7">7</span>
                            </td>
                        </tr>
                        <tr>
                            <td height="23"  class="table_label">����</td>
                            <td height="23">
                                <span id="Label8">8</span>
                            </td>
                        </tr>
                        </table>
            
                
        </div>
    </div>  
    
    
 <!--<a class="l-button" onclick="getChecked()" style="float:left;margin-right:10px;">��ȡѡ��(��ѡ��)</a> -->
        <div style="display:none">
    <!--  ����ͳ�ƴ��� --> 
    </div>
    </form>
</body>
</html>
