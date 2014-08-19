<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
   <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/json.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>

    <script src="../../JS/FusionCharts/FusionCharts.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });


            $("#maingrid5").ligerGrid({
                columns: [
                    { display: '编号', width: 40, render: function (item, i) { return i + 1; } },
                    {
                        display: '跟进方式', name: 'yy', width: 120, render: function (item) {
                            if (item.yy == "")
                                item.yy = "未分类";
                            return item.yy;
                        }
                    },
                    {
                        display: '时间一', name: 'dt1', width: 120, render: function (item) {
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                return item.dt1;
                            else
                                return "0";
                        }
                    },
                    {
                        display: '时间二', name: 'dt2', width: 120, render: function (item) {
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                return item.dt2;
                            else
                                return "0";
                        }
                    },
                    {
                        display: '增幅', name: 'm3', width: 120, render: function (item) {
                            var dt1, dt2;
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                dt1 = item.dt1;
                            else
                                dt1 = 0;
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                dt2 = item.dt2;
                            else
                                dt2 = 0;
                            if (dt1 > dt2)
                                return "↓ " + (dt2 - dt1);
                            else
                                return "↑ " + (dt2 - dt1);
                        }
                    },
                    {
                        display: '比例', name: 'm4', width: 120, render: function (item) {
                            var dt1, dt2;
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                dt1 = item.dt1;
                            else
                                dt1 = 0;
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                dt2 = item.dt2;
                            else
                                dt2 = 0;
                            if (dt1 == 0)
                                return "--";
                            if (dt1 > dt2)
                                return "↓ " + Math.round((dt2 - dt1) * 100 / dt1) + " %";
                            else
                                return "↑ " + Math.round((dt2 - dt1) * 100 / dt1) + " %";
                        }
                    }
                ],
                url: '../../data/CRM_Follow.ashx',
                usePager: false,
                //dataAction: 'local', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                width: '100%', height: '100%',
                title: "年度统计表",
                heightDiff: -6
            });

            var items = [];

            items.push({ type: 'textbox', id: 'tree1', text: '部门：' });
            items.push({ type: 'textbox', id: 'date1', text: '时间1：' });
            items.push({ type: 'textbox', id: 'date2', text: '时间2：' });
            items.push({ type: 'button', text: '统计', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });
            items.push({ type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { $("#serchform").each(function () { this.reset(); $(".l-selected").removeClass("l-selected"); }); } });

            $("#toolbar").ligerToolBar({
                items: items
            });

            initSelect();

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height() - 350);
            

            $("#maingrid5").ligerGetGridManager().onResize();

            $("#date1").ligerDateEditor({ format: "yyyy年MM月" });
            $("#date2").ligerDateEditor({ format: "yyyy年MM月" });
            //doserch();
        });

        function initSelect() {
            var d = new Date();
            var nowYear = +d.getFullYear();
            var syearData = [];
            for (var i = nowYear; i >= nowYear - 20; i--) {
                syearData.push({ 'text': i, 'id': i });
            }
            $("#syear").ligerComboBox({
                width: 100,
                data: syearData,
                initValue: nowYear
            })



            $('#tree1').ligerComboBox({
                width: 200,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: true,
                //checkbox: true,
                tree: {
                    url: '../../data/Base.ashx?Action=getUserTree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: true
                },
                onSelected: function (newvalue, newtext, newid) {

                    //alert($("#tree1_val").val())

                }
            });


        }

        function test(GridData) {
            var chart_myNext = new FusionCharts("../../js/FusionCharts/FCF_MSColumn3D.swf", "myNext", "860", "280", "0", "0");
            var xmltext = "";
            xmltext += "<graph caption='员工客户跟进'  decimalPrecision='0' formatNumberScale='0' baseFontSize='12' yAxisMinValue='0' yAxisMaxValue='1' >";
            xmltext += "   <categories>";
            xmltext += "        <category name='" + $("#date1").val() + "' />";
            xmltext += "        <category name='" + $("#date2").val() + "' />";
            xmltext += "   </categories>";

            var color = ['AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE', 'AFD8F8', 'F6BD0F', '8BBA00', 'FF8E46', '008E8E', 'D64646', '8E468E', '588526', 'B3AA00', '008ED6', '9D080D', 'A186BE'];
            for (var i = 0; i < GridData.Rows.length; i++) {
                GridData.Rows[i].dt1 = (GridData.Rows[i].dt1 == 0 ? 0.001 : GridData.Rows[i].dt1);
                GridData.Rows[i].dt2 = (GridData.Rows[i].dt2 == 0 ? 0.001 : GridData.Rows[i].dt2);
                xmltext += "   <dataset seriesName='" + GridData.Rows[i].yy + "' color='" + color[i] + "'><set value='" + GridData.Rows[i].dt1 + "' /><set value='" + GridData.Rows[i].dt2 + "' /></dataset>";
            }
            xmltext += "</graph>"

            chart_myNext.setDataXML(xmltext);
            chart_myNext.render("graph");
        }
        function doserch() {
            var sendtxt = "&Action=Compared_empcusfollow&idlist=" + $("#tree1_val").val() + "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid5").ligerGetGridManager();

            top.$.ligerDialog.waitting('数据查询中,请稍候...');
            $.ajax({
                url: "../../data/CRM_Follow.ashx", type: "POST",
                data: serchtxt,
                dataType: 'json',
                beforeSend: function () {
                    manager.showData({ Rows: [], Total: 0 });
                },
                success: function (responseText) {
                    manager.setURL("../../data/CRM_Follow.ashx?" + serchtxt);
                    manager.showData(responseText);
                    top.$.ligerDialog.closeWaitting();
                    test(responseText);
                },
                error: function () {
                    top.$.ligerDialog.closeWaitting();
                    top.$.ligerDialog.error('查询失败！请检查查询项。');
                }
            });
            //test();
        }
        function f_reload() {
            var manager = $("#maingrid5").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
</head>
<body>
    <div style="position: relative; z-index: 9999">
        <form id="serchform">
            <div id="toolbar"></div>
        </form>
    </div>

    <form id="form1">
        <div id="griddiv">
            <div id="graph" style="height: 280px; margin: 5px;"></div>
            
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>
    </form>


</body>
</html>
