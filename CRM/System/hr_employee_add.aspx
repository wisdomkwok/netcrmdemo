<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" >
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            $("#T_dep").ligerComboBox({
                width: 180,
                selectBoxWidth: 180,
                selectBoxHeight: 180,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                }
            })

            if (getparastr("empid")) {
                loadForm(getparastr("empid"));
            }


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("empid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/hr_employee.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    //alert(obj.constructor); //String 构造函数
                    $("#T_uid").val(obj.uid);
                    $("#T_email").val(obj.email);
                    $("#T_name").val(obj.name);
                    $("#T_birthday").val(obj.birthday);
                    $("#T_idcard").val(obj.idcard);
                    $("#T_tel").val(obj.tel);
                    $("#T_entryDate").val(obj.EntryDate);
                    $("#T_Adress").val(obj.address);
                    $("#T_school").val(obj.schools);
                    $("#T_edu").val(obj.education);
                    $("#T_professional").val(obj.professional);
                    $("#T_remarks").val(obj.remarks);

                    $("#T_sex").ligerGetComboBoxManager().selectValue(obj.sex);
                    $("#T_dep").ligerGetComboBoxManager().selectValue(obj.d_id);
                    $("#T_Position").ligerGetComboBoxManager().selectValue(obj.zhiwuid);
                    $("#T_status").ligerGetComboBoxManager().selectValue(obj.status);

                    $("#T_uid").ligerGetTextBoxManager().setDisabled();
                    $("#T_uid").attr("validate", "{required:true}");

                }
            });
        }
        
    </script>
    
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        function remote1() {
            var url = "../data/hr_employee.ashx?Action=Exist&rnd=" + Math.random();
            return url;
        }
        
    </script>
</head>
<body style="margin:5px 5px 5px 5px">
    <form id="form1" >
    <div>
        <table class="bodytable0" border="0" cellpadding="3" cellspacing="1" 
            style="background: #fff; width:560px;">
            
            <tr>
                <td  width="70px" class="style1" ><div align="right" style="width: 61px">
                    账号：</div></td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_uid"   name="T_uid"    ltype="text" ligerui="{width:180}" 
                            validate="{required:true,username:true,remote:'../data/hr_employee.ashx?Action=Exist',messages:{required:'请输入账号名',remote:'此账户存在!'}}" />
                    </div>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 61px">
                    Email：</div>
                    
                </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_email" name="T_email"    ltype="text" ligerui="{width:180}" validate="{required:false,email:true}" />
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" >
                    <div align="right" style="width: 62px">姓名：</div>
                    
                 </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_name" name="T_name"    ltype="text" ligerui="{width:180}" validate="{required:true,minlength:1,maxlength:10,messages:{required:'请输入姓名',maxlength:'你的名字有这么长嘛？'}}"/>
                    </div>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 62px">生日：</div>
                    
                </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_birthday" name="T_birthday"    ltype="date"  validate="{required:true}" ligerui="{width:180}"  />
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" >
                    <div align="right" style="width: 62px">性别：</div>
                    
                 </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_sex" name="T_sex"  ltype="select" ligerui="{width:180,data:[{id:'男',text:'男'},{id:'女',text:'女'}]}" validate="{required:true}" /> 
                    </div>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 61px">
                    身份证：</div>
                    
                </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_idcard"  name="T_idcard"    ltype="text" ligerui="{width:180}"   validate="{required:false,IdNumber:true,messages:{required:'请输入身份证号码'}}" />
                        
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" ><div align="right" style="width: 61px">
                    手机：</div></td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_tel" name="T_tel"    ltype="text" ligerui="{width:180}"  validate="{required:true,cellphone:true,messages:{required:'请输入手机号'}}" />
                    </div>
                    
                </td>
                <td  class="style1" >
                    &nbsp;</td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" >
                    <div align="right" style="width: 62px">部门：</div>
                    
                 </td>
                <td  class="style1" >
                        <input type="text" id="T_dep" name="T_dep"  validate="{required:true,messages:{required:'请选择部门'}}"/>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 62px">职务：</div>
                    
                 </td>
                <td  class="style1" >
                        <input type="text" id="T_Position" name="T_Position"  ltype="select" ligerui="{width:180,url:'../data/hr_position.ashx?Action=combo'}"  validate="{required:true}"/>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" >
                    <div align="right" style="width: 62px">状态：</div>
                    
                 </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_status" name="T_status"  ltype="select" ligerui="{width:180,data:[{id:'在职',text:'在职'},{id:'离职',text:'离职'}]}"/> 
                    </div>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 62px">入职日期：</div>
                    
                 </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_entryDate" name="T_entryDate"    ltype="date"  ligerui="{width:180}"/>
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" ><div align="right" style="width: 61px">
                    地址：</div></td>
                <td  class="style1" colspan="3" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_Adress"  name="T_Adress"    ltype="text" ligerui="{width:457}"/>
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" >
                    <div align="right" style="width: 62px">毕业院校：</div>
                    
                 </td>
                <td  class="style1" colspan="3" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_school" name="T_school"    ltype="text" ligerui="{width:457}"/>
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td  width="70px" class="style1" ><div align="right" style="width: 61px">
                    学历：</div></td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_edu" name="T_edu"    ltype="text" ligerui="{width:180}"/>
                    </div>
                    
                </td>
                <td  class="style1" >
                    <div align="right" style="width: 62px">专业：</div>
                    
                </td>
                <td  class="style1" >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_professional" name="T_professional"    ltype="text" ligerui="{width:180}"/>
                    </div>
                    
                </td>
            </tr>



            <tr>
                <td  class="style1" ><div align="right" style="width: 62px">备注：</div></td>
                <td  class="style1" colspan="3" >
                        <textarea cols="100" id="T_remarks" name="T_remarks" rows="4" class="l-textarea" style="width:457px" ></textarea></td>
            </tr>
            </table>
    </div>
    </form>
</body>
</html>
