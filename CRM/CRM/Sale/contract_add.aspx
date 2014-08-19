<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../../lib/json2.js" type="text/javascript"></script>

    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            $("#T_Customer").ligerComboBox({
                width: 457,
                onBeforeOpen: f_selectContact
            })
            initcombo();
            if (getparastr("cid")) {
                loadForm(getparastr("cid"));
            }
            if (getparastr("Customer_id")) {
                $.ajax({
                    type: "GET",
                    url: "../../data/crm_customer.ashx", /* 注意后面的名字对应CS的方法名称 */
                    data: { Action: 'form', cid: getparastr("Customer_id"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = eval(result);
                        for (var n in obj) {
                            
                        }
                        //alert(obj.constructor); //String 构造函数
                        $("#T_Customer").val(obj.Customer);
                        $("#T_Customer_val").val(obj.id);
                        $("#T_department").ligerGetComboBoxManager().selectValue(obj.Department_id, obj.Employee_id);
                        $("#T_Customer").ligerGetComboBoxManager().setReadOnly();
                    }
                });
            }
        });
        
        function initcombo() {
            a = $('#T_employee').ligerComboBox({ readonly: true, width: 82 });
            b = $('#T_department').ligerComboBox({
                width: 96,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                readonly: true,
                tree: {
                    url: '../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue, newtext, newid) {
                    $.get("../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        a.setData(eval(data));
                        a.selectValue(newid);
                    });
                }
            });
            c = $('#T_employee1').ligerComboBox({ width: 82 });
            d = $('#T_department1').ligerComboBox({
                width: 96,
                selectBoxWidth: 240,
                selectBoxHeight:200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue, newtext, newid) {
                    $.get("../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        c.setData(eval(data));
                        c.selectValue(newid);
                    });
                }
            });
        }
        function f_selectContact() {
            $.ligerDialog.open({
                title: '选择客户', width: 600, height: 350, url: '../../crm/customer/GetCustomer.aspx', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_Customer").val(data.Customer);
            $("#T_Customer_val").val(data.id);
            $("#T_department").ligerGetComboBoxManager().selectValue(data.Department_id, data.Employee_id);
            dialog.close();
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/Crm_contract.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    //alert(obj.constructor); //String 构造函数
                    $("#T_Customer").val(obj.Customer_name);
                    $("#T_Customer_val").val(obj.Customer_id);
                    $("#T_contract_num").val(obj.Serialnumber);
                    $("#T_contract_name").val(obj.Contract_name);
                    $("#T_contract_amount").val(toMoney(obj.Contract_amount));
                    $("#T_pay_cycle").val(obj.Pay_cycle);
                    $("#T_contractor").val(obj.Customer_Contractor);

                    $("#T_start_date").val(obj.Start_date);
                    $("#T_end_date").val(obj.End_date);
                    $("#T_contract_date").val(obj.Sign_date);

                    $("#T_content").val(obj.Main_Content);
                    $("#T_remarks").val(obj.Remarks);

                    $("#T_department").ligerGetComboBoxManager().selectValue(obj.C_depid, obj.C_empid);
                    $("#T_department1").ligerGetComboBoxManager().selectValue(obj.Our_Contractor_depid, obj.Our_Contractor_id);
                }
            });
        }   

        function set_tomoney(value)
        {
            $("#T_contract_amount").val(toMoney(value));
        }
        
        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&cid=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }        
    </script>

</head>
<body style="margin: 0">
    <form id="form1">
        <div>
            <table style="width: 620px; margin: 5px;" class='bodytable1'>
                <tr>
                    <td colspan="4" class="table_title1">合同基本信息</td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td width="72px">
                                    <div align="right" style="width: 61px">
                                        客户：
                                    </div>
                                </td>
                                <td colspan="3">
                                    <input type="text" id="T_Customer" name="T_Customer" validate="{required:true}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        合同名称：
                                    </div>
                                </td>
                                <td colspan="3">
                                    <input type="text" id="T_contract_name" name="T_contract_name" ltype="text" ligerui="{width:457}" validate="{required:true}" />
                                </td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        合同编号：
                                    </div>
                                </td>
                                <td>
                                   <input type="text" id="T_contract_num" name="T_contract_num" ltype="text" ligerui="{width:182}" validate="{required:true}" /></td>
                                <td width="62px">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        合同金额：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_contract_amount" name="T_contract_amount" style="text-align:right" ltype="text" ligerui="{width:182,number: true}" onchange="set_tomoney(this.value)" validate="{required:true}" />
                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        开始时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_start_date" name="T_start_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        付款期数：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_pay_cycle" name="T_pay_cycle" ltype="spinner" ligerui="{width:182,type:'int'}" validate="{required:true}" />
                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        结束时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_end_date" name="T_end_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        对方签约：
                                    </div>
                                </td>
                                <td>
                                   <input type="text" id="T_contractor" name="T_contractor" ltype="text" ligerui="{width:182}" validate="{required:true}" />
                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        签订时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_contract_date" name="T_contract_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        我方签约：
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 100px; float: left">
                                        <input id="T_department1" name="T_department1" type="text" validate="{required:true}" style="width: 97px" />
                                    </div>
                                    <div style="width: 98px; float: left">
                                        <input id="T_employee1" name="T_employee1" type="text" validate="{required:true}" style="width: 96px" />
                                    </div>

                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        客户归属：
                                    </div>
                                </td>
                                <td>
                                     <div style="width: 100px; float: left">
                                        <input id="T_department" name="T_department" type="text" validate="{required:true}" style="width: 97px" />
                                    </div>
                                    <div style="width: 98px; float: left">
                                        <input id="T_employee" name="T_employee" type="text" validate="{required:true}" style="width: 96px" />
                                    </div>
                                </td>
                            </tr>
                            </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="table_title1">合同条款</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td width="72px">
                                    <div align="right" style="width: 62px">主要条款：</div>
                                </td>
                                <td>
                                    <textarea cols="100" id="T_content" name="T_content" rows="8" class="l-textarea" style="width: 455px"></textarea></td>
                            </tr>
                            <tr>
                                <td >
                                    <div align="right" style="width: 62px">备注：</div>
                                </td>
                                <td>
                                    <textarea cols="100" id="T_remarks" name="T_remarks" rows="3" class="l-textarea" style="width: 455px"></textarea></td>
                            </tr>
                            </table>
                    </td>
                </tr>
            </table>






        </div>
    </form>

</body>
</html>
