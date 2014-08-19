<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" >
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>    
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js"></script>
    
    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            $("#T_depname").ligerComboBox({
                width: 150,
                selectBoxWidth: 150,
                selectBoxHeight: 150,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                initValue: getparastr("depid"),
                readonly:false,
                tree: {
                    url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                }
            })

            if (getparastr("postid")) {
                loadForm(getparastr("postid"));
            }
            else
            {
                $("#T_position").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 150,
                    selectBoxHeight: 150,
                    valueField: 'id',
                    textField: 'text',
                    url: "../data/hr_position.ashx?Action=combo&companyid=0",
                    onSelected: positionselected
                });
                $("#T_emp").ligerComboBox({
                    width: 390,
                    onBeforeOpen: f_selectContact
                });
            }


        });
        function f_selectContact() {
            top.$.ligerDialog.open({
                title: '选择联系人', width: 500, height: 350, url: 'hr/getemp.aspx', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ],zindex:9003
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择员工!');
                return;
            }
            $("#T_emp").val(data.name);
            $("#T_emp_val").val(data.ID);
            dialog.close();
        }
        
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
        function positionselected(newval){
            //alert(newval);
            //$("#T_position_leavel").val(newval);
            $.ajax({
                type: "GET",
                url: "../data/hr_position.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getlevel', position_id: newval, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $("#T_position_leavel").val(result);
                }
            });
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&postid=" + getparastr("postid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/hr_post.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', postid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_postname").val(obj.post_name);
                    $("#T_descript").val(obj.note);                    

                    $("#T_position").ligerComboBox({
                        width: 150,
                        selectBoxWidth: 150,
                        selectBoxHeight: 150,
                        valueField: 'id',
                        textField: 'text',
                        initValue:obj.position_id,
                        url: "../data/hr_position.ashx?Action=combo",
                        onSelected: positionselected
                    });
                    $("#T_emp").ligerComboBox({
                        width: 390,
                        onBeforeOpen: f_selectContact
                    });
                    $("#T_emp").val(obj.emp_name);
                    $("#T_emp_val").val(obj.emp_id);

                }
            });
        }
       

    </script>
</head>
<body>
    <form id="form1" >

         <table align="left" border="0" cellpadding="3" cellspacing="1"  style="width:462px;">
            
            <tr>
                <td ><div align="left" style="width: 60px">岗位名称：</div></td>
                <td ><input type='text' id="T_postname" name="T_postname" ltype="text" ligerui="{width:150}" validate="{required:true}" /></td>
                <td ><div align="left" style="width: 60px">部门名称：</div></td>
                <td ><input type="text" id="T_depname" name="T_depname"  validate="{required:true}"  />
                </td>
            </tr>
            <tr>
                <td  ><div align="left" style="width: 60px">岗位级别：</div></td>
                <td  ><input type="text"  id="T_position" name="T_position"  validate="{required:true}" /></td>
                <td  ><div align="left" style="width: 60px">级别排序：</div></td>
                <td ><input type='text' id="T_position_leavel" name="T_position_leavel"  ltype='text' ligerui="{width:150,disabled:true}" value="20" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td  ><div align="left" style="width: 60px">岗位员工：</div></td>
                <td colspan="3"  ><input type='text' id="T_emp" name="T_emp"/></td>
            </tr>
            <tr>
                <td   style="vertical-align:top"><div align="left" style="width: 54px">描述：</div></td>
                <td   colspan="3"><input type="text" id="T_descript" name="T_descript" ltype="text" ligerui="{width:390}" /> </td>
            </tr>
        </table>
    </form>
</body>
</html>
